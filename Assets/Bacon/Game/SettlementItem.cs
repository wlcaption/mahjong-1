using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Game {
    public class SettlementItem {
        public long Idx { get; set; }
        public long Chip { get; set; }
        public long Left { get; set; }

        public List<long> Win { get; set; }
        public List<long> Lose { get; set; }
        public long Gang { get; set; }
        public long HuCode { get; set; }
        public long HuJiao { get; set; }
        public long HuGang { get; set; }
        public long HuaZhu { get; set; }
        public long DaJiao { get; set; }
        public long TuiSui { get; set; }
    }
}
