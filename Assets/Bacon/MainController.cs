using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using Sproto;

namespace Bacon {
    class MainController : Controller {
        public MainController(Context ctx) : base(ctx) {
            _name = "main";
            //EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_MUI_MATCH, OnSendMatch);
            //_ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_MUI_CREATE, OnSendCreate);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_MUI_JOIN, OnSendJoin);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);
        }

        public override void Update(float delta) {
            base.Update(delta);
        }

        public override void Enter() {
            base.Enter();
            InitService service = (InitService)_ctx.QueryService(InitService.Name);
            SMActor actor = service.SMActor;
            actor.LoadScene(_name);
        }

        public override void OnUdpSync() {
            base.OnUdpSync();
        }

        public void OnSendMatch(EventCmd e) {
            if (((AppConfig)_ctx.Config).VERSION == AppConfig.VERSION_TYPE.TEST) {
                _ctx.Push("game");
            } else {
                C2sSprotoType.match.request request = new C2sSprotoType.match.request();
                request.mode = 1;
                _ctx.SendReq<C2sProtocol.match>(C2sProtocol.match.Tag, request);
            }
        }

        public void Match(SprotoTypeBase responseObj) {
            C2sSprotoType.match.response obj = responseObj as C2sSprotoType.match.response;
            UnityEngine.Debug.Assert(obj.errorcode == Errorcode.SUCCESS);
        }

        public SprotoTypeBase OnMatch(SprotoTypeBase requestObj) {
            S2cSprotoType.match.request obj = requestObj as S2cSprotoType.match.request;

            //GameService service = _ctx.QueryService(GameService.Name) as GameService;
            //service.RoomId = (int)obj.roomid;

            //C2sSprotoType.join.request request = new C2sSprotoType.join.request();
            //request.roomid = obj.roomid;
            //_ctx.SendReq<C2sProtocol.join>(C2sProtocol.join.Tag, request);

            S2cSprotoType.match.response responseObj = new S2cSprotoType.match.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }

        public void OnSendCreate(EventCmd e) {
            C2sSprotoType.create.request request = new C2sSprotoType.create.request();
            request.count = 1;
            request.rule = 1;
            _ctx.SendReq<C2sProtocol.create>(C2sProtocol.create.Tag, request);
        }

        public void OnSendJoin(EventCmd e) {
            int roomid =  (int)e.Msg["roomid"];
            GameService service = (GameService)_ctx.QueryService(GameService.Name);
            service.RoomId = roomid;


            C2sSprotoType.join.request request = new C2sSprotoType.join.request();
            request.roomid = roomid;

            _ctx.SendReq<C2sProtocol.join>(C2sProtocol.join.Tag, request);
        }

    }
}
