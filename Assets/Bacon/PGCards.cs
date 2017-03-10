using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
    public class PGCards {
        private long _opcode;
        private long _hor;
        private float _width;
        private List<Card> _cards;
        
        public long Opcode { get { return _opcode; } set { _opcode = value; } }
        public long Hor { get { return _hor; } set { _hor = value; } }
        public List<Card> Cards { get { return _cards; } set { _cards = value; } }
        
        public float Width { get { return _width; } set { _width = value; } }
    }
}
