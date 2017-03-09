using Maria;
using Maria.Network;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    public class Player : Actor {

        protected int _idx;
        protected long _d1;
        protected long _d2;

        protected bool _takefirst = false;
        protected const float _takeleftoffset = 0.44f;
        protected const float _takebottomoffset = 0.20f;
        protected int _takecardsidx = 0;
        protected int _takecardscnt = 0;
        protected int _takecardslen = 0;
        protected Dictionary<int, Card> _takecards = new Dictionary<int, Card>();

        protected const float _holddowndelat = 0.3f;
        protected const float _abdicateholddelta = 0.6f;
        protected const float _holdflydelta = 1.2f;
        protected const float _sortcardsdelta = 0.8f;
        protected const float _leftoffset = 0.56f;
        protected const float _bottomoffset = 0.1f;
        protected List<Card> _cards = new List<Card>();

        protected const float _leadleftoffset = 0.7f;
        protected const float _leadbottomoffset = 0.7f;  // 偏移起始值
        protected List<Card> _leadcards = new List<Card>();

        protected const float _putmargin = 0.02f;
        protected const float _putrightoffset = 0.1f;
        protected const float _putbottomoffset = 0.25f;
        protected int _putidx = 0;
        protected List<PGCards> _putcards = new List<PGCards>();

        protected const float _holddelta = 0.1f;
        protected const float _holdleftoffset = 0.02f;
        protected Card _holdcard;
        protected Card _leadcard;

        public Player(Context ctx, GameController controller)
            : base(ctx, controller) {
        }

        public Player(Context ctx, GameService service)
            : base(ctx, service) {
        }

        public int Idx { get { return _idx; } set { _idx = value; } }

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

        public virtual void Boxing(List<long> cs, Dictionary<long, Card> cards) {
            for (int i = 0; i < cs.Count; i++) {
                try {
                    long c = cs[i];
                    if (cards.ContainsKey(c)) {
                        Card card = cards[c];
                        card.SetPlayer(this);
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
            long min = Math.Min(_d1, _d2);
            _takecardsidx = (int)(min * 2);
            _ctx.EnqueueRenderQueue(RenderThrowDice);
        }

        protected virtual void RenderThrowDice() {
            ((GameController)_controller).RenderThrowDice(_d1, _d2);
        }

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

        public void TakeTurn(long c) {
            Card card;
            if (((GameController)_controller).TakeCard(out card)) {
                _holdcard = card;
                UnityEngine.Debug.Assert(card.Value == c);
                _ctx.EnqueueRenderQueue(RenderTakeTurn);
            }
        }

        protected virtual void RenderTakeTurn() { }

        private void Insert(Card card) {
            UnityEngine.Debug.Assert(_cards.Count > 0);
            _cards.Add(card);
            UnityEngine.Debug.Assert(_cards[_cards.Count - 1].Value == card.Value);
            for (int i = _cards.Count - 2; i >= 0; i--) {
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

        public void SetupCall(long card, long countdown) {
            _ctx.EnqueueRenderQueue(RenderCall);
        }

        protected virtual void RenderCall() { }

        public void Lead(long c) {
            if (_holdcard == null) {
                // peng lead
                for (int i = 0; i < _cards.Count; i++) {
                    if (_cards[i].Value == c) {
                        _leadcard = _cards[i];
                        break;
                    }
                }
                UnityEngine.Debug.Assert(_leadcard.Value == c);
                Remove(_leadcard);
                _leadcards.Add(_leadcard);
            } else {
                if (_holdcard.Value == c) {
                    _leadcard = _holdcard;
                    //_holdcard = null;  // 此时不能删除_holdcard,表现的时候需要用到
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
                    _leadcards.Add(_leadcard);

                    Insert(_holdcard);
                    //_holdcard = null;
                }
            }

            ((GameController)_controller).LastCard = _leadcard;
            ((GameController)_controller).CurIdx = _idx;

            _ctx.EnqueueRenderQueue(RenderLead);
        }

        protected virtual void RenderLead() { }

        protected virtual void RenderInsert() { }

        public void Peng(Card card) {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < _cards.Count; i++) {
                if (card.Value == _cards[i].Value) {
                    cards.Add(_cards[i]);
                }
                if (cards.Count == 2) {
                    break;
                }
            }
            UnityEngine.Debug.Assert(cards.Count == 2);
            for (int i = 0; i < cards.Count; i++) {
                _cards.Remove(cards[i]);
            }
            cards.Add(card);
            PGCards pgcards = new PGCards();
            pgcards.Cards = cards;
            pgcards.Opcode = OpCodes.OPCODE_PENG;
            pgcards.Hor = UnityEngine.Random.Range(0, 2);
            pgcards.Width = Card.Width * 2 + Card.Length + 0.1f;
            _putcards.Add(pgcards);
            _putidx = _putcards.Count - 1;

            ((GameController)_controller).CurIdx = _idx;
            _ctx.EnqueueRenderQueue(RenderPeng);
        }

        protected virtual void RenderPeng() { }

        public void Gang(uint opcode, Card card) {
            if (opcode == OpCodes.OPCODE_ANGANG) {
                List<Card> cards = new List<Card>();
                for (int i = 0; i < _cards.Count; i++) {
                    if (card.Value == _cards[i].Value) {
                        cards.Add(_cards[i]);
                    }
                    if (cards.Count == 4) {
                        break;
                    }
                }
                UnityEngine.Debug.Assert(cards.Count == 4);
                PGCards pg = new PGCards();
                pg.Cards = cards;
                pg.Opcode = opcode;
                pg.Hor = 0;
                pg.Width = 0.0f;
                _putcards.Add(pg);
                _putidx = _putcards.Count - 1;
                ((GameController)_controller).CurIdx = _idx;
                _ctx.EnqueueRenderQueue(RenderGang);
            } else if (opcode == OpCodes.OPCODE_ZHIGANG) {
                List<Card> cards = new List<Card>();
                for (int i = 0; i < _cards.Count; i++) {
                    if (card.Value == _cards[i].Value) {
                        cards.Add(_cards[i]);
                    }
                    if (cards.Count == 3) {
                        break;
                    }
                }
                UnityEngine.Debug.Assert(cards.Count == 3);
                PGCards pg = new PGCards();
                pg.Cards = cards;
                pg.Opcode = opcode;
                pg.Hor = 0;
                pg.Width = 0.0f;
                _putcards.Add(pg);
                _putidx = _putcards.Count - 1;
                ((GameController)_controller).CurIdx = _idx;
                _ctx.EnqueueRenderQueue(RenderGang);
            } else if (opcode == OpCodes.OPCODE_BUGANG) {
                for (int i = 0; i < _putcards.Count; i++) {
                    PGCards pg = _putcards[i];
                    if (pg.Opcode == OpCodes.OPCODE_PENG && pg.Cards[0] == card) {
                        pg.Cards.Add(card);
                        pg.Opcode = OpCodes.OPCODE_BUGANG;
                        _putidx = i;
                        break;
                    }
                }
                ((GameController)_controller).CurIdx = _idx;
                _ctx.EnqueueRenderQueue(RenderGang);
            }
        }

        protected virtual void RenderGang() { }

        public void Hu(Card card) {
            ((GameController)_controller).CurIdx = _idx;
            _ctx.EnqueueRenderQueue(RenderHu);
        }

        protected virtual void RenderHu() { }

    }
}
