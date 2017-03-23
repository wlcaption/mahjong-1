using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

namespace Bacon {
    class BottomPlayer : Player {

        private Card _leadcard1 = null;
        private GameObject _xuanpao = null;
        private GameObject _xuanque = null;

        public BottomPlayer(Context ctx, GameService service) : base(ctx, service) {

            _upv = Quaternion.AngleAxis(0.0f, Vector3.up);
            _uph = Quaternion.AngleAxis(-90.0f, Vector3.up);
            _downv = Quaternion.AngleAxis(180.0f, Vector3.forward);
            _backv = Quaternion.AngleAxis(0.0f, Vector3.up) * Quaternion.AngleAxis(180.0f, Vector3.forward);


            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_BOTTOMPLAYER, OnSetup);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_PENG, OnSendPeng);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);

            EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_GANG, OnSendGang);
            _ctx.EventDispatcher.AddCmdEventListener(listener3);

            EventListenerCmd listener4 = new EventListenerCmd(MyEventCmd.EVENT_HU, OnSendHu);
            _ctx.EventDispatcher.AddCmdEventListener(listener4);

            EventListenerCmd listener5 = new EventListenerCmd(MyEventCmd.EVENT_GUO, OnSendGuo);
            _ctx.EventDispatcher.AddCmdEventListener(listener5);

            EventListenerCmd listener6 = new EventListenerCmd(MyEventCmd.EVENT_LEAD, OnSendLead);
            _ctx.EventDispatcher.AddCmdEventListener(listener6);

            EventListenerCmd listener7 = new EventListenerCmd(MyEventCmd.EVENT_XUANQUE, OnSendQue);
            _ctx.EventDispatcher.AddCmdEventListener(listener7);

            EventListenerCmd listener8 = new EventListenerCmd(MyEventCmd.EVENT_XUANPAO, OnSendPao);
            _ctx.EventDispatcher.AddCmdEventListener(listener8);
        }

        private void OnSetup(EventCmd e) {
            _go = e.Orgin;
            ((GameController)_controller).SendStep();
            _ctx.EnqueueRenderQueue(RenderSetup);
        }

        private void RenderSetup() {
            _go.GetComponent<global::BottomPlayer>().ShowUI();
        }

        protected override Vector3 CalcPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            float x = _leftoffset + Card.Width * pos + Card.Width / 2.0f;
            float y = Card.Length / 2.0f;
            float z = _bottomoffset + Card.Height / 2.0f;

            return new Vector3(x, y, z);
        }

        protected override Vector3 CalcLeadPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            //int row = (pos + 1) / 6;
            //int col = (pos + 1) % 6;
            int row = (pos) / 6;
            int col = (pos) % 6;

            float x = _leadleftoffset + (Card.Width * col) + (Card.Width / 2.0f);
            float y = Card.Height / 2.0f;
            float z = _leadbottomoffset - (Card.Length * row) - (Card.Length / 2.0f);

            return new Vector3(x, y, z);
        }

        private void OnSendPeng(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.op = new C2sSprotoType.opinfo();
            request.op.idx = _idx;
            request.op.card = Call.Card;
            request.op.guo = OpCodes.OPCODE_NONE;
            request.op.peng = Call.Peng;
            request.op.gang = OpCodes.OPCODE_NONE;
            request.op.hu = new C2sSprotoType.huinfo();
            request.op.hu.idx = _idx;
            request.op.hu.code = HuType.NONE;

            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendGang(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.op = new C2sSprotoType.opinfo();
            request.op.idx = _idx;
            request.op.card = Call.Card;
            request.op.peng = OpCodes.OPCODE_NONE;
            request.op.gang = Call.Gang;
            request.op.hu = new C2sSprotoType.huinfo();
            request.op.hu.idx = _idx;
            request.op.hu.code = HuType.NONE;

            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendGuo(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.op = new C2sSprotoType.opinfo();
            request.op.idx = _idx;
            request.op.guo = OpCodes.OPCODE_GUO;
            request.op.peng = OpCodes.OPCODE_NONE;
            request.op.gang = OpCodes.OPCODE_NONE;
            request.op.hu = new C2sSprotoType.huinfo();
            request.op.hu.idx = _idx;
            request.op.hu.code = OpCodes.OPCODE_NONE;

            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendHu(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.op = new C2sSprotoType.opinfo();
            request.op.idx = _idx;
            request.op.guo = OpCodes.OPCODE_NONE;
            request.op.peng = OpCodes.OPCODE_NONE;
            request.op.gang = OpCodes.OPCODE_NONE;

            request.op.hu = new C2sSprotoType.huinfo();
            request.op.hu.idx = _idx;
            request.op.hu.code = Call.Hu.Code;
            request.op.hu.card = Call.Hu.Card;
            request.op.hu.jiao = Call.Hu.Jiao;
            request.op.hu.dian = Call.Hu.Dian;

            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendLead(EventCmd e) {
            Card card = null;
            for (int i = 0; i < _cards.Count; i++) {
                if (_cards[i].Go == e.Orgin) {
                    card = _cards[i];
                }
            }
            if (card == null) {
                UnityEngine.Debug.Assert(e.Orgin == _holdcard.Go);
                card = _holdcard;
            }

            _leadcard1 = card;

            C2sSprotoType.lead.request request = new C2sSprotoType.lead.request();
            request.idx = _idx;
            request.card = card.Value;
            _ctx.SendReq<C2sProtocol.lead>(C2sProtocol.lead.Tag, request);
        }

        private void OnSendQue(EventCmd e) {
            C2sSprotoType.xuanque.request request = new C2sSprotoType.xuanque.request();
            request.idx = _idx;
            request.que = (long)(Card.CardType)e.Msg["cardtype"];
            _ctx.SendReq<C2sProtocol.xuanque>(C2sProtocol.xuanque.Tag, request);
        }

        private void OnSendPao(EventCmd e) {
            C2sSprotoType.xuanpao.request request = new C2sSprotoType.xuanpao.request();
            request.idx = _idx;
            request.fen = (long)e.Msg["fen"];
            _ctx.SendReq<C2sProtocol.xuanpao>(C2sProtocol.xuanpao.Tag, request);
        }

        protected override void RenderBoxing() {
            Desk desk = ((GameController)_controller).Desk;
            for (int i = 0; i < _takecards.Count; i++) {
                int idx = i / 2;
                float x = desk.Width - (_takeleftoffset + idx * Card.Width + Card.Width / 2.0f);
                float y = Card.Height / 2.0f;
                float z = _takebottomoffset + Card.Length / 2.0f;
                if (i % 2 == 0) {
                    y = Card.Height / 2.0f + Card.Height;
                } else if (i % 2 == 1) {
                    y = Card.Height / 2.0f;
                }
                Card card = _takecards[i];
                card.Go.transform.localPosition = new UnityEngine.Vector3(x, y, z);
                card.Go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.forward);
            }

            Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_BOXINGCARDS);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderThrowDice() {
            base.RenderThrowDice();
        }

        protected override void RenderDeal() {
            int i = 0;
            if (_cards.Count == 13) {
                i = 12;
            } else {
                i = _cards.Count - 4;
            }
            for (; i < _cards.Count; i++) {
                Vector3 dst = CalcPos(i);
                var card = _cards[i];
                card.Go.transform.localPosition = dst;
                card.Go.transform.localRotation = Quaternion.AngleAxis(-60, Vector3.right);
            }
            Command cmd = new Command(MyEventCmd.EVENT_TAKEDEAL);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderSortCards() {
            int count = 0;
            for (int i = 0; i < _cards.Count; i++) {
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(-120.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        for (int j = 0; j < _cards.Count; j++) {
                            Vector3 dst = CalcPos(j);
                            _cards[j].Go.transform.localPosition = dst;
                            _go.GetComponent<global::BottomPlayer>().Add(_cards[j]);
                        }
                    })
                    .Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(-60.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        count++;
                        if (count >= _cards.Count) {
                            UnityEngine.Debug.LogFormat("bottom player send event sortcards");
                            Command cmd = new Command(MyEventCmd.EVENT_SORTCARDS);
                            _ctx.Enqueue(cmd);
                        }
                    });
            }
        }

        protected override void RenderTakeXuanPao() {
            if (_xuanpao == null) {
                ResourceManager.Instance.LoadAssetAsync<GameObject>("Prefabs/UI/SxXuanPao.prefab", "SxXuanPao", (GameObject go) => {
                    GameObject inst = GameObject.Instantiate<GameObject>(go);
                    if (inst) {
                        _xuanpao = inst;
                        _xuanpao.transform.SetParent(_go.GetComponent<global::BottomPlayer>()._Canvas.transform);
                    }
                });
            } else {
                _xuanpao.GetComponent<XuanPao>().Show();
            }
        }

        protected override void RenderXuanPao() {
            _go.GetComponent<global::BottomPlayer>().Head.SetMark(string.Format("{0}", _fen));
        }

        protected override void RenderTakeFirstCard() {
            UnityEngine.Debug.Assert(_takefirst);
            Vector3 dst = CalcPos(_cards.Count + 1);
            dst.y = dst.y + Card.Length;
            _holdcard.Go.transform.localPosition = dst;
            _holdcard.Go.transform.localRotation = Quaternion.AngleAxis(-60, Vector3.right);

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(_holdcard.Go.transform.DOMoveY(Card.Length / 2.0f, _holddelta))
                .AppendCallback(() => {
                    _go.GetComponent<global::BottomPlayer>().HoldCard = _holdcard.Go;
                    Command cmd = new Command(MyEventCmd.EVENT_TAKEFIRSTCARD);
                    _ctx.Enqueue(cmd);
                });
        }

        protected override void RenderTakeXuanQue() {
            if (_xuanque == null) {
                ABLoader.current.LoadResAsync<GameObject>("Prefabs/UI/ScXuanQue", (GameObject go) => {
                    GameObject inst = GameObject.Instantiate<GameObject>(go);
                    if (inst) {
                        _xuanque = inst;
                        _xuanque.transform.SetParent(_go.GetComponent<global::BottomPlayer>()._Canvas.transform);
                    }
                });
            } else {
                _xuanque.GetComponent<XuanQue>().Show();
            }
        }

        protected override void RenderXuanQue() {
            if (_que == Card.CardType.Bam) {
                _go.GetComponent<global::BottomPlayer>().Head.SetMark("条");
            } else if (_que == Card.CardType.Crak) {
                _go.GetComponent<global::BottomPlayer>().Head.SetMark("万");
            } else if (_que == Card.CardType.Dot) {
                _go.GetComponent<global::BottomPlayer>().Head.SetMark("同");
            }
            RenderSortCardsToDo(() => {

            });
        }

        protected override void RenderTakeTurn() {
            _go.GetComponent<global::BottomPlayer>().CloseAll();
            if (_turntype == 1) {
                Vector3 dst = CalcPos(_cards.Count + 1);
                dst.y = dst.y + Card.Length;
                _holdcard.Go.transform.localPosition = dst;
                _holdcard.Go.transform.localRotation = Quaternion.AngleAxis(-60, Vector3.right);

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_holdcard.Go.transform.DOMoveY(Card.Length / 2.0f, _holddelta))
                    .AppendCallback(() => {
                        _go.GetComponent<global::BottomPlayer>().HoldCard = _holdcard.Go;
                        _go.GetComponent<global::BottomPlayer>().SwitchOnTouch();
                    });
            } else if (_turntype == 0) {
                Vector3 dst = CalcPos(_cards.Count + 1);
                _holdcard.Go.transform.localRotation = Quaternion.AngleAxis(-60, Vector3.right);

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_holdcard.Go.transform.DOMove(dst, _holddelta))
                    .AppendCallback(() => {
                        _go.GetComponent<global::BottomPlayer>().HoldCard = _holdcard.Go;
                        _go.GetComponent<global::BottomPlayer>().SwitchOnTouch();
                    });
            } else if (_turntype == 2) {
                if (_xuanque) {
                    _xuanque.GetComponent<XuanQue>().Close();
                }
                _go.GetComponent<global::BottomPlayer>().Head.ShowTips("你是庄家，请先出牌");
                _go.GetComponent<global::BottomPlayer>().HoldCard = _holdcard.Go;
                _go.GetComponent<global::BottomPlayer>().SwitchOnTouch();
            }
        }

        protected override void RenderInsert(Action cb) {
            Vector3 to = CalcPos(_holdcard.Pos);
            Tween t = _holdcard.Go.transform.DOMove(to, _holddowndelat);
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(t)
            .AppendCallback(() => {
                _holdcard = null;
                _leadcard = null;
                cb();
            });
        }

        protected override void RenderSortCardsAfterFly(Action cb) {
            int count = 0;
            for (int i = 0; i < _cards.Count; i++) {
                if (_cards[i].Value == _holdcard.Value) {
                    continue;
                }
                Vector3 dst = CalcPos(i);
                Sequence s = DOTween.Sequence();
                s.Append(_cards[i].Go.transform.DOMove(dst, _abdicateholddelta))
                    .AppendCallback(() => {
                        count++;
                        if (count >= _cards.Count - 1) {
                            RenderInsert(cb);
                        }
                    });
            }
        }

        protected override void RenderFly(Action cb) {
            float h = 0.05f;
            Vector3 to = CalcPos(_holdcard.Pos);
            to.y = to.y + Card.Length + h;
            Vector3 from = _holdcard.Go.transform.localPosition;

            // x,y 不同
            Vector3[] waypoints = new[] {
                        from,
                        new Vector3((to.x - from.x) * 0.2f + from.x, (to.y - from.y) * 0.2f + from.y, from.z),
                        new Vector3((to.x - from.x) * 0.3f + from.x, (to.y - from.y) * 0.3f + from.y, from.z ),
                        new Vector3((to.x - from.x) * 0.5f + from.x, (to.y - from.y) * 0.5f + from.y, from.z),
                        new Vector3((to.x - from.x) * 0.8f + from.x, (to.y - from.y) * 0.8f + from.y, from.z),
                        to,
                    };
            Tween t = _holdcard.Go.transform.DOPath(waypoints, _holdflydelta).SetOptions(false);

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(t).AppendCallback(() => {
                RenderSortCardsAfterFly(cb);
            });
        }

        protected override void RenderLead() {
            //UnityEngine.Debug.Assert(_leadcard1.Value == _leadcard.Value);
            UnityEngine.Debug.Assert(_leadcards.Count > 0);
            Vector3 dst = CalcLeadPos(_leadcards.Count - 1);
            _leadcard.Go.transform.localPosition = dst;
            _leadcard.Go.transform.localRotation = _upv;
            dst.y = dst.y + 0.1f;
            ((GameController)_controller).Desk.RenderChangeCursor(dst);

            if (_leadcard.Value != _holdcard.Value) {
                _go.GetComponent<global::BottomPlayer>().Remove(_leadcard);
                _go.GetComponent<global::BottomPlayer>().Add(_holdcard);

                if (_holdcard.Pos == (_cards.Count - 1)) {
                    RenderSortCardsToDo(() => {
                        UnityEngine.Debug.LogFormat("bottom player send event lead card");
                        Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                        _ctx.Enqueue(cmd);
                    });
                } else {
                    RenderFly(() => {
                        UnityEngine.Debug.LogFormat("bottom player send event lead card");
                        Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                        _ctx.Enqueue(cmd);
                    });
                }
            } else {
                // 播放出牌动画，手上拿的是刚摸起来的牌
                _go.GetComponent<global::BottomPlayer>().HoldCard = null;
                _holdcard = null;
                _leadcard = null;
                UnityEngine.Debug.LogFormat("bottom player send event lead card");
                Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                _ctx.Enqueue(cmd);
            }
        }

        protected override void RenderCall() {
            if (Call.Gang == OpCodes.OPCODE_ANGANG ||
                Call.Gang == OpCodes.OPCODE_BUGANG ||
                (Call.Hu.Code != HuType.NONE && Call.Hu.Jiao == JiaoType.ZIMO)) {
                Vector3 dst = CalcPos(_cards.Count + 1);
                dst.y = dst.y + Card.Length;
                _holdcard.Go.transform.localPosition = dst;
                _holdcard.Go.transform.localRotation = Quaternion.AngleAxis(-60, Vector3.right);

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_holdcard.Go.transform.DOMoveY(Card.Length / 2.0f, _holddelta))
                    .AppendCallback(() => {
                        if (Call.Peng == OpCodes.OPCODE_PENG) {
                            _go.GetComponent<global::BottomPlayer>().ShowPeng();
                        }
                        if (Call.Gang == OpCodes.OPCODE_ANGANG || Call.Gang == OpCodes.OPCODE_BUGANG || Call.Gang == OpCodes.OPCODE_ZHIGANG) {
                            _go.GetComponent<global::BottomPlayer>().ShowGang();
                        }
                        if (Call.Hu.Code != HuType.NONE) {
                            _go.GetComponent<global::BottomPlayer>().ShowHu();
                        }
                        _go.GetComponent<global::BottomPlayer>().ShowGuo();
                    });

            } else {
                if (Call.Peng == OpCodes.OPCODE_PENG) {
                    _go.GetComponent<global::BottomPlayer>().ShowPeng();
                }
                if (Call.Gang == OpCodes.OPCODE_ANGANG || Call.Gang == OpCodes.OPCODE_BUGANG || Call.Gang == OpCodes.OPCODE_ZHIGANG) {
                    _go.GetComponent<global::BottomPlayer>().ShowGang();
                }
                if (Call.Hu.Code != HuType.NONE) {
                    _go.GetComponent<global::BottomPlayer>().ShowHu();
                }
                _go.GetComponent<global::BottomPlayer>().ShowGuo();
            }
        }

        protected override void RenderPeng() {
            Desk desk = ((GameController)_controller).Desk;
            PGCards pg = _putcards[_putidx];
            UnityEngine.Debug.Assert(pg.Cards.Count == 3);
            float offset = _putrightoffset;
            float move = 0.1f;
            for (int i = 0; i < _putidx; i++) {
                offset += _putcards[i].Width + _putmargin;
            }
            int count = 0;
            pg.Width = 0.0f;
            for (int i = 0; i < pg.Cards.Count; i++) {
                float x = 0.0f;
                float y = Card.Height / 2.0f;
                float z = _putbottomoffset;
                if (i == pg.Hor) {
                    x = desk.Width - (offset + Card.Length / 2.0f + move);
                    z = _putbottomoffset + Card.Width / 2.0f;
                    offset += Card.Length;
                    pg.Width += Card.Length;
                    pg.Cards[i].Go.transform.localRotation = _uph;
                } else {
                    x = desk.Width - (offset + Card.Width / 2.0f + move);
                    z = _putbottomoffset + Card.Length / 2.0f;
                    offset += Card.Width;
                    pg.Width += Card.Width;
                    pg.Cards[i].Go.transform.localRotation = _upv;
                }
                pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z);

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(pg.Cards[i].Go.transform.DOMoveX(x + move, _putmovedelta))
                    .AppendCallback(() => {
                        count++;
                        if (count >= _putcards[_putidx].Cards.Count) {
                            RenderSortCardsToDo(() => {
                                Command cmd = new Command(MyEventCmd.EVENT_PENGCARD);
                                _ctx.Enqueue(cmd);
                            });
                        }
                    });
            }
        }

        protected override void RenderGang() {
            Desk desk = ((GameController)_controller).Desk;
            PGCards pg = _putcards[_putidx];
            float offset = _putrightoffset;
            float move = 0.1f;
            for (int i = 0; i < _putidx; i++) {
                offset += _putcards[i].Width + _putmargin;
            }
            int count = 0;
            pg.Width = 0.0f;
            if (pg.Opcode == OpCodes.OPCODE_ZHIGANG) {

                for (int i = 0; i < pg.Cards.Count; i++) {
                    float x = 0.0f;
                    float y = Card.Height / 2.0f;
                    float z = _putbottomoffset;
                    if (i == pg.Hor) {
                        x = desk.Width - (offset + Card.Length / 2.0f + move);
                        z = _putbottomoffset + Card.Width / 2.0f;
                        offset += Card.Length;
                        pg.Width += Card.Length;
                        pg.Cards[i].Go.transform.localRotation = _uph;
                    } else {
                        x = desk.Width - (offset + Card.Width / 2.0f + move);
                        z = _putbottomoffset + Card.Length / 2.0f;
                        offset += Card.Width;
                        pg.Width += Card.Width;
                        pg.Cards[i].Go.transform.localRotation = _upv;
                    }
                    pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z);

                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(pg.Cards[i].Go.transform.DOMoveX(x + move, _putmovedelta))
                        .AppendCallback(() => {
                            count++;
                            if (count >= _putcards[_putidx].Cards.Count) {
                                RenderSortCardsToDo(() => {
                                    Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                    _ctx.Enqueue(cmd);
                                });
                            }
                        });
                }
            } else if (pg.Opcode == OpCodes.OPCODE_ANGANG) {
                for (int i = 0; i < pg.Cards.Count; i++) {
                    float x = desk.Width - (offset + Card.Width / 2.0f + move);
                    float y = Card.Height / 2.0f;
                    float z = _putbottomoffset + Card.Length / 2.0f;
                    offset += Card.Width;
                    pg.Width += Card.Width;

                    if (i == pg.Hor) {
                        pg.Cards[i].Go.transform.localRotation = _upv;
                    } else {
                        pg.Cards[i].Go.transform.localRotation = _backv;
                    }
                    pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z);

                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(pg.Cards[i].Go.transform.DOMoveX(x + move, _putmovedelta))
                        .AppendCallback(() => {
                            count++;
                            if (count >= _putcards[_putidx].Cards.Count) {
                                if (pg.Cards[3].Value == _holdcard.Value) {
                                    RenderSortCardsToDo(() => {
                                        Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                        _ctx.Enqueue(cmd);
                                    });
                                } else {
                                    if (_holdcard.Pos == (_cards.Count - 1)) {
                                        RenderSortCardsToDo(() => {
                                            Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                            _ctx.Enqueue(cmd);
                                        });
                                    } else {
                                        RenderFly(() => {
                                            Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                            _ctx.Enqueue(cmd);
                                        });
                                    }
                                }
                            }
                        });
                }
            } else if (pg.Opcode == OpCodes.OPCODE_BUGANG) {

                float x = desk.Width - (offset + (Card.Width * pg.Hor) + (Card.Length / 2.0f) + move);
                float y = Card.Height / 2.0f;
                float z = _putbottomoffset + Card.Width + Card.Width / 2.0f;
                pg.Cards[3].Go.transform.localPosition = new Vector3(x, y, z);
                pg.Cards[3].Go.transform.localRotation = _uph;

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(pg.Cards[3].Go.transform.DOMoveX(x + move, _putmovedelta))
                    .AppendCallback(() => {
                        if (_holdcard.Value == pg.Cards[3].Value) {
                            _holdcard = null;
                            Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                            _ctx.Enqueue(cmd);
                        } else {
                            RenderFly(() => {
                                Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                _ctx.Enqueue(cmd);
                            });
                        }
                    });

            } else {
                UnityEngine.Debug.Assert(false);
            }
        }

        protected override void RenderHu() {
            _go.GetComponent<global::BottomPlayer>().Head.SetHu(true);
            Command cmd = new Command(MyEventCmd.EVENT_HUCARD);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderWinAndLose() {
            _go.GetComponent<global::BottomPlayer>().Head.ShowTips(string.Format("{0}", _wal));
        }

        protected override void RenderOver() {
            Desk desk = ((GameController)_controller).Desk;
            for (int i = 0; i < _cards.Count; i++) {
                float x = _leftoffset + Card.Width * i + Card.Width / 2.0f;
                float y = Card.Height / 2.0f;
                float z = _bottomoffset + Card.Length / 2.0f;

                _cards[i].Go.transform.localPosition = new Vector3(x, y, z);
                _cards[i].Go.transform.localRotation = _upv;
            }
        }

        protected override void RenderRestart() {
            _go.GetComponent<global::BottomPlayer>().Head.CloseTips();
            _go.GetComponent<global::BottomPlayer>().Head.CloseWAL();
            _go.GetComponent<global::BottomPlayer>().Head.SetHu(false);
            _go.GetComponent<global::BottomPlayer>().Head.SetReady(true);
        }

        protected override void RenderSay() {
            _go.GetComponent<global::BottomPlayer>().Say(_say);
        }
    }
}
