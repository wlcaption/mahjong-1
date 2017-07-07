using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Model {
    public class TipsMgr {
        private AppContext _ctx;
        private string _tipscontent = string.Empty;

        public TipsMgr(AppContext ctx) {
            _ctx = ctx;
        }

        public string TipsContent { get; set; }
    }
}
