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

            _cs.RegisterRequest(S2cProtocol.ready.Tag, ready);
            _cs.RegisterRequest(S2cProtocol.shuffle.Tag, shuffle);
            _cs.RegisterRequest(S2cProtocol.dice.Tag, dice);
            _cs.RegisterRequest(S2cProtocol.deal.Tag, deal);
            _cs.RegisterRequest(S2cProtocol.call.Tag, call);
            _cs.RegisterRequest(S2cProtocol.take_turn.Tag, take_turn);

            _cs.RegisterRequest(S2cProtocol.peng.Tag, peng);
            _cs.RegisterRequest(S2cProtocol.gang.Tag, gang);
            _cs.RegisterRequest(S2cProtocol.hu.Tag, hu);
            _cs.RegisterRequest(S2cProtocol.lead.Tag, lead);

            _cs.RegisterRequest(S2cProtocol.over.Tag, over);
            _cs.RegisterRequest(S2cProtocol.restart.Tag, restart);
            _cs.RegisterRequest(S2cProtocol.take_restart.Tag, take_restart);
            _cs.RegisterRequest(S2cProtocol.take_xuanpao.Tag, take_xuanpao);
            _cs.RegisterRequest(S2cProtocol.take_xuanque.Tag, take_xuanque);
            _cs.RegisterRequest(S2cProtocol.xuanpao.Tag, xuanpao);
            _cs.RegisterRequest(S2cProtocol.xuanque.Tag, xuanque);
            _cs.RegisterRequest(S2cProtocol.rchat.Tag, rchat);

            _cs.RegisterRequest(S2cProtocol.radio.Tag, radio);
        }

        public SprotoTypeBase handshake(uint session, SprotoTypeBase requestObj) {
            S2cSprotoType.handshake.response responseObj = new S2cSprotoType.handshake.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }

        #region enter room
        public SprotoTypeBase join(uint session, SprotoTypeBase requestObj) {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            UnityEngine.Debug.Assert(service != null);
            return service.OnJoin(requestObj);
        }

        public SprotoTypeBase leave(uint session, SprotoTypeBase requestObj) {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            UnityEngine.Debug.Assert(service != null);
            return service.OnLeave(requestObj);
        }

        public SprotoTypeBase match(uint session, SprotoTypeBase requestObj) {
            MainController controller = _ctx.Top() as MainController;
            return controller.OnMatch(requestObj);
        }
        #endregion


        public SprotoTypeBase ready(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnReady(requestObj);
        }

        public SprotoTypeBase shuffle(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnShuffle(requestObj);
        }

        public SprotoTypeBase dice(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnDice(requestObj);
        }

        public SprotoTypeBase deal(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnDeal(requestObj);
        }

        public SprotoTypeBase take_turn(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnTakeTurn(requestObj);
        }

        public SprotoTypeBase call(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnCall(requestObj);
        }

        public SprotoTypeBase peng(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnPeng(requestObj);
        }

        public SprotoTypeBase gang(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnGang(requestObj);
        }

        public SprotoTypeBase hu(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnHu(requestObj);
        }

        public SprotoTypeBase lead(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnLead(requestObj);
        }

        public SprotoTypeBase over(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnOver(requestObj);
        }

        public SprotoTypeBase restart(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnRestart(requestObj);
        }

        public SprotoTypeBase take_restart(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnTakeRestart(requestObj);
        }

        public SprotoTypeBase take_xuanpao(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnTakeXuanPao(requestObj);
        }

        public SprotoTypeBase xuanpao(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnXuanPao(requestObj);
        }

        public SprotoTypeBase take_xuanque(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnTakeXuanQue(requestObj);
        }

        public SprotoTypeBase xuanque(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnXuanQue(requestObj);
        }

        public SprotoTypeBase rchat(uint session, SprotoTypeBase requestObj) {
            GameController controller = _ctx.Top() as GameController;
            return controller.OnRChat(requestObj);
        }

        public SprotoTypeBase radio(uint session, SprotoTypeBase requestObj) {
            InitService service = _ctx.QueryService<InitService>(InitService.Name);
            return service.OnRadio(requestObj);
        }

    }
}
