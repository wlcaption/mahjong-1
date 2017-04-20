using Maria;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Bacon {
    public class LoginActor : Actor {
        public LoginActor(Context ctx, Controller controller) : base(ctx, controller) {
            EventListenerCmd listener1 = new EventListenerCmd(EventCmd.EVENT_LOGIN, Login);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(EventCmd.EVENT_SETUP_LOGINPANEL, SetupLoginPanel);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);
        }

        public void SetupLoginPanel(EventCmd e) {
            _go = e.Orgin;
        }

        public void Login(EventCmd e) {
            
            Message msg = e.Msg;
            string server = (string)msg["server"];
            string username = (string)msg["username"];
            string password = (string)msg["password"];
            LoginController controller = _controller as LoginController;
            controller.LoginAuth(server, username, password);
        }

        public void EnableCommitOk() {
            _ctx.EnqueueRenderQueue(RenderEnableCommitOk);
        }

        private void RenderEnableCommitOk() {
            var com = _go.GetComponent<LoginPanelBehaviour>();
            com.EnableCommitOk();
        }
    }
}
