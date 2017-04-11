using Maria;
using Maria.Network;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Bacon {
    public class Player : Actor {

        public enum Orient {
            BOTTOM,
            LEFT,
            RIGHT,
            TOP,
        }

        protected Quaternion _upv = Quaternion.identity;
        protected Quaternion _uph = Quaternion.identity;
        protected Quaternion _downv = Quaternion.identity;
        protected Quaternion _backv = Quaternion.identity;
        protected const float _curorMH = 0.1f;

        protected Orient _ori;
        protected int _idx;
        protected int _sex; // 1, 男； 0， 女
        protected int _chip;
        protected int _sid;
        protected string _name;

        protected long _d1;
        protected long _d2;

        // 每个player可能不一样
        protected float _takeleftoffset = 0.5f;
        protected float _takebottomoffset = 0.35f;
        protected float _takemove = 0.15f;
        protected float _takemovedelta = 1.0f;

        protected int _takecardsidx = 0;
        protected int _takecardscnt = 0;
        protected int _takecardslen = 0;
        protected Dictionary<int, Card> _takecards = new Dictionary<int, Card>();

        // 重写
        protected float _leftoffset = 0.56f;
        protected float _bottomoffset = 0.1f;

        protected bool _takefirst = false;                 // 庄家
        protected List<Card> _cards = new List<Card>();

        // 重写
        protected float _leadleftoffset = 0.7f;
        protected float _leadbottomoffset = 0.7f;  // 偏移起始值
        protected List<Card> _leadcards = new List<Card>();

        protected Vector3 _putmove = Vector3.zero;
        protected const float _putmovedelta = 0.1f;
        protected const float _putmargin = 0.02f;

        // 重写

        protected float _putrightoffset = 0.1f;
        protected float _putbottomoffset = 0.1f;
        protected int _putidx = 0;
        protected List<PGCards> _putcards = new List<PGCards>();

        protected const float _hurightoffset = 0.2f;
        protected const float _hubottomoffset = 0.4f;
        protected List<Card> _hucards = new List<Card>();

        protected Vector3 _holdnaoffset = new Vector3(0.0f, Card.Length + 0.1f, 0.0f);

        protected const float _holddowndelat = 0.3f;
        protected const float _abdicateholddelta = 0.6f;
        protected const float _holdflydelta = 1.2f;
        protected const float _sortcardsdelta = 0.8f;

        protected const float _holddelta = 0.1f;
        protected const float _holdleftoffset = 0.02f;
        protected Card _holdcard;
        protected Card _leadcard;
        protected Vector3 _leadcardoffset = Vector3.one;

        protected long _turntype;
        protected long _fen;
        protected Card.CardType _que;
        protected bool _hashu = false;

        protected List<SettlementItem> _settle = new List<SettlementItem>();
        protected long _wal;         // 赢的钱或者输的钱
        protected long _say;

        // 手
        protected int _oknum;
        protected GameObject _rhand = null;
        protected GameObject _lhand = null;
        protected Vector3 _rhandinitpos = Vector3.one;
        protected Quaternion _rhandinitrot = Quaternion.identity;
        protected Vector3 _rhandtakeoffset = Vector3.one;
        protected Vector3 _rhandleadoffset = Vector3.one;
        protected Vector3 _rhandnaoffset = Vector3.one;
        protected Vector3 _rhandpgoffset = Vector3.zero;
        protected Vector3 _rhandhuoffset = Vector3.zero;

        protected Vector3 _lhandinitpos = Vector3.one;
        protected Quaternion _lhandinitrot = Quaternion.identity;
        protected Vector3 _lhandhuoffset = Vector3.zero;


        protected float _diushaizishendelta = 1.0f;
        protected float _diushaizishoudelta = 1.0f;
        protected float _chupaishendelta = 3.0f;
        protected float _chupaishoudelta = 1.0f;
        protected float _napaishendelta = 3.0f;
        protected float _fangpaishoudelta = 1.0f;
        protected float _hupaishendelta = 1.0f;
        protected float _hupaishoudelta = 1.0f;
        protected float _penggangshendelta = 1.0f;
        protected float _penggangshoudelta = 1.0f;

        // 牌
        protected float _fangdaodelta = 1.0f;
        protected float _dealcarddelta = 1.0f;

        public Player(Context ctx, GameController controller)
            : base(ctx, controller) {
        }

        public Player(Context ctx, GameService service)
            : base(ctx, service) {
        }

        public Orient Ori { get { return _ori; } }
        public int Idx { get { return _idx; } set { _idx = value; } }
        public int Sex { get { return _sex; } set { _sex = value; } }
        public int Chip { get { return _chip; } set { _chip = value; } }
        public int Sid { get { return _sid; } set { _sid = value; } }
        public string Name { get { return _name; } set { _name = value; } }

        public int Takecardsidx { get { return _takecardsidx; } set { _takecardsidx = value; } }
        public List<long> CS { get; set; }
        public CallInfo Call { get; set; }

        public void Start() {
            _takecards.Clear();
            _cards.Clear();
            _putcards.Clear();
            _holdcard = null;
            _leadcard = null;
        }

        public bool TakeCard(out Card nx) {
            if (_takecardscnt > 0) {
                var card = _takecards[_takecardsidx];
                nx = card;

                _takecardscnt--;
                _takecards.Remove(_takecardsidx);
                _takecardsidx++;
                if (_takecardsidx >= _takecardslen) {
                    _takecardsidx = 0;
                    return false;
                }
                return true;
            } else {
                UnityEngine.Debug.Assert(false);
                nx = null;
                return false;
            }
        }

        protected virtual Vector3 CalcPos(int pos) {
            return Vector3.zero;
        }

        protected virtual Vector3 CalcLeadPos(int pos) {
            return Vector3.zero;
        }

        public void FixDirMark() {
            _ctx.EnqueueRenderQueue(RenderFixDirMark);
        }

        protected virtual void RenderFixDirMark() { }

        public virtual void Boxing(List<long> cs, Dictionary<long, Card> cards) {
            for (int i = 0; i < cs.Count; i++) {
                try {
                    long c = cs[i];
                    if (cards.ContainsKey(c)) {
                        Card card = cards[c];
                        if (card.Parent == null) {
                            card.Parent = this;
                        } else {
                            UnityEngine.Debug.Assert(false);
                        }
                        card.Parent = this;
                        _takecards[i] = card;
                    } else {
                        UnityEngine.Debug.LogError(string.Format("not found key {0}", c));
                    }
                } catch (Exception ex) {
                    UnityEngine.Debug.LogException(ex);
                }
            }
            _takecardsidx = 0;
            _takecardscnt = cs.Count;
            _takecardslen = cs.Count;
            UnityEngine.Debug.Assert(cs.Count == 28 || cs.Count == 26);
            _ctx.EnqueueRenderQueue(RenderBoxing);
        }

        protected virtual void RenderBoxing() { }

        public void ThrowDice(long d1, long d2) {
            _d1 = d1;
            _d2 = d2;
            _ctx.EnqueueRenderQueue(RenderThrowDice);
        }

        protected virtual void RenderThrowDice() { }

        public void Deal() {
            List<Card> cards = ((GameController)_controller).TakeBlock();
            if (cards != null) {
                UnityEngine.Debug.Assert((cards.Count == 4 || cards.Count == 1));
                for (int i = 0; i < cards.Count; i++) {
                    _cards.Add(cards[i]);
                }
                _ctx.EnqueueRenderQueue(RenderDeal);
            } else {
                UnityEngine.Debug.Log("take block is null.");
            }
        }

        protected virtual void RenderDeal() { }

        private void QuickSort(int low, int high) {
            if (low >= high) {
                return;
            }
            int first = low;
            int last = high;
            Card key = _cards[first];
            while (first < last) {
                while (first < last) {
                    if (_cards[last].CompareTo(key) >= 0) {
                        --last;
                    } else {
                        _cards[first] = _cards[last];
                        _cards[first].Pos = first;
                        break;
                    }
                }
                while (first < last) {
                    if (_cards[first].CompareTo(key) < 0) {
                        ++first;
                    } else {
                        _cards[last] = _cards[first];
                        _cards[last].Pos = last;
                        break;
                    }
                }
            }
            _cards[first] = key;
            _cards[first].Pos = first;
            QuickSort(low, first - 1);
            QuickSort(first + 1, high);
        }

        public void SortCards() {
            QuickSort(0, _cards.Count - 1);
            for (int i = 0; i < _cards.Count; i++) {
                UnityEngine.Debug.Assert(_cards[i].Value == CS[i]);
            }
            _ctx.EnqueueRenderQueue(RenderSortCards);
        }

        protected virtual void RenderSortCards() { }

        public void TakeXuanPao() {
            _ctx.EnqueueRenderQueue(RenderTakeXuanPao);
        }

        protected virtual void RenderTakeXuanPao() { }

        public void XuanPao() {
            _ctx.EnqueueRenderQueue(RenderXuanPao);
        }

        protected virtual void RenderXuanPao() { }

        public void TakeFirsteCard(long c) {
            _takefirst = true;
            Card card;
            if (((GameController)_controller).TakeCard(out card)) {
                _holdcard = card;
                _holdcard.SetQue(_que);
                UnityEngine.Debug.Assert(card.Value == c);
            }
            _ctx.EnqueueRenderQueue(RenderTakeFirstCard);
        }

        protected virtual void RenderTakeFirstCard() { }

        protected virtual void RenderTakeCard(Action cb) {
            Vector3 cdst = CalcPos(_cards.Count + 1);
            Vector3 hdst = cdst + _rhandtakeoffset;
            Vector3 cdst1 = new Vector3(cdst.x, cdst.y + Card.Length, cdst.z);
            Vector3 hdst1 = cdst1 + _rhandtakeoffset;

            // 1.0 伸手
            Animator animator = _rhand.GetComponent<Animator>();
            animator.SetTrigger("BeforeFangpai");
            Tween t1 = _rhand.transform.DOLocalMove(hdst1, _napaishendelta);
            Sequence mySequence1 = DOTween.Sequence();
            mySequence1.Append(t1)
                .AppendCallback(() => {

                    // 2.1 牌下移
                    _holdcard.Go.transform.localPosition = cdst1;
                    _holdcard.Go.transform.localRotation = _backv;

                    Sequence mySequence21 = DOTween.Sequence();
                    mySequence21.Append(_holdcard.Go.transform.DOLocalMove(cdst, _holddelta))
                        .AppendCallback(() => {
                        });

                    // 2.2 手下移
                    Sequence mySequence22 = DOTween.Sequence();
                    mySequence22.Append(_rhand.transform.DOLocalMove(hdst, _holddelta))
                    .AppendCallback(() => {
                        // 3.0 放手
                        Hand hand = _rhand.GetComponent<Hand>();
                        hand.Rigster(Hand.EVENT.FANGPAI_COMPLETED, () => {
                            // 4.0 收手
                            Tween t4 = _rhand.transform.DOLocalMove(_rhandinitpos, _fangpaishoudelta);
                            Sequence mySequence4 = DOTween.Sequence();
                            mySequence4.Append(t4)
                            .AppendCallback(() => {

                                cb();

                                // 5.0 归位
                                animator.SetBool("Idle", true);
                            });
                        });
                        animator.SetBool("Fangpai", true);
                    });
                });
        }

        public void TakeXuanQue() {
            _ctx.EnqueueRenderQueue(RenderTakeXuanQue);
        }

        protected virtual void RenderTakeXuanQue() { }

        public void XuanQue(long que) {
            _que = (Card.CardType)que;
            foreach (var item in _cards) {
                item.SetQue(_que);
            }
            if (_takefirst) {
                _holdcard.SetQue(_que);
            }
            QuickSort(0, _cards.Count - 1);
            _ctx.EnqueueRenderQueue(RenderXuanQue);
        }

        protected virtual void RenderXuanQue() { }

        public void TakeTurn(long type, long c) {
            _turntype = type;
            if (type == 1) {
                Card card;
                if (((GameController)_controller).TakeCard(out card)) {
                    _holdcard = card;
                    _holdcard.SetQue(_que);
                    UnityEngine.Debug.Assert(card.Value == c);
                    _ctx.EnqueueRenderQueue(RenderTakeTurn);
                } else {
                    // over
                }
            } else if (type == 0) {
                // 碰后出牌，先找出holdcard
                UnityEngine.Debug.Assert(c != 0);
                UnityEngine.Debug.Assert(_cards.Count > 0);
                Card card = _cards[_cards.Count - 1];
                UnityEngine.Debug.Assert(card.Value == c);
                _holdcard = card;
                Remove(_holdcard);

                //for (int i = _cards.Count - 1; i >= 0; i--) {
                //    if (_cards[i].Value == c) {
                //        _holdcard = _cards[i];
                //        Remove(_holdcard);
                //        break;
                //    }
                //}
                //UnityEngine.Debug.Assert(_holdcard.Value == c);

                _ctx.EnqueueRenderQueue(RenderTakeTurn);
            } else if (type == 2) {
                // 选缺后的庄家出牌
                UnityEngine.Debug.Assert(c == 0);
                if (_takefirst) {
                    UnityEngine.Debug.Assert(_holdcard != null);
                }
                _ctx.EnqueueRenderQueue(RenderTakeTurn);
            } else if (type == 3) {
                UnityEngine.Debug.Assert(_holdcard.Value == c);
                _ctx.EnqueueRenderQueue(RenderTakeTurn);
            }
        }

        protected virtual void RenderTakeTurn() { }

        private void Insert(Card card) {
            UnityEngine.Debug.Assert(_cards.Count > 0);
            _cards.Add(card);
            int last = _cards.Count - 1;
            UnityEngine.Debug.Assert(_cards[last].Value == card.Value);
            _cards[last].Pos = last;
            for (int i = last - 1; i >= 0; i--) {
                if (_cards[i + 1].CompareTo(_cards[i]) < 0) {
                    Card tmp = _cards[i + 1];
                    _cards[i + 1] = _cards[i];
                    _cards[i + 1].Pos = i + 1;
                    _cards[i] = tmp;
                    _cards[i].Pos = i;
                }
            }
        }
        private void Remove(Card card) {
            for (int i = 0; i < _cards.Count; i++) {
                if (_cards[i].Value == card.Value) {
                    UnityEngine.Debug.Assert(i == card.Pos);
                    if (i == (_cards.Count - 1)) {
                        _cards.RemoveAt(i);
                    } else {
                        for (int j = i; j < (_cards.Count - 1); j++) {
                            _cards[j] = _cards[j + 1];
                            _cards[j].Pos = j;
                        }
                        _cards.RemoveAt(_cards.Count - 1);
                    }
                    break;
                }
            }
        }
        private void RemoveLead(Card card) {
            UnityEngine.Debug.Assert(_leadcards.Count > 0);
            Card other = _leadcards[_leadcards.Count - 1];
            UnityEngine.Debug.Assert(card.Value == other.Value);
            _leadcards.Remove(card);
        }

        public void Lead(long c) {
            UnityEngine.Debug.Assert(_holdcard != null);
            if (_holdcard.Value == c) {
                _leadcard = _holdcard;
                _leadcard.Clear();
                _leadcards.Add(_leadcard);
            } else {
                for (int i = 0; i < _cards.Count; i++) {
                    if (_cards[i].Value == c) {
                        _leadcard = _cards[i];
                        break;
                    }
                }
                UnityEngine.Debug.Assert(_leadcard.Value == c);
                Remove(_leadcard);
                _leadcard.Clear();
                _leadcards.Add(_leadcard);
                Insert(_holdcard);
                //_holdcard = null;
            }

            ((GameController)_controller).LastCard = _leadcard;
            ((GameController)_controller).LastIdx = _idx;

            _ctx.EnqueueRenderQueue(RenderLead);
        }

        protected virtual void RenderLead() {
            string prefix = "Sound/";
            string path = prefix;
            string name = string.Empty;
            if (_sex == 1) {
                path += "Man";
            } else {
                path += "Woman";
            }

            if (_leadcard.Type == Card.CardType.Bam) {
                name = "bam";
            } else if (_leadcard.Type == Card.CardType.Crak) {
                name = "crak";
            } else if (_leadcard.Type == Card.CardType.Dot) {
                name = "dot";
            }
            name += string.Format("{0}", _leadcard.Num);

            ABLoader.current.LoadAssetAsync<AudioClip>(path, name, (AudioClip clip) => {
                SoundMgr.current.PlaySound(_leadcard.Go, clip);
            });
        }

        protected virtual void RenderLead1(Action cb) {
            UnityEngine.Debug.Assert(_leadcards.Count > 0);

            Vector3 cdst = CalcLeadPos(_leadcards.Count - 1);
            Vector3 hdst = cdst + _rhandleadoffset;

            Vector3 cdst1 = cdst + _leadcardoffset;
            Vector3 hdst1 = cdst1 + _rhandleadoffset;

            // 1.0 伸手
            Animator animator = _rhand.GetComponent<Animator>();
            animator.SetTrigger("BeforeChupai");
            animator.SetBool("Chupai", true);

            Sequence mySequence1 = DOTween.Sequence();
            mySequence1.Append(_rhand.transform.DOLocalMove(hdst1, _chupaishendelta))
                .AppendCallback(() => {
                    // 21. 牌向前移
                    _leadcard.Go.transform.localPosition = cdst1;
                    _leadcard.Go.transform.localRotation = _upv;

                    Tween t21 = _leadcard.Go.transform.DOLocalMove(cdst, 1.0f);
                    Sequence mySequence21 = DOTween.Sequence();
                    mySequence21.Append(t21)
                    .AppendCallback(() => {
                        ((GameController)_controller).Desk.RenderChangeCursor(new Vector3(cdst.x, cdst.y + _curorMH, cdst.z));
                    });

                    Tween t22 = _rhand.transform.DOLocalMove(hdst, 1.0f);
                    Sequence mySequence22 = DOTween.Sequence();
                    mySequence22.Append(t22)
                    .AppendCallback(() => {
                        // 
                        //3.0 收手
                        Sequence mySequence4 = DOTween.Sequence();
                        mySequence4.Append(_rhand.transform.DOLocalMove(_rhandinitpos, _chupaishoudelta))
                        .AppendCallback(() => {
                            // 4.1 归为
                            animator.SetBool("Idle", true);
                            _rhand.transform.localRotation = _rhandinitrot;

                            // 4.2 整理手上的牌
                            cb();
                        });
                    });
                });
        }

        protected virtual void RenderLead1Cb() {
            if (_leadcard.Value != _holdcard.Value) {
                if (_holdcard.Pos == (_cards.Count - 1)) {
                    RenderSortCardsToDo(() => {
                        Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                        _ctx.Enqueue(cmd);
                    });
                } else {
                    RenderFly(() => {
                        Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                        _ctx.Enqueue(cmd);
                    });
                }
            } else {
                _holdcard = null;
                _leadcard = null;
                Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
                _ctx.Enqueue(cmd);
            }
        }

        protected virtual void RenderSortCardsToDo(Action cb) {
            _oknum = 0;
            for (int i = 0; i < _cards.Count; i++) {
                Vector3 dst = CalcPos(i);
                Sequence s = DOTween.Sequence();
                s.Append(_cards[i].Go.transform.DOMove(dst, _abdicateholddelta))
                    .AppendCallback(() => {
                        _oknum++;
                        if (_oknum >= _cards.Count) {
                            cb();
                        }
                    });
            }
        }

        protected virtual void RenderInsert(Action cb) {
            // 1.0
            Vector3 to = CalcPos(_holdcard.Pos);

            Vector3 cdst = to;
            Vector3 hdst = cdst + _rhandnaoffset;

            Animator animator = _rhand.GetComponent<Animator>();

            // 1.1 牌下放
            Tween t11 = _holdcard.Go.transform.DOLocalMove(cdst, _holddowndelat);
            Sequence mySequence11 = DOTween.Sequence();
            mySequence11.Append(t11)
            .AppendCallback(() => {
                _holdcard = null;
                _leadcard = null;
                //cb();
            });

            // 1.2 手下放
            Tween t12 = _rhand.transform.DOLocalMove(hdst, _holddowndelat);
            Sequence mySequence12 = DOTween.Sequence();
            mySequence12.Append(t12)
                .AppendCallback(() => {
                    // 2.0 放手
                    Hand hand = _rhand.GetComponent<Hand>();
                    hand.Rigster(Hand.EVENT.FANGPAI_COMPLETED, () => {
                        // 3.0 收手
                        Tween t31 = _rhand.transform.DOLocalMove(_rhandinitpos, _fangpaishoudelta);
                        Sequence mySequence31 = DOTween.Sequence();
                        mySequence31.Append(t31)
                        .AppendCallback(() => {
                            animator.SetBool("Idle", true);
                            cb();
                        });
                    });
                    animator.SetBool("Fangpai", true);
                });
        }

        protected virtual void RenderSortCardsAfterFly(Action cb) {
            _oknum = 0;
            for (int i = 0; i < _cards.Count; i++) {
                if (_cards[i].Value == _holdcard.Value) {
                    continue;
                }
                Vector3 dst = CalcPos(i);
                Sequence s = DOTween.Sequence();
                s.Append(_cards[i].Go.transform.DOMove(dst, _abdicateholddelta))
                    .AppendCallback(() => {
                        _oknum++;
                        if (_oknum >= _cards.Count - 1) {
                            RenderInsert(cb);
                        }
                    });
            }
        }

        protected virtual void RenderFly(Action cb) {
            Vector3 cfrom = _holdcard.Go.transform.localPosition;
            Vector3 hfrom = cfrom + _rhandnaoffset;

            // 1.0
            Animator animator = _rhand.GetComponent<Animator>();
            animator.SetTrigger("BeforeNapai");
            Tween t1 = _rhand.transform.DOLocalMove(hfrom, _napaishendelta);
            Sequence mySequence1 = DOTween.Sequence();
            mySequence1.Append(t1)
                .AppendCallback(() => {

                    return;

                    // 2.0 拿牌
                    Hand hand = _rhand.GetComponent<Hand>();
                    hand.Rigster(Hand.EVENT.NAPAI_COMPLETED, () => {

                        // 3.1 上提到目标位置
                        Vector3 cdst1 = cfrom + _holdnaoffset;
                        Vector3 hdst1 = cdst1 + _rhandnaoffset;
                        _holdcard.Go.transform.DOLocalMove(cdst1, 1.0f);

                        // 3.2 
                        Sequence mySequence32 = DOTween.Sequence();
                        Tween t32 = _rhand.transform.DOLocalMove(hdst1, 1.0f);
                        mySequence32.Append(t32)
                        .AppendCallback(() => {
                            Vector3 to = CalcPos(_holdcard.Pos);

                            Vector3 cdst2 = to + _holdnaoffset;
                            Vector3 hdst2 = cdst2 + _rhandnaoffset;
                            // 4.1 移动到目标位置
                            _holdcard.Go.transform.DOLocalMove(cdst2, 1.0f);

                            // 4.2 移动手
                            Tween t42 = _rhand.transform.DOLocalMove(hdst2, 1.0f);
                            Sequence mySequence42 = DOTween.Sequence();
                            mySequence42.Append(t42)
                            .AppendCallback(() => {
                                RenderSortCardsAfterFly(cb);
                            });
                        });

                        //float h = 0.05f;
                        //to.y = to.y + Card.Length + h;
                        //Vector3[] waypoints = new[] {
                        //    from,
                        //    new Vector3(from.x, (to.y - from.y) * 0.2f + from.y, (to.z - from.z) * 0.2f + from.z),
                        //    new Vector3(from.x, (to.y - from.y) * 0.3f + from.y, (to.z - from.z) * 0.3f + from.z),
                        //    new Vector3(from.x, (to.y - from.y) * 0.5f + from.y, (to.z - from.z) * 0.5f + from.z),
                        //    new Vector3(from.x, (to.y - from.y) * 0.8f + from.y, (to.z - from.z) * 0.8f + from.z),
                        //    to,
                        //};

                        //Tween t = _holdcard.Go.transform.DOPath(waypoints, _holdflydelta).SetOptions(false);
                        //Sequence mySequence = DOTween.Sequence();
                        //mySequence.Append(t).AppendCallback(() => {
                        //    RenderSortCardsAfterFly(cb);
                        //});

                        //_rhand.transform.DOPath(waypoints, _holdflydelta).SetOptions(false);
                    });
                    animator.SetBool("Napai", true);
                });
        }

        public void SetupCall(long c, long countdown) {
            if (Call.Gang == OpCodes.OPCODE_ANGANG ||
                Call.Gang == OpCodes.OPCODE_BUGANG ||
                (Call.Hu.Code != HuType.NONE && Call.Hu.Jiao == JiaoType.ZIMO) ||
                (Call.Hu.Code != HuType.NONE && Call.Hu.Jiao == JiaoType.DIANGANGHUA) ||
                (Call.Hu.Code != HuType.NONE && Call.Hu.Jiao == JiaoType.ZIGANGHUA)) {
                Card card;
                if (((GameController)_controller).TakeCard(out card)) {
                    _holdcard = card;
                    _holdcard.SetQue(_que);
                    UnityEngine.Debug.Assert(card.Value == c);
                } else {
                    // over
                }
            }
            _ctx.EnqueueRenderQueue(RenderCall);
        }

        protected virtual void RenderCall() { }

        public void ClearCall() {
            _ctx.EnqueueRenderQueue(RenderClearCall);
        }
        protected virtual void RenderClearCall() { }

        public void Peng(long code, long c, long hor, Player player, Card card) {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < _cards.Count; i++) {
                if (card == _cards[i]) {
                    cards.Add(_cards[i]);
                }
                if (cards.Count == 2) {
                    break;
                }
            }
            UnityEngine.Debug.Assert(cards.Count == 2);
            for (int i = 0; i < cards.Count; i++) {
                Remove(cards[i]);
            }
            player.RemoveLead(card);
            cards.Add(card);
            UnityEngine.Debug.Assert(cards.Count == 3);

            PGCards pgcards = new PGCards();
            pgcards.Cards = cards;
            pgcards.Opcode = code;
            pgcards.Hor = hor;
            pgcards.Width = 0.0f;
            _putcards.Add(pgcards);
            _putidx = _putcards.Count - 1;

            ((GameController)_controller).CurIdx = _idx;
            _ctx.EnqueueRenderQueue(RenderPeng);
        }

        protected virtual void RenderPeng() {
            string prefix = "Sound/";
            string path = prefix;
            string name = string.Empty;
            if (_sex == 1) {
                path += "Man";
            } else {
                path += "Woman";
            }

            name = "peng";

            ABLoader.current.LoadAssetAsync<AudioClip>(path, name, (AudioClip clip) => {
                SoundMgr.current.PlaySound(_go, clip);
            });
        }

        protected virtual void RenderPeng1() {
            PGCards pg = _putcards[_putidx];

            // 1.0 伸手
            Animator animator = _rhand.GetComponent<Animator>();
            animator.SetTrigger("BeforePenggang");
            Tween t1 = _rhand.transform.DOMove(pg.Cards[2].Go.transform.localPosition, _penggangshendelta);
            Sequence mySequence1 = DOTween.Sequence();
            mySequence1.Append(t1)
                .AppendCallback(() => {
                    // 2.0
                    _oknum = 0;
                    // 2.1 牌移动
                    for (int i = 0; i < pg.Cards.Count; i++) {
                        Tween t2 = pg.Cards[i].Go.transform.DOLocalMove(pg.Cards[i].Go.transform.localPosition + _putmove, _putmovedelta);
                        Sequence mySequence21 = DOTween.Sequence();
                        mySequence21.Append(t2)
                            .AppendCallback(() => {
                                _oknum++;
                                if (_oknum >= pg.Cards.Count) {
                                    // 3.0 收手
                                    Vector3 c2pos = pg.Cards[2].Go.transform.localPosition;
                                    ((GameController)_controller).Desk.RenderChangeCursor(new Vector3(c2pos.x, c2pos.y + _curorMH, c2pos.z));
                                }
                            });
                    }

                    // 2.2 手移动                    
                    Tween t22 = _rhand.transform.DOLocalMove(_rhand.transform.localPosition + _putmove, _putmovedelta);
                    Sequence mySequence22 = DOTween.Sequence();
                    mySequence22.Append(t22)
                    .AppendCallback(() => {

                        // 3.0 收手
                        Tween t3 = _rhand.transform.DOMove(_rhandinitpos, _penggangshoudelta);
                        Sequence mySequence3 = DOTween.Sequence();
                        mySequence3.Append(t3)
                        .AppendCallback(() => {
                            RenderSortCardsToDo(() => {
                                // 4.0 归为
                                animator.SetBool("Idle", true);

                                Command cmd = new Command(MyEventCmd.EVENT_PENGCARD);
                                _ctx.Enqueue(cmd);
                            });
                        });
                    });
                });
        }

        public void Gang(long code, long c, long hor, Player player, Card card) {
            if (code == OpCodes.OPCODE_ZHIGANG) {
                List<Card> cards = new List<Card>();
                for (int i = 0; i < _cards.Count; i++) {
                    if (card == _cards[i]) {
                        cards.Add(_cards[i]);
                    }
                    if (cards.Count == 3) {
                        break;
                    }
                }

                UnityEngine.Debug.Assert(cards.Count == 3);
                for (int i = 0; i < cards.Count; i++) {
                    Remove(cards[i]);
                }
                player.RemoveLead(card);
                cards.Add(card);
                UnityEngine.Debug.Assert(cards.Count == 4);

                PGCards pg = new PGCards();
                pg.Cards = cards;
                pg.Opcode = code;
                pg.Hor = hor;
                pg.Width = 0.0f;
                _putcards.Add(pg);
                _putidx = _putcards.Count - 1;

            } else if (code == OpCodes.OPCODE_ANGANG) {
                UnityEngine.Debug.Assert(_holdcard != null);
                List<Card> cards = new List<Card>();
                if (_holdcard.Value == c) {
                    for (int i = 0; i < _cards.Count; i++) {
                        if (_holdcard == _cards[i]) {
                            cards.Add(_cards[i]);
                        }
                        if (cards.Count == 3) {
                            break;
                        }
                    }
                    UnityEngine.Debug.Assert(cards.Count == 3);
                    for (int i = 0; i < cards.Count; i++) {
                        Remove(cards[i]);
                    }
                    cards.Add(_holdcard);
                } else {
                    Card xx = null;
                    for (int i = 0; i < _cards.Count; i++) {
                        if (_cards[i].Value == c) {
                            xx = _cards[i];
                            break;
                        }
                    }
                    UnityEngine.Debug.Assert(xx != null);

                    for (int i = 0; i < _cards.Count; i++) {
                        if (xx == _cards[i]) {
                            cards.Add(_cards[i]);
                        }
                        if (cards.Count == 4) {
                            break;
                        }
                    }
                    UnityEngine.Debug.Assert(cards.Count == 4);
                    for (int i = 0; i < cards.Count; i++) {
                        Remove(cards[i]);
                    }
                    Insert(_holdcard);
                }

                UnityEngine.Debug.Assert(cards.Count == 4);
                PGCards pg = new PGCards();
                pg.Cards = cards;
                pg.Opcode = code;
                pg.Hor = 0;
                pg.Width = 0.0f;
                _putcards.Add(pg);
                _putidx = _putcards.Count - 1;

            } else if (code == OpCodes.OPCODE_BUGANG) {
                // 找出那个card
                Card xcard = null;
                if (_holdcard.Value == c) {
                    xcard = _holdcard;
                } else {
                    for (int i = 0; i < _cards.Count; i++) {
                        if (_cards[i].Value == c) {
                            xcard = _cards[i];
                            break;
                        }
                    }
                    UnityEngine.Debug.Assert(xcard != null);
                    Remove(xcard);
                    Insert(_holdcard);
                }

                for (int i = 0; i < _putcards.Count; i++) {
                    PGCards pg = _putcards[i];
                    if (pg.Opcode == OpCodes.OPCODE_PENG && pg.Cards[0] == xcard) {
                        UnityEngine.Debug.Assert(pg.Cards.Count == 3);
                        pg.Cards.Add(xcard);
                        pg.Opcode = OpCodes.OPCODE_BUGANG;
                        _putidx = i;
                        break;
                    }
                }
            } else {
                UnityEngine.Debug.Assert(false);
            }

             ((GameController)_controller).CurIdx = _idx;
            _ctx.EnqueueRenderQueue(RenderGang);
        }

        public Card QiangGang(long c) {
            Card res;
            PGCards pg = _putcards[_putidx];
            UnityEngine.Debug.Assert(pg.Opcode == OpCodes.OPCODE_BUGANG);
            UnityEngine.Debug.Assert(pg.Cards.Count == 4);
            res = pg.Cards[3];
            pg.Cards.RemoveAt(3);
            pg.Opcode = OpCodes.OPCODE_PENG;
            UnityEngine.Debug.Assert(res.Value == c);
            return res;
        }

        protected virtual void RenderGang() {
            string prefix = "Sound/";
            string path = prefix;
            string name = string.Empty;
            if (_sex == 1) {
                path += "Man";
            } else {
                path += "Woman";
            }

            name = "gang";

            ABLoader.current.LoadAssetAsync<AudioClip>(path, name, (AudioClip clip) => {
                SoundMgr.current.PlaySound(_go, clip);
            });
        }

        protected virtual void RenderGang1(Action cb) {
            PGCards pg = _putcards[_putidx];

            // 1.0 伸手
            Animator animator = _rhand.GetComponent<Animator>();
            animator.SetTrigger("BeforePenggang");
            Tween t1 = _rhand.transform.DOMove(pg.Cards[3].Go.transform.localPosition, _penggangshendelta);
            Sequence mySequence1 = DOTween.Sequence();
            mySequence1.Append(t1)
                .AppendCallback(() => {
                    // 2.0
                    _oknum = 0;
                    // 2.1 牌移动
                    if (pg.Opcode == OpCodes.OPCODE_BUGANG) {
                        Sequence mySequence21 = DOTween.Sequence();
                        mySequence1.Append(pg.Cards[3].Go.transform.DOMove(pg.Cards[3].Go.transform.localPosition + _putmove, _putmovedelta))
                        .AppendCallback(() => {
                            Vector3 c2pos = pg.Cards[3].Go.transform.localPosition;
                            ((GameController)_controller).Desk.RenderChangeCursor(new Vector3(c2pos.x, c2pos.y + _curorMH, c2pos.z));
                        });
                    } else {
                        for (int i = 0; i < pg.Cards.Count; i++) {
                            Tween t2 = pg.Cards[i].Go.transform.DOLocalMove(pg.Cards[i].Go.transform.localPosition + _putmove, _putmovedelta);
                            Sequence mySequence21 = DOTween.Sequence();
                            mySequence21.Append(t2)
                                .AppendCallback(() => {
                                    _oknum++;
                                    if (_oknum >= pg.Cards.Count) {
                                        // 3.0 收手
                                        Vector3 c2pos = pg.Cards[3].Go.transform.localPosition;
                                        ((GameController)_controller).Desk.RenderChangeCursor(new Vector3(c2pos.x, c2pos.y + _curorMH, c2pos.z));
                                    }
                                });
                        }
                    }


                    // 2.2 手移动                    
                    Tween t22 = _rhand.transform.DOLocalMove(_rhand.transform.localPosition + _putmove, _putmovedelta);
                    Sequence mySequence22 = DOTween.Sequence();
                    mySequence22.Append(t22)
                    .AppendCallback(() => {

                        // 3.0 收手
                        Tween t3 = _rhand.transform.DOMove(_rhandinitpos, _penggangshoudelta);
                        Sequence mySequence3 = DOTween.Sequence();
                        mySequence3.Append(t3)
                        .AppendCallback(() => {
                            RenderSortCardsToDo(() => {
                                // 4.0 归为
                                animator.SetBool("Idle", true);

                                cb();
                            });
                        });
                    });
                });
        }

        public void GangSettle() {
            _ctx.EnqueueRenderQueue(RenderGangSettle);
        }

        protected virtual void RenderGangSettle() { }

        public void Hu(long code, long c, long jiao, long gang, long dian, Player player, Card card) {
            if (jiao == JiaoType.PINGFANG) {
                UnityEngine.Debug.Assert(c == card.Value);
                player.RemoveLead(card);
                _hucards.Add(card);
            } else if (jiao == JiaoType.GANGSHANGPAO) {
                UnityEngine.Debug.Assert(c == card.Value);
                _hucards.Add(card);
            } else if (jiao == JiaoType.QIANGGANGHU) {
                Card qiang = ((GameService)_service).GetPlayer(dian).QiangGang(c);
                UnityEngine.Debug.Assert(c == qiang.Value);
                _hucards.Add(qiang);
            } else if (jiao == JiaoType.DIANGANGHUA) {
                UnityEngine.Debug.Assert(c == _holdcard.Value);
                _hucards.Add(_holdcard);
                _holdcard = null;
            } else if (jiao == JiaoType.ZIGANGHUA) {
                UnityEngine.Debug.Assert(c == _holdcard.Value);
                _hucards.Add(_holdcard);
                _holdcard = null;
            } else if (jiao == JiaoType.ZIMO) {
                UnityEngine.Debug.Assert(c == _holdcard.Value);
                _hucards.Add(_holdcard);
                _holdcard = null;
            }

            if (!_hashu) {
                _hashu = true;
            }

            ((GameController)_controller).CurIdx = _idx;
            _ctx.EnqueueRenderQueue(RenderHu);
        }

        protected virtual void RenderHu() {
            string prefix = "Sound/";
            string path = prefix;
            string name = string.Empty;
            if (_sex == 1) {
                path += "Man";
            } else {
                path += "Woman";
            }

            name = "hu";

            ABLoader.current.LoadAssetAsync<AudioClip>(path, name, (AudioClip clip) => {
                SoundMgr.current.PlaySound(_go, clip);
            });
        }

        public void HuSettle() {
            UnityEngine.Debug.Assert(_settle.Count >= 1);
            _ctx.EnqueueRenderQueue(RenderHuSettle);
        }

        protected virtual void RenderHuSettle() { }

        public void Over() {
            _ctx.EnqueueRenderQueue(RenderOver);
        }

        protected virtual void RenderOver() { }

        protected virtual void RenderOverShen(Action<Action> act31, Action cb) {
            // 1.0 伸手
            Animator ranimator = _rhand.GetComponent<Animator>();
            ranimator.SetTrigger("BeforeHupai");

            _lhand.SetActive(true);
            Animator lanimator = _rhand.GetComponent<Animator>();
            lanimator.SetTrigger("BeforeHupai");

            // 5.0
            Action act5 = delegate () {
                // 发事件
                cb();
            };

            // 3.0 放到牌
            Action act3 = delegate () {
                act31(() => {
                    // 4.0 收手
                    _oknum = 0;
                    Sequence mySequence4r = DOTween.Sequence();
                    mySequence4r.Append(_rhand.transform.DOLocalMove(_rhandinitpos, _hupaishoudelta))
                    .AppendCallback(() => {
                        _oknum++;
                        if (_oknum >= 2) {
                            act5();
                        }
                    });

                    Sequence mySequence4l = DOTween.Sequence();
                    mySequence4l.Append(_lhand.transform.DOLocalMove(_lhandinitpos, _hupaishoudelta))
                    .AppendCallback(() => {
                        _oknum++;
                        if (_oknum >= 2) {
                            act5();
                        }
                    });
                });
            };

            // 2.0
            Action act2 = delegate () {
                _oknum = 0;
                Hand rhand = _rhand.GetComponent<Hand>();
                rhand.Rigster(Hand.EVENT.HUPAI_COMPLETED, () => {
                    _oknum++;
                    if (_oknum >= 2) {
                        // 3.0
                        act3();
                    }
                });
                ranimator.SetBool("Hupai", true);

                Hand lhand = _lhand.GetComponent<Hand>();
                lhand.Rigster(Hand.EVENT.HUPAI_COMPLETED, () => {
                    _oknum++;
                    if (_oknum >= 2) {
                        // 3.0
                        act3();
                    }
                });
                lanimator.SetBool("Hupai", true);
            };

            _oknum = 0;
            Tween t1r = _rhand.transform.DOLocalMove(_cards[_cards.Count - 1].Go.transform.localPosition, _hupaishendelta);
            Sequence mySequence1r = DOTween.Sequence();
            mySequence1r.Append(t1r)
                .AppendCallback(() => {
                    _oknum++;
                    if (_oknum >= 2) {
                        // 2.0
                        act2();
                    }
                });

            Tween t1l = _lhand.transform.DOLocalMove(_cards[0].Go.transform.localPosition, _hupaishendelta);
            Sequence mySequence1l = DOTween.Sequence();
            mySequence1l.Append(t1l)
                .AppendCallback(() => {
                    _oknum++;
                    if (_oknum >= 2) {
                        // 2.0
                        act2();
                    }
                });
        }

        public void Settle() { }

        protected virtual void RenderSettle() { }

        public void FinalSettle() { }

        protected virtual void RenderFinalSettle() { }

        public void Restart() {
            _ctx.EnqueueRenderQueue(RenderRestart);
        }

        protected virtual void RenderRestart() { }

        public void TakeRestart() {
            UnityEngine.Debug.LogFormat("player {0} take restart", _idx);
            _d1 = 0;
            _d2 = 0;

            _takecardsidx = 0;
            _takecardscnt = 0;
            _takecardslen = 0;
            _takecards = new Dictionary<int, Card>();

            _takefirst = false;                 // 庄家
            _cards = new List<Card>();
            _leadcards = new List<Card>();

            _putidx = 0;
            _putcards = new List<PGCards>();
            _hucards = new List<Card>();

            _holdcard = null;
            _leadcard = null;

            _turntype = 0;
            _fen = 0;
            _que = 0;
            _hashu = false;

            _wal = 0;         // 赢的钱或者输的钱
            _say = 0;
        }

        protected virtual void RenderTakeRestart() { }

        public void Say(long code) {
            _say = code;
        }

        protected virtual void RenderSay() { }

        public void ClearSettle() {
            _settle.Clear();
        }

        public void AddSettle(SettlementItem item) {
            _settle.Add(item);
        }
    }
}
