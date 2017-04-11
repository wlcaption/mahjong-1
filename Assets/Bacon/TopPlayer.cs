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
            _takeleftoffset = 0.5f;
            _takebottomoffset = 0.405f - (Card.Length / 2.0f);

            _leftoffset = 0.5f;
            _bottomoffset = 1.72f;

            _leadleftoffset = 0.8f;
            _leadbottomoffset = 0.8f;

            _leadcardoffset = new Vector3(-0.05f, 0.0f, 0.0f);

            _rhandinitpos = new Vector3(1.0f, -2.0f, 3.0f);
            _rhandinitrot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            _rhandleadoffset = new Vector3(0.597f, -1.967f, 1.124f);
            _rhandtakeoffset = new Vector3(0.399f, -2.034f, 0.497f);
            _rhandnaoffset = new Vector3(0.43f, -2.136f, 0.4299f);
            _rhandpgoffset = Vector3.zero;
            _lhandhuoffset = Vector3.zero;

            _lhandinitpos = new Vector3(1.0f, -2.0f, 3.0f);
            _lhandinitrot = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            _lhandhuoffset = Vector3.zero;

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
            float y = Card.Length / 2.0f + Card.HeightMZ;
            float z = _bottomoffset;
            return new Vector3(x, y, z);
        }

        protected override Vector3 CalcLeadPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            int row = pos / 6;
            int col = pos % 6;

            float x = desk.Width - (_leadleftoffset + (Card.Width * col) + (Card.Width / 2.0f));
            float y = Card.Height / 2.0f + Card.HeightMZ;
            float z = desk.Length - (_leadbottomoffset - (Card.Length * row) - Card.Length / 2.0f);

            return new Vector3(x, y, z);
        }

        protected override void RenderFixDirMark() {
            if (_idx == 1) {
                ((GameController)_controller).Desk.RenderSetDongAtTop();
            } else if (_idx == 2) {
                ((GameController)_controller).Desk.RenderSetNanAtTop();
            } else if (_idx == 3) {
                ((GameController)_controller).Desk.RenderSetXiAtTop();
            } else if (_idx == 4) {
                ((GameController)_controller).Desk.RenderSetBeiAtTop();
            } else {
                UnityEngine.Debug.Assert(false);
            }

            GameObject rori = ABLoader.current.LoadAsset<GameObject>("Prefabs/Hand", "girlrhand");
            _rhand = GameObject.Instantiate<GameObject>(rori);
            _rhand.transform.SetParent(_go.transform);
            _rhand.transform.localPosition = _rhandinitpos;
            _rhand.transform.localRotation = _rhandinitrot;

            GameObject lori = ABLoader.current.LoadAsset<GameObject>("Prefabs/Hand", "girllhand");
            _lhand = GameObject.Instantiate<GameObject>(lori);
            _lhand.transform.SetParent(_go.transform);
            _lhand.transform.localPosition = _lhandinitpos;
            _lhand.transform.localRotation = _lhandinitrot;

        }

        protected override void RenderBoxing() {
            try {
                int count = 0;
                Desk desk = ((GameController)_controller).Desk;
                desk.RenderShowTopSlot(() => {
                });

                for (int i = 0; i < _takecards.Count; i++) {
                    int idx = i / 2;
                    float x = _takeleftoffset + idx * Card.Width + Card.Width / 2.0f;
                    float y = Card.HeightMZ + Card.Height / 2.0f;
                    float z = desk.Length - (_takebottomoffset + Card.Length / 2.0f);
                    if (i % 2 == 0) {
                        y = Card.HeightMZ + Card.Height + Card.Height / 2.0f;
                    } else if (i % 2 == 1) {
                        y = Card.HeightMZ + Card.Height / 2.0f;
                    }
                    Card card = _takecards[i];
                    card.Go.transform.localRotation = _downv;

                    card.Go.transform.localPosition = new UnityEngine.Vector3(x, y - _takemove, z);
                    Tween t = card.Go.transform.DOLocalMoveY(y, _takemovedelta);

                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(t)
                    .AppendCallback(() => {
                        count++;
                        if (count == _takecards.Count) {
                            Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_BOXINGCARDS);
                            _ctx.Enqueue(cmd);
                        }
                    });
                }

                desk.RenderCloseTopSlot(() => {
                });
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
            }
        }

        protected override void RenderThrowDice() {
            // 1.0 伸手
            Animator animator = _rhand.GetComponent<Animator>();
            Tween t = _rhand.transform.DOLocalMove(new Vector3(1.845f, -1.948f, 1.847f), _diushaizishendelta);
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(t)
                .AppendCallback(() => {
                    // 2.0 丢色子
                    Hand hand = _rhand.GetComponent<Hand>();
                    hand.Rigster(Hand.EVENT.DIUSHAIZI_COMPLETED, () => {
                        // 3.1
                        UnityEngine.Debug.Log("top diu saizi ");
                        ((GameController)_controller).RenderThrowDice(_d1, _d2);
                        // 3.2
                        Tween t32 = _rhand.transform.DOLocalMove(_rhandinitpos, _diushaizishoudelta);
                        Sequence mySequence32 = DOTween.Sequence();
                        mySequence32.Append(t32).
                        AppendCallback(() => {
                            // 4.0 归原
                            animator.SetBool("Idle", true);
                        });
                    });
                    animator.SetBool("Diushaizi", true);
                });
        }

        protected override void RenderDeal() {
            _oknum = 0;
            int count = 0;
            int i = 0;
            if (_cards.Count == 13) {
                i = 12;
                count = 1;
            } else {
                i = _cards.Count - 4;
                count = 4;
            }
            for (; i < _cards.Count; i++) {
                Vector3 dst = CalcPos(i);
                var card = _cards[i];
                card.Go.transform.localPosition = dst;
                card.Go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(-115.0f, Vector3.right);
                Tween t = card.Go.transform.DOLocalRotateQuaternion(_backv, 1.0f);
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(t)
                    .AppendCallback(() => {
                        _oknum++;
                        if (_oknum >= count) {
                            _oknum = 0;
                            Command cmd = new Command(MyEventCmd.EVENT_TAKEDEAL);
                            _ctx.Enqueue(cmd);
                        }
                    });
            }
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
            RenderTakeCard(() => {
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
                RenderTakeCard(() => { });
            } else if (_turntype == 0) {
                Vector3 dst = CalcPos(_cards.Count + 1);
                _holdcard.Go.transform.localRotation = _backv;

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_holdcard.Go.transform.DOMove(dst, _holddelta));
            }
        }

        protected override void RenderInsert(Action cb) {
            base.RenderInsert(cb);
        }

        protected override void RenderSortCardsAfterFly(Action cb) {
            base.RenderSortCardsAfterFly(cb);
        }

        protected override void RenderFly(Action cb) {
            base.RenderFly(cb);
        }

        protected override void RenderLead() {
            base.RenderLead();

            RenderLead1(RenderLead1Cb);
        }

        protected override void RenderClearCall() {
            _com.Head.CloseWAL();
        }

        protected override void RenderPeng() {
            base.RenderPeng();

            Desk desk = ((GameController)_controller).Desk;
            PGCards pg = _putcards[_putidx];
            UnityEngine.Debug.Assert(pg.Cards.Count == 3);
            float offset = _putrightoffset;
            for (int i = 0; i < _putidx; i++) {
                UnityEngine.Debug.Assert(_putcards[i].Width > 0.0f);
                offset += _putcards[i].Width + _putmargin;
            }

            _putmove = new Vector3(-1.0f, 0.0f, 0.0f);
            for (int i = 0; i < pg.Cards.Count; i++) {
                float x = 0.0f;
                float y = Card.Height / 2.0f + Card.HeightMZ;
                float z = _putbottomoffset;
                if (i == pg.Hor) {
                    x = offset + Card.Length / 2.0f;
                    z = desk.Length - (_putbottomoffset + Card.Width / 2.0f);
                    offset += Card.Length;
                    pg.Width += Card.Length;
                    pg.Cards[i].Go.transform.localRotation = _uph;
                } else {
                    x = offset + Card.Width / 2.0f;
                    z = desk.Length - (_putbottomoffset + Card.Length / 2.0f);
                    offset += Card.Width;
                    pg.Width += Card.Width;
                    pg.Cards[i].Go.transform.localRotation = _upv;
                }
                pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z) + _putmove;
            }

            RenderPeng1();
        }

        protected override void RenderGang() {
            base.RenderGang();

            Desk desk = ((GameController)_controller).Desk;
            PGCards pg = _putcards[_putidx];
            UnityEngine.Debug.Assert(pg.Cards.Count == 4);

            float offset = _putrightoffset;
            for (int i = 0; i < _putidx; i++) {
                UnityEngine.Debug.Assert(_putcards[i].Width > 0.0f);
                offset += _putcards[i].Width + _putmargin;
            }

            _putmove = new Vector3(-1.0f, 0.0f, 0.0f);
            if (pg.Opcode == OpCodes.OPCODE_ZHIGANG) {
                for (int i = 0; i < pg.Cards.Count; i++) {
                    float x = 0.0f;
                    float y = Card.Height / 2.0f + Card.HeightMZ;
                    float z = _putbottomoffset;
                    if (i == pg.Hor) {
                        x = offset + Card.Length / 2.0f;
                        z = desk.Length - (_putbottomoffset + Card.Width / 2.0f);
                        offset += Card.Length;
                        pg.Width += Card.Length;
                        pg.Cards[i].Go.transform.localRotation = _uph;
                    } else {
                        x = offset + Card.Width / 2.0f;
                        z = desk.Length - (_putbottomoffset + Card.Length / 2.0f);
                        offset += Card.Width;
                        pg.Width += Card.Width;
                        pg.Cards[i].Go.transform.localRotation = _upv;
                    }
                    pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z) - _putmove;
                }
                RenderGang1(() => {
                    RenderSortCardsToDo(() => {
                        Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                        _ctx.Enqueue(cmd);
                    });
                });
            } else if (pg.Opcode == OpCodes.OPCODE_ANGANG) {
                for (int i = 0; i < pg.Cards.Count; i++) {
                    float x = offset + Card.Width / 2.0f;
                    float y = Card.Height / 2.0f + Card.HeightMZ;
                    float z = desk.Length - (_putbottomoffset + Card.Length / 2.0f);
                    offset += Card.Width;
                    pg.Width += Card.Width;

                    if (i == pg.Hor) {
                        pg.Cards[i].Go.transform.localRotation = _upv;
                    } else {
                        pg.Cards[i].Go.transform.localRotation = _backv;
                    }
                    pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z) - _putmove;
                }
                RenderGang1(() => {
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
                });
            } else if (true) {

                float x = offset + Card.Width * pg.Hor + Card.Width / 2.0f;
                float y = Card.Height / 2.0f + Card.HeightMZ;
                float z = desk.Length - (_putbottomoffset + Card.Width / 2.0f + Card.Width);
                pg.Cards[3].Go.transform.localPosition = new Vector3(x, y, z) - _putmove;
                pg.Cards[3].Go.transform.localRotation = _uph;

                RenderGang1(() => {
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

            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(1.0f)
                .AppendCallback(() => {
                    Command cmd = new Command(MyEventCmd.EVENT_HUCARD);
                    _ctx.Enqueue(cmd);
                });
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
                SettlementItem item = _settle[0];
                if (item.TuiSui == 1) {
                    _com.Head.SetGold((int)left);
                    _com.Head.ShowWAL("退税");

                    Sequence mySequence = DOTween.Sequence();
                    mySequence.AppendInterval(1.0f)
                        .AppendCallback(() => {
                            _com.Head.ShowWAL(string.Format("{0}", chip));
                        })
                    .AppendInterval(1.0f)
                    .AppendCallback(() => {
                        Command cmd = new Command(MyEventCmd.EVENT_SETTLE_NEXT);
                        _ctx.Enqueue(cmd);
                    });
                } else {
                    Sequence mySequence = DOTween.Sequence();
                    mySequence.AppendInterval(1.0f)
                        .AppendCallback(() => {
                            Command cmd = new Command(MyEventCmd.EVENT_SETTLE_NEXT);
                            _ctx.Enqueue(cmd);
                        });
                }
            }
        }

        protected override void RenderFinalSettle() {
            _com.Head.SetHu(false);
            _com.Head.CloseWAL();
            _com.OverWnd.SettleLeft(_settle);
        }

        protected override void RenderRestart() {
            _com.Head.SetHu(false);
            _com.Head.CloseWAL();
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
