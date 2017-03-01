using Maria;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    public class Scene : Actor {

        private View _view = null;
        private Map _map = null;

        public Scene(Context ctx, Controller controller, GameObject go) : base(ctx, controller, go) {
        }

        public View View { get { return _view; } set { _view = value; } }

        public Map Map { get { return _map; } set { _map = value; } }

        public View SetupView(GameObject go) {
            _view = new View(_ctx, _controller, go, this);
            return _view;
        }

        public Map SetupMap(GameObject go) {
            _map = new Map(_ctx, _controller, go, this);
            return _map;
        }

        public void SetupCard(EventCmd e) {
        }

        public void SetupPlayer() {
            _ctx.EnqueueRenderQueue(RenderSetupPlayer);
        }

        public void RenderSetupPlayer() {
            _go.GetComponent<GameRoot>().SetupPlayer();
        }
    }
}
