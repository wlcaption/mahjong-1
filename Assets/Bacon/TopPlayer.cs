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

        private global::TopPlayer _com;

        public TopPlayer(Context ctx, GameService service) : base(ctx, service) {
            _upv = Quaternion.AngleAxis(180.0f, Vector3.up);
            _uph = Quaternion.AngleAxis(90.0f, Vector3.up);
            _downv = Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(180.0f, Vector3.forward);
            _backv = Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(-90.0f, Vector3.right);

            _ori = Orient.TOP;

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_TOPPLAYER, OnSetup);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        private void OnSetup(EventCmd e) {
            _go = e.Orgin;
            _ctx.EnqueueRenderQueue(RenderSetup);
        }

        private void RenderSetup() {
            _com = _go.GetComponent<global::TopPlayer>();
            _com.ShowUI();
            _com.Head.SetGold(_chip);
        }

        protected override Vector3 CalcPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            float x = desk.Width - (_leftoffset + Card.Width * pos + Card.Width / 2.0f);
            float y = Card.Length / 2.0f;
            float z = desk.Length - (_bottomoffset + Card.Height / 2.0f);
            return new Vector3(x, y, z);
        }

        protected override Vector3 CalcLeadPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            int row = pos / 6;
            int col = pos % 6;

            float x = desk.Width - (_leadleftoffset + (Card.Width * col) + (Card.Width / 2.0f));
            float y = Card.Height / 2.0f;
            float z = desk.Length - (_leadbottomoffset - (Card.Length * row) - Card.Length / 2.0f);

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
                    card.Go.transform.localRotation = _downv;
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
            int i = 0;
            for (; i < _cards.Count; i++) {
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(-120.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        for (int j = 0; j < _cards.Count; j++) {
                            Vector3 dst = CalcPos(j);
                            _cards[j].Go.transform.localPosition = dst;
                        }
                    })
                    .Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(-90.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        count++;
                        if (count >= _cards.Count) {
                            UnityEngine.Debug.LogFormat("player top send sort cards");
                            Command cmd = new Command(MyEventCmd.EVENT_SORTCARDS);
                            _ctx.Enqueue(cmd);
                        }
                    });
            }
        }

        protected override void RenderTakeXuanPao() {
            _go.GetComponent<global::TopPlayer>().Head.SetMark(string.Format("{0}", _fen));
        }

        protected override void RenderXuanPao() {
        }

        protected override void RenderTakeFirstCard() {
            UnityEngine.Debug.Assert(_takefirst);
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
                _go.GetComponent<global::TopPlayer>().Head.SetMark("条");
            } else if (_que == Card.CardType.Crak) {
                _go.GetComponent<global::TopPlayer>().Head.SetMark("万");
            } else if (_que == Card.CardType.Dot) {
                _go.GetComponent<global::TopPlayer>().Head.SetMark("同");
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
                mySequence.Append(_holdcard.Go.transform.DOMove(dst, _holddelta));
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

            Vector3[] waypoints = new[] {
                    from,
                    new Vector3((to.x - from.x) * 0.2f + from.x, (to.y - from.y) * 0.2f + from.y, from.z),
                    new Vector3((to.x - from.x) * 0.3f + from.x, (to.y - from.y) * 0.3f + from.y, from.z),
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
            base.RenderLead();

            // 出牌
            UnityEngine.Debug.Assert(_leadcards.Count > 0);
            Vector3 dst = CalcLeadPos(_leadcards.Count - 1);
            _leadcard.Go.transform.localPosition = dst;
            _leadcard.Go.transform.localRotation = _upv;
            dst.y = dst.y + 0.1f;
            ((GameController)_controller).Desk.RenderChangeCursor(dst);

            if (_leadcard.Value != _holdcard.Value) {
                if (_holdcard.Value == (_cards.Count - 1)) {
                    RenderSortCardsToDo(() => {
                        UnityEngine.Debug.LogFormat("top player send event lead card");
                        Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                        _ctx.Enqueue(cmd);
                    });
                } else {
                    RenderFly(() => {
                        UnityEngine.Debug.LogFormat("top player send event lead card");
                        Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                        _ctx.Enqueue(cmd);
                    });
                }
            } else {
                _holdcard = null;
                _leadcard = null;
                UnityEngine.Debug.LogFormat("top player send event lead card");
                Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                _ctx.Enqueue(cmd);
            }
        }

        protected override void RenderClearCall() {
            _com.Head.CloseWAL();
        }

        protected override void RenderPeng() {
            base.RenderPeng();

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
                    z = desk.Length - (_putbottomoffset + Card.Width / 2.0f);
                    offset += Card.Length;
                    pg.Width += Card.Length;
                    pg.Cards[i].Go.transform.localRotation = _uph;
                } else {
                    x = offset + Card.Width / 2.0f + move;
                    z = desk.Length - (_putbottomoffset + Card.Length / 2.0f);
                    offset += Card.Width;
                    pg.Width += Card.Width;
                    pg.Cards[i].Go.transform.localRotation = _upv;
                }
                pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z);

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(pg.Cards[i].Go.transform.DOMoveX(x - move, _putmovedelta))
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
            base.RenderGang();

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
                        x = offset + Card.Length / 2.0f + move;
                        z = desk.Length - (_putbottomoffset + Card.Width / 2.0f);
                        offset += Card.Length;
                        pg.Width += Card.Length;
                        pg.Cards[i].Go.transform.localRotation = _uph;
                    } else {
                        x = offset + Card.Width / 2.0f + move;
                        z = desk.Length - (_putbottomoffset + Card.Length / 2.0f);
                        offset += Card.Width;
                        pg.Width += Card.Width;
                        pg.Cards[i].Go.transform.localRotation = _upv;
                    }
                    pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z);

                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(pg.Cards[i].Go.transform.DOMoveX(x - move, _putmovedelta))
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
                    float x = offset + Card.Width / 2.0f + move;
                    float y = Card.Height / 2.0f;
                    float z = desk.Length - (_putbottomoffset + Card.Length / 2.0f);
                    offset += Card.Width;
                    pg.Width += Card.Width;

                    if (i == pg.Hor) {
                        pg.Cards[i].Go.transform.localRotation = _upv;
                    } else {
                        pg.Cards[i].Go.transform.localRotation = _backv;
                    }
                    pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z);

                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(pg.Cards[i].Go.transform.DOMoveX(x - move, 0.1f))
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
            } else if (true) {

                float x = offset + Card.Width * pg.Hor + Card.Width / 2.0f + move;
                float y = Card.Height / 2.0f;
                float z = desk.Length - (_putbottomoffset + Card.Width / 2.0f + Card.Width);
                pg.Cards[3].Go.transform.localPosition = new Vector3(x, y, z);
                pg.Cards[3].Go.transform.localRotation = _uph;

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(pg.Cards[3].Go.transform.DOMoveX(x - move, 0.1f))
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
            }
        }

        protected override void RenderGangSettle() {
            long chip = 0;
            long left = 0;
            if (_settle.Count > 0) {
                for (int i = 0; i < _settle.Count; i++) {
                    chip = _settle[i].Chip;
                    left = _settle[i].Left > left ? _settle[i].Left : left;
                }
                _com.Head.SetGold((int)left);
                _com.Head.ShowWAL(string.Format("{0}", chip));
            }
        }

        protected override void RenderHu() {
            base.RenderHu();

            int idx = _hucards.Count - 1;
            Card card = _hucards[idx];

            Desk desk = ((GameController)_controller).Desk;
            float x = _hurightoffset + Card.Width / 2.0f + Card.Width * idx;
            float y = Card.Height / 2.0f;
            float z = desk.Length - (_hubottomoffset + Card.Length / 2.0f);
            card.Go.transform.localPosition = new Vector3(x, y, z);

            _com.Head.SetHu(true);

            Command cmd = new Command(MyEventCmd.EVENT_HUCARD);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderHuSettle() {
            long chip = 0;
            long left = 0;
            if (_settle.Count > 0) {
                for (int i = 0; i < _settle.Count; i++) {
                    chip = _settle[i].Chip;
                    left = _settle[i].Left > left ? _settle[i].Left : left;
                }
                _com.Head.SetGold((int)left);
                _com.Head.ShowWAL(string.Format("{0}", chip));
            }
        }

        protected override void RenderOver() {
            Desk desk = ((GameController)_controller).Desk;
            for (int i = 0; i < _cards.Count; i++) {
                float x = desk.Width - (_leftoffset + Card.Width * i + Card.Width / 2.0f);
                float y = Card.Height / 2.0f;
                float z = desk.Length - (_bottomoffset + Card.Length / 2.0f);

                _cards[i].Go.transform.localPosition = new Vector3(x, y, z);
                _cards[i].Go.transform.localRotation = _upv;
            }
        }

        protected override void RenderSettle() {
            long chip = 0;
            long left = 0;
            if (_settle.Count > 0) {
                for (int i = 0; i < _settle.Count; i++) {
                    chip += _settle[i].Chip;
                    left = _settle[i].Left > left ? _settle[i].Left : left;
                }
                _chip = (int)left;
                _com.Head.SetGold(_chip);
                _com.Head.ShowWAL(string.Format("{0}", chip));
            }
        }

        protected override void RenderFinalSettle() {
            _com.OverWnd.SettleLeft(_settle);
        }

        protected override void RenderRestart() {
            _com.Head.CloseWAL();
            _com.Head.SetHu(false);
            _com.Head.SetReady(true);
        }

        protected override void RenderTakeRestart() {
            _com.Head.SetReady(false);
        }

        protected override void RenderSay() {
            _com.Say(_say);
        }

    }
}
