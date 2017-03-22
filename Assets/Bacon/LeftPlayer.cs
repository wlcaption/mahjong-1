using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

namespace Bacon {
    class LeftPlayer : Player {

        public LeftPlayer(Context ctx, GameService service)
            : base(ctx, service) {
            _upv = Quaternion.AngleAxis(90.0f, Vector3.up);
            _uph = Quaternion.AngleAxis(0.0f, Vector3.up);
            _downv = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(180.0f, Vector3.forward);
            _backv = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(-90.0f, Vector3.right);

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_LEFTPLAYER, OnSetup);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        private void OnSetup(EventCmd e) {
            _go = e.Orgin;
            ((GameController)_controller).SendStep();
            _ctx.EnqueueRenderQueue(RenderSetup);
        }

        private void RenderSetup() {
            _go.GetComponent<global::LeftPlayer>().ShowUI();
        }

        protected override Vector3 CalcPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            float x = _bottomoffset + Card.Height / 2.0f;
            float y = Card.Length / 2.0f;
            float z = desk.Length - (_leftoffset + Card.Width * pos + Card.Width / 2.0f);

            return new Vector3(x, y, z);
        }

        protected override Vector3 CalcLeadPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            int row = pos / 6;
            int col = pos % 6;

            float x = _leadbottomoffset - (Card.Length * row) - (Card.Length / 2.0f);
            float y = Card.Height / 2.0f;
            float z = desk.Length - (_leadleftoffset + (Card.Width * col) + (Card.Width / 2.0f));

            return new Vector3(x, y, z);
        }

        protected override void RenderBoxing() {
            try {
                Desk desk = ((GameController)_controller).Desk;
                for (int i = 0; i < _takecards.Count; i++) {
                    int idx = i / 2;
                    float x = _takebottomoffset + Card.Length / 2.0f;
                    float y = Card.Height / 2.0f;
                    float z = _takeleftoffset + idx * Card.Width + Card.Width / 2.0f;
                    if (i % 2 == 0) {
                        y = Card.Height / 2.0f + Card.Height;
                    } else if (i % 2 == 1) {
                        y = Card.Height;
                    }
                    Card card = _takecards[i];
                    card.Go.transform.localPosition = new UnityEngine.Vector3(x, y, z);
                    card.Go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(180.0f, Vector3.forward);
                }

                Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_BOXINGCARDS);
                _ctx.Enqueue(cmd);
            } catch (NullReferenceException ex) {
                UnityEngine.Debug.LogException(ex);
            }
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
                card.Go.transform.localRotation = _backv;
            }
            Command cmd = new Command(MyEventCmd.EVENT_TAKEDEAL);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderSortCards() {
            int count = 0;
            Desk desk = ((GameController)_controller).Desk;
            for (int i = 0; i < _cards.Count; i++) {

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(-120.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        for (int j = 0; j < _cards.Count; j++) {
                            Vector3 dst = CalcPos(j);
                            _cards[j].Go.transform.localPosition = dst;
                        }
                    })
                    .Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(-90.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        count++;
                        if (count >= _cards.Count) {
                            UnityEngine.Debug.LogFormat("player left send sortcards");
                            Command cmd = new Command(MyEventCmd.EVENT_SORTCARDS);
                            _ctx.Enqueue(cmd);
                        }
                    });
            }
        }

        protected override void RenderTakeXuanPao() { }

        protected override void RenderXuanPao() {
            _go.GetComponent<global::LeftPlayer>().Head.SetMark(string.Format("{0}", _fen));
        }

        protected override void RenderTakeFirstCard() {

            UnityEngine.Debug.Assert(_takefirst);
            UnityEngine.Debug.Assert(_holdcard != null);
            Vector3 dst = CalcPos(_cards.Count + 1);
            dst.y = dst.y + Card.Length;
            _holdcard.Go.transform.localPosition = dst;
            _holdcard.Go.transform.localRotation = _backv;

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(_holdcard.Go.transform.DOMoveY(Card.Length / 2.0f, _holddelta))
                .AppendCallback(() => {
                    Command cmd = new Command(MyEventCmd.EVENT_TAKEFIRSTCARD);
                    _ctx.Enqueue(cmd);
                });
        }

        protected override void RenderTakeXuanQue() {
        }

        protected override void RenderXuanQue() {
            if (_que == Card.CardType.Bam) {
                _go.GetComponent<global::LeftPlayer>().Head.SetMark("条");
            } else if (_que == Card.CardType.Crak) {
                _go.GetComponent<global::LeftPlayer>().Head.SetMark("万");
            } else if (_que == Card.CardType.Dot) {
                _go.GetComponent<global::LeftPlayer>().Head.SetMark("同");
            }
            RenderSortCardsToDo(() => {
            });
        }

        protected override void RenderTakeTurn() {
            if (_turntype == 1) {
                Vector3 dst = CalcPos(_cards.Count + 1);
                dst.y = dst.y + Card.Length;
                _holdcard.Go.transform.localPosition = dst;
                _holdcard.Go.transform.localRotation = _backv;

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_holdcard.Go.transform.DOMoveY(Card.Length / 2.0f, _holddelta));
            } else if (_turntype == 0) {
                Vector3 dst = CalcPos(_cards.Count + 1);
                _holdcard.Go.transform.localRotation = _backv;

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_holdcard.Go.transform.DOMoveY(Card.Length / 2.0f, _holddelta));
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
            Desk desk = ((GameController)_controller).Desk;
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

                            Vector3 to = CalcPos(_holdcard.Pos);
                            Tween t = _holdcard.Go.transform.DOMove(to, _holddowndelat);
                            Sequence mySequence = DOTween.Sequence();
                            mySequence.Append(t)
                            .AppendCallback(() => {
                                _holdcard = null;
                                _leadcard = null;
                                UnityEngine.Debug.LogFormat("left player send event lead card");
                                Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                                _ctx.Enqueue(cmd);
                            });
                        }
                    });
            }
        }

        protected override void RenderFly(Action cb) {
            float h = 0.05f;
            Vector3 to = CalcPos(_holdcard.Pos);
            to.y = to.y + Card.Length + h;
            Vector3 from = _holdcard.Go.transform.localPosition;

            Vector3[] waypoints = new[] {
                    from,
                    new Vector3(from.x, (to.y - from.y) * 0.2f + from.y, (to.z - from.z) * 0.2f + from.z),
                    new Vector3(from.x, (to.y - from.y) * 0.3f + from.y, (to.z - from.z) * 0.3f + from.z),
                    new Vector3(from.x, (to.y - from.y) * 0.5f + from.y, (to.z - from.z) * 0.5f + from.z),
                    new Vector3(from.x, (to.y - from.y) * 0.8f + from.y, (to.z - from.z) * 0.8f + from.z),
                    to,
                };
            Tween t = _holdcard.Go.transform.DOPath(waypoints, _holdflydelta).SetOptions(false);
            t.SetEase(Ease.Linear).SetLoops(1);

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(t).AppendCallback(() => {
                RenderSortCardsAfterFly(cb);
            });
        }

        protected override void RenderLead() {
            // 设置好出牌位置后
            UnityEngine.Debug.Assert(_leadcards.Count > 0);
            Vector3 dst = CalcLeadPos(_leadcards.Count - 1);
            _leadcard.Go.transform.localPosition = dst;
            _leadcard.Go.transform.localRotation = _upv;
            dst.y = dst.y + 0.1f;
            ((GameController)_controller).Desk.RenderChangeCursor(dst);

            // 可能更新插牌位置
            if (_leadcard.Value != _holdcard.Value) {
                if (_holdcard.Pos == (_cards.Count - 1)) {
                    RenderSortCardsToDo(() => {
                        UnityEngine.Debug.LogFormat("left player send event lead card");
                        Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                        _ctx.Enqueue(cmd);
                    });
                } else {
                    RenderFly(() => {
                        UnityEngine.Debug.LogFormat("left player send event lead card");
                        Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                        _ctx.Enqueue(cmd);
                    });
                }
            } else {
                // 播放
                _holdcard = null;
                _leadcard = null;
                UnityEngine.Debug.LogFormat("left player send event lead card");
                Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                _ctx.Enqueue(cmd);
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
                float x = _putbottomoffset;
                float y = Card.Height / 2.0f;
                float z = 0.0f;
                if (i == pg.Hor) {
                    x = _putbottomoffset + Card.Width / 2.0f;
                    z = offset + Card.Length / 2.0f + move;
                    offset += Card.Length;
                    pg.Width += Card.Length;
                    pg.Cards[i].Go.transform.localRotation = _uph;
                } else {
                    x = _putbottomoffset + Card.Length / 2.0f;
                    z = offset + Card.Width / 2.0f + move;
                    offset += Card.Width;
                    pg.Width += Card.Width;
                    pg.Cards[i].Go.transform.localRotation = _upv;
                }
                pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z);

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(pg.Cards[i].Go.transform.DOMoveZ(z - move, _putmovedelta))
                    .AppendCallback(() => {
                        count++;
                        if (count >= pg.Cards.Count) {
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
                    float x = _putbottomoffset;
                    float y = Card.Height / 2.0f;
                    float z = 0.0f;
                    if (i == pg.Hor) {
                        x = _putbottomoffset + Card.Width / 2.0f;
                        z = offset + Card.Length / 2.0f + move;
                        offset += Card.Length;
                        pg.Width += Card.Length;
                        pg.Cards[i].Go.transform.localRotation = _uph;
                    } else {
                        x = _putbottomoffset + Card.Length / 2.0f;
                        z = offset + Card.Width / 2.0f + move;
                        offset += Card.Width;
                        pg.Width += Card.Width;
                        pg.Cards[i].Go.transform.localRotation = _upv;
                    }
                    pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z);

                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(pg.Cards[i].Go.transform.DOMoveZ(z - move, _putmovedelta))
                        .AppendCallback(() => {
                            count++;
                            if (count >= pg.Cards.Count) {
                                RenderSortCardsToDo(() => {
                                    Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                    _ctx.Enqueue(cmd);
                                });
                            }
                        });
                }
            } else if (pg.Opcode == OpCodes.OPCODE_ANGANG) {
                for (int i = 0; i < pg.Cards.Count; i++) {
                    float x = _putbottomoffset + Card.Length / 2.0f;
                    float y = Card.Height / 2.0f;
                    float z = offset + Card.Width / 2.0f + move;
                    offset += Card.Width;
                    pg.Width += Card.Width;
                    if (i == 0) {
                        pg.Cards[i].Go.transform.localRotation = _upv;
                    } else {
                        pg.Cards[i].Go.transform.localRotation = _downv;
                    }
                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(pg.Cards[i].Go.transform.DOMoveZ(x - move, 0.1f))
                        .AppendCallback(() => {
                            count++;
                            if (count >= pg.Cards.Count) {
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
                float x = _putbottomoffset + Card.Width / 2.0f + Card.Width;
                float y = Card.Height / 2.0f;
                float z = offset + Card.Width * pg.Hor + Card.Length / 2.0f + move;
                pg.Cards[3].Go.transform.localPosition = new Vector3(x, y, z);
                pg.Cards[3].Go.transform.localRotation = _uph;

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(pg.Cards[3].Go.transform.DOMoveZ(z - move, 0.1f))
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
            _go.GetComponent<global::LeftPlayer>().Head.SetHu(true);
            Command cmd = new Command(MyEventCmd.EVENT_HUCARD);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderWinAndLose() {
            _go.GetComponent<global::LeftPlayer>().Head.ShowWAL(string.Format("{0}", _wal));
        }

        protected override void RenderOver() {
            Desk desk = ((GameController)_controller).Desk;
            for (int i = 0; i < _cards.Count; i++) {
                float x = _bottomoffset + Card.Length / 2.0f;
                float y = Card.Height / 2.0f;
                float z = desk.Length - (_leftoffset + Card.Width * i + Card.Width / 2.0f);
                _cards[i].Go.transform.localPosition = new Vector3(x, y, z);
                _cards[i].Go.transform.localRotation = _upv;
            }
        }

        protected override void RenderSay() {
            _go.GetComponent<global::LeftPlayer>().Say(_say);
        }
    }
}
