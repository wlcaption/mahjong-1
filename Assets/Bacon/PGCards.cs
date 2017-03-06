using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
    public class PGCards {
        private List<Card> _cards;
        private uint _opcode;
        private int _hor;
        private float _width;

        public List<Card> Cards { get { return _cards; } set { _cards = value; } }
        public uint Opcode { get { return _opcode; } set { _opcode = value; } }
        public int Hor { get { return _hor; } set { _hor = value; } }
        public float Width { get { return _width; } set { _width = value; } }
    }
}
