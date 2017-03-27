using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using Sproto;

namespace Bacon {
    class PlayService : Service {
        public static readonly string Name = "Play";

        private string _r = string.Empty;
        private GameController _controller = null;

        public PlayService(Context ctx) : base(ctx) {
        }

        public void record(SprotoTypeBase responseObj) {
            C2sSprotoType.record.response obj = responseObj as C2sSprotoType.record.response;
            _r = obj.r;
            _controller = (GameController)_ctx.Push(typeof(GameController));
            _controller.Type = GameType.PLAY;
        }

        private void Parse() {
            JSONObject json = new JSONObject(_r);
            if (json.type == JSONObject.Type.ARRAY) {
                for (int i = 0; i < json.list.Count; i++) {
                    string key = string.Format("Play{0}", i);
                    JSONObject tnode = json.list[i];
                    string protocol = tnode["protocol"].str;
                    long pt = tnode["pt"].i;
                    JSONObject args = tnode["args"];
                    if (protocol == "players") {
                        C2sSprotoType.join.response obj = new C2sSprotoType.join.response();
                        for (int j = 0; j < args.list.Count; j++) {
                            C2sSprotoType.player player = new C2sSprotoType.player();
                            JSONObject p = args.list[i];
                            if (p["uid"].i == _ctx.U.Uid) {
                                player.idx = p["idx"].i;
                                obj.me = player;
                            } else {
                                player.idx = p["idx"].i;
                                if (obj.ps == null) {
                                    obj.ps = new List<C2sSprotoType.player>();
                                }
                                obj.ps.Add(player);
                            } 
                        }
                        GameService service = _ctx.QueryService<GameService>(GameService.Name);
                        service.OnJoin(obj);
                    } else if (protocol == "peng") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            S2cSprotoType.peng.request request = new S2cSprotoType.peng.request();
                            request.idx = args["idx"].i;
                            request.card = args["card"].i;
                            request.code = args["code"].i;
                            request.hor = args["hor"].i;
                            _controller.OnPeng(request);
                        });
                    } else if (protocol == "gang") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            S2cSprotoType.gang.request request = new S2cSprotoType.gang.request();
                            request.idx = args["idx"].i;
                            request.card = args["card"].i;
                            request.code = args["code"].i;
                            request.hor = args["hor"].i;
                            request.dian = args["dian"].i;
                            request.chip = args["chip"].i;
                            _controller.OnGang(request);
                        });
                    } else if (protocol == "hu") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            S2cSprotoType.hu.request request = new S2cSprotoType.hu.request();
                            request.hus = new List<S2cSprotoType.huinfo>();
                            JSONObject hus = args["hus"];
                            for (int j = 0; j < hus.list.Count; j++) {
                                JSONObject huinfo = hus.list[i];
                                S2cSprotoType.huinfo item = new S2cSprotoType.huinfo();
                                item.idx = huinfo["idx"].i;
                                item.card = huinfo["card"].i;
                                item.code = huinfo["code"].i;
                                item.jiao = huinfo["jiao"].i;
                                item.dian = huinfo["dian"].i;
                                item.chip = huinfo["chip"].i;

                                request.hus.Add(item);
                            }
                            _controller.OnHu(request);
                        });
                    } else if (protocol == "shuffle") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            S2cSprotoType.shuffle.request request = new S2cSprotoType.shuffle.request();
                            request.first = args["first"].i;
                            request.p1 = new List<long>();
                            {
                                JSONObject p1 = args["p1"];
                                for (int j = 0; j < p1.list.Count; j++) {
                                    request.p1.Add(p1.list[i].i);
                                }
                            }
                            {
                                JSONObject p2 = args["p2"];
                                for (int j = 0; j < p2.list.Count; j++) {
                                    request.p2.Add(p2.list[i].i);
                                }
                            }
                            {
                                JSONObject p3 = args["p3"];
                                for (int j = 0; j < p3.list.Count; j++) {
                                    request.p3.Add(p3.list[i].i);
                                }
                            }
                            {
                                JSONObject p4 = args["p4"];
                                for (int j = 0; j < p4.list.Count; j++) {
                                    request.p4.Add(p4.list[i].i);
                                }
                            }
                            _controller.OnHu(request);
                        });
                    } else if (protocol == "dice") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            S2cSprotoType.dice.request request = new S2cSprotoType.dice.request();
                            request.first = args["first"].i;
                            request.firsttake = args["firsttake"].i;
                            request.d1 = args["d1"].i;
                            request.d2 = args["d2"].i;
                            _controller.OnDice(request);
                        });
                    } else if (protocol == "lead") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            S2cSprotoType.lead.request request = new S2cSprotoType.lead.request();
                            request.idx = args["idx"].i;
                            request.card = args["card"].i;
                            _controller.OnLead(request);
                        });

                    } else if (protocol == "deal") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            S2cSprotoType.deal.request request = new S2cSprotoType.deal.request();
                            request.firstidx = args["firstidx"].i;
                            request.firsttake = args["firsttake"].i;
                            {
                                JSONObject p1 = args["p1"];
                                for (int j = 0; j < p1.list.Count; j++) {
                                    request.p1.Add(p1.list[i].i);
                                }
                            }
                            {
                                JSONObject p2 = args["p2"];
                                for (int j = 0; j < p2.list.Count; j++) {
                                    request.p2.Add(p2.list[i].i);
                                }
                            }
                            {
                                JSONObject p3 = args["p3"];
                                for (int j = 0; j < p3.list.Count; j++) {
                                    request.p3.Add(p3.list[i].i);
                                }
                            }

                            {
                                JSONObject p4 = args["p4"];
                                for (int j = 0; j < p4.list.Count; j++) {
                                    request.p4.Add(p4.list[i].i);
                                }
                            }
                            _controller.OnDeal(request);

                        });
                    }
                }

            }
        }
    }
}
