using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Bacon {
    class BottomPlayer : Player {

        private Card _leadcard1 = null;

        public BottomPlayer(Context ctx, GameService service) : base(ctx, service) {

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_BOTTOMPLAYER, OnSetup);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_PENG, OnSendPeng);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);

            EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_GANG, OnSendGang);
            _ctx.EventDispatcher.AddCmdEventListener(listener3);

            EventListenerCmd listener4 = new EventListenerCmd(MyEventCmd.EVENT_HU, OnSendHu);
            _ctx.EventDispatcher.AddCmdEventListener(listener4);

            EventListenerCmd listener5 = new EventListenerCmd(MyEventCmd.EVENT_GANG, OnSendGuo);
            _ctx.EventDispatcher.AddCmdEventListener(listener5);

            EventListenerCmd listener6 = new EventListenerCmd(MyEventCmd.EVENT_LEAD, OnSendLead);
            _ctx.EventDispatcher.AddCmdEventListener(listener6);

        }

        private void OnSetup(EventCmd e) {
            _go = e.Orgin;
            ((GameController)_controller).SendStep();
        }

        private void OnSendPeng(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.idx = _idx;
            request.opcode = OpCodes.OPCODE_PENG;
            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendGang(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.idx = _idx;
            request.opcode = OpCodes.OPCODE_GANG;
            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendGuo(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.idx = _idx;
            request.opcode = OpCodes.OPCODE_GUO;
            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendHu(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.idx = _idx;
            request.opcode = OpCodes.OPCODE_HU;
            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendLead(EventCmd e) {
            Card card = null;
            for (int i = 0; i < _cards.Count; i++) {
                if (_cards[i].Go == e.Orgin) {
                    card = _cards[i];
                }
            }
            UnityEngine.Debug.Assert(card != null);
            _leadcard1 = card;

            C2sSprotoType.lead.request request = new C2sSprotoType.lead.request();
            request.idx = _idx;
            request.card = card.Value;
            _ctx.SendReq<C2sProtocol.lead>(C2sProtocol.lead.Tag, request);
        }

        protected override void RenderBoxing() {
            Desk desk = ((GameController)_controller).Desk;
            for (int i = 0; i < _takecards.Count; i++) {
                int idx = i / 2;
                float x = desk.Width - (_takeleftoffset + idx * Card.Width + Card.Width / 2.0f);
                float y = Card.Height / 2.0f;
                float z = _takebottomoffset + Card.Length / 2.0f;
                if (i % 2 == 0) {
                    y = Card.Height / 2.0f + Card.Height;
                } else if (i % 2 == 1) {
                    y = Card.Height / 2.0f;
                }
                Card card = _takecards[i];
                card.Go.transform.localPosition = new UnityEngine.Vector3(x, y, z);
                card.Go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.forward);
            }

            Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_BOXINGCARDS);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderThrowDice() {
            base.RenderThrowDice();
        }

        protected override void RenderDeal() {
            int i = 0;
            if (_cards.Count == 13) {
                i = 12;
            } else {
                i = _cards.Count - 4;
            }
            for (; i < _cards.Count; i++) {
                var card = _cards[i];
                float x = _leftoffset + Card.Width * i + Card.Width / 2.0f;
                float y = Card.Length / 2.0f;
                float z = _bottomoffset + Card.Height / 2.0f;
                card.Go.transform.localPosition = new Vector3(x, y, z);
                card.Go.transform.localRotation = Quaternion.AngleAxis(-60, Vector3.right);
            }

            Command cmd = new Command(MyEventCmd.EVENT_TAKEDEAL);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderSortCards() {
            for (int i = 0; i < _cards.Count; i++) {
                var card = _cards[i];
                float x = _leftoffset + Card.Width * i + Card.Width / 2.0f;
                float y = Card.Length / 2.0f;
                float z = _bottomoffset + Card.Height / 2.0f;
                card.Go.transform.localPosition = new Vector3(x, y, z);
                card.Go.transform.localRotation = Quaternion.AngleAxis(-60, Vector3.right);

                _go.GetComponent<global::BottomPlayer>().Add(card);
            }

            Command cmd = new Command(MyEventCmd.EVENT_SORTCARDS);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderTakeTurn() {

            float x = _leftoffset + Card.Width * (_cards.Count + 1) + Card.Width / 2.0f + _holdleftoffset;
            float y = Card.Length / 2.0f;
            float z = _bottomoffset + Card.Height / 2.0f;

            _holdcard.Go.transform.localPosition = new Vector3(x, y, z);
            _holdcard.Go.transform.localRotation = Quaternion.AngleAxis(-60, Vector3.right);

            _go.GetComponent<global::BottomPlayer>().SwitchOnTouch();
        }

        protected override void RenderLead() {
            UnityEngine.Debug.Assert(_leadcard1 == _leadcard);
            UnityEngine.Debug.Assert(_leadcards.Count > 0);
            int idx = _leadcards.Count - 1;
            Card last = _leadcards[idx];
            int row = _leadcards.Count / 6;
            int col = _leadcards.Count % 6;

            float x = _leadleftoffset + Card.Width * col + Card.Width / 2.0f;
            float y = Card.Height / 2.0f;
            float z = _leadbottomoffset + Card.Length / 2.0f - row * Card.Length;

            Desk desk = ((GameController)_controller).Desk;
            last.Go.transform.localPosition = new Vector3(x, y, z);
            last.Go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);

            RenderSortCards();

            Command cmd = new Command(MyEventCmd.EVENT_LEADCARD);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderPeng() {
            List<Card> cards = _putcards[_putidx];
            for (int i = 0; i < cards.Count; i++) {
                float x = 2.0f - (1.0f + _putidx * 0.2f + i * Card.Width);
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
                if (i == 3) {
                    float x = 2.0f - (1.0f + _putidx * 0.2f + 1 * Card.Width);
                    float y = Card.Height / 2.0f;
                    float z = 0.25f;
                    cards[i].Go.transform.localPosition = new Vector3(x, y, z);
                    cards[i].Go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
                } else {
                    float x = 2.0f - (1.0f + _putidx * 0.2f + i * Card.Width);
                    float y = Card.Height / 2.0f;
                    float z = 0.25f;
                    cards[i].Go.transform.localPosition = new Vector3(x, y, z);
                    cards[i].Go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
                }
            }
            Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
            _ctx.Enqueue(cmd);
        }

        protected override void RenderHu() { }

        protected override void RenderCall() {
            UnityEngine.Debug.Assert(_opcodes.Count > 0);
            for (int i = 0; i < _opcodes.Count; i++) {
                if (_opcodes[i] == OpCodes.OPCODE_PENG) {
                    _go.GetComponent<global::BottomPlayer>().ShowPeng();
                } else if (_opcodes[i] == OpCodes.OPCODE_GANG) {
                    _go.GetComponent<global::BottomPlayer>().ShowHu();
                } else if (_opcodes[i] == OpCodes.OPCODE_HU) {
                    _go.GetComponent<global::BottomPlayer>().ShowGang();
                }
            }
            _go.GetComponent<global::BottomPlayer>().ShowGuo();
        }

    }
}
