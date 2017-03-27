using Maria;

namespace Bacon {
    class AppConfig : Config {
        public enum VERSION_TYPE {
            TEST,
            DEV,
            PUBLIC,
        }

        public AppConfig() : base() {

            VTYPE = VERSION_TYPE.PUBLIC;
            UpdateRes = false;
            if (VTYPE == VERSION_TYPE.PUBLIC) {
                _loginIp = "120.76.248.223";
                _loginPort = 3002;
                _gateIp = "120.76.248.223";
                _gatePort = 3301;

            } else {
                _loginIp = "192.168.1.123";
                _loginPort = 3002;
                _gateIp = "192.168.1.123";
                _gatePort = 3301;
            }


            c2s = C2sProtocol.Instance;
            s2c = S2cProtocol.Instance;

        }

        public VERSION_TYPE VTYPE { get; set; }
        public bool UpdateRes { get; set; }

    }
}
