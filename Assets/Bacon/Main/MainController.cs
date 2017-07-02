using System.Collections.Generic;
using Maria;
using Sproto;
using UnityEngine;
using XLua;
using Bacon.Service;
using Bacon.Game;
using Bacon.Helper;
using Bacon.Event;

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
        private string _tipscontent = string.Empty;

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
            _ctx.EnqueueRenderQueue(RenderWaiting);
        }

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

        private void RenderExit() {
            SoundMgr.current.StopMusic();
        }

        public void SetupUI(EventCmd e) {
            _uiroot = e.Orgin;
            _ctx.SendReq<C2sProtocol.first>(C2sProtocol.first.Tag, null);
            _ctx.SendReq<C2sProtocol.fetchsysmail>(C2sProtocol.fetchsysmail.Tag, null);
            _ctx.SendReq<C2sProtocol.records>(C2sProtocol.records.Tag, null);
            _ctx.EnqueueRenderQueue(RenderSetupUI);
        }

        public void RenderSetupUI() {
            ABLoader.current.LoadAssetAsync<AudioClip>("Sound/MusicEx", "MusicEx_Welcome", (AudioClip clip) => {
                SoundMgr.current.PlayMusic(clip);
            });
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
            u.Sex = obj.sex;

            _service.Board = obj.board;
            _service.Adver = obj.adver;

            _ctx.EnqueueRenderQueue(RenderFirst);
        }

        public void RenderFirst() {
            MUIRoot com = _uiroot.GetComponent<global::MUIRoot>();
            com.SetBoard(_service.Board);
            com.SetAdver(_service.Adver);
            var title = com._Title.GetComponent<Title>();
            title.SetName(_service.User.Name);
            string nameid = string.Format("ID:{0}", _service.User.NameId);
            title.SetNameId(nameid);

            string rcard = string.Format("{0}", _service.User.RCard);
            title.SetRCard(rcard);

            if (_role == null) {
                if (_service.User.Sex == 1) {
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

        public static void test() {
            UnityEngine.Debug.Log("abc");
        }

        public void FetchSysmail(SprotoTypeBase responseObj) {
            C2sSprotoType.fetchsysmail.response obj = responseObj as C2sSprotoType.fetchsysmail.response;
            if (_service == null) {
                _service = _ctx.QueryService<InitService>(InitService.Name);
            }

            SysInbox sib = _service.SysInBox;
            sib.Clear();
            for (int i = 0; i < obj.inbox.Count; i++) {
                var mail = sib.CreateMail();
                mail.Id = obj.inbox[i].id;
                mail.DateTime = obj.inbox[i].datetime;
                mail.Title = obj.inbox[i].title;
                mail.Content = obj.inbox[i].content;
                sib.Add(mail);
            }
            // 更新红点
            _ctx.EnqueueRenderQueue(RenderFetchSysmail);
        }

        public void FetchRecords(SprotoTypeBase responseObj) {
            C2sSprotoType.records.response obj = responseObj as C2sSprotoType.records.response;
            for (int i = 0; i < obj.records.Count; i++) {
                Record record = new Bacon.Record();
                record.Id = obj.records[i].id;
                record.DateTime = obj.records[i].datetime;
                record.Player1 = obj.records[i].player1;
                record.Player2 = obj.records[i].player2;
                record.Player3 = obj.records[i].player3;
                record.Player4 = obj.records[i].player4;
                _service.RecordMgr.Add(record);
            }
            // 更新红点
            _ctx.EnqueueRenderQueue(RenderFetchSysmail);
        }



        public void RenderFetchSysmail() {
            var com = _uiroot.GetComponent<MUIRoot>();
            var title = com._Title.GetComponent<Title>();
            title.SetMsgRed(_service.SysInBox.Count);
        }

        private void OnShowCreate(EventCmd e) {
            _ctx.EnqueueRenderQueue(RenderShowCreate);
        }

        public void RenderShowCreate() {
            _uiroot.GetComponent<MUIRoot>().ShowCreate((int)_service.User.RCard);
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

        public void OnSendMsg(EventCmd e) {
            List<long> mailids = new List<long>();
            if (_service.SysInBox.Count > 0) {
                foreach (var item in _service.SysInBox) {
                    mailids.Add(item.Id);
                }
            }
            C2sSprotoType.syncsysmail.request request = new C2sSprotoType.syncsysmail.request();
            request.all = mailids;
            _ctx.SendReq<C2sProtocol.syncsysmail>(C2sProtocol.syncsysmail.Tag, request);
        }

        public void SyncSysmail(SprotoTypeBase responseObj) {
            C2sSprotoType.syncsysmail.response obj = responseObj as C2sSprotoType.syncsysmail.response;
            _service.SysInBox.Clear();
            if (obj.inbox.Count > 0) {
                for (int i = 0; i < obj.inbox.Count; i++) {
                    var mail = _service.SysInBox.CreateMail();
                    mail.Id = obj.inbox[i].id;
                    mail.Title = obj.inbox[i].title;
                    mail.DateTime = obj.inbox[i].datetime;
                    mail.Content = obj.inbox[i].content;
                    _service.SysInBox.Add(mail);
                }
            }
            // 显示邮件
            _ctx.EnqueueRenderQueue(RenderSyncSysMail);
        }

        public void RenderSyncSysMail() {
            var com = _uiroot.GetComponent<MUIRoot>();
            var msgwnd = com._MailWnd.GetComponent<MailWnd>();
            msgwnd.ShowSysMsg(_service.SysInBox);
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

        private void RenderViewMail() {
            if (_curtype == MsgItem.Type.Sys) {
                Sysmail mail = _service.SysInBox.GetMail(_curmsgid);
                var com = _uiroot.GetComponent<MUIRoot>();
                var mailwnd = com._MailWnd.GetComponent<MailWnd>();
                mailwnd._InfoPage.GetComponent<MsgItemInfo>().Show(_curtype, _curmsgid, mail.Title, mail.Content);
            }
        }

        public void OnViewedMail(EventCmd e) {
            MsgItem.Type type = e.Msg.GetField<MsgItem.Type>("type");
            long id = e.Msg.GetField<long>("id");
            _curmsgid = id;
            _curtype = type;
            Sysmail mail = _service.SysInBox.GetMail(id);
            _service.SysInBox.Remove(mail);

            C2sSprotoType.viewedsysmail.request request = new C2sSprotoType.viewedsysmail.request();
            request.mailid = id;
            _ctx.SendReq<C2sProtocol.viewedsysmail>(C2sProtocol.viewedsysmail.Tag, request);

            _ctx.EnqueueRenderQueue(RenderSyncSysMail);
        }

        public void RenderViewedMail() { }

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

        public void OnSendJoin(EventCmd e) {
            int roomid = (int)e.Msg["roomid"];
            GameService service = _ctx.QueryService<GameService>(GameService.Name);

            C2sSprotoType.join.request request = new C2sSprotoType.join.request();
            request.roomid = roomid;
            _ctx.SendReq<C2sProtocol.join>(C2sProtocol.join.Tag, request);
        }

        public void OnSendRecords(EventCmd e) {

        }

        public void Records(SprotoTypeBase responseObj) {

        }

        public void Record(SprotoTypeBase responseObj) {

        }

        public void OnLogout(EventCmd e) {
            _ctx.Logined = false;
            _ctx.SendReq<C2sProtocol.logout>(C2sProtocol.logout.Tag, null);
        }

        public void Logout(SprotoTypeBase responseObj) {
            _ctx.Pop();
        }

        public override void Logout() {
            _ctx.Pop();
        }

        public void ShowTips(string content) {
            _tipscontent = content;
            _ctx.EnqueueRenderQueue(RenderShowTips);
        }

        private void RenderShowTips() {
            _uiroot.GetComponent<MUIRoot>().ShowTips(_tipscontent);
        }
    }
}
