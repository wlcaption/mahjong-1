using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using UnityEngine;

namespace Bacon {
    public class Desk : Actor {

        private float _width = 2.0f;
        private float _length = 2.0f;
        private float _height = 2.0f;
        private int _clockleft = 0;

        public Desk(Context ctx, Controller controller, GameObject go)
            : base(ctx, controller, go) {
        }

        public float Width { get { return _width; } }
        public float Length { get { return _length; } }
        public float Height { get { return _height; } }

        public void UpdateClock(int left) {
            _clockleft = left;
            _ctx.EnqueueRenderQueue(RenderUpdateClock);
        }

        protected void RenderUpdateClock() {
            GameObject clock = _go.transform.FindChild("Clock").gameObject;
            if (clock) {
                clock.SetActive(true);
                clock.GetComponent<TextMesh>().text = string.Format("{0}", _clockleft);
            }
        }
    }
}
