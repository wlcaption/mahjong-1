using Maria;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    class StartActor : Actor {

        public StartActor(Context ctx, Controller controller) : base(ctx, controller) {
            EventListenerCmd listener1 = new EventListenerCmd(Bacon.MyEventCmd.EVENT_SETUP_STARTROOT, SetupStartRoot);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(Bacon.MyEventCmd.EVENT_UPdATERES, CountdownCb);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);
        }

        private void SetupStartRoot(EventCmd e) {
            _go = e.Orgin;
            if (((AppConfig)_ctx.Config).UpdateRes) {
                _ctx.EnqueueRenderQueue(RenderUpdateRes);
            } else {
                _ctx.Push(typeof(LoginController));
            }
        }

        public void RenderUpdateRes() {
            _go.GetComponent<StartBehaviour>().UpdateRes();
        }

        private void CountdownCb(EventCmd e) {
            _ctx.Push(typeof(LoginController));
        }

    }
}
