using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;

namespace Bacon {
    class MyPlayer : Player {
        public MyPlayer(Context ctx, GameController controller, uint session) : base(ctx, controller, session) {

            EventListenerCmd listener4 = new EventListenerCmd(MyEventCmd.EVENT_PENG, Peng);
            _ctx.EventDispatcher.AddCmdEventListener(listener4);

            EventListenerCmd listener5 = new EventListenerCmd(MyEventCmd.EVENT_GANG, Gang);
            _ctx.EventDispatcher.AddCmdEventListener(listener5);

            EventListenerCmd listener6 = new EventListenerCmd(MyEventCmd.EVENT_HU, Hu);
            _ctx.EventDispatcher.AddCmdEventListener(listener6);

            EventListenerCmd listener7 = new EventListenerCmd(MyEventCmd.EVENT_GUO, Guo);
            _ctx.EventDispatcher.AddCmdEventListener(listener7);
        }

        public void Peng(EventCmd e) {
            //Vector3 dir = new Vector3(0, 0, 1);
            //_player.ChangeDir(dir);
            //try {
            //    if (_mysession != 0) {
            //        C2sSprotoType.opcode.request obj = new C2sSprotoType.opcode.request();
            //        obj.code = (long)(OpCodes.OPCODE_PRESSUP);
            //        _ctx.SendReq<C2sProtocol.opcode>(C2sProtocol.opcode.Tag, obj);
            //    }
            //} catch (KeyNotFoundException ex) {
            //    Debug.LogError(ex.Message);
            //}
        }

        public void Gang(EventCmd e) {
            //Vector3 dir = new Vector3(1, 0, 0);
            //_player.ChangeDir(dir);
            //try {
            //    if (_mysession != 0) {
            //        C2sSprotoType.opcode.request obj = new C2sSprotoType.opcode.request();
            //        obj.code = OpCodes.OPCODE_PRESSRIGHT;
            //        _ctx.SendReq<C2sProtocol.opcode>(C2sProtocol.opcode.Tag, obj);
            //    }
            //} catch (KeyNotFoundException ex) {
            //    Debug.LogError(ex.Message);
            //}
        }

        public void Hu(EventCmd e) {
            //Vector3 dir = new Vector3(0, -1, 0);
            //_player.ChangeDir(dir);
            //try {
            //    if (_mysession != 0) {
            //        C2sSprotoType.opcode.request obj = new C2sSprotoType.opcode.request();
            //        obj.code = OpCodes.OPCODE_PRESSDOWN;
            //        _ctx.SendReq<C2sProtocol.opcode>(C2sProtocol.opcode.Tag, obj);
            //    }
            //} catch (KeyNotFoundException ex) {
            //    Debug.LogError(ex.Message);
            //}
        }

        public void Guo(EventCmd e) {
            //Vector3 dir = new Vector3(-1, 0, 0);
            //_player.ChangeDir(dir);
            //try {
            //    if (_mysession != 0) {
            //        C2sSprotoType.opcode.request obj = new C2sSprotoType.opcode.request();
            //        obj.code = OpCodes.OPCODE_PRESSLEFT;
            //        _ctx.SendReq<C2sProtocol.opcode>(C2sProtocol.opcode.Tag, obj);
            //    }
            //} catch (KeyNotFoundException ex) {
            //    Debug.LogError(ex.Message);
            //}
        }
    }
}
