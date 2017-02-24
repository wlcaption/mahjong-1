using Maria;
using Maria.Network;
using Sproto;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    class GameController : Controller {

        private UIRootActor _ui = null;
        private GameObject _cardsgo = null;
        private GameObject _uiroot = null;
        private Dictionary<long, Card> _cards = new Dictionary<long, Card>();


        private Map _map = null;
        private View _view = null;
        private Scene _scene = null;

        public GameController(Context ctx) : base(ctx) {
            _ui = new UIRootActor(_ctx, this);

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_SCENE, SetupScene);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            //EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_MAP, SetupMap);
            //_ctx.EventDispatcher.AddCmdEventListener(listener2);

            //EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_VIEW, SetupCamera);
            //_ctx.EventDispatcher.AddCmdEventListener(listener3);

            EventListenerCmd listener4 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_GUIROOT, SetupUI);
            _ctx.EventDispatcher.AddCmdEventListener(listener4);
        }

        public override void Update(float delta) {
            base.Update(delta);
            if (_scene != null) {
                _scene.Update(delta);
            }
            //Sync1(delta);
        }

        public override void Enter() {
            base.Enter();
            InitService service = (InitService)_ctx.QueryService("init");
            if (service != null) {
                SMActor actor = service.SMActor;
                actor.LoadScene("game");
            }
        }

        public override void Exit() {
            base.Exit();
        }

        public void SetupCamera(EventCmd e) {
            // 无论_camera == null,新场景启动都要重置
            GameObject go = e.Orgin;
            UnityEngine.Debug.Assert(_scene != null);
            _view = _scene.SetupView(go);
        }

        public void SetupMap(EventCmd e) {
            GameObject map = e.Orgin;
            UnityEngine.Debug.Assert(_scene != null);
            _map = _scene.SetupMap(map);
        }

        public void SetupScene(EventCmd e) {
            GameObject word = e.Orgin;
            _scene = new Scene(_ctx, this, word);
            _ctx.EnqueueRenderQueue(RenderLoadCard);
        }

        public void RenderLoadCard() {
            _cardsgo = _scene.Go.transform.FindChild("cards").gameObject;
            string prefix = "Mahjongs/";
            string folder = string.Empty;
            for (int i = 1; i < 4; i++) {
                if (i == (int)Card.CardType.Crak) {
                    folder = prefix + "Crak_";
                } else if (i == (int)Card.CardType.Bam) {
                    folder = prefix + "Bam_";
                } else if (i == (int)Card.CardType.Dot) {
                    folder = prefix + "Dot_";
                }
                for (int j = 1; j < 10; j++) {
                    string path = folder + string.Format("{0}", j);
                    UnityEngine.Object model = Resources.Load(path, typeof(GameObject));
                    if (model == null) {
                        UnityEngine.Debug.LogErrorFormat("Type:{0}, Num:{1} load failed.", i, j);
                        continue;
                    }
                    for (int k = 1; k < 5; k++) {
                    
                        GameObject go = GameObject.Instantiate(model) as GameObject;
                        go.transform.SetParent(_cardsgo.transform);

                        lock (_cards) {
                            long value = ((i & 0xff) << 8) | ((j & 0x0f) << 4) | (k & 0x0f);
                            _cards[value] = new Card(_ctx, this, go);
                            _cards[value].Type = (Card.CardType)i;
                            _cards[value].Num = j;
                            _cards[value].Idx = k;
                        }
                    }
                }
            }
            Command cmd = new Command(MyEventCmd.EVENT_LOADEDCARDS);
            _ctx.Enqueue(cmd);
        }

        public void SetupUI(EventCmd e) {
            _uiroot = e.Orgin;
            _ctx.EnqueueRenderQueue(RenderUI);
        }

        public void RenderUI() {
            GameService service = (GameService)_ctx.QueryService(GameService.Name);
            _uiroot.GetComponent<GUIRoot>().InitUI((int)service.RoomId);
        }

        public SprotoTypeBase OnShuffle(SprotoTypeBase requestObj) {
            S2cSprotoType.shuffle.request obj = requestObj as S2cSprotoType.shuffle.request;
            try {
                GameService service = (GameService)_ctx.QueryService(GameService.Name);
                Player player1 = service.GetPlayer(1);
                player1.Boxing(obj.p1, _cards);

                Player player2 = service.GetPlayer(2);
                player2.Boxing(obj.p2, _cards);

                Player player3 = service.GetPlayer(2);
                player3.Boxing(obj.p3, _cards);

                Player player4 = service.GetPlayer(2);
                player4.Boxing(obj.p4, _cards);

                S2cSprotoType.leave.response responseObj = new S2cSprotoType.leave.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.leave.response responseObj = new S2cSprotoType.leave.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public SprotoTypeBase OnDie(SprotoTypeBase requestObj) {
            //S2cSprotoType.die.request obj = requestObj as S2cSprotoType.die.request;
            //try {
            //    var ball =_scene.Leave(obj.ballid);
            //    Player player = _playes[obj.session];
            //    player.Remove(ball);

            //    S2cSprotoType.leave.response responseObj = new S2cSprotoType.leave.response();
            //    responseObj.errorcode = Errorcode.SUCCESS;
            //    return responseObj;
            //} catch (KeyNotFoundException ex) {
            //    UnityEngine.Debug.LogError(ex.Message);
            //    throw;
            //}
            return null;
        }

        public SprotoTypeBase OnCall(SprotoTypeBase requestObj) {
            S2cSprotoType.call.request obj = requestObj as S2cSprotoType.call.request;
            try {
                var player = ((GameService)_ctx.QueryService(GameService.Name)).GetPlayer(obj.your_turn);
                player.SetupCall(obj.opcode, obj.countdown);

                S2cSprotoType.leave.response responseObj = new S2cSprotoType.leave.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (KeyNotFoundException ex) {
                UnityEngine.Debug.LogError(ex.Message);
                throw;
            }
            return null;
        }

    }
}
