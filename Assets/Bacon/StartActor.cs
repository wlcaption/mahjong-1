using Maria;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    class StartActor : Actor {

        public StartActor(Context ctx, Controller controller) : base(ctx, controller) {
            EventListenerCmd listener1 = new EventListenerCmd(Bacon.MyEventCmd.EVENT_SETUP_STARTROOT, SetupStartRoot);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        private void SetupStartRoot(EventCmd e) {
            _go = e.Orgin;
            _ctx.Countdown("startcontroller", 2, CountdownCb);

        }

        private void CountdownCb() {
            _ctx.Push("login");
        }

        private void OnRecv(byte[] buffer, int start, int len) {
            for (int i = 0; i < len; i++) {
                UnityEngine.Debug.Log(string.Format("{0}", buffer[i]));
            }
        }
    }
}
