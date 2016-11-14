using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon {
    public class Card : Actor {
        private int _value;
        private int _type;
        private int _num;
        private int _idx;

        public Card(Context ctx, Controller controller, int value)
            : base(ctx, controller) {
            _value = value;
            _ctx.EnqueueRenderQueue(RenderInitCard);
        }

        public void SetIdx(int idx) {
            _idx = idx;
        }

        public int GetIdx() {
            return _idx;
        }

        public void SetPosition() {
        }

        private void RenderInitCard() {

            string path = "Prefabs/App/";
            string pathname = path + string.Format("");

            UnityEngine.Object o = Resources.Load(pathname, typeof(GameObject));
            _go = GameObject.Instantiate(o) as GameObject;
        }
    }
}
