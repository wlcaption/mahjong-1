using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using UnityEngine;
using Sproto;

namespace Bacon
{
    class GameController : Controller
    {
        public GameController(Context ctx) : base(ctx)
        {
        }

        // 进入房间
        public override void Enter()
        {
            //_ctx.GetController<Init>

        }


        public override void Exit()
        {
        }

        public void Shullle()
        {
        }

        public SprotoTypeBase OnJoin(uint session, SprotoTypeBase requestObj) {
            return null;
        }
    }
}
