using Maria;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    public class Scene : Actor {

        private View _view = null;
        private Map _map = null;

        public Scene(Context ctx, Controller controller, GameObject go) : base(ctx, controller, go) {
        }

        public void SetupPlayer() {
            _ctx.EnqueueRenderQueue(RenderSetupPlayer);
        }

        public void RenderSetupPlayer() {
            _go.GetComponent<GameRoot>().SetupPlayer();
        }
    }
}
