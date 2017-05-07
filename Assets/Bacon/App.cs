using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XLua;
using System.Reflection;

namespace Bacon {
    class App : Application {

        private AppConfig _config = null;

        public App(global::App app) : base(app) {
            _config = new AppConfig();
            _ctx = new AppContext(this, _config, _tiSync);
            _dispatcher = _ctx.EventDispatcher;
        }

        public global::App GApp {
            get { return _app; }
        }
    }
}
