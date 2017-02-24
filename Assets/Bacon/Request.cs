using Maria;
using Maria.Network;
using Sproto;

namespace Bacon {
    class Request {
        private Context _ctx;
        private ClientSocket _cs;
        public Request(Context ctx, ClientSocket cs) {
            _ctx = ctx;
            _cs = cs;

            _cs.RegisterRequest(S2cProtocol.handshake.Tag, handshake);
            _cs.RegisterRequest(S2cProtocol.match.Tag, match);
            _cs.RegisterRequest(S2cProtocol.join.Tag, join);
            _cs.RegisterRequest(S2cProtocol.leave.Tag, leave);
            _cs.RegisterRequest(S2cProtocol.peng.Tag, peng);
            _cs.RegisterRequest(S2cProtocol.gang.Tag, gang);
            _cs.RegisterRequest(S2cProtocol.hu.Tag, hu);
            _cs.RegisterRequest(S2cProtocol.call.Tag, call);
            _cs.RegisterRequest(S2cProtocol.shuffle.Tag, shuffle);
            _cs.RegisterRequest(S2cProtocol.dice.Tag, dice);
        }

        public SprotoTypeBase handshake(uint session, SprotoTypeBase requestObj) {
            S2cSprotoType.handshake.response responseObj = new S2cSprotoType.handshake.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }

        public SprotoTypeBase join(uint session, SprotoTypeBase requestObj) {
            GameService service = (GameService)_ctx.QueryService(GameService.Name);
            UnityEngine.Debug.Assert(service != null);
            return service.OnJoin(requestObj);
        }

        public SprotoTypeBase leave(uint session, SprotoTypeBase requestObj) {
            GameService service = (GameService)_ctx.QueryService(GameService.Name);
            UnityEngine.Debug.Assert(service != null);
            return service.OnLeave(requestObj);
        }

        public SprotoTypeBase die(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnDie(requestObj);
        }

        public SprotoTypeBase match(uint session, SprotoTypeBase requestObj) {
            MainController controller = _ctx.Top() as MainController;
            return controller.OnMatch(requestObj);
        }

        public SprotoTypeBase take_turn(uint session, SprotoTypeBase requestObj) {
            MainController controller = _ctx.Top() as MainController;
            return controller.OnMatch(requestObj);
        }

        public SprotoTypeBase peng(uint session, SprotoTypeBase requestObj) {
            MainController controller = _ctx.Top() as MainController;
            return controller.OnMatch(requestObj);
        }

        public SprotoTypeBase gang(uint session, SprotoTypeBase requestObj) {
            MainController controller = _ctx.Top() as MainController;
            return controller.OnMatch(requestObj);
        }

        public SprotoTypeBase hu(uint session, SprotoTypeBase requestObj) {
            MainController controller = _ctx.Top() as MainController;
            return controller.OnMatch(requestObj);
        }

        public SprotoTypeBase call(uint session, SprotoTypeBase requestObj) {
            GameService service = (GameService)_ctx.QueryService(GameService.Name);
            UnityEngine.Debug.Assert(service != null);
            return service.OnJoin(requestObj);
        }

        public SprotoTypeBase shuffle(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnShuffle(requestObj);
        }

        public SprotoTypeBase dice(uint session, SprotoTypeBase requestObj) {
            MainController controller = _ctx.Top() as MainController;
            return controller.OnMatch(requestObj);
        }
    }
}
