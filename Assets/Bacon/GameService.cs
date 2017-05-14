using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Sproto;
using UnityEngine;

namespace Bacon {
    public class GameService : Service {

        public static readonly string Name = "game";

        private long _roomid = 0;
        private long _max = 0;
        private long _myidx = 0;
        private long _joined = 0;
        private long _online = 0;
        private bool _host = false;

        private Dictionary<long, Player> _playes = new Dictionary<long, Player>();
        private GameController _controller = null;
        private bool _loadedcards = false;

        public GameService(Context ctx) : base(ctx) {
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_LOADEDCARDS, LoadedCards);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_EXITROOM, SendLeave);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);
        }

        public long RoomId { get { return _roomid; } }
        public long MyIdx { get { return _myidx; } }
        public long Max { get { return _max; } }
        public bool Host { get { return _host; } }

        public Player GetPlayer(long idx) {
            if (_playes.ContainsKey(idx)) {
                return _playes[idx];
            } else {
                UnityEngine.Debug.Assert(false);
                return null;
            }
        }

        public void Foreach(Action<Player> cb) {
            for (int i = 1; i <= _max; i++) {
                cb(_playes[i]);
            }
        }

        public void Create(SprotoTypeBase responseObj) {

            C2sSprotoType.create.response obj = responseObj as C2sSprotoType.create.response;
            if (obj.errorcode == Errorcode.SUCCESS) {
                try {
                    _roomid = obj.roomid;
                    _max = obj.room_max;
                    _host = true;

                    C2sSprotoType.join.request request = new C2sSprotoType.join.request();
                    request.roomid = _roomid;
                    _ctx.SendReq<C2sProtocol.join>(C2sProtocol.join.Tag, request);

                } catch (Exception ex) {
                    UnityEngine.Debug.LogException(ex);
                }
            }
        }

        public void Join(SprotoTypeBase responseObj) {
            C2sSprotoType.join.response obj = responseObj as C2sSprotoType.join.response;
            if (obj.errorcode == Errorcode.SUCCESS) {
                _roomid = obj.roomid;
                _max = obj.room_max;
                _host = false;

                _controller = (GameController)_ctx.Push(typeof(GameController));

                Player player = new BottomPlayer(_ctx, this);
                player.Idx = (int)obj.me.idx;
                player.Chip = (int)obj.me.chip;
                player.Sid = (int)obj.me.sid;
                player.Sex = (int)obj.me.sex;
                player.Name = obj.me.name;
                player.Controller = _controller;
                player.Init();

                _myidx = obj.me.idx;
                _playes[_myidx] = player;
                UnityEngine.Debug.Assert(_ctx.U.Subid == obj.me.sid);

                _joined++;
                _online++;

                if (obj.ps != null && obj.ps.Count > 0) {
                    for (int i = 0; i < obj.ps.Count; i++) {
                        var item = obj.ps[i];
                        long offset = 0;
                        if (item.idx > _myidx) {
                            offset = item.idx - _myidx;
                        } else {
                            offset = item.idx + 4 - _myidx;
                        }
                        switch (offset) {
                            case 1: {
                                    var rplayer = new Bacon.RightPlayer(_ctx, this);
                                    rplayer.Idx = (int)item.idx;
                                    rplayer.Chip = (int)item.chip;
                                    rplayer.Sid = (int)item.sid;
                                    rplayer.Sex = (int)item.sex;
                                    rplayer.Name = item.name;
                                    rplayer.Controller = _controller;
                                    rplayer.Init();
                                    _playes[item.idx] = rplayer;
                                }
                                break;
                            case 2: {
                                    var tplayer = new Bacon.TopPlayer(_ctx, this);
                                    tplayer.Idx = (int)item.idx;
                                    tplayer.Chip = (int)item.chip;
                                    tplayer.Sid = (int)item.sid;
                                    tplayer.Sex = (int)item.sex;
                                    tplayer.Name = item.name;
                                    tplayer.Controller = _controller;
                                    tplayer.Init();
                                    _playes[item.idx] = tplayer;
                                }
                                break;
                            case 3: {
                                    var lplayer = new Bacon.LeftPlayer(_ctx, this);
                                    lplayer.Idx = (int)item.idx;
                                    lplayer.Chip = (int)item.chip;
                                    lplayer.Sid = (int)item.sid;
                                    lplayer.Sex = (int)item.sex;
                                    lplayer.Name = item.name;
                                    lplayer.Controller = _controller;
                                    lplayer.Init();
                                    _playes[item.idx] = lplayer;
                                }
                                break;
                            default:
                                break;
                        }
                        _joined++;
                        _online++;
                    }
                }
                SendStep();
            } else if (obj.errorcode == Errorcode.NOEXiST_ROOMID) {
                MainController controller = _ctx.Peek<MainController>();
                controller.ShowTips("不存在相应的房间号");
            } else if (obj.errorcode == Errorcode.ROOM_FULL) {

                MainController controller = _ctx.Peek<MainController>();
                controller.ShowTips("房间已经满了");
            }
        }

        public SprotoTypeBase OnJoin(SprotoTypeBase requestObj) {
            S2cSprotoType.join.request obj = requestObj as S2cSprotoType.join.request;
            if (obj != null) {
                long offset = 0;
                if (obj.p.idx > _myidx) {
                    offset = obj.p.idx - _myidx;
                } else {
                    offset = obj.p.idx + 4 - _myidx;
                }
                switch (offset) {
                    case 1: {
                            var rplayer = new RightPlayer(_ctx, this);
                            rplayer.Idx = (int)obj.p.idx;
                            rplayer.Chip = (int)obj.p.chip;
                            rplayer.Sid = (int)obj.p.sid;
                            rplayer.Sex = (int)obj.p.sex;
                            rplayer.Name = obj.p.name;
                            rplayer.Controller = _controller;
                            rplayer.Init();
                            _playes[obj.p.idx] = rplayer;
                            if (_loadedcards) {
                                _controller.Scene.SetupRightPlayer();
                            }
                        }
                        break;
                    case 2: {
                            var tplayer = new Bacon.TopPlayer(_ctx, this);
                            tplayer.Idx = (int)obj.p.idx;
                            tplayer.Chip = (int)obj.p.chip;
                            tplayer.Sid = (int)obj.p.sid;
                            tplayer.Sex = (int)obj.p.sex;
                            tplayer.Name = obj.p.name;
                            tplayer.Controller = _controller;
                            tplayer.Init();
                            _playes[obj.p.idx] = tplayer;
                            if (_loadedcards) {
                                _controller.Scene.SetupTopPlayer();
                            }
                        }
                        break;
                    case 3: {
                            var lplayer = new LeftPlayer(_ctx, this);
                            lplayer.Idx = (int)obj.p.idx;
                            lplayer.Chip = (int)obj.p.chip;
                            lplayer.Sid = (int)obj.p.sid;
                            lplayer.Sex = (int)obj.p.sex;
                            lplayer.Name = obj.p.name;
                            lplayer.Controller = _controller;
                            lplayer.Init();
                            _playes[obj.p.idx] = lplayer;
                            if (_loadedcards) {
                                _controller.Scene.SetupLeftPlayer();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            _joined++;
            _online++;
            SendStep();

            S2cSprotoType.join.response responseObj = new S2cSprotoType.join.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }

        public void Leave(SprotoTypeBase responseObj) {
            C2sSprotoType.leave.response obj = responseObj as C2sSprotoType.leave.response;
            if (obj.errorcode == Errorcode.SUCCESS) {
                _ctx.Pop();
            }
        }

        public SprotoTypeBase OnLeave(SprotoTypeBase requestObj) {
            //S2cSprotoType.leave.request obj = requestObj as S2cSprotoType.leave.request;
            _joined--;
            _online--;

            S2cSprotoType.leave.response responseObj = new S2cSprotoType.leave.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }

        public SprotoTypeBase OnAfk(SprotoTypeBase requestObj) {
            S2cSprotoType.afk.request obj = requestObj as S2cSprotoType.afk.request;
            //_playes[obj.idx]:Afk()

            _online--;

            S2cSprotoType.afk.response responseObj = new S2cSprotoType.afk.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }

        public SprotoTypeBase OnAuthed(SprotoTypeBase requestObj) {
            S2cSprotoType.authed.request obj = requestObj as S2cSprotoType.authed.request;

            _online++;

            S2cSprotoType.authed.response responseObj = new S2cSprotoType.authed.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }

        public void SendStep() {
            if ((_online == _max) && _loadedcards) {

                C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                request.idx = _myidx;
                _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
            }
        }

        public void Step(SprotoTypeBase responseObj) {
            C2sSprotoType.step.response obj = responseObj as C2sSprotoType.step.response;
            UnityEngine.Debug.Assert(obj.errorcode == Errorcode.SUCCESS);
        }

        public void LoadedCards(EventCmd e) {
            _loadedcards = true;
            _controller.FlushUI();
            for (int i = 1; i <= _max; i++) {
                if (_playes.ContainsKey(i)) {
                    if (_playes[i].Ori == Player.Orient.BOTTOM) {
                        _controller.Scene.SetupBottomPlayer();
                    } else if (_playes[i].Ori == Player.Orient.LEFT) {
                        _controller.Scene.SetupLeftPlayer();
                    } else if (_playes[i].Ori == Player.Orient.TOP) {
                        _controller.Scene.SetupTopPlayer();
                    } else if (_playes[i].Ori == Player.Orient.RIGHT) {
                        _controller.Scene.SetupRightPlayer();
                    }
                }
            }

            SendStep();
        }

        private void SendLeave(EventCmd e) {
            C2sSprotoType.leave.request request = new C2sSprotoType.leave.request();
            request.idx = _myidx;
            _ctx.SendReq<C2sProtocol.leave>(C2sProtocol.leave.Tag, request);
        }
    }
}
