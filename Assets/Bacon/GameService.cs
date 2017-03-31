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

        private long _roomid = 0;
        private long _max = 0;
        private long _myidx = 0;
        private long _online = 0;
        
        private Dictionary<long, Player> _playes = new Dictionary<long, Player>();
        private GameController _controller = null;
        private bool _loadedcards = false;

        public GameService(Context ctx) : base(ctx) {
            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_LOADEDCARDS, LoadedCards);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);
        }

        public long RoomId { get { return _roomid; } }
        public long MyIdx { get { return _myidx; } }

        public Player GetPlayer(long idx) {
            if (_playes.ContainsKey(idx)) {
                return _playes[idx];
            } else {
                UnityEngine.Debug.Assert(false);
                return null;
            }
        }

        public void Foreach(Action<Player> cb) {
            for (int i = 1; i <= _max; i++) {
                cb(_playes[i]);
            }
        }

        public void Create(SprotoTypeBase responseObj) {
            _controller = (GameController)_ctx.Push(typeof(GameController));

            C2sSprotoType.create.response obj = responseObj as C2sSprotoType.create.response;
            UnityEngine.Debug.Assert(obj.errorcode == Errorcode.SUCCESS);
            _roomid = obj.roomid;
            _max = obj.room_max;

            Player player = new BottomPlayer(_ctx, this);
            player.Idx = (int)obj.me.idx;
            player.Chip = (int)obj.me.chip;
            player.Sid = (int)obj.me.sid;
            player.Sex = (int)obj.me.sex;
            player.Name = obj.me.name;
            player.Controller = _controller;

            _myidx = obj.me.idx;
            _playes[_myidx] = player;
            UnityEngine.Debug.Assert(_ctx.U.Subid == obj.me.sid);
            
            _online++;
        }

        public void Join(SprotoTypeBase responseObj) {
            _controller = (GameController)_ctx.Push(typeof(GameController));

            C2sSprotoType.join.response obj = responseObj as C2sSprotoType.join.response;
            _roomid = obj.roomid;
            _max = obj.room_max;

            Player player = new BottomPlayer(_ctx, this);
            player.Idx = (int)obj.me.idx;
            player.Chip = (int)obj.me.chip;
            player.Sid = (int)obj.me.sid;
            player.Sex = (int)obj.me.sex;
            player.Name = obj.me.name;
            player.Controller = _controller;

            _myidx = obj.me.idx;
            _playes[_myidx] = player;
            UnityEngine.Debug.Assert(_ctx.U.Subid == obj.me.sid);
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
                                var lplayer = new Bacon.LeftPlayer(_ctx, this);
                                lplayer.Idx = (int)item.idx;
                                lplayer.Chip = (int)item.chip;
                                lplayer.Sid = (int)item.sid;
                                lplayer.Sex = (int)item.sex;
                                lplayer.Name = item.name;
                                lplayer.Controller = _controller;
                                _playes[item.idx] = lplayer;

                            }
                            break;
                        case 2: {
                                var tplayer = new Bacon.TopPlayer(_ctx, this);
                                tplayer.Idx = (int)item.idx;
                                tplayer.Chip = (int)item.chip;
                                tplayer.Sid = (int)item.sid;
                                tplayer.Sex = (int)item.sex;
                                tplayer.Name = item.name;
                                tplayer.Controller = _controller;
                                _playes[item.idx] = tplayer;
                            }
                            break;
                        case 3: {
                                var rplayer = new Bacon.RightPlayer(_ctx, this);
                                rplayer.Idx = (int)item.idx;
                                rplayer.Chip = (int)item.chip;
                                rplayer.Sid = (int)item.sid;
                                rplayer.Sex = (int)item.sex;
                                rplayer.Name = item.name;
                                rplayer.Controller = _controller;
                                _playes[item.idx] = rplayer;
                            }
                            break;
                        default:
                            break;
                    }
                    _online++;
                }
            }
            SendStep();
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
                            var lplayer = new Bacon.LeftPlayer(_ctx, this);
                            lplayer.Idx = (int)obj.p.idx;
                            lplayer.Chip = (int)obj.p.chip;
                            lplayer.Sid = (int)obj.p.sid;
                            lplayer.Sex = (int)obj.p.sex;
                            lplayer.Name = obj.p.name;
                            lplayer.Controller = _controller;
                            _playes[obj.p.idx] = lplayer;
                            if (_loadedcards) {
                                _controller.Scene.SetupLeftPlayer();
                            }
                        }
                        break;
                    case 2: {
                            var tplayer = new Bacon.TopPlayer(_ctx, this);
                            tplayer.Idx = (int)obj.p.idx;
                            tplayer.Chip = (int)obj.p.chip;
                            tplayer.Sid = (int)obj.p.sid;
                            tplayer.Sex = (int)obj.p.sex;
                            tplayer.Name = obj.p.name;
                            tplayer.Controller = _controller;
                            _playes[obj.p.idx] = tplayer;
                            if (_loadedcards) {
                                _controller.Scene.SetupTopPlayer();
                            }
                        }
                        break;
                    case 3: {
                            var rplayer = new Bacon.RightPlayer(_ctx, this);
                            rplayer.Idx = (int)obj.p.idx;
                            rplayer.Chip = (int)obj.p.chip;
                            rplayer.Sid = (int)obj.p.sid;
                            rplayer.Sex = (int)obj.p.sex;
                            rplayer.Name = obj.p.name;
                            rplayer.Controller = _controller;
                            _playes[obj.p.idx] = rplayer;
                            if (_loadedcards) {
                                _controller.Scene.SetupRightPlayer();
                            }
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
            //S2cSprotoType.leave.request obj = requestObj as S2cSprotoType.leave.request;
            _online--;

            S2cSprotoType.leave.response responseObj = new S2cSprotoType.leave.response();
            responseObj.errorcode = Errorcode.SUCCESS;
            return responseObj;
        }

        public void SendStep() {
            if ((_online == _max) && _loadedcards) {

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
            _controller.FlushUI();
            for (int i = 1; i <= _max; i++) {
                if (_playes.ContainsKey(i)) {
                    if (_playes[i].Ori == Player.Orient.BOTTOM) {
                        _controller.Scene.SetupBottomPlayer();
                    } else if (_playes[i].Ori == Player.Orient.LEFT) {
                        _controller.Scene.SetupLeftPlayer();
                    } else if (_playes[i].Ori == Player.Orient.TOP) {
                        _controller.Scene.SetupTopPlayer();
                    } else if (_playes[i].Ori == Player.Orient.RIGHT) {
                        _controller.Scene.SetupRightPlayer();
                    }
                }
            }

            SendStep();
        }

    }
}
