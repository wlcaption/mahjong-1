using System.Collections.Generic;
using Maria;
using Sproto;
using UnityEngine;
using XLua;
using Bacon.Service;
using Bacon.Game;
using Bacon.Helper;
using Bacon.Event;
using Bacon.Model;

namespace Bacon {

    [Hotfix]
    [LuaCallCSharp]
    class MainController : Controller {
        private InitService _service = null;
        private GameObject _uiroot = null;
        private GameObject _role = null;
        private GameObject _waiting = null;

        private long _curmsgid;
        private MsgItem.Type _curtype;

        public MainController(Context ctx) : base(ctx) {
            _name = "main";

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_MUI_CREATE, OnSendCreate);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_MUI_JOIN, OnSendJoin);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);

            EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_MUI_MSG, OnSendMsg);
            _ctx.EventDispatcher.AddCmdEventListener(listener3);

            EventListenerCmd listener4 = new EventListenerCmd(MyEventCmd.EVENT_MUI_VIEWMAIL, OnSendViewMail);
            _ctx.EventDispatcher.AddCmdEventListener(listener4);

            EventListenerCmd listener5 = new EventListenerCmd(MyEventCmd.EVENT_MUI_VIEWEDMAIL, OnViewedMail);
            _ctx.EventDispatcher.AddCmdEventListener(listener5);

            EventListenerCmd listener6 = new EventListenerCmd(MyEventCmd.EVENT_MUI_MSGCLOSED, OnMsgClosed);
            _ctx.EventDispatcher.AddCmdEventListener(listener6);

            EventListenerCmd listener7 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_MUI, SetupUI);
            _ctx.EventDispatcher.AddCmdEventListener(listener7);

            EventListenerCmd listener8 = new EventListenerCmd(MyEventCmd.EVENT_MUI_SHOWCREATE, OnShowCreate);
            _ctx.EventDispatcher.AddCmdEventListener(listener8);

            EventListenerCmd listener9 = new EventListenerCmd(MyEventCmd.EVENT_MUI_EXITLOGIN, OnLogout);
            _ctx.EventDispatcher.AddCmdEventListener(listener9);

        }

        public override void OnEnter() {
            base.OnEnter();
            if (_service == null) {
                _service = _ctx.QueryService<InitService>(InitService.Name);
            }
            SMActor actor = _service.SMActor;
            actor.LoadScene(_name);
        }

        public override void OnExit() {
            base.OnExit();
            _ctx.EnqueueRenderQueue(RenderExit);
        }

        public override void OnGateAuthed(int code) {
            base.OnGateAuthed(code);
            _ctx.EnqueueRenderQueue(RenderCancelWaiting);
        }

        public override void OnGateDisconnected() {
            base.OnGateDisconnected();
            UnityEngine.Debug.LogFormat("main controller gate diconnented, start connect ...");
            _ctx.EnqueueRenderQueue(RenderWaiting);
        }

        public override void Logout() {
            _ctx.Pop();
        }

        #region event
        public void SetupUI(EventCmd e) {
            _uiroot = e.Orgin;

            AppContext ctx = _ctx as AppContext;
            EntityMgr mgr = ctx.GetEntityMgr();
            Entity entity = mgr.MyEntity;
            entity.GetComponent<Bacon.Model.UComUser>().Fetch();

            _ctx.SendReq<C2sProtocol.fetchsysmail>(C2sProtocol.fetchsysmail.Tag, null);
            _ctx.SendReq<C2sProtocol.records>(C2sProtocol.records.Tag, null);

            
            ctx.GetBoardMgr().FetchAdver();
            ctx.GetBoardMgr().FetchBoard();

            _ctx.EnqueueRenderQueue(RenderSetupUI);
        }

        private void OnShowCreate(EventCmd e) {
            _ctx.EnqueueRenderQueue(RenderShowCreate);
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

        public void OnSendMsg(EventCmd e) {
            //List<long> mailids = new List<long>();
            //if (_service.SysInBox.Count > 0) {
            //    foreach (var item in _service.SysInBox) {
            //        mailids.Add(item.Id);
            //    }
            //}
            //C2sSprotoType.syncsysmail.request request = new C2sSprotoType.syncsysmail.request();
            //request.all = mailids;
            //_ctx.SendReq<C2sProtocol.syncsysmail>(C2sProtocol.syncsysmail.Tag, request);
        }

        public void OnSendViewMail(EventCmd e) {
            MsgItem.Type type = e.Msg.GetField<MsgItem.Type>("type");
            long id = e.Msg.GetField<long>("id");
            _curmsgid = id;
            _curtype = type;

            _ctx.EnqueueRenderQueue(RenderViewMail);
            //var mailwnd = com._MailWnd.GetComponent<MailWnd>();
            //if ((MsgItem.Type)e.Msg["type"] == MsgItem.Type.Sys) {
            //    Sysmail mail = _service.SysInBox.GetMail((long)e.Msg["id"]);
            //    mailwnd.ShowMailInfo()
            //}

            //_service.SysInBox
            //mailwnd.ShowMailInfo()
            //C2sSprotoType.syncsysmail.request request = new C2sSprotoType.syncsysmail.request();
            //request.all = mailids;
            //_ctx.SendReq<C2sProtocol.syncsysmail>(C2sProtocol.syncsysmail.Tag, request);
        }

        public void OnViewedMail(EventCmd e) {
            //MsgItem.Type type = e.Msg.GetField<MsgItem.Type>("type");
            //long id = e.Msg.GetField<long>("id");
            //_curmsgid = id;
            //_curtype = type;
            //Sysmail mail = _service.SysInBox.GetMail(id);
            //_service.SysInBox.Remove(mail);

            //C2sSprotoType.viewedsysmail.request request = new C2sSprotoType.viewedsysmail.request();
            //request.mailid = id;
            //_ctx.SendReq<C2sProtocol.viewedsysmail>(C2sProtocol.viewedsysmail.Tag, request);

            //_ctx.EnqueueRenderQueue(RenderSyncSysMail);
        }

        public void OnSendJoin(EventCmd e) {
            int roomid = (int)e.Msg["roomid"];
            GameService service = _ctx.QueryService<GameService>(GameService.Name);

            C2sSprotoType.join.request request = new C2sSprotoType.join.request();
            request.roomid = roomid;
            _ctx.SendReq<C2sProtocol.join>(C2sProtocol.join.Tag, request);
        }

        public void OnMsgClosed(EventCmd e) {
            _ctx.EnqueueRenderQueue(RenderFetchSysmail);
        }

        public void OnSendCreate(EventCmd e) {
            C2sSprotoType.create.request request = new C2sSprotoType.create.request();

            int provice = (int)e.Msg[CrCode.provice];
            if (provice == Provice.Sichuan) {
                request.provice = Provice.Sichuan;
                request.ju = (int)e.Msg[CrCode.ju];
                request.overtype = e.Msg.GetField<int>(CrCode.overtype);
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
                request.overtype = e.Msg.GetField<int>(CrCode.overtype);
                request.sx = new C2sSprotoType.crsx();
                request.sx.huqidui = (int)e.Msg[CrCode.sxqidui];
                request.sx.qingyise = (int)e.Msg[CrCode.sxqingyise];
            }

            _ctx.SendReq<C2sProtocol.create>(C2sProtocol.create.Tag, request);
        }

        public void OnSendRecords(EventCmd e) {

        }

        public void OnLogout(EventCmd e) {
            _ctx.Logined = false;
            _ctx.SendReq<C2sProtocol.logout>(C2sProtocol.logout.Tag, null);
        }
        #endregion

        #region request
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
        #endregion

        #region response
        public void First(SprotoTypeBase responseObj) {
            EntityMgr mgr = ((AppContext)_ctx).GetEntityMgr();
            mgr.MyEntity.GetComponent<Bacon.Model.UComUser>().First(responseObj);
        }

        public void FetchSysmail(SprotoTypeBase responseObj) {
            EntityMgr mgr = ((AppContext)_ctx).GetEntityMgr();
            mgr.MyEntity.GetComponent<UComSysInbox>().FetchSysmail(responseObj);
            C2sSprotoType.fetchsysmail.response obj = responseObj as C2sSprotoType.fetchsysmail.response;
            if (_service == null) {
                _service = _ctx.QueryService<InitService>(InitService.Name);
            }
        }

        public void FetchRecords(SprotoTypeBase responseObj) {

            AppContext ctx = _ctx as AppContext;
            EntityMgr mgr = ctx.GetEntityMgr();
            Entity entity = mgr.MyEntity;
            entity.GetComponent<UComRecordMgr>().FetchRecords(responseObj);
        }

        public void Match(SprotoTypeBase responseObj) {
            C2sSprotoType.match.response obj = responseObj as C2sSprotoType.match.response;
            UnityEngine.Debug.Assert(obj.errorcode == Errorcode.SUCCESS);
        }

        public void SyncSysmail(SprotoTypeBase responseObj) {
            //C2sSprotoType.syncsysmail.response obj = responseObj as C2sSprotoType.syncsysmail.response;
            //_service.SysInBox.Clear();
            //if (obj.inbox.Count > 0) {
            //    for (int i = 0; i < obj.inbox.Count; i++) {
            //        var mail = _service.SysInBox.CreateMail();
            //        mail.Id = obj.inbox[i].id;
            //        mail.Title = obj.inbox[i].title;
            //        mail.DateTime = obj.inbox[i].datetime;
            //        mail.Content = obj.inbox[i].content;
            //        _service.SysInBox.Add(mail);
            //    }
            //}
            //// 显示邮件
            //_ctx.EnqueueRenderQueue(RenderSyncSysMail);
        }

        public void Records(SprotoTypeBase responseObj) {

        }

        public void Record(SprotoTypeBase responseObj) {

        }

        public void Logout(SprotoTypeBase responseObj) {
            _ctx.Pop();
        }

        public void Adver(SprotoTypeBase responseObj) {
            AppContext ctx = _ctx as AppContext;
            ctx.GetBoardMgr().Adver(responseObj);
        }

        public void Board(SprotoTypeBase responseObj) {
            AppContext ctx = _ctx as AppContext;
            ctx.GetBoardMgr().Board(responseObj);
        }
        #endregion

        #region render

        private void RenderExit() {
            SoundMgr.current.StopMusic();
        }

        public void RenderSetupUI() {
            ABLoader.current.LoadAssetAsync<AudioClip>("Sound/MusicEx", "MusicEx_Welcome", (AudioClip clip) => {
                SoundMgr.current.PlayMusic(clip);
            });
        }

        public void RenderFetchSysmail() {
            var com = _uiroot.GetComponent<MUIRoot>();
            var title = com._Title.GetComponent<Title>();
            //title.SetMsgRed(_service.SysInBox.Count);
        }

        public void RenderShowCreate() {
            //_uiroot.GetComponent<MUIRoot>().ShowCreate((int)_service.User.RCard);
        }

        public void RenderSyncSysMail() {
            var com = _uiroot.GetComponent<MUIRoot>();
            var msgwnd = com._MailWnd.GetComponent<MailWnd>();
            //msgwnd.ShowSysMsg(_service.SysInBox);
        }

        private void RenderViewMail() {
            //if (_curtype == MsgItem.Type.Sys) {
            //    Sysmail mail = _service.SysInBox.GetMail(_curmsgid);
            //    var com = _uiroot.GetComponent<MUIRoot>();
            //    var mailwnd = com._MailWnd.GetComponent<MailWnd>();
            //    mailwnd._InfoPage.GetComponent<MsgItemInfo>().Show(_curtype, _curmsgid, mail.Title, mail.Content);
            //}
        }

        public void RenderViewedMail() { }

        private void RenderCancelWaiting() {
            if (_uiroot && _waiting) {
                _waiting.SetActive(false);
            }
        }

        private void RenderWaiting() {
            if (_uiroot) {
                MUIRoot com = _uiroot.GetComponent<global::MUIRoot>();
                ABLoader.current.LoadAssetAsync<GameObject>("Prefabs/Common", "Waiting", (GameObject go) => {
                    _waiting = go;
                    go.transform.SetParent(com._Extra.transform);
                });
            }
        }

        private void RenderShowTips() {
            //_uiroot.GetComponent<MUIRoot>().ShowTips(_tipscontent);
        }

        public void RenderAdver() {
            BoardMgr mgr = ((AppContext)_ctx).GetBoardMgr();

            MUIRoot muiroot = _uiroot.GetComponent<MUIRoot>();
            muiroot.SetAdver(mgr.AdverMsg);
        }

        public void RenderBoard() {
            BoardMgr mgr = ((AppContext)_ctx).GetBoardMgr();

            MUIRoot muiroot = _uiroot.GetComponent<MUIRoot>();
            muiroot.SetBoard(mgr.BoardMsg);
        }

        public void RenderFirst() {

            MUIRoot com = _uiroot.GetComponent<global::MUIRoot>();
            var title = com._Title.GetComponent<Title>();

            AppContext ctx = _ctx as AppContext;
            EntityMgr mgr = ctx.GetEntityMgr();
            Entity entity = mgr.MyEntity;
            UComUser u = entity.GetComponent<Bacon.Model.UComUser>();

            title.SetName(u.Name);
            string nameid = string.Format("ID:{0}", u.NameId);
            title.SetNameId(nameid);

            string rcard = string.Format("{0}", u.RCard);
            title.SetRCard(rcard);

            if (_role == null) {
                if (u.Sex == 1) {
                    ABLoader.current.LoadAssetAsync<GameObject>("Prefabs/Role", "boy", (GameObject go) => {
                        _role = GameObject.Instantiate<GameObject>(go);
                        _role.transform.SetParent(_uiroot.transform);
                        _role.transform.localPosition = new Vector3(0.0f, -1.13f, 2.3f);
                        _role.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up);
                    });
                } else {
                    ABLoader.current.LoadAssetAsync<GameObject>("Prefabs/Role", "girl", (GameObject go) => {
                        _role = GameObject.Instantiate<GameObject>(go);
                        _role.transform.SetParent(_uiroot.transform);
                        _role.transform.localPosition = new Vector3(0.0f, -1.13f, 2.3f);
                        _role.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up);
                    });
                }
            }

        }
        #endregion
    }
}
