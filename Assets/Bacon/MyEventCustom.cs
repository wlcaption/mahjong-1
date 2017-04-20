using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
    public class MyEventCustom : EventCustom {

        public static readonly string LOGOUT = "LOGOUT";

        public MyEventCustom(string name)
            : base(name) {
        }

        public MyEventCustom(string name, object ud) :
            base(name, ud) {
        }
    }
}
