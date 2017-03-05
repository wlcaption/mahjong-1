using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using DG.Tweening;

namespace Bacon {
    class LeftPlayer : Player {

        public LeftPlayer(Context ctx, GameService service)
            : base(ctx, service) {
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_LEFTPLAYER, OnSetup);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        private void OnSetup(EventCmd e) {
            _go = e.Orgin;
            ((GameController)_controller).SendStep();
        }

        private void RenderSetup() {
            _go.GetComponent<global::LeftPlayer>().ShowUI();
        }

        protected override void RenderBoxing() {
            try {
                Desk desk = ((GameController)_controller).Desk;
                for (int i = 0; i < _takecards.Count; i++) {
                    int idx = i / 2;
                    float x = _takebottomoffset + Card.Length / 2.0f;
                    float y = Card.Height / 2.0f;
                    float z = _takeleftoffset + idx * Card.Width + Card.Width / 2.0f;
                    if (i % 2 == 0) {
                        y = Card.Height / 2.0f + Card.Height;
                    } else if (i % 2 == 1) {
                        y = Card.Height;
                    }
                    Card card = _takecards[i];
                    card.Go.transform.localPosition = new UnityEngine.Vector3(x, y, z);
                    card.Go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(180.0f, Vector3.forward);
                }

                Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_BOXINGCARDS);
                _ctx.Enqueue(cmd);
            } catch (NullReferenceException ex) {
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
                float x = _bottomoffset + Card.Height / 2.0f;
                float y = Card.Length / 2.0f;
                float z = desk.Length - (_leftoffset + Card.Width * i + Card.Width / 2.0f);

                card.Go.transform.localPosition = new Vector3(x, y, z);
                card.Go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(-90.0f, Vector3.right);
            }
            Command cmd = new Command(MyEventCmd.EVENT_TAKEDEAL);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderSortCards() {
            int count = 0;
            Desk desk = ((GameController)_controller).Desk;
            for (int i = 0; i < _cards.Count; i++) {
                float x = _bottomoffset + Card.Height / 2.0f;
                float y = Card.Length / 2.0f;
                float z = desk.Length - (_leftoffset + Card.Width * i + Card.Width / 2.0f);

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(-120.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        var card = _cards[i];
                        card.Go.transform.localPosition = new Vector3(x, y, z);
                        card.Go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(-90.0f, Vector3.right);
                    })
                    .Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(-90.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        if (count >= _cards.Count) {
                            Command cmd = new Command(MyEventCmd.EVENT_SORTCARDS);
                            _ctx.Enqueue(cmd);
                        }
                    });
            }
        }

        protected override void RenderTakeTurn() {
            Desk desk = ((GameController)_controller).Desk;

            float x = _bottomoffset + Card.Height / 2.0f;
            float y = Card.Length / 2.0f;
            float z = desk.Length - (_leftoffset + Card.Width * (_cards.Count + 1) + Card.Width / 2.0f + _holdleftoffset);

            _holdcard.Go.transform.localPosition = new Vector3(x, y, z);
            _holdcard.Go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up) * Quaternion.AngleAxis(-90.0f, Vector3.right);
        }

        protected override void RenderInsert() {
        }

        protected override void RenderLead() {
            Desk desk = ((GameController)_controller).Desk;

            UnityEngine.Debug.Assert(_leadcards.Count > 0);
            int idx = _leadcards.Count - 1;
            Card last = _leadcards[idx];
            int row = _leadcards.Count / 6;
            int col = _leadcards.Count % 6;

            float x = _leadbottomoffset + Card.Length / 2.0f - row * Card.Length;
            float y = Card.Height / 2.0f;
            float z = desk.Length - (_leadleftoffset + Card.Width * col + Card.Width / 2.0f);

            last.Go.transform.localPosition = new Vector3(x, y, z);
            last.Go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up);

            RenderSortCards();

            Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderPeng() {
            List<Card> cards = _putcards[_putidx];
            for (int i = 0; i < cards.Count; i++) {
                float x = 0.15f;
                float y = 0.05f;
                float z = 0.1f + _putidx * 0.3f + Card.Width * i;

                cards[i].Go.transform.localPosition = new Vector3(x, y, z);
                cards[i].Go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up);
            }
            Command cmd = new Command(MyEventCmd.EVENT_PENGCARD);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderGang() {
            List<Card> cards = _putcards[_putidx];
            for (int i = 0; i < cards.Count; i++) {
                float x = 0.15f;
                if (i == 3) {
                    x = 0.15f + Card.Width;
                } else {
                    x = 0.15f;
                }
                float y = 0.05f;
                float z = 0.1f + _putidx * 0.3f + Card.Width * i;

                cards[i].Go.transform.localPosition = new Vector3(x, y, z);
                cards[i].Go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up);
            }
            Command cmd = new Command(MyEventCmd.EVENT_PENGCARD);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderHu() {
        }
    }
}
