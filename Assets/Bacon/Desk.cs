using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using UnityEngine;

namespace Bacon {
    public class Desk : Actor {

        private float _width = 2.0f;
        private float _length = 2.0f;
        private float _height = 2.0f;
        private int _clockleft = 0;

        public Desk(Context ctx, Controller controller, GameObject go)
            : base(ctx, controller, go) {
        }

        public float Width { get { return _width; } }
        public float Length { get { return _length; } }
        public float Height { get { return _height; } }

        public void UpdateClock(int left) {
            _clockleft = left;
            _ctx.EnqueueRenderQueue(RenderUpdateClock);
        }

        protected void RenderUpdateClock() {
            _go.GetComponent<Board>().ShowCountdown(_clockleft);
        }

        public void ShowCountdown() {
            _ctx.EnqueueRenderQueue(RenderShowCountdown);
        }

        private void RenderShowCountdown() {
            _go.GetComponent<Board>().ShowCountdown();
        }

        public void RenderChangeCursor(Vector3 pos) {
            _go.GetComponent<Board>().ChangeCursor(pos);
        }

        public void RenderThrowDice(long d1, long d2) {
            _go.GetComponent<Board>().ThrowDice(d1, d2);
        }

        public void RenderShowBottomSlot(Action cb) {
            _go.GetComponent<Board>().ShowBottomSlot(cb);
        }

        public void RenderCloseBottomSlot(Action cd) {
            _go.GetComponent<Board>().CloseBottomSlot(cd);
        }

        public void RenderShowRightSlot(Action cd) {
            _go.GetComponent<Board>().ShowRightSlot(cd);
        }

        public void RenderCloseRightSlot(Action cb) {
            _go.GetComponent<Board>().CloseRightSlot(cb);
        }

        public void RenderShowTopSlot(Action cd) {
            _go.GetComponent<Board>().ShowTopSlot(cd);
        }

        public void RenderCloseTopSlot(Action cb) {
            _go.GetComponent<Board>().CloseTopSlot(cb);
        }

        public void RenderShowLeftSlot(Action cb) {
            _go.GetComponent<Board>().ShowLeftSlot(cb);
        }

        public void RenderCloseLeftSlot(Action cb) {
            _go.GetComponent<Board>().CloseLeftSlot(cb);
        }

        public void RenderSetDongAtRight() {
            _go.GetComponent<Board>().SetDongAtRight();
        }

        public void RenderSetDongAtTop() {
            _go.GetComponent<Board>().SetDongAtTop();
        }

        public void RenderSetDongAtLeft() {
            _go.GetComponent<Board>().SetDongAtLeft();
        }

        public void RenderSetDongAtBottom() {
            _go.GetComponent<Board>().SetDongAtBottom();
        }

        public void RenderSetNanAtRight() {
            _go.GetComponent<Board>().SetNanAtRight();
        }

        public void RenderSetNanAtTop() {
            _go.GetComponent<Board>().SetNanAtTop();
        }

        public void RenderSetNanAtLeft() {
            _go.GetComponent<Board>().SetNanAtLeft();
        }

        public void RenderSetNanAtBottom() {
            _go.GetComponent<Board>().SetNanAtBottom();
        }

        public void RenderSetXiAtRight() {
            _go.GetComponent<Board>().SetXiAtRight();
        }

        public void RenderSetXiAtTop() {
            _go.GetComponent<Board>().SetXiAtTop();
        }

        public void RenderSetXiAtLeft() {
            _go.GetComponent<Board>().SetXiAtLeft();
        }

        public void RenderSetXiAtBottom() {
            _go.GetComponent<Board>().SetXiAtBottom();
        }

        public void RenderSetBeiAtTop() {
            _go.GetComponent<Board>().SetBeiAtTop();
        }
        public void RenderSetBeiAtLeft() {
            _go.GetComponent<Board>().SetBeiAtLeft();
        }

        public void RenderSetBeiAtBottom() {
            _go.GetComponent<Board>().SetBeiAtBottom();
        }

        public void RenderSetBeiAtRight() {
            _go.GetComponent<Board>().SetBeiAtRight();
        }

        public void RenderTakeOnDong() {
            _go.GetComponent<Board>().TakeOnDong(false);
        }

        public void RenderTakeOffDong() {
            _go.GetComponent<Board>().TakeOffDong();
        }

        public void RenderTakeTurnDong() {
            _go.GetComponent<Board>().TakeTurnDong();
        }

        public void RenderTakeOnNan() {
            _go.GetComponent<Board>().TakeOnNan(false);
        }

        public void RenderTakeOffNan() {
            _go.GetComponent<Board>().TakeOffNan();
        }

        public void RenderTakeTurnNan() {
            _go.GetComponent<Board>().TakeTurnNan();
        }

        public void RenderTakeOnXi() {
            _go.GetComponent<Board>().TakeOnXi(false);
        }

        public void RenderTakeOffXi() {
            _go.GetComponent<Board>().TakeOffXi();
        }

        public void RenderTakeTurnXi() {
            _go.GetComponent<Board>().TakeTurnXi();
        }

        public void RenderTakeOnBei() {
            _go.GetComponent<Board>().TakeOnBei(false);
        }

        public void RenderTakeOffBei() {
            _go.GetComponent<Board>().TakeOffBei();
        }

        public void RenderTakeTurnBei() {
            _go.GetComponent<Board>().TakeTurnBei();
        }
        
    }
}
