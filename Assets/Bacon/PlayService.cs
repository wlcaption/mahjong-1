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
                            JSONObject settles = args["settles"];
                            request.settles = new List<S2cSprotoType.settle>();
                            for (int j = 0; j < settles.list.Count; j++) {
                                JSONObject jsettle = settles.list[j];
                                S2cSprotoType.settle ssettle = new S2cSprotoType.settle();
                                for (int m = 1; m < 5; m++) {
                                    string pkey = string.Format("p{0}", m);
                                    JSONObject p = jsettle.GetField(pkey);
                                    if (p != null && !p.IsNull) {
                                        S2cSprotoType.settlementitem item = new S2cSprotoType.settlementitem();
                                        item.idx = p["idx"].i;
                                        item.chip = p["chip"].i;
                                        item.left = p["left"].i;

                                        JSONObject win = p["win"];
                                        item.win = new List<long>();
                                        for (int k = 0; k < win.list.Count; k++) {
                                            item.win.Add(win.list[j].i);
                                        }
                                        JSONObject lose = p["lose"];
                                        for (int k = 0; k < lose.list.Count; k++) {
                                            item.lose.Add(lose.list[j].i);
                                        }
                                        item.gang = p["gang"].i;
                                        item.hucode = p["hucode"].i;
                                        item.hujiao = p["hujiao"].i;
                                        item.hugang = p["hugang"].i;
                                        item.huazhu = p["huazhu"].i;
                                        item.dajiao = p["dajiao"].i;
                                        item.tuisui = p["tuisui"].i;

                                        if (m == 1) {
                                            ssettle.p1 = item;
                                        } else if (m == 2) {
                                            ssettle.p2 = item;
                                        } else if (m == 3) {
                                            ssettle.p3 = item;
                                        } else if (m == 4) {
                                            ssettle.p4 = item;
                                        }
                                    }

                                }
                                request.settles.Add(ssettle);

                            }
                            _controller.OnGang(request);
                        });
                    } else if (protocol == "hu") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            S2cSprotoType.hu.request request = new S2cSprotoType.hu.request();
                            request.hus = new List<S2cSprotoType.huinfo>();
                            JSONObject hus = args["hus"];
                            for (int m = 0; m < hus.list.Count; m++) {
                                JSONObject huinfo = hus.list[m];
                                S2cSprotoType.huinfo item = new S2cSprotoType.huinfo();
                                item.idx = huinfo["idx"].i;
                                item.card = huinfo["card"].i;
                                item.code = huinfo["code"].i;
                                item.jiao = huinfo["jiao"].i;
                                item.dian = huinfo["dian"].i;
                                item.gang = huinfo["gang"].i;
                                request.hus.Add(item);
                            }
                            JSONObject settles = args["settles"];
                            request.settles = new List<S2cSprotoType.settle>();
                            for (int j = 0; j < settles.list.Count; j++) {
                                JSONObject jsettle = settles.list[j];
                                S2cSprotoType.settle ssettle = new S2cSprotoType.settle();
                                for (int m = 1; m < 5; m++) {
                                    string pkey = string.Format("p{0}", m);
                                    JSONObject p = jsettle.GetField(pkey);
                                    if (p != null && !p.IsNull) {
                                        S2cSprotoType.settlementitem item = new S2cSprotoType.settlementitem();
                                        item.idx = p["idx"].i;
                                        item.chip = p["chip"].i;
                                        item.left = p["left"].i;

                                        JSONObject win = p["win"];
                                        item.win = new List<long>();
                                        for (int k = 0; k < win.list.Count; k++) {
                                            item.win.Add(win.list[j].i);
                                        }
                                        JSONObject lose = p["lose"];
                                        for (int k = 0; k < lose.list.Count; k++) {
                                            item.lose.Add(lose.list[j].i);
                                        }
                                        item.gang = p["gang"].i;
                                        item.hucode = p["hucode"].i;
                                        item.hujiao = p["hujiao"].i;
                                        item.hugang = p["hugang"].i;
                                        item.huazhu = p["huazhu"].i;
                                        item.dajiao = p["dajiao"].i;
                                        item.tuisui = p["tuisui"].i;

                                        if (m == 1) {
                                            ssettle.p1 = item;
                                        } else if (m == 2) {
                                            ssettle.p2 = item;
                                        } else if (m == 3) {
                                            ssettle.p3 = item;
                                        } else if (m == 4) {
                                            ssettle.p4 = item;
                                        }
                                    }

                                }
                                request.settles.Add(ssettle);

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
                    } else if (protocol == "over") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            _controller.OnOver(null);
                        });
                    } else if (protocol == "xuanpao") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            _controller.OnXuanPao(null);
                        });
                    } else if (protocol == "take_xuanque") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            S2cSprotoType.take_xuanque.request request = new S2cSprotoType.take_xuanque.request();
                            request.countdown = args["countdown"].i;
                            request.your_turn = args["your_turn"].i;
                            request.card = args["card"].i;
                            _controller.OnXuanQue(request);
                        });
                    } else if (protocol == "xuanque") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            S2cSprotoType.xuanque.request request = new S2cSprotoType.xuanque.request();
                            request.idx = args["idx"].i;
                            request.que = args["que"].i;
                            _controller.OnXuanQue(request);
                        });
                    } else if (protocol == "settle") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            S2cSprotoType.settle.request request = new S2cSprotoType.settle.request();

                            JSONObject settles = args["settles"];
                            request.settles = new List<S2cSprotoType.settle>();
                            for (int j = 0; j < settles.list.Count; j++) {
                                JSONObject jsettle = settles.list[j];
                                S2cSprotoType.settle ssettle = new S2cSprotoType.settle();
                                for (int m = 1; m < 5; m++) {
                                    string pkey = string.Format("p{0}", m);
                                    JSONObject p = jsettle.GetField(pkey);
                                    if (p != null && !p.IsNull) {
                                        S2cSprotoType.settlementitem item = new S2cSprotoType.settlementitem();
                                        item.idx = p["idx"].i;
                                        item.chip = p["chip"].i;
                                        item.left = p["left"].i;

                                        JSONObject win = p["win"];
                                        item.win = new List<long>();
                                        for (int k = 0; k < win.list.Count; k++) {
                                            item.win.Add(win.list[j].i);
                                        }
                                        JSONObject lose = p["lose"];
                                        for (int k = 0; k < lose.list.Count; k++) {
                                            item.lose.Add(lose.list[j].i);
                                        }
                                        item.gang = p["gang"].i;
                                        item.hucode = p["hucode"].i;
                                        item.hujiao = p["hujiao"].i;
                                        item.hugang = p["hugang"].i;
                                        item.huazhu = p["huazhu"].i;
                                        item.dajiao = p["dajiao"].i;
                                        item.tuisui = p["tuisui"].i;

                                        if (m == 1) {
                                            ssettle.p1 = item;
                                        } else if (m == 2) {
                                            ssettle.p2 = item;
                                        } else if (m == 3) {
                                            ssettle.p3 = item;
                                        } else if (m == 4) {
                                            ssettle.p4 = item;
                                        }
                                    }

                                }
                                request.settles.Add(ssettle);

                            }

                            _controller.OnSettle(request);
                        });
                    } else if (protocol == "final_settle") {
                        _ctx.Countdown(key, (int)pt, null, () => {
                            S2cSprotoType.final_settle.request request = new S2cSprotoType.final_settle.request();

                            JSONObject p1 = args["p1"];
                            request.p1 = new List<S2cSprotoType.settlementitem>();
                            for (int j = 0; j < p1.list.Count; j++) {
                                S2cSprotoType.settlementitem item = new S2cSprotoType.settlementitem();
                                item.idx = p1.list[j]["idx"].i;
                                item.chip = p1.list[j]["chip"].i;
                                item.left = p1.list[j]["left"].i;

                                JSONObject win = p1.list[j]["win"];
                                item.win = new List<long>();
                                for (int k = 0; k < win.list.Count; k++) {
                                    item.win.Add(win.list[j].i);
                                }
                                JSONObject lose = p1.list[j]["lose"];
                                for (int k = 0; k < lose.list.Count; k++) {
                                    item.lose.Add(lose.list[j].i);
                                }
                                item.gang = p1.list[j]["gang"].i;
                                item.hucode = p1.list[j]["hucode"].i;
                                item.hujiao = p1.list[j]["hujiao"].i;
                                item.hugang = p1.list[j]["hugang"].i;
                                item.huazhu = p1.list[j]["huazhu"].i;
                                item.dajiao = p1.list[j]["dajiao"].i;
                                item.tuisui = p1.list[j]["tuisui"].i;
                                request.p1.Add(item);
                            }

                            JSONObject p2 = args["p2"];
                            request.p2 = new List<S2cSprotoType.settlementitem>();
                            for (int j = 0; j < p2.list.Count; j++) {
                                S2cSprotoType.settlementitem item = new S2cSprotoType.settlementitem();
                                item.idx = p2.list[j]["idx"].i;
                                item.chip = p2.list[j]["chip"].i;
                                item.left = p2.list[j]["left"].i;

                                JSONObject win = p2.list[j]["win"];
                                item.win = new List<long>();
                                for (int k = 0; k < win.list.Count; k++) {
                                    item.win.Add(win.list[j].i);
                                }
                                JSONObject lose = p2.list[j]["lose"];
                                for (int k = 0; k < lose.list.Count; k++) {
                                    item.lose.Add(lose.list[j].i);
                                }
                                item.gang = p2.list[j]["gang"].i;
                                item.hucode = p2.list[j]["hucode"].i;
                                item.hujiao = p2.list[j]["hujiao"].i;
                                item.hugang = p2.list[j]["hugang"].i;
                                item.huazhu = p2.list[j]["huazhu"].i;
                                item.dajiao = p2.list[j]["dajiao"].i;
                                item.tuisui = p2.list[j]["tuisui"].i;
                                request.p2.Add(item);
                            }

                            JSONObject p3 = args["p3"];
                            request.p3 = new List<S2cSprotoType.settlementitem>();
                            for (int j = 0; j < p3.list.Count; j++) {
                                S2cSprotoType.settlementitem item = new S2cSprotoType.settlementitem();
                                item.idx = p3.list[j]["idx"].i;
                                item.chip = p3.list[j]["chip"].i;
                                item.left = p3.list[j]["left"].i;

                                JSONObject win = p3.list[j]["win"];
                                item.win = new List<long>();
                                for (int k = 0; k < win.list.Count; k++) {
                                    item.win.Add(win.list[j].i);
                                }
                                JSONObject lose = p3.list[j]["lose"];
                                for (int k = 0; k < lose.list.Count; k++) {
                                    item.lose.Add(lose.list[j].i);
                                }
                                item.gang = p3.list[j]["gang"].i;
                                item.hucode = p3.list[j]["hucode"].i;
                                item.hujiao = p3.list[j]["hujiao"].i;
                                item.hugang = p3.list[j]["hugang"].i;
                                item.huazhu = p3.list[j]["huazhu"].i;
                                item.dajiao = p3.list[j]["dajiao"].i;
                                item.tuisui = p3.list[j]["tuisui"].i;
                                request.p3.Add(item);
                            }

                            JSONObject p4 = args["p4"];
                            request.p4 = new List<S2cSprotoType.settlementitem>();
                            for (int j = 0; j < p4.list.Count; j++) {
                                S2cSprotoType.settlementitem item = new S2cSprotoType.settlementitem();
                                item.idx = p4.list[j]["idx"].i;
                                item.chip = p4.list[j]["chip"].i;
                                item.left = p4.list[j]["left"].i;

                                JSONObject win = p4.list[j]["win"];
                                item.win = new List<long>();
                                for (int k = 0; k < win.list.Count; k++) {
                                    item.win.Add(win.list[j].i);
                                }
                                JSONObject lose = p4.list[j]["lose"];
                                for (int k = 0; k < lose.list.Count; k++) {
                                    item.lose.Add(lose.list[j].i);
                                }
                                item.gang = p4.list[j]["gang"].i;
                                item.hucode = p4.list[j]["hucode"].i;
                                item.hujiao = p4.list[j]["hujiao"].i;
                                item.hugang = p4.list[j]["hugang"].i;
                                item.huazhu = p4.list[j]["huazhu"].i;
                                item.dajiao = p4.list[j]["dajiao"].i;
                                item.tuisui = p4.list[j]["tuisui"].i;
                                request.p4.Add(item);
                            }
                            request.over = args.GetField("over").b;
                            _controller.OnFinalSettle(request);
                        });
                    }
                }

            }
        }
    }
}
