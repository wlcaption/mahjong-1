using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Game {
    public class CallInfo {
        public class HuInfo {
            public long Card { get; set; }
            public long Code { get; set; }
            public long Gang { get; set; }
            public long Jiao { get; set; }
            public long Dian { get; set; }
        }

        public long Card { get; set; }
        public long Dian { get; set; }
        public long Peng { get; set; }
        public long Gang { get; set; }
        public HuInfo Hu { get; set; }
    }
}
