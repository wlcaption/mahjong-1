using Bacon;
using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon {
    class TopPlayer : Player {

        public TopPlayer(Context ctx, GameService service) : base(ctx, service) {
        }

        public override void Boxing(List<long> cs, Dictionary<long, Card> cards) {
            for (int i = 0; i < cs.Count; i++) {
                try {
                    long c = cs[i];
                    if (cards.ContainsKey(c)) {
                        Card card = cards[c];
                        UnityEngine.Debug.Assert(card.GetPlayer() == null);
                        card.SetPlayer(this);
                        _takecards.Add(card);
                    } else {
                        UnityEngine.Debug.LogError(string.Format("not found key {0}", c));
                    }
                } catch (Exception ex) {
                    UnityEngine.Debug.LogException(ex);
                }
            }
            UnityEngine.Debug.Assert(cs.Count == 28 || cs.Count == 26);
            _ctx.EnqueueRenderQueue(RenderBoxing);
        }

        public void RenderBoxing() {
            for (int i = 0; i < _takecards.Count; i++) {
                int idx = i / 2;
                float x = 0.3f + idx * 0.1f + 0.5f;
                float y = 0.0f;
                float z = 0.25f;
                if (i % 2 == 0) {
                    y = -(0.05f + 0.1f + 0.05f);
                } else if (i % 2 == 1) {
                    y = -(0.05f + 0.05f);
                }
                Card card = _cards[i];
                card.Go.transform.localPosition = new UnityEngine.Vector3(x, y, z);
                card.Go.transform.localEulerAngles.Set(0.0f, 180.0f, 180.0f);
            }
        }
    }
}
