using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon {
    public class Card : Maria.Actor, IComparable<Card> {

        public static float Width = 0.08f;
        public static float Height = 0.05f;
        public static float Length = 0.14f;

        public enum CardType {
            Crak = 1,
            Bam = 2,
            Dot = 3,
        }

        protected long _value;
        protected CardType _type;
        protected int _num;
        protected int _idx;
        protected int _pos;
        protected int _que;
        protected Player _player = null;

        public Card(Context ctx, Controller controller, GameObject go)
            : base(ctx, controller, go) {
        }

        public long Value { set { _value = value; } get { return _value; } }
        public CardType Type { set { _type = value; } get { return _type; } }
        public int Num { set { _num = value; } get { return _num; } }
        public int Idx { set { _idx = value; } get { return _idx; } }
        public int Pos { set { _pos = value; } get { return _pos; } }

        public void SetQue(CardType value) {
            if (_type == value) {
                _que = 1;
            } else {
                _que = 0;
            }
        }

        public void Clear() {
            _que = 0;
        }

        public void SetPlayer(Player player) { _player = player; }
        public void ClearPlayer() { _player = null; }
        public Player GetPlayer() { return _player; }

        public int CompareTo(Card other) {
            if (_que == other._que) {
                return (int)(_value - other._value);
            } else {
                return _que - other._que;
            }
        }

        public static bool operator ==(Card lhs, Card rhs) {
            if (object.Equals(lhs, null) && object.Equals(rhs, null)) {
                return true;
            } else if (!object.Equals(lhs, null) && object.Equals(rhs, null)) {
                return false;
            } else if (object.Equals(lhs, null) && !object.Equals(rhs, null)) {
                return false;
            }
            return (lhs._type == rhs._type) && (lhs._num == rhs._num);
        }

        public static bool operator !=(Card lhs, Card rhs) {
            return !(lhs == rhs);
        }
    }
}
