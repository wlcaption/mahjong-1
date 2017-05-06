using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XLua;
using System.Reflection;

namespace Bacon {
    class App : Application {

        [LuaCallCSharp]
        public static List<Type> LuaCallCsByProperty {
            get {
                List<Type> l = new List<Type>();
                Assembly asm = Assembly.GetExecutingAssembly();
                foreach (Type t in asm.GetTypes()) {
                    if (t.Namespace == "Bacon") {
                        l.Add(t);
                    } else if (t.Namespace == "C2sSprotoType") {
                        l.Add(t);
                    } else if (t.Namespace == "S2cSprotoType") {
                        l.Add(t);
                    }
                }
                return l;
            }
        }

        [Hotfix]
        public static List<Type> HotFixByProperty {
            get {
                List<Type> l = new List<Type>();
                Assembly asm = Assembly.GetExecutingAssembly();
                foreach (Type t in asm.GetTypes()) {
                    if (t.Namespace == "Bacon") {
                        l.Add(t);
                    }
                }
                return l;
            }
        }

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
