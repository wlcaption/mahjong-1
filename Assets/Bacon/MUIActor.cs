using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using UnityEngine;

namespace Bacon {
    class MUIActor : Actor {
        private InitService _service = null;
        private bool _first = false;

        public MUIActor(Context ctx, Controller controller) : base(ctx, controller) {
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_MUI, SetupUI);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        public void SetupUI(EventCmd e) {
            _go = e.Orgin;
            if (_service == null) {
                _service = (InitService)_ctx.QueryService(InitService.Name);
            }
            if (_first) {
                _ctx.EnqueueRenderQueue(RenderSetupUI);
            }
        }

        public void SetupFirst() {
            _first = true;
            if (_first && _go != null) {
                _ctx.EnqueueRenderQueue(RenderSetupUI);
            }
        }

        private void RenderSetupUI() {
            if (_service == null) {
                _service = (InitService)_ctx.QueryService(InitService.Name);
            }
            MUIRoot com = _go.GetComponent<global::MUIRoot>();
            com.SetBoard(_service.Board);
            com.SetAdver(_service.Adver);
            com.SetName(_service.User.Name);
            string nameid = string.Format("ID:{0}", _service.User.NameId);
            com.SetNameId(nameid);
            string rcard = string.Format("{0}", _service.User.RCard);
            com.SetRCard(rcard);
        }

        public void SetupMsg() {
            _ctx.EnqueueRenderQueue(RenderSettupMsg);
        }

        private void RenderSettupMsg() {
            //MUIMsg com = _go.GetComponent<global::MUIMsg>();
            //com.ShowMsg(_service.SysInBox);
        }
    }
}
