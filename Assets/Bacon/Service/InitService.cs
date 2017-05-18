using Maria;
using Maria.Network;
using Sproto;
using UnityEngine;
using System;
using XLua;
using Bacon.Helper;
using Bacon.Event;

namespace Bacon.Service {

    [LuaCallCSharp]
    class InitService : Maria.Service {

        public static readonly string Name = "init";

        private bool _authed = false;
        private float _handshakecd = 5f;
        private long _last = 0;
        private int _lag;
        private SMActor _smactor = null;
        private TimeSync _ts = null;

        private User _user;
        private SysInbox _sysinbox = null;
        private RecordMgr _recordmgr = null;

        public InitService(Context ctx) : base(ctx) {
            _ts = ctx.TiSync;
            _smactor = new SMActor(ctx, this);
            _user = new User();
            _sysinbox = new SysInbox();
            _recordmgr = new RecordMgr();

            _ctx.EventDispatcher.AddCustomEventListener(EventCustom.OnDisconnected, OnDiconnected, null);
            _ctx.EventDispatcher.AddCustomEventListener(EventCustom.OnAuthed, OnAuthed, null);
            _ctx.EventDispatcher.AddCustomEventListener(MyEventCustom.LOGOUT, Logout, null);
        }

        public override void Update(float delta) {
            base.Update(delta);
            SendHandshake(delta);
        }

        public SMActor SMActor { get { return _smactor; } }

        public int Ping { get { return _lag; } }

        public object DataTime { get; private set; }

        public User User { get { return _user; } }
        public string Board { get; set; }
        public string Adver { get; set; }
        public SysInbox SysInBox { get { return _sysinbox; } }
        public RecordMgr RecordMgr { get { return _recordmgr; } }

        public void SendHandshake(float delta) {
            if (!_ctx.Logined) {
                return;
            }
            if (!_authed) {
                return;
            }

            if (_handshakecd > 0) {
                _handshakecd -= delta;
                if (_handshakecd <= 0) {
                    _handshakecd = 5f;
                    _ctx.SendReq<C2sProtocol.handshake>(C2sProtocol.handshake.Tag, null);
                    _last = _ts.GetTimeMs();
                }
            }
        }

        public void Handshake(SprotoTypeBase responseObj) {
            C2sSprotoType.handshake.response o = responseObj as C2sSprotoType.handshake.response;
            UnityEngine.Debug.Log(string.Format("handshake {0}", o.errorcode));
            int lag = (int)(_ts.GetTimeMs() - _last); // ms
            _lag = lag;
        }

        private void OnAuthed(EventCustom e) {
            _authed = true;
        }

        private void OnDiconnected(EventCustom e) {
            _authed = false;
            if (_ctx.Logined) {
                _ctx.GateAuth();
            }
        }

        private void Logout(EventCustom e) {
            _authed = false;
        }

        public SprotoTypeBase OnRadio(SprotoTypeBase requestObj) {
            S2cSprotoType.radio.request obj = requestObj as S2cSprotoType.radio.request;
            Board = obj.board;
            Adver = obj.adver;

            S2cSprotoType.radio.response responseObj = new S2cSprotoType.radio.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }

    }
}
