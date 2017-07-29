using Sproto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Model {
    public class BoardMgr {
        private AppContext _ctx = null;
        private string _board = string.Empty;
        private string _adver = string.Empty;

        public BoardMgr(AppContext ctx) {
            _ctx = ctx;
        }

        public string BoardMsg { get { return _board; } set { _board = value; } }
        public string AdverMsg { get { return _adver; } set { _adver = value; } }

        public void FetchBoard() {
            C2sSprotoType.toast1.request request = new C2sSprotoType.toast1.request();
            request.uid = _ctx.U.Uid;
            request.subid = _ctx.U.Subid;
            _ctx.SendReq<C2sProtocol.toast1>(C2sProtocol.toast1.Tag, request);
        }

        public void Board(SprotoTypeBase responseObj) {
            C2sSprotoType.toast1.response obj = responseObj as C2sSprotoType.toast1.response;

            BoardMgr mgr = ((AppContext)_ctx).GetBoardMgr();
            mgr.BoardMsg = obj.text;

            UnityEngine.Debug.LogFormat("board msg : {0}", obj.text);
            MainController controller = _ctx.Peek<MainController>();
            if (controller != null) {
                _ctx.EnqueueRenderQueue(controller.RenderBoard);
            }
        }

        public void FetchAdver() {
            C2sSprotoType.toast2.request request = new C2sSprotoType.toast2.request();
            request.uid = _ctx.U.Uid;
            request.subid = _ctx.U.Subid;
            _ctx.SendReq<C2sProtocol.toast2>(C2sProtocol.toast2.Tag, request);
        }

        public void Adver(SprotoTypeBase responseObj) {
            C2sSprotoType.toast2.response obj = responseObj as C2sSprotoType.toast2.response;

            BoardMgr mgr = ((AppContext)_ctx).GetBoardMgr();
            mgr.AdverMsg = obj.text;

            UnityEngine.Debug.LogFormat("adver msg : {0}", obj.text);
            MainController controller = _ctx.Peek<MainController>();
            if (controller != null) {
                _ctx.EnqueueRenderQueue(controller.RenderAdver);
            }
        }

       

    }
}
