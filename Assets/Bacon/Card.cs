using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon {
    class Card : Maria.Actor {

        protected long _value;
        protected int _type;
        protected int _num;
        protected int _idx;

        public Card(Context ctx, Controller controller, GameObject go)
            : base(ctx, controller, go){
        }

        public long Value { set; get; }
        public int Type { set; get; }
        public int Num { set; get; }
        public int Idx { set; get; }



    }
}
