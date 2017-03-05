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
        protected float _takeleftoffset = 0.44f;
        protected float _takebottomoffset = 0.20f;
        protected int _takecardsidx = 0;
        protected int _takecardscnt = 0;
        protected int _takecardslen = 0;
        protected Dictionary<int, Card> _takecards = new Dictionary<int, Card>();

        protected float _sortcardsdelta = 0.8f;
        protected float _leftoffset = 0.56f;
        protected float _bottomoffset = 0.1f;
        protected List<Card> _cards = new List<Card>();

        protected float _leadleftoffset = 0.7f;
        protected float _leadbottomoffset = 0.7f;
        protected List<Card> _leadcards = new List<Card>();

        protected int _putidx = 0;
        protected List<List<Card>> _putcards = new List<List<Card>>();

        protected float _holdleftoffset = 0.05f;
        protected Card _holdcard;
        protected Card _leadcard;

        protected List<long> _opcodes;

        protected long _hucode;
        protected long _pengcode;
        protected long _gangcode;

        public Player(Context ctx, GameController controller)
            : base(ctx, controller) {
        }

        public Player(Context ctx, GameService service)
            : base(ctx, service) {
        }

        public int Idx { get { return _idx; } set { _idx = value; } }

        public List<long> CS { get; set; }

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
            UnityEngine.Debug.Assert(cards != null && (cards.Count == 4 || cards.Count == 1));
            for (int i = 0; i < cards.Count; i++) {
                _cards.Add(cards[i]);
            }
            _ctx.EnqueueRenderQueue(RenderDeal);
        }

        protected virtual void RenderDeal() {}

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
            if (card.CompareTo(_cards[_cards.Count-1]) > 0) {
                int idx = _cards.Count;
                _cards[idx] = card;
                _cards[idx].Pos = idx;
            } else {
                int idx = _cards.Count - 1;
                for (int i = idx; i >= 0; i--) {
                    if (_cards[i].CompareTo(card) > 0) {
                        _cards[i + 1] = _cards[i];
                        _cards[i + 1].Pos = i + 1;
                    } else {
                        _cards[i + 1] = card;
                        _cards[i + 1].Pos = i + 1;
                        break;
                    }
                }
            }
        }

        public void SetupCall(List<long> opcodes, long countdown) {
            _opcodes = opcodes;
            _ctx.EnqueueRenderQueue(RenderCall);
        }

        protected virtual void RenderCall() {}

        public void Lead(long c) {
            for (int i = 0; i < _cards.Count; i++) {
                if (_cards[i].Value == c) {
                    _leadcard = _cards[i];
                    break;
                }
            }
            UnityEngine.Debug.Assert(_leadcard.Value == c);
            _cards.Remove(_leadcard);
            _leadcards.Add(_leadcard);
            ((GameController)_controller).LastCard = _leadcard;
            ((GameController)_controller).CurIdx = _idx;

            UnityEngine.Debug.Assert(_holdcard != null);
            if (_leadcard != _holdcard) {
                Insert(_holdcard);
            }
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
            _putcards.Add(cards);
            _putidx = _putcards.Count - 1;

            ((GameController)_controller).CurIdx = _idx;
            _ctx.EnqueueRenderQueue(RenderPeng);
        }

        protected virtual void RenderPeng() { }

        public void Gang(Card card) {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < _cards.Count; i++) {
                if (card.Value == _cards[i].Value) {
                    cards.Add(_cards[i]);
                }
                if (cards.Count == 3) {
                    break;
                }
            }
            if (cards.Count == 3) {
                for (int i = 0; i < cards.Count; i++) {
                    _cards.Remove(cards[i]);
                }
                cards.Add(card);
                _putcards.Add(cards);
                ((GameController)_controller).CurIdx = _idx;
                _ctx.EnqueueRenderQueue(RenderGang);
            } else {
                for (int i = 0; i < _putcards.Count; i++) {
                    List<Card> xx = _putcards[i];
                    if (xx[0].Value == card.Value) {
                        _putidx = i;
                    }
                }
                UnityEngine.Debug.Assert(_putcards[_putidx].Count == 3);
                _putcards[_putidx].Add(card);
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
