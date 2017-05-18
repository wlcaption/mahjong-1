using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using UnityEngine;
using Bacon.Service;
using Bacon.Event;

namespace Bacon.Game {
    class GUIRootActor : Actor {

        public GUIRootActor(Context ctx, Controller controller) : base(ctx, controller) {
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_GUIROOT, SetupGuiRoot);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_RESTART, OnSendRestart);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);

            EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_GAME_OPENSETTING, OnOpenSetting);
            _ctx.EventDispatcher.AddCmdEventListener(listener3);

            EventListenerCmd listener4 = new EventListenerCmd(MyEventCmd.EVENT_GAME_INVITE, OnInvite);
            _ctx.EventDispatcher.AddCmdEventListener(listener4);
        }

        private void SetupGuiRoot(EventCmd e) {
            _go = e.Orgin;
        }

        public void RenderRoomId(int roomid) {
            _go.GetComponent<GUIRoot>().InitUI(roomid);
        }

        public void ShowOver() {
            _ctx.EnqueueRenderQueue(RenderShowOver);
        }

        public void RenderShowOver() {
            _go.GetComponent<GUIRoot>().ShowOver();
        }

        public void CloseOver() {
            _ctx.EnqueueRenderQueue(RenderCloseOver);
        }

        private void RenderCloseOver() {
            _go.GetComponent<GUIRoot>().CloseOver();
        }

        private void OnSendRestart(EventCmd e) {
            UnityEngine.Debug.LogFormat("send restart.");
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            C2sSprotoType.restart.request request = new C2sSprotoType.restart.request();
            request.idx = service.MyIdx;
            _ctx.SendReq<C2sProtocol.restart>(C2sProtocol.restart.Tag, request);
        }

        private void OnOpenSetting(EventCmd e) {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            _ctx.EnqueueRenderQueue(RenderOpenSetting);
        }

        private void RenderOpenSetting() {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            if (service.Host) {
                _go.GetComponent<GUIRoot>().ShowSetting(SettingWnd.ExitType.JIESHAN_ROOM);
            } else {
                _go.GetComponent<GUIRoot>().ShowSetting(SettingWnd.ExitType.EXIT_ROOM);
            }
        }

        private void OnInvite(EventCmd e) {
            _ctx.EnqueueRenderQueue(RenderInvite);
        }

        private void RenderInvite() {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
#elif UNITY_IOS || UNITY_ANDROID
        try {
                AndroidJavaClass c = new AndroidJavaClass("com.emberfarkas.mahjong.wxapi.WXEntryActivity");
                AndroidJavaObject o = c.GetStatic<AndroidJavaObject>("currentWXActivity");
                o.Call("ahareApp", service.RoomId);
            } catch (System.Exception ex) {
                UnityEngine.Debug.LogException(ex);
            }
#endif
        }
    }
}
