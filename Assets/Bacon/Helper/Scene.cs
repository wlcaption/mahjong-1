using Maria;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    public class Scene : Actor {

        public Scene(Context ctx, Controller controller, GameObject go) : base(ctx, controller, go) {
        }

        public void SetupBottomPlayer() {
            _ctx.EnqueueRenderQueue(RenderSetupBottomPlayer);
        }

        public void RenderSetupBottomPlayer() {
            _go.GetComponent<GameRoot>().SetupBottomPlayer();
        }

        public void SetupLeftPlayer() {
            _ctx.EnqueueRenderQueue(RenderSetupLeftPlayer);
        }

        private void RenderSetupLeftPlayer() {
            _go.GetComponent<GameRoot>().SetupLeftPlayer();
        }

        public void SetupTopPlayer() {
            _ctx.EnqueueRenderQueue(RenderSetupTopPlayer);
        }
        private void RenderSetupTopPlayer() {
            _go.GetComponent<GameRoot>().SetupTopPlayer();
        }

        public void SetupRightPlayer() {
            _ctx.EnqueueRenderQueue(RenderSetupRightPlayer);
        }
        private void RenderSetupRightPlayer() {
            _go.GetComponent<GameRoot>().SetupRightPlayer();
        }

    }
}
