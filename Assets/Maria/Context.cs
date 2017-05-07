using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Maria.Network;
using System;
using Sproto;
using System.Text;
using XLua;

namespace Maria {

    [LuaCallCSharp]
    public class Context : DisposeObject, INetwork {

        protected Application _application;
        protected Config _config = null;
        protected TimeSync _ts = null;
        protected SharpC _sharpc = null;
        protected Debug _logger = null;

        protected EventDispatcher _dispatcher = null;
        protected Dictionary<string, Controller> _hash = new Dictionary<string, Controller>();
        protected Stack<Controller> _stack = new Stack<Controller>();

        private Dictionary<string, Timer> _timer = new Dictionary<string, Timer>();
        private Dictionary<string, Service> _services = new Dictionary<string, Service>();

        protected ClientLogin _login = null;
        protected ClientSocket _client = null;
        protected User _user = new User();

        protected bool _authtcp = false;
        protected bool _authudp = false;
        protected bool _logined = false;
        protected System.Random _rand = new System.Random();

        public Context(Application application, Config config, TimeSync ts) {
            _application = application;
            _config = config;
            _ts = ts;
            _sharpc = new SharpC();
            _dispatcher = new EventDispatcher(this);

            _login = new ClientLogin(this);
            _login.OnLogined = LoginAuthCb;
            _login.OnDisconnected = LoginDisconnect;

            _client = new ClientSocket(this, _config.s2c, _config.c2s);
            _client.OnAuthed = OnGateAuthed;
            _client.OnDisconnected = OnGateDisconnected;
            _client.OnSyncUdp = OnUdpSync;
            _client.OnRecvUdp = OnUdpRecv;
        }

        protected override void Dispose(bool disposing) {
            if (_disposed) {
                return;
            }
            if (disposing) {
                // 清理托管资源，调用自己管理的对象的Dispose方法
                _client.Dispose();
            }
            // 清理非托管资源

            _disposed = true;
        }

        // Update is called once per frame
        public virtual void Update(float delta) {
            _login.Update();
            _client.Update();

            int now = _ts.LocalTime();
            foreach (var item in _timer) {
                Timer tm = item.Value as Timer;
                if (!tm.Enable) {
                    continue;
                }
                int past = now - tm.ST;
                if ((past / 100) > (tm.PT + 1)) {
                    tm.PT = tm.PT + 1;
                    if (tm.DCB != null) {
                        tm.DCB(tm.PT, tm.CD - tm.PT);
                    }
                }
                if (tm.PT >= tm.CD) {
                    if (tm.CB != null) {
                        tm.CB();
                    }
                    tm.Enable = false;
                }
            }

            foreach (var item in _services) {
                item.Value.Update(delta);
            }

            if (_stack.Count > 0) {
                Controller controller = Peek();
                if (controller != null) {
                    controller.Update(delta);
                }
            }
        }

        public EventDispatcher EventDispatcher { get { return _dispatcher; } }
        public Config Config { get { return _config; } }
        public TimeSync TiSync { get { return _ts; } }
        public SharpC SharpC { get { return _sharpc; } }
        public bool Logined { get { return _logined; } set { _logined = value; } }

        public User U { get { return _user; } }

        public T GetController<T>(string name) where T : Controller {
            try {
                if (_hash.ContainsKey(name)) {
                    Controller controller = _hash[name];
                    return controller as T;
                } else {
                    UnityEngine.Debug.LogError(string.Format("{0} no't exitstence", name));
                    return null;
                }
            } catch (KeyNotFoundException ex) {
                UnityEngine.Debug.LogError(ex.Message);
                return null;
            }
        }

        public int Range(int min, int max) {
            return _rand.Next(min, max);
        }

        // login
        public void LoginAuth(string s, string u, string pwd) {
            _authtcp = false;
            _logined = false;

            _user.Server = s;
            _user.Username = u;
            _user.Password = pwd;
            string ip = Config.LoginIp;
            int port = Config.LoginPort;
            _login.Auth(ip, port, s, u, pwd);
        }

        public void LoginAuthCb(int code, byte[] secret, string dummy) {
            if (code == 200) {
                int _1 = dummy.IndexOf('#');
                int _2 = dummy.IndexOf('@', _1);
                int _3 = dummy.IndexOf(':', _2);

                string uid = dummy.Substring(0, _1);
                int sid = Int32.Parse(dummy.Substring(_1 + 1, _2 - _1 - 1));
                string gip = dummy.Substring(_2 + 1, _3 - _2 - 1);
                int gpt = Int32.Parse(dummy.Substring(_3 + 1));

                UnityEngine.Debug.Log(string.Format("uid: {0}, sid: {1}", uid, sid));
                UnityEngine.Debug.Log("login");

                _user.Secret = secret;
                _user.Uid = uid;
                _user.Subid = sid;

                //_config.GateIp = gip;
                //_config.GatePort = gpt;
                GateAuth();
            } else {
            }
        }

        public virtual void LoginDisconnect() {
        }

        // TCP
        public void SendReq<T>(int tag, SprotoTypeBase obj) {
            _client.SendReq<T>(tag, obj);
        }

        public void GateAuth() {
            _client.Auth(Config.GateIp, Config.GatePort, _user);
        }

        // UDP
        public void UdpAuth(long session, string ip, int port) {
            _client.UdpAuth(session, ip, port);
        }


        public void SendUdp(byte[] data) {
            if (_authudp) {
                _client.SendUdp(data);
            }
        }

        public Controller Peek() {
            if (_stack.Count > 0) {
                return _stack.Peek();
            }
            return null;
        }

        public T Peek<T>() where T : Controller {
            if (_stack.Count > 0) {
                return _stack.Peek() as T;
            }
            return null;
        }

        public Controller Push(Type type) {
            Controller controller = (Controller)Activator.CreateInstance(type, this);
            if (_stack.Count > 0) {
                _stack.Peek().Exit();
            }
            _stack.Push(controller);
            controller.Enter();
            return controller;
        }

        public T Push<T>() where T : Controller {
            T controller = Activator.CreateInstance(typeof(T), this) as T;
            if (_stack.Count > 0) {
                _stack.Peek().Exit();
            }
            _stack.Push(controller);
            controller.Enter();
            return controller;
        }

        public Controller Pop() {
            Controller controller = null;
            if (_stack.Count > 0) {
                controller = _stack.Peek();
                controller.Exit();
                _stack.Pop();
            }
            if (_stack.Count > 0) {
                _stack.Peek().Enter();
            }
            return controller;
        }

        public T Pop<T>() where T : Controller {
            T controller = null;
            if (_stack.Count > 0) {
                controller = _stack.Peek() as T;
                controller.Exit();
                _stack.Pop();
            }
            if (_stack.Count > 0) {
                _stack.Peek().Enter();
            }
            return controller;
        }

        public void Countdown(string name, int cd, Timer.CountdownDeltaCb dcb, Timer.CountdownCb cb) {
            if (cd < 0) {
                if (_timer.ContainsKey(name)) {
                    Timer tm = _timer[name];
                    tm.Enable = false;
                }
            } else {
                Timer tm;
                if (_timer.ContainsKey(name)) {
                    tm = _timer[name];
                    tm.Enable = true;
                    tm.ST = _ts.LocalTime();
                    tm.PT = 0;
                    tm.CD = cd;
                    tm.DCB = dcb;
                    tm.CB = cb;
                } else {
                    tm = new Timer();
                    tm.Name = name;
                    tm.Enable = true;
                    tm.ST = _ts.LocalTime();
                    tm.PT = 0;
                    tm.CD = cd;
                    tm.DCB = dcb;
                    tm.CB = cb;
                    _timer[name] = tm;
                }
            }
        }

        public void RegService(string name, Service s) {
            if (_services.ContainsKey(name)) {
                _services[name] = s;
            } else {
                _services[name] = s;
            }
        }

        public void UnrService(string name, Service s) {
            if (_services.ContainsKey(name)) {
                _services.Remove(name);
            }
        }

        public T QueryService<T>(string name) where T : Service {
            if (_services.ContainsKey(name)) {
                return _services[name] as T;
            }
            return null;
        }

        public Service QueryService(string name) {
            if (_services.ContainsKey(name)) {
                return _services[name];
            }
            return null;
        }

        public void Enqueue(Command cmd) {
            _application.Enqueue(cmd);
        }

        public void EnqueueRenderQueue(Actor.RenderHandler handler) {
            _application.EnqueueRenderQueue(handler);
        }

        public void OnGateAuthed(int code) {
            if (code == 200) {
                _authtcp = true;
                _logined = true;

                string dummy = string.Empty;
                //
                EventDispatcher.FireCustomEvent(EventCustom.OnAuthed, null);

                //
                if (_stack.Count > 0) {
                    Controller controller = Peek();

                    controller.OnGateAuthed(code);
                }
            } else if (code == 403) {
                //LoginAuth(_user.Server, _user.Username, _user.Password);
            }
        }

        public void OnGateDisconnected() {
            if (_logined) {
                EventDispatcher.FireCustomEvent(EventCustom.OnDisconnected, null);
                var controller = Peek();
                if (controller != null) {
                    controller.OnGateDisconnected();
                }
            } else {
                if (_stack.Count > 0) {
                    var controller = Peek();
                    if (controller != null) {
                        controller.Logout();
                    }
                }
            }

        }

        public void OnUdpSync() {
            var controller = Peek();
            if (controller != null) {
                controller.OnUdpSync();
            }
        }

        public void OnUdpRecv(PackageSocketUdp.R r) {
            var controller = Peek();
            if (controller != null) {
                controller.OnUdpRecv(r);
            }
        }
    }
}

