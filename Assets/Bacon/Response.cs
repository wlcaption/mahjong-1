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
            _cs.RegisterResponse(C2sProtocol.match.Tag, match);
            _cs.RegisterResponse(C2sProtocol.create.Tag, create);
            _cs.RegisterResponse(C2sProtocol.join.Tag, join);
            _cs.RegisterResponse(C2sProtocol.leave.Tag, leave);

            _cs.RegisterResponse(C2sProtocol.call.Tag, call);
            _cs.RegisterResponse(C2sProtocol.lead.Tag, lead);
            _cs.RegisterResponse(C2sProtocol.step.Tag, step);

            _cs.RegisterResponse(C2sProtocol.restart.Tag, restart);

            _cs.RegisterResponse(C2sProtocol.first.Tag, first);
            _cs.RegisterResponse(C2sProtocol.fetchsysmail.Tag, fetchsysmail);
            _cs.RegisterResponse(C2sProtocol.syncsysmail.Tag, syncsysmail);
            _cs.RegisterResponse(C2sProtocol.rchat.Tag, rchat);
            _cs.RegisterResponse(C2sProtocol.xuanpao.Tag, xuanpao);
            _cs.RegisterResponse(C2sProtocol.xuanque.Tag, xuanque);

            _cs.RegisterResponse(C2sProtocol.records.Tag, records);
            _cs.RegisterResponse(C2sProtocol.record.Tag, record);

            _cs.RegisterResponse(C2sProtocol.logout.Tag, logout);
        }

        public void handshake(uint session, SprotoTypeBase responseObj) {
            InitService service = _ctx.QueryService<InitService>(InitService.Name);
            if (service != null) {
                service.Handshake(responseObj);
            }
        }

        public void match(uint session, SprotoTypeBase responseObj) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.Match(responseObj);
        }

        public void create(uint session, SprotoTypeBase responseObj) {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            UnityEngine.Debug.Assert(service != null);
            service.Create(responseObj);
        }

        public void join(uint session, SprotoTypeBase responseObj) {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            if (service != null) {
                service.Join(responseObj);
            }
        }

        public void leave(uint session, SprotoTypeBase responseObj) {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            if (service != null) {
                service.Leave(responseObj);
            }
        }

        public void call(uint session, SprotoTypeBase responseObj) { }

        public void lead(uint session, SprotoTypeBase responseObj) { }

        public void step(uint session, SprotoTypeBase responseObj) {
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            if (service != null) {
                service.Step(responseObj);
            }
        }

        public void restart(uint session, SprotoTypeBase responseObj) {
        }

        public void first(uint session, SprotoTypeBase responseObj) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.First(responseObj);
        }

        public void fetchsysmail(uint session, SprotoTypeBase responseObj) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.FetchSysmail(responseObj);
        }

        public void syncsysmail(uint session, SprotoTypeBase responseObj) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.SyncSysmail(responseObj);
        }

        public void rchat(uint session, SprotoTypeBase responseObj) { }

        public void xuanpao(uint session, SprotoTypeBase responseObj) { }
        public void xuanque(uint session, SprotoTypeBase responseObj) { }

        public void records(uint session, SprotoTypeBase responseObj) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.Records(responseObj);
        }

        public void record(uint session, SprotoTypeBase responseObj) {
            PlayService service = _ctx.QueryService<PlayService>(PlayService.Name);
            UnityEngine.Debug.Assert(service != null);
            service.record(responseObj);
        }

        public void logout(uint session, SprotoTypeBase responseObj) {
            MainController ctr = _ctx.Peek<MainController>();
            ctr.FetchSysmail(responseObj);
        }
    }
}
