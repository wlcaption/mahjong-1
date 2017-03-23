using Maria;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    public class Scene : Actor {

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
