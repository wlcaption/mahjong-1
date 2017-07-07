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
            C2sSprotoType.board.request request = new C2sSprotoType.board.request();
            request.subid = _ctx.U.Subid;
            _ctx.SendReq<C2sProtocol.board>(C2sProtocol.board.Tag, request);
        }

        public void FetchAdver() {
            C2sSprotoType.adver.request request = new C2sSprotoType.adver.request();
            request.subid = _ctx.U.Subid;
            _ctx.SendReq<C2sProtocol.adver>(C2sProtocol.adver.Tag, request);
        }

        public void Adver(SprotoTypeBase responseObj) {
            C2sSprotoType.adver.response obj = responseObj as C2sSprotoType.adver.response;

            BoardMgr mgr = ((AppContext)_ctx).GetBoardMgr();
            mgr.AdverMsg = obj.msg;

            UnityEngine.Debug.LogFormat("adver msg : {0}", obj.msg);
            MainController controller = _ctx.Peek<MainController>();
            if (controller != null) {
                _ctx.EnqueueRenderQueue(controller.RenderAdver);
            }
        }

        public void Board(SprotoTypeBase responseObj) {
            C2sSprotoType.board.response obj = responseObj as C2sSprotoType.board.response;

            BoardMgr mgr = ((AppContext)_ctx).GetBoardMgr();
            mgr.BoardMsg = obj.msg;

            UnityEngine.Debug.LogFormat("board msg : {0}", obj.msg);
            MainController controller = _ctx.Peek<MainController>();
            if (controller != null) {
                _ctx.EnqueueRenderQueue(controller.RenderBoard);
            }
        }

    }
}
