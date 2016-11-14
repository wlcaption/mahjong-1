using Maria;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon
{
    class Board : Actor
    {
        private List<Card> _eastSequence = new List<Card>();
        private List<Card> _southSequence = new List<Card>();
        private List<Card> _westSequence = new List<Card>();
        private List<Card> _northSequence = new List<Card>();

        public Board(Context ctx, Controller controller, GameObject go) 
            : base(ctx, controller, go) {
        }

    }
}
