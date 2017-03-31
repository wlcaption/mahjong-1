using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using Sproto;
using UnityEngine;

namespace Bacon {
    class MainController : Controller {
        private InitService _service = null;
        private MUIActor _mui = null;

        public MainController(Context ctx) : base(ctx) {
            _name = "main";
            _mui = new MUIActor(_ctx, this);

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_MUI_CREATE, OnSendCreate);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_MUI_JOIN, OnSendJoin);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);

            EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_MUI_MSG, OnSendMsg);
            _ctx.EventDispatcher.AddCmdEventListener(listener3);

            EventListenerCmd listener4 = new EventListenerCmd(MyEventCmd.EVENT_MUI_VIEWMAIL, OnSendViewMail);
            _ctx.EventDispatcher.AddCmdEventListener(listener4);
        }

        public override void Update(float delta) {
            base.Update(delta);
        }

        public override void Enter() {
            base.Enter();
            if (_service == null) {
                _service = _ctx.QueryService<InitService>(InitService.Name);
            }
            SMActor actor = _service.SMActor;
            actor.LoadScene(_name);

            _ctx.SendReq<C2sProtocol.first>(C2sProtocol.first.Tag, null);
            _ctx.SendReq<C2sProtocol.fetchsysmail>(C2sProtocol.fetchsysmail.Tag, null);
            _ctx.EnqueueRenderQueue(RenderEnter);
        }

        protected void RenderEnter() {
            ABLoader.current.LoadResAsync<AudioClip>("Sound/MusicEx/MusicEx_Welcome", (AudioClip clip) => {
                SoundMgr.current.PlayMusic(clip);
            });
        }

        public override void Exit() {
            base.Exit();
            _ctx.EnqueueRenderQueue(RenderExit);
        }

        protected void RenderExit() {
            SoundMgr.current.StopMusic();
        }

        public void First(SprotoTypeBase responseObj) {
            C2sSprotoType.first.response obj = responseObj as C2sSprotoType.first.response;
            if (_service == null) {
                _service = _ctx.QueryService<InitService>(InitService.Name);
            }
            User u = _service.User;
            u.Name = obj.name;
            u.NameId = obj.nameid;
            u.RCard = obj.rcard;

            _service.Board = obj.board;
            _service.Adver = obj.adver;

            _mui.SetupFirst();
        }

        public void FetchSysmail(SprotoTypeBase responseObj) {
            C2sSprotoType.fetchsysmail.response obj = responseObj as C2sSprotoType.fetchsysmail.response;
            if (_service == null) {
                _service = _ctx.QueryService<InitService>(InitService.Name);
            }

            SysInbox sib = _service.SysInBox;
            for (int i = 0; i < obj.inbox.Count; i++) {
                var mail = sib.CreateMail();
                mail.Id = obj.inbox[i].id;
                mail.DateTime = obj.inbox[i].datetime;
                mail.Title = obj.inbox[i].title;
                mail.Content = obj.inbox[i].content;
                sib.Add(mail);
            }
        }

        public void OnSendMatch(EventCmd e) {
            if (((AppConfig)_ctx.Config).VTYPE == AppConfig.VERSION_TYPE.TEST) {
                _ctx.Push(typeof(GameController));
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

            uint provice = (uint)e.Msg[CrCode.provice];
            if (provice == Provice.Sichuan) {
                request.provice = Provice.Sichuan;
                request.ju = (int)e.Msg[CrCode.ju];
                request.sc = new C2sSprotoType.crsc();
                request.sc.hujiaozhuanyi = (int)e.Msg[CrCode.hujiaozhuanyi];
                request.sc.zimo = (int)e.Msg[CrCode.zimo];
                request.sc.dianganghua = (int)e.Msg[CrCode.dianganghua];
                request.sc.daiyaojiu = (int)e.Msg[CrCode.daiyaojiu];
                request.sc.duanyaojiu = (int)e.Msg[CrCode.duanyaojiu];
                request.sc.jiangdui = (int)e.Msg[CrCode.jiangdui];
                request.sc.tiandihu = (int)e.Msg[CrCode.tiandihu];
                request.sc.top = (int)e.Msg[CrCode.tiandihu];
            } else if (provice == Provice.Shaanxi) {
                request.provice = Provice.Shaanxi;
                request.ju = (int)e.Msg[CrCode.ju];
                request.sx = new C2sSprotoType.crsx();
                request.sx.huqidui = (int)e.Msg[CrCode.sxqidui];
                request.sx.qingyise = (int)e.Msg[CrCode.sxqingyise];
            }

            _ctx.SendReq<C2sProtocol.create>(C2sProtocol.create.Tag, request);
        }

        public void OnSendJoin(EventCmd e) {
            int roomid = (int)e.Msg["roomid"];
            GameService service = _ctx.QueryService<GameService>(GameService.Name);

            C2sSprotoType.join.request request = new C2sSprotoType.join.request();
            request.roomid = roomid;

            _ctx.SendReq<C2sProtocol.join>(C2sProtocol.join.Tag, request);
        }

        public void OnSendMsg(EventCmd e) {
            _ctx.SendReq<C2sProtocol.fetchsysmail1>(C2sProtocol.fetchsysmail1.Tag, null);
        }

        public void OnSendViewMail(EventCmd e) {

        }

        public void FetchSysmail1(SprotoTypeBase responseObj) {
            C2sSprotoType.fetchsysmail1.response obj = responseObj as C2sSprotoType.fetchsysmail1.response;
            if (_service == null) {
                _service = _ctx.QueryService<InitService>(InitService.Name);
            }

            //SysInbox sib = _service.SysInBox;
            //for (int i = 0; i < obj.inbox.Count; i++) {
            //    var mail = sib.CreateMail();
            //    mail.Id = obj.inbox[i].id;
            //    mail.DateTime = obj.inbox[i].datetime;
            //    mail.Title = obj.inbox[i].title;
            //    mail.Content = obj.inbox[i].content;
            //    sib.Add(mail);
            //}
            _mui.SetupMsg();
        }

        public void Records(SprotoTypeBase responseObj) {

        }

        public void Record(SprotoTypeBase responseObj) {

        }

    }
}
