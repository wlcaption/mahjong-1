using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Sproto;
using UnityEngine;

namespace Bacon {
    public class GameService : Service {

        public static readonly string Name = "game";

        private long _myidx = 0;
        private Dictionary<long, Player> _playes = new Dictionary<long, Player>();
        private int _online = 0;
        private bool _loadedcards = false;

        public GameService(Context ctx) : base(ctx) {
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_LOADEDCARDS, LoadedCards);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        public long RoomId { get; set; }

        public long MyIdx { get { return _myidx; } }

        public Player GetPlayer(long idx) {
            if (_playes.ContainsKey(idx)) {
                return _playes[idx];
            } else {
                UnityEngine.Debug.Assert(false);
                return null;
            }
        }

        public void Create(SprotoTypeBase responseObj) {
            C2sSprotoType.create.response obj = responseObj as C2sSprotoType.create.response;
            UnityEngine.Debug.Assert(obj.errorcode == Errorcode.SUCCESS);

            RoomId = obj.roomid;
            UnityEngine.Debug.Assert(_ctx.U.Subid == obj.me.sid);
            _myidx = obj.me.idx;
            _playes[obj.me.idx] = new BottomPlayer(_ctx, this);
            _playes[obj.me.idx].Idx = (int)obj.me.idx;
            _online++;
            SendStep();
            _ctx.Push("game");
        }

        public void Join(SprotoTypeBase responseObj) {
            C2sSprotoType.join.response obj = responseObj as C2sSprotoType.join.response;
            UnityEngine.Debug.Assert(_ctx.U.Subid == obj.me.sid);
            _myidx = obj.me.idx;
            _playes[obj.me.idx] = new BottomPlayer(_ctx, this);
            _playes[obj.me.idx].Idx = (int)obj.me.idx;
            _online++;
            if (obj.ps != null && obj.ps.Count > 0) {
                for (int i = 0; i < obj.ps.Count; i++) {
                    var item = obj.ps[i];
                    long offset = 0;
                    if (item.idx > _myidx) {
                        offset = item.idx - _myidx;
                    } else {
                        offset = item.idx + 4 - _myidx;
                    }
                    switch (offset) {
                        case 1: {
                                var player = new Bacon.LeftPlayer(_ctx, this);
                                _playes[item.idx] = player;
                                player.Idx = (int)item.idx;
                            }
                            break;
                        case 2: {
                                var player = new Bacon.TopPlayer(_ctx, this);
                                _playes[item.idx] = player;
                                player.Idx = (int)item.idx;
                            }
                            break;
                        case 3: {
                                var player = new Bacon.RightPlayer(_ctx, this);
                                _playes[item.idx] = player;
                                player.Idx = (int)item.idx;
                            }
                            break;
                        default:
                            break;
                    }
                    _online++;
                }
            }
            SendStep();
            _ctx.Push("game");
        }

        public SprotoTypeBase OnJoin(SprotoTypeBase requestObj) {
            S2cSprotoType.join.request obj = requestObj as S2cSprotoType.join.request;
            if (obj != null) {
                long offset = 0;
                if (obj.p.idx > _myidx) {
                    offset = obj.p.idx - _myidx;
                } else {
                    offset = obj.p.idx + 4 - _myidx;
                }
                switch (offset) {
                    case 1: {
                            var player = new Bacon.LeftPlayer(_ctx, this);
                            _playes[obj.p.idx] = player;
                            player.Idx = (int)obj.p.idx;

                        }
                        break;
                    case 2: {
                            var player = new Bacon.TopPlayer(_ctx, this);
                            _playes[obj.p.idx] = player;
                            player.Idx = (int)obj.p.idx;

                        }
                        break;
                    case 3: {
                            var player = new Bacon.RightPlayer(_ctx, this);
                            _playes[obj.p.idx] = player;
                            player.Idx = (int)obj.p.idx;
                        }
                        break;
                    default:
                        break;
                }
            }
            _online++;
            SendStep();
            S2cSprotoType.join.response responseObj = new S2cSprotoType.join.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }

        public void Leave(SprotoTypeBase responseObj) {
            C2sSprotoType.leave.response obj = responseObj as C2sSprotoType.leave.response;
        }

        public SprotoTypeBase OnLeave(SprotoTypeBase requestObj) {
            S2cSprotoType.leave.request obj = requestObj as S2cSprotoType.leave.request;
            _online--;

            S2cSprotoType.leave.response responseObj = new S2cSprotoType.leave.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }

        public void SendStep() {
            if (_online == 4 && _loadedcards) {
                C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                request.idx = _myidx;
                _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
            }
        }

        public void Step(SprotoTypeBase responseObj) {
            C2sSprotoType.step.response obj = responseObj as C2sSprotoType.step.response;
            UnityEngine.Debug.Assert(obj.errorcode == Errorcode.SUCCESS);
        }


        public void LoadedCards(EventCmd e) {
            _loadedcards = true;
            SendStep();
        }
    }
}
