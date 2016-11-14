using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
    public class Player : Actor {
        public enum Orientation {
        }

        private uint _uid;
        private Orientation _orient;

        public Player(Context ctx, Controller controller)
            : base(ctx, controller){

        }
        
    }
}
