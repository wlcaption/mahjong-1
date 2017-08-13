using Bacon;
using Bacon.Service;
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
            _cs.RegisterResponse(C2sProtocol.logout.Tag, logout);
            _cs.RegisterResponse(C2sProtocol.match.Tag, match);
            _cs.RegisterResponse(C2sProtocol.create.Tag, create);
            _cs.RegisterResponse(C2sProtocol.join.Tag, join);
            _cs.RegisterResponse(C2sProtocol.leave.Tag, leave);
            _cs.RegisterResponse(C2sProtocol.first.Tag, first);

            _cs.RegisterResponse(C2sProtocol.call.Tag, call);
            _cs.RegisterResponse(C2sProtocol.lead.Tag, lead);
            _cs.RegisterResponse(C2sProtocol.step.Tag, step);
            _cs.RegisterResponse(C2sProtocol.toast1.Tag, board);
            _cs.RegisterResponse(C2sProtocol.toast2.Tag, adver);
            _cs.RegisterResponse(C2sProtocol.fetchsysmail.Tag, fetchsysmail);
            _cs.RegisterResponse(C2sProtocol.syncsysmail.Tag, syncsysmail);

            _cs.RegisterResponse(C2sProtocol.restart.Tag, restart);
            _cs.RegisterResponse(C2sProtocol.rchat.Tag, rchat);
            _cs.RegisterResponse(C2sProtocol.xuanpao.Tag, xuanpao);
            _cs.RegisterResponse(C2sProtocol.xuanque.Tag, xuanque);

            _cs.RegisterResponse(C2sProtocol.records.Tag, records);
            _cs.RegisterResponse(C2sProtocol.record.Tag, record);

        }

        public void handshake(uint session, SprotoTypeBase responseObj, object ud) {
            InitService service = _ctx.QueryService<InitService>(InitService.Name);
            if (service != null) {
                service.OnRspHandshake(responseObj);
            }
        }

        public void logout(uint session, SprotoTypeBase responseObj, object ud) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.FetchSysmail(responseObj);
        }

        public void match(uint session, SprotoTypeBase responseObj, object ud) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.Match(responseObj);
        }

        public void create(uint session, SprotoTypeBase responseObj, object ud) {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            UnityEngine.Debug.Assert(service != null);
            service.OnRspCreate(responseObj);
        }

        public void join(uint session, SprotoTypeBase responseObj, object ud) {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            if (service != null) {
                service.OnRspJoin(responseObj);
            }
        }

        public void leave(uint session, SprotoTypeBase responseObj, object ud) {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            if (service != null) {
                service.OnRspLeave(responseObj);
            }
        }

        public void call(uint session, SprotoTypeBase responseObj, object ud) { }

        public void lead(uint session, SprotoTypeBase responseObj, object ud) { }

        public void step(uint session, SprotoTypeBase responseObj, object ud) {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            if (service != null) {
                service.OnRspStep(responseObj);
            }
        }

        public void restart(uint session, SprotoTypeBase responseObj, object ud) {
        }

        public void first(uint session, SprotoTypeBase responseObj, object ud) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.First(responseObj);
        }

        public void fetchsysmail(uint session, SprotoTypeBase responseObj, object ud) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.FetchSysmail(responseObj);
        }

        public void syncsysmail(uint session, SprotoTypeBase responseObj, object ud) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.SyncSysmail(responseObj);
        }

        public void rchat(uint session, SprotoTypeBase responseObj, object ud) { }

        public void xuanpao(uint session, SprotoTypeBase responseObj, object ud) { }
        public void xuanque(uint session, SprotoTypeBase responseObj, object ud) { }

        public void records(uint session, SprotoTypeBase responseObj, object ud) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.Records(responseObj);
        }

        public void record(uint session, SprotoTypeBase responseObj, object ud) {
            PlayService service = _ctx.QueryService<PlayService>(PlayService.Name);
            UnityEngine.Debug.Assert(service != null);
            service.OnRspRecord(responseObj);
        }



        public void adver(uint session, SprotoTypeBase responseObj, object ud) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.Adver(responseObj);
        }

        public void board(uint session, SprotoTypeBase responseObj, object ud) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.Board(responseObj);
        }
    }
}
