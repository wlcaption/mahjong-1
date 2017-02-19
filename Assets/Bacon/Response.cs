using Bacon;
using Sproto;
using System;
using System.Text;
using UnityEngine;

namespace Maria.Network {
    class Response {
        private Context _ctx;
        private ClientSocket _cs;
        public Response(Context ctx, ClientSocket cs) {
            _ctx = ctx;
            _cs = cs;

            _cs.RegisterResponse(C2sProtocol.handshake.Tag, handshake);
            _cs.RegisterResponse(C2sProtocol.join.Tag, join);
            //_cs.RegisterResponse(C2sProtocol.born.Tag, born);
            //_cs.RegisterResponse(C2sProtocol.opcode.Tag, opcode);
            _cs.RegisterResponse(C2sProtocol.match.Tag, match);
        }

        public void handshake(uint session, SprotoTypeBase responseObj) {
            InitService service = (InitService)_ctx.QueryService("init");
            if (service != null) {
                service.Handshake(responseObj);
            }
        }

        public void match(uint session, SprotoTypeBase responseObj) {
            MainController ctr = _ctx.Top() as MainController;
            ctr.Match(responseObj);
        }

        public void create(uint session, SprotoTypeBase responseObj) {
            Bacon.MainController ctr = _ctx.Top() as Bacon.MainController;
            ctr.Create(responseObj);
        }

        // 进入房间这个协议, authudp
        public void join(uint session, SprotoTypeBase responseObj) {
            Bacon.MainController ctr = _ctx.Top() as Bacon.MainController;
            ctr.Join(responseObj);
        }

        public void leave(uint session, SprotoTypeBase responseObj) {
        }

        public void lead(uint session, SprotoTypeBase responseObj) {

        }

        public void call(uint session, SprotoTypeBase responseObj) {

        }
    }
}
