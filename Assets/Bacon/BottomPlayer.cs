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

        public BottomPlayer(Context ctx, GameService service) : base(ctx, service) {

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_BOTTOMPLAYER, OnSetup);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_PENG, OnSendPeng);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);

            EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_GANG, OnSendGang);
            _ctx.EventDispatcher.AddCmdEventListener(listener3);

            EventListenerCmd listener4 = new EventListenerCmd(MyEventCmd.EVENT_HU, OnSendHu);
            _ctx.EventDispatcher.AddCmdEventListener(listener4);

            EventListenerCmd listener5 = new EventListenerCmd(MyEventCmd.EVENT_GANG, OnSendGuo);
            _ctx.EventDispatcher.AddCmdEventListener(listener5);

            EventListenerCmd listener6 = new EventListenerCmd(MyEventCmd.EVENT_LEAD, OnSendLead);
            _ctx.EventDispatcher.AddCmdEventListener(listener6);

        }

        private void OnSetup(EventCmd e) {
            _go = e.Orgin;
            ((GameController)_controller).SendStep();
            _ctx.EnqueueRenderQueue(RenderSetup);
        }

        private void RenderSetup() {
            _go.GetComponent<global::BottomPlayer>().ShowUI();
        }

        private Vector3 CalcPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            float x = _leftoffset + Card.Width * pos + Card.Width / 2.0f;
            float y = Card.Length / 2.0f;
            float z = _bottomoffset + Card.Height / 2.0f;

            return new Vector3(x, y, z);
        }

        private Vector3 CalcLeadPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            int row = (pos + 1) / 6;
            int col = (pos + 1) % 6;

            float x = _leadleftoffset + Card.Width * col + Card.Width / 2.0f;
            float y = Card.Height / 2.0f;
            float z = _leadbottomoffset + Card.Length / 2.0f - row * Card.Length;

            return new Vector3(x, y, z);
        }

        private void OnSendPeng(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.idx = _idx;
            request.opcode = OpCodes.OPCODE_PENG;
            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendGang(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.idx = _idx;
            request.opcode = _gangcode;
            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendGuo(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.idx = _idx;
            request.opcode = OpCodes.OPCODE_GUO;
            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendHu(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.idx = _idx;
            request.opcode = OpCodes.OPCODE_HU;
            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendLead(EventCmd e) {
            Card card = null;
            for (int i = 0; i < _cards.Count; i++) {
                if (_cards[i].Go == e.Orgin) {
                    card = _cards[i];
                }
            }
            UnityEngine.Debug.Assert(card != null);
            _leadcard1 = card;

            C2sSprotoType.lead.request request = new C2sSprotoType.lead.request();
            request.idx = _idx;
            request.card = card.Value;
            _ctx.SendReq<C2sProtocol.lead>(C2sProtocol.lead.Tag, request);
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
                var card = _cards[i];
                float x = _leftoffset + Card.Width * i + Card.Width / 2.0f;
                float y = Card.Length / 2.0f;
                float z = _bottomoffset + Card.Height / 2.0f;
                card.Go.transform.localPosition = new Vector3(x, y, z);
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
                            float x = _leftoffset + Card.Width * j + Card.Width / 2.0f;
                            float y = Card.Length / 2.0f;
                            float z = _bottomoffset + Card.Height / 2.0f;

                            _cards[j].Go.transform.localPosition = new Vector3(x, y, z);
                            _go.GetComponent<global::BottomPlayer>().Add(_cards[j]);
                        }
                    })
                    .Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(-60.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        count++;
                        UnityEngine.Debug.LogFormat("bottom count {0}", count);
                        if (count >= (_cards.Count - 1)) {
                            UnityEngine.Debug.LogFormat("bottom player send event sortcards");
                            Command cmd = new Command(MyEventCmd.EVENT_SORTCARDS);
                            _ctx.Enqueue(cmd);
                        }
                    });
            }
        }

        protected override void RenderTakeTurn() {

            float x = _leftoffset + Card.Width * (_cards.Count + 1) + Card.Width / 2.0f + _holdleftoffset;
            float y = Card.Length / 2.0f + Card.Length;
            float z = _bottomoffset + Card.Height / 2.0f;

            _holdcard.Go.transform.localPosition = new Vector3(x, y, z);
            _holdcard.Go.transform.localRotation = Quaternion.AngleAxis(-60, Vector3.right);

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(_holdcard.Go.transform.DOMoveY(Card.Length / 2.0f, _holddelta))
                .AppendCallback(() => {
                    _go.GetComponent<global::BottomPlayer>().SwitchOnTouch();
                });
        }

        protected override void RenderInsert() {
            int count = 0;
            for (int i = 0; i < _cards.Count; i++) {
                if (_cards[i].Value == _holdcard.Value) {
                    continue;
                }
                float x = _leftoffset + Card.Width * i + Card.Width / 2.0f;
                float y = Card.Length / 2.0f;
                float z = _bottomoffset + Card.Height / 2.0f;

                Sequence s = DOTween.Sequence();
                s.Append(_cards[i].Go.transform.DOMove(new Vector3(x, y, z), 0.1f))
                    .AppendCallback(() => {
                        count++;
                        if (count >= _cards.Count - 2) {

                            float dx = _leftoffset + Card.Width * _holdcard.Pos + Card.Width / 2.0f;
                            float dy = Card.Length / 2.0f;
                            float dz = _bottomoffset + Card.Height / 2.0f;

                            Sequence mySequence = DOTween.Sequence();
                            Tween t = _holdcard.Go.transform.DOMove(new Vector3(dx, dy, dz), 0.1f);
                            mySequence.Append(t)
                            .AppendCallback(() => {
                                Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                                _ctx.Enqueue(cmd);
                            });
                        }
                    });
            }
        }

        protected override void RenderLead() {
            UnityEngine.Debug.Assert(_leadcard1 == _leadcard);
            UnityEngine.Debug.Assert(_leadcards.Count > 0);

            int row = _leadcards.Count / 6;
            int col = _leadcards.Count % 6;

            float x = _leadleftoffset + Card.Width * col + Card.Width / 2.0f;
            float y = Card.Height / 2.0f;
            float z = _leadbottomoffset + Card.Length / 2.0f - row * Card.Length;

            Desk desk = ((GameController)_controller).Desk;
            _leadcard.Go.transform.localPosition = new Vector3(x, y, z);
            _leadcard.Go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);

            float h = 0.05f;
            if (_leadcard.Value != _holdcard.Value) {
                float hx = _leftoffset + Card.Width * _holdcard.Pos + Card.Width / 2.0f;
                float hy = Card.Length / 2.0f + Card.Length + h;
                float hz = _bottomoffset + Card.Height / 2.0f;

                float ox = _holdcard.Go.transform.localPosition.x;
                float oy = _holdcard.Go.transform.localPosition.y;
                float oz = _holdcard.Go.transform.localPosition.z;

                Vector3[] waypoints = new[] {
                    new Vector3(hx, (hy - oy) * 0.8f + oy, hz),
                    new Vector3(hx, (hy - oy) * 0.7f + oy, hz),
                    new Vector3(hx, (hy - oy) * 0.6f + oy, hz),
                    new Vector3(hx, (hy - oy) * 0.5f + oy, hz),
                    new Vector3(hx, (hy - oy) * 0.3f + oy, hz),
                };
                Tween t = _holdcard.Go.transform.DOPath(waypoints, 1f).SetOptions(true);

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(t).AppendCallback(() => {
                    RenderInsert();
                });
            }
        }

        protected override void RenderPeng() {
            Desk desk = ((GameController)_controller).Desk;
            PGCards pg = _putcards[_putidx];
            float offset = _putrightoffset;
            float move = 0.1f;
            for (int i = 0; i < _putidx; i++) {
                offset += _putcards[i].Width + _putmargin;
            }
            int count = 0;
            pg.Width = 0.0f;
            for (int i = 0; i < pg.Cards.Count; i++) {
                float y = Card.Height / 2.0f;
                float z = _putbottomoffset;
                float x = 0.0f;
                if (i == pg.Hor) {
                    x = desk.Width - (offset + Card.Length / 2.0f) - move;
                    z = _putbottomoffset + Card.Width / 2.0f;
                    offset += Card.Length;
                    pg.Width += Card.Length;
                    pg.Cards[i].Go.transform.localRotation = Quaternion.AngleAxis(-90.0f, Vector3.up);
                } else {
                    x = desk.Width - (offset + Card.Width / 2.0f) - move;
                    z = _putbottomoffset + Card.Length / 2.0f;
                    offset += Card.Width;
                    pg.Width += Card.Width;
                    pg.Cards[i].Go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
                }

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(pg.Cards[i].Go.transform.DOMoveX(x + move, 0.1f))
                    .AppendCallback(() => {
                        count++;
                        if (count >= (_putcards[_putidx].Cards.Count - 1)) {
                            Command cmd = new Command(MyEventCmd.EVENT_PENGCARD);
                            _ctx.Enqueue(cmd);
                        }
                    });
            }
        }

        protected override void RenderGang() {

            Desk desk = ((GameController)_controller).Desk;
            PGCards pg = _putcards[_putidx];
            if (pg.Opcode == OpCodes.OPCODE_ZHIGANG) {
                float offset = _putrightoffset;
                float move = 0.1f;
                for (int i = 0; i < _putidx; i++) {
                    offset += _putcards[i].Width + _putmargin;
                }
                int count = 0;
                pg.Width = 0.0f;
                for (int i = 0; i < pg.Cards.Count; i++) {
                    float y = Card.Height / 2.0f;
                    float z = _putbottomoffset;
                    float x = 0.0f;
                    if (i == pg.Hor) {
                        x = desk.Width - (offset + Card.Length / 2.0f) - move;
                        z = _putbottomoffset + Card.Width / 2.0f;
                        offset += Card.Length;
                        pg.Width += Card.Length;
                        pg.Cards[i].Go.transform.localRotation = Quaternion.AngleAxis(-90.0f, Vector3.up);
                    } else {
                        x = desk.Width - (offset + Card.Width / 2.0f) - move;
                        z = _putbottomoffset + Card.Length / 2.0f;
                        offset += Card.Width;
                        pg.Width += Card.Width;
                        pg.Cards[i].Go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
                    }

                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(pg.Cards[i].Go.transform.DOMoveX(x + move, 0.1f))
                        .AppendCallback(() => {
                            count++;
                            if (count >= (_putcards[_putidx].Cards.Count - 1)) {
                                Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                _ctx.Enqueue(cmd);
                            }
                        });
                }
            } else if (pg.Opcode == OpCodes.OPCODE_ANGANG) {
                float offset = _putrightoffset;
                float move = 0.1f;
                for (int i = 0; i < _putidx; i++) {
                    offset += _putcards[i].Width + _putmargin;
                }
                int count = 0;
                pg.Width = 0.0f;
                for (int i = 0; i < pg.Cards.Count; i++) {
                    float x = desk.Width - (offset + Card.Width / 2.0f) - move;
                    float y = Card.Height / 2.0f;
                    float z = _putbottomoffset + Card.Length / 2.0f;
                    offset += Card.Width;
                    pg.Width += Card.Width;

                    if (i == 0) {
                        pg.Cards[i].Go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
                    } else {
                        pg.Cards[i].Go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up) * Quaternion.AngleAxis(180.0f, Vector3.forward);
                    }
                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(pg.Cards[i].Go.transform.DOMoveX(x + move, 0.1f))
                        .AppendCallback(() => {
                            count++;
                            if (count >= (_putcards[_putidx].Cards.Count - 1)) {
                                Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                _ctx.Enqueue(cmd);
                            }
                        });
                }
            } else if (pg.Opcode == OpCodes.OPCODE_BUGANG) {
                float offset = _putrightoffset;
                float move = 0.1f;
                for (int i = 0; i < _putidx; i++) {
                    offset += _putcards[i].Width + _putmargin;
                }

                float x = desk.Width - (offset + (Card.Width * pg.Hor) + Card.Width / 2.0f) - move;
                float y = Card.Height / 2.0f;
                float z = _putbottomoffset + Card.Width + Card.Width / 2.0f;

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(pg.Cards[3].Go.transform.DOMoveX(x + move, 0.1f))
                    .AppendCallback(() => {
                        Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                        _ctx.Enqueue(cmd);
                    });
            } else {
                UnityEngine.Debug.Assert(false);
            }
        }

        protected override void RenderHu() { }

        protected override void RenderCall() {
            UnityEngine.Debug.Assert(_opcodes.Count > 0);
            for (int i = 0; i < _opcodes.Count; i++) {
                if (_opcodes[i] == OpCodes.OPCODE_PENG) {
                    _go.GetComponent<global::BottomPlayer>().ShowPeng();
                } else if (_opcodes[i] == OpCodes.OPCODE_ANGANG) {
                    _go.GetComponent<global::BottomPlayer>().ShowGang();
                } else if (_gangcode == OpCodes.OPCODE_ZHIGANG) {
                    _go.GetComponent<global::BottomPlayer>().ShowGang();
                } else if (_gangcode == OpCodes.OPCODE_BUGANG) {
                    _go.GetComponent<global::BottomPlayer>().ShowGang();
                } else if (_opcodes[i] == OpCodes.OPCODE_HU) {
                    _go.GetComponent<global::BottomPlayer>().ShowGang();
                }
            }
            _go.GetComponent<global::BottomPlayer>().ShowGuo();
        }

    }
}
