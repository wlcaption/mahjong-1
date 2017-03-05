using Bacon;
using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

namespace Bacon {
    class TopPlayer : Player {
        public TopPlayer(Context ctx, GameService service) : base(ctx, service) {
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_TOPPLAYER, OnSetup);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        private void OnSetup(EventCmd e) {
            _go = e.Orgin;
            ((GameController)_controller).SendStep();
            _ctx.EnqueueRenderQueue(RenderSetup);
        }

        private void RenderSetup() {
            _go.GetComponent<global::TopPlayer>().ShowUI();
        }

        protected override void RenderBoxing() {
            try {
                Desk desk = ((GameController)_controller).Desk;
                for (int i = 0; i < _takecards.Count; i++) {
                    int idx = i / 2;
                    float x = _takeleftoffset + idx * Card.Width + Card.Width / 2.0f;
                    float y = Card.Height / 2.0f;
                    float z = desk.Length - (_takebottomoffset + Card.Length / 2.0f);
                    if (i % 2 == 0) {
                        y = Card.Height / 2.0f + Card.Height;
                    } else if (i % 2 == 1) {
                        y = Card.Height / 2.0f;
                    }
                    Card card = _takecards[i];
                    card.Go.transform.localPosition = new UnityEngine.Vector3(x, y, z);
                    card.Go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(180.0f, Vector3.forward);
                }
                Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_BOXINGCARDS);
                _ctx.Enqueue(cmd);
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
            }
        }

        protected override void RenderThrowDice() {
            base.RenderThrowDice();
        }

        protected override void RenderDeal() {
            Desk desk = ((GameController)_controller).Desk;
            int i = 0;
            if (_cards.Count == 13) {
                i = 12;
            } else {
                i = _cards.Count - 4;
            }
            for (; i < _cards.Count; i++) {
                var card = _cards[i];
                float x = desk.Width - (_leftoffset + Card.Width * i + Card.Width / 2.0f);
                float y = Card.Length / 2.0f;
                float z = desk.Length - (_bottomoffset + Card.Height / 2.0f);
                card.Go.transform.localPosition = new Vector3(x, y, z);
                card.Go.transform.localRotation = Quaternion.AngleAxis(180, Vector3.up) * Quaternion.AngleAxis(-80, Vector3.right);
            }
            Command cmd = new Command(MyEventCmd.EVENT_TAKEDEAL);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderSortCards() {
            Desk desk = ((GameController)_controller).Desk;
            int count = 0;
            int i = 0;
            for (; i < _cards.Count; i++) {
                float x = desk.Width - (_leftoffset + Card.Width * i + Card.Width / 2.0f);
                float y = Card.Length / 2.0f;
                float z = desk.Length - (_bottomoffset + Card.Height / 2.0f);

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(-120.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        var card = _cards[i];
                        card.Go.transform.localPosition = new Vector3(x, y, z);
                    })
                    .Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(180.0f, Vector3.up) * Quaternion.AngleAxis(-90.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        count++;
                        if (count >= (_cards.Count-1)) {
                            UnityEngine.Debug.LogFormat("player top send sort cards");
                            Command cmd = new Command(MyEventCmd.EVENT_SORTCARDS);
                            _ctx.Enqueue(cmd);
                        }
                    });
            }
        }

        protected override void RenderTakeTurn() {
            Desk desk = ((GameController)_controller).Desk;
            float x = desk.Width - (_leftoffset + Card.Width * (_cards.Count + 1) + Card.Width / 2.0f + _holdleftoffset);
            float y = Card.Length / 2.0f + Card.Length;
            float z = desk.Length - (_bottomoffset + Card.Height / 2.0f);

            _holdcard.Go.transform.localPosition = new Vector3(x, y, z);
            _holdcard.Go.transform.localRotation = Quaternion.AngleAxis(180, Vector3.up) * Quaternion.AngleAxis(-80, Vector3.right);

            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(_holdcard.Go.transform.DOMoveY(Card.Length / 2.0f, 0.1f));
        }

        protected override void RenderInsert() { }

        protected override void RenderLead() {
            UnityEngine.Debug.Assert(_leadcards.Count > 0);
            Desk desk = ((GameController)_controller).Desk;

            int idx = _leadcards.Count - 1;
            Card last = _leadcards[idx];
            int row = _leadcards.Count / 6;
            int col = _leadcards.Count % 6;

            float x = desk.Width - (_leadleftoffset + Card.Width * col + Card.Width / 2.0f);
            float y = Card.Height / 2.0f;
            float z = desk.Length - (_leadbottomoffset + Card.Length / 2.0f - row * Card.Length);

            last.Go.transform.localPosition = new Vector3(x, y, z);
            last.Go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up);

            RenderSortCards();

            Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderPeng() {
            List<Card> cards = _putcards[_putidx];
            for (int i = 0; i < cards.Count; i++) {
                float x = 1.0f + _putidx * 0.2f + i * Card.Width;
                float y = Card.Height / 2.0f;
                float z = 0.25f;

                cards[i].Go.transform.localPosition = new Vector3(x, y, z);
                cards[i].Go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
            }
            Command cmd = new Command(MyEventCmd.EVENT_PENGCARD);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderGang() {
            List<Card> cards = _putcards[_putidx];
            for (int i = 0; i < cards.Count; i++) {
                float x = 2.0f - (1.0f + _putidx * 0.2f + 1 * Card.Width);
                float y = 0.1f;
                float z = 1.75f;
                if (i == 3) {
                    z = 1.75f - Card.Width;
                }
                cards[i].Go.transform.localPosition = new Vector3(x, y, z);
                cards[i].Go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
            }
            Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderHu() {
        }

    }
}
