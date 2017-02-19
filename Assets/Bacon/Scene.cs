using Maria;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    public class Scene : Actor {
        private Matrix4x4 _mat = Matrix4x4.identity;

        private View _view = null;
        private Map _map = null;
        private Dictionary<long, Card> _ballidBalls = new Dictionary<long, Card>();

        public Scene(Context ctx, Controller controller, GameObject go) : base(ctx, controller, go) {

            _view = null;
            _map = null;

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_CARD, SetupCard);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

        }

        public override void Update(float delta) {
            base.Update(delta);

            //Ball myball = _sessionBalls[_mysession];
            //Debug.Assert(myball != null);
            //foreach (var item in _balls) {
            //    if (myball.AABB.intersects(item.AABB)) {

            //    }
            //}
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




    }
}
