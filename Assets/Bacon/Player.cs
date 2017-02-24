using Maria;
using Maria.Network;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    class Player {

        protected Context _ctx = null;
        protected GameController _controller = null;
        protected GameService _service = null;
        protected int _idx;
        protected List<Card> _takecards = new List<Card>();
        protected List<Card> _cards = new List<Card>();
        protected List<Card> _leadcards = new List<Card>();

        public Player(Context ctx, GameController controller) {
            _ctx = ctx;
            _controller = controller;


            //EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUPG_TANK, SetupTank);
            //_ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        public Player(Context ctx, GameService service) {
            _ctx = ctx;
            _service = service;
        }

        public int Idx { get { return _idx; } set { _idx = value; } }

        public virtual void SetupCall(long opcode, long countdown) { }

        public virtual void Boxing(List<long> cs, Dictionary<long, Card> cards) { }
    }
}
