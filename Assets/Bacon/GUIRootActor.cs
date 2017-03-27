using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;

namespace Bacon {
    class GUIRootActor : Actor {

        public GUIRootActor(Context ctx, Controller controller) :base(ctx, controller) {
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_GUIROOT, SetupGuiRoot);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_RESTART, OnSendRestart);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);
        }

        private void SetupGuiRoot(EventCmd e) {
            _go = e.Orgin;
            _ctx.EnqueueRenderQueue(RenderRoomId);
        }

        private void RenderRoomId() {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            _go.GetComponent<GUIRoot>().InitUI((int)service.RoomId);
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

    }
}
