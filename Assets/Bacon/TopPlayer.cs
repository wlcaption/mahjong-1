using Bacon;
using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

namespace Bacon {
    class TopPlayer : Player {
        public TopPlayer(Context ctx, GameService service) : base(ctx, service) {
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_TOPPLAYER, OnSetup);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        private void OnSetup(EventCmd e) {
            _go = e.Orgin;
            ((GameController)_controller).SendStep();
            _ctx.EnqueueRenderQueue(RenderSetup);
        }

        private void RenderSetup() {
            _go.GetComponent<global::TopPlayer>().ShowUI();
        }

        private Vector3 CalcPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            float x = desk.Width - (_leftoffset + Card.Width * pos + Card.Width / 2.0f);
            float y = Card.Length / 2.0f;
            float z = desk.Length - (_bottomoffset + Card.Height / 2.0f);
            return new Vector3(x, y, z);
        }

        private Vector3 CalcLeadPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            int row = (pos + 1) / 6;
            int col = (pos + 1) % 6;

            float x = desk.Width - (_leadleftoffset + Card.Width * col + Card.Width / 2.0f);
            float y = Card.Height / 2.0f;
            float z = desk.Length - (_leadbottomoffset + Card.Length / 2.0f - row * Card.Length);

            return new Vector3(x, y, z);
        }

        protected override void RenderBoxing() {
            try {
                Desk desk = ((GameController)_controller).Desk;
                for (int i = 0; i < _takecards.Count; i++) {
                    int idx = i / 2;
                    float x = _takeleftoffset + idx * Card.Width + Card.Width / 2.0f;
                    float y = Card.Height / 2.0f;
                    float z = desk.Length - (_takebottomoffset + Card.Length / 2.0f);
                    if (i % 2 == 0) {
                        y = Card.Height / 2.0f + Card.Height;
                    } else if (i % 2 == 1) {
                        y = Card.Height / 2.0f;
                    }
                    Card card = _takecards[i];
                    card.Go.transform.localPosition = new UnityEngine.Vector3(x, y, z);
                    card.Go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(180.0f, Vector3.forward);
                }
                Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_BOXINGCARDS);
                _ctx.Enqueue(cmd);
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
            }
        }

        protected override void RenderThrowDice() {
            base.RenderThrowDice();
        }

        protected override void RenderDeal() {
            Desk desk = ((GameController)_controller).Desk;
            int i = 0;
            if (_cards.Count == 13) {
                i = 12;
            } else {
                i = _cards.Count - 4;
            }
            for (; i < _cards.Count; i++) {
                var card = _cards[i];
                float x = desk.Width - (_leftoffset + Card.Width * i + Card.Width / 2.0f);
                float y = Card.Length / 2.0f;
                float z = desk.Length - (_bottomoffset + Card.Height / 2.0f);
                card.Go.transform.localPosition = new Vector3(x, y, z);
                card.Go.transform.localRotation = Quaternion.AngleAxis(180, Vector3.up) * Quaternion.AngleAxis(-80, Vector3.right);
            }
            Command cmd = new Command(MyEventCmd.EVENT_TAKEDEAL);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderSortCards() {
            Desk desk = ((GameController)_controller).Desk;
            int count = 0;
            int i = 0;
            for (; i < _cards.Count; i++) {

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(-120.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        for (int j = 0; j < _cards.Count; j++) {
                            float x = desk.Width - (_leftoffset + Card.Width * j + Card.Width / 2.0f);
                            float y = Card.Length / 2.0f;
                            float z = desk.Length - (_bottomoffset + Card.Height / 2.0f);

                            _cards[j].Go.transform.localPosition = new Vector3(x, y, z);
                        }
                    })
                    .Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(-90.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        count++;
                        if (count >= (_cards.Count - 1)) {
                            UnityEngine.Debug.LogFormat("player top send sort cards");
                            Command cmd = new Command(MyEventCmd.EVENT_SORTCARDS);
                            _ctx.Enqueue(cmd);
                        }
                    });
            }
        }

        protected override void RenderTakeTurn() {
            Desk desk = ((GameController)_controller).Desk;
            float x = desk.Width - (_leftoffset + Card.Width * (_cards.Count + 1) + Card.Width / 2.0f + _holdleftoffset);
            float y = Card.Length / 2.0f + Card.Length;
            float z = desk.Length - (_bottomoffset + Card.Height / 2.0f);

            _holdcard.Go.transform.localPosition = new Vector3(x, y, z);
            _holdcard.Go.transform.localRotation = Quaternion.AngleAxis(180, Vector3.up) * Quaternion.AngleAxis(-80, Vector3.right);

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(_holdcard.Go.transform.DOMoveY(Card.Length / 2.0f, _holddelta));
        }

        protected override void RenderInsert() {
            Desk desk = ((GameController)_controller).Desk;
            int count = 0;
            for (int i = 0; i < _cards.Count; i++) {
                if (_cards[i].Value == _holdcard.Value) {
                    continue;
                }
                float x = desk.Width - (_leftoffset + Card.Width * i + Card.Width / 2.0f);
                float y = Card.Length / 2.0f;
                float z = desk.Length - (_bottomoffset + Card.Height / 2.0f);
                Sequence s = DOTween.Sequence();
                s.Append(_cards[i].Go.transform.DOMove(new Vector3(x, y, z), 0.1f))
                    .AppendCallback(() => {
                        count++;
                        if (count >= _cards.Count - 2) {
                            float hx = desk.Width - (_leftoffset + Card.Width * _holdcard.Pos + Card.Width / 2.0f);
                            float hy = Card.Length / 2.0f;
                            float hz = desk.Length - (_bottomoffset + Card.Height / 2.0f);

                            Sequence mySequence = DOTween.Sequence();
                            Tween t = _holdcard.Go.transform.DOMove(new Vector3(hx, hy, hz), 0.1f);
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
            UnityEngine.Debug.Assert(_leadcards.Count > 0);
            Desk desk = ((GameController)_controller).Desk;

            int row = _leadcards.Count / 6;
            int col = _leadcards.Count % 6;

            float x = desk.Width - (_leadleftoffset + Card.Width * col + Card.Width / 2.0f);
            float y = Card.Height / 2.0f;
            float z = desk.Length - (_leadbottomoffset + Card.Length / 2.0f - row * Card.Length);

            _leadcard.Go.transform.localPosition = new Vector3(x, y, z);
            _leadcard.Go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up);

            float h = 0.05f;
            if (_leadcard.Value != _holdcard.Value) {
                float hx = desk.Width - (_leftoffset + Card.Width * _holdcard.Pos + Card.Width / 2.0f);
                float hy = Card.Length / 2.0f + Card.Length + h;
                float hz = desk.Length - (_bottomoffset + Card.Height / 2.0f);

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
                float x = 0.0f;
                float y = Card.Height / 2.0f;
                float z = _putbottomoffset;
                if (i == pg.Hor) {
                    x = offset + Card.Length / 2.0f + move;
                    z = _putbottomoffset + Card.Width / 2.0f;
                    offset += Card.Length;
                    pg.Width += Card.Length;
                    pg.Cards[i].Go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up);
                } else {
                    x = offset + Card.Width / 2.0f + move;
                    z = _putbottomoffset + Card.Length / 2.0f;
                    offset += Card.Width;
                    pg.Width += Card.Width;
                    pg.Cards[i].Go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up);
                }
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(pg.Cards[i].Go.transform.DOMoveX(x - move, 0.1f))
                    .AppendCallback(() => {
                        count++;
                        if (count >= (pg.Cards.Count - 1)) {
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
                    float x = 0.0f;
                    float y = Card.Height / 2.0f;
                    float z = _putbottomoffset;
                    if (i == pg.Hor) {
                        x = offset + Card.Length / 2.0f + move;
                        z = _putbottomoffset + Card.Width / 2.0f;
                        offset += Card.Length;
                        pg.Width += Card.Length;
                        pg.Cards[i].Go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up);
                    } else {
                        x = offset + Card.Width / 2.0f + move;
                        z = _putbottomoffset + Card.Length / 2.0f;
                        offset += Card.Width;
                        pg.Width += Card.Width;
                        pg.Cards[i].Go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up);
                    }
                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(pg.Cards[i].Go.transform.DOMoveX(x - move, 0.1f))
                        .AppendCallback(() => {
                            count++;
                            if (count >= (pg.Cards.Count - 1)) {
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
                    float x = offset + Card.Width / 2.0f + move;
                    float y = Card.Height / 2.0f;
                    float z = _putbottomoffset + Card.Length / 2.0f;
                    offset += Card.Width;
                    pg.Width += Card.Width;

                    if (i == pg.Hor) {
                        pg.Cards[i].Go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up);
                    } else {
                        pg.Cards[i].Go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(180.0f, Vector3.forward);
                    }
                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(pg.Cards[i].Go.transform.DOMoveX(x - move, 0.1f))
                        .AppendCallback(() => {
                            count++;
                            if (count >= (pg.Cards.Count - 1)) {
                                Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                _ctx.Enqueue(cmd);
                            }
                        });
                }
            } else if (true) {
                float offset = _putrightoffset;
                float move = 0.1f;
                for (int i = 0; i < _putidx; i++) {
                    offset += _putcards[i].Width + _putmargin;
                }

                float x = offset + Card.Width * pg.Hor + Card.Width / 2.0f + move;
                float y = Card.Height / 2.0f;
                float z = _putbottomoffset + Card.Width / 2.0f + Card.Width;

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(pg.Cards[3].Go.transform.DOMoveX(x - move, 0.1f))
                    .AppendCallback(() => {
                        Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                        _ctx.Enqueue(cmd);
                    });
            }
        }

        protected override void RenderHu() {
        }

    }
}
