using Maria;
using XLua;

namespace Bacon {

    [CSharpCallLua]
    class App : Application {

        [CSharpCallLua]
        public delegate Maria.Lua.Env Main(Context ctx);

        private AppConfig _config = null;
        private Maria.Lua.Env _envScript = null;

        public App(global::App app) : base(app) {
            _config = new AppConfig();
            _ctx = new AppContext(this, _config, _tiSync);
            _dispatcher = _ctx.EventDispatcher;

            //var env = _luaenv.Global.Get<Maria.Lua.Env>("g");
            //env.update();

            Main main = _luaenv.Global.Get<Main>("main");
            _envScript = main(_ctx);
            _ctx.EnvScript = _envScript;
            _envScript.update();

            //Action<Context> main = _luaenv.Global.Get<Action<Context>>("main");
            //main(_ctx);
        }

        public global::App GApp {
            get { return _app; }
        }

        public Maria.Lua.Env EnvScript { get { return _envScript; } }

        public override void Update() {
            base.Update();
        }
    }
}
