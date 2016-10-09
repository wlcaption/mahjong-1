using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using UnityEngine;

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
            LoadScene("sky");

        }


        public override void Exit()
        {
        }

        public void Shullle()
        {
        }

        public override void Run()
        {
            base.Run();
            _ctx.Push("game");
        }
    }
}
