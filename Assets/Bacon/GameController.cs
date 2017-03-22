using Maria;
using Maria.Network;
using Sproto;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    public class GameController : Controller {

        private GameService _service = null;
        private UIRootActor _ui = null;
        private GameObject _cardsgo = null;
        private GameObject _uiroot = null;
        private GameObject _d1 = null;
        private GameObject _d2 = null;
        private GameObject _cuour = null;  // 游标
        private Dictionary<long, Card> _cards = new Dictionary<long, Card>();

        private Map _map = null;
        private View _view = null;
        private Scene _scene = null;
        private Desk _desk = null;

        private long _fistidx = 0;
        private long _fisttake = 0;

        private long _curidx = 0;
        private long _curtake = 0;

        private int _hus = 0;
        private int _oknum = 0;
        private int _take1time = 0;
        private int _takeround = 0;
        private int _takepoint = 0;  // 最多是6 

        private long _lastidx = 0;
        private Card _lastCard = null;

        public GameController(Context ctx) : base(ctx) {

            _service = (GameService)_ctx.QueryService(GameService.Name);
            _ui = new UIRootActor(_ctx, this);

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_SCENE, SetupScene);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_BOXINGCARDS, BoxingCards);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);

            EventListenerCmd listener4 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_GUIROOT, SetupUI);
            _ctx.EventDispatcher.AddCmdEventListener(listener4);

            EventListenerCmd listener5 = new EventListenerCmd(MyEventCmd.EVENT_THROWDICE, OnThrowDice);
            _ctx.EventDispatcher.AddCmdEventListener(listener5);

            EventListenerCmd listener6 = new EventListenerCmd(MyEventCmd.EVENT_TAKEDEAL, OnTakeDeal);
            _ctx.EventDispatcher.AddCmdEventListener(listener6);

            EventListenerCmd listener7 = new EventListenerCmd(MyEventCmd.EVENT_PENGCARD, PengCard);
            _ctx.EventDispatcher.AddCmdEventListener(listener7);

            EventListenerCmd listener8 = new EventListenerCmd(MyEventCmd.EVENT_GANGCARD, GangCard);
            _ctx.EventDispatcher.AddCmdEventListener(listener8);

            EventListenerCmd listener9 = new EventListenerCmd(MyEventCmd.EVENT_HUCARD, HuCard);
            _ctx.EventDispatcher.AddCmdEventListener(listener9);

            EventListenerCmd listener10 = new EventListenerCmd(MyEventCmd.EVENT_SORTCARDS, OnSortCards);
            _ctx.EventDispatcher.AddCmdEventListener(listener10);

            EventListenerCmd listener11 = new EventListenerCmd(MyEventCmd.EVENT_LEADCARD, OnLeadCard);
            _ctx.EventDispatcher.AddCmdEventListener(listener11);

            EventListenerCmd listener12 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_BOARD, SetupMap);
            _ctx.EventDispatcher.AddCmdEventListener(listener12);

            EventListenerCmd listener13 = new EventListenerCmd(MyEventCmd.EVENT_SENDCHATMSG, OnSendChatMsg);
            _ctx.EventDispatcher.AddCmdEventListener(listener13);

            EventListenerCmd listener14 = new EventListenerCmd(MyEventCmd.EVENT_TAKEFIRSTCARD, OnTakeFirstCard);
            _ctx.EventDispatcher.AddCmdEventListener(listener14);
        }

        public override void Update(float delta) {
            base.Update(delta);
            if (_scene != null) {
                _scene.Update(delta);
            }
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

        public Desk Desk { get { return _desk; } }

        public long CurIdx { get { return _curidx; } set { _curidx = value; } }
        public long LastIdx { get { return _lastidx; } set { _lastidx = value; } }
        public Card LastCard { get { return _lastCard; } set { _lastCard = value; } }
        public int TakeRound { get { return _takeround; } }

        public void SetupMap(EventCmd e) {
            GameObject map = e.Orgin;
            _desk = new Desk(_ctx, this, map);
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
                        var bc = go.AddComponent<BoxCollider>();
                        bc.center = Vector3.zero;
                        bc.size = new Vector3(Card.Width / 2.0f, Card.Height / 2.0f, Card.Length / 2.0f);

                        lock (_cards) {
                            long value = ((i & 0xff) << 8) | ((j & 0x0f) << 4) | (k & 0x0f);
                            _cards[value] = new Card(_ctx, this, go);
                            _cards[value].Type = (Card.CardType)i;
                            _cards[value].Num = j;
                            _cards[value].Idx = k;
                            _cards[value].Value = value;
                        }
                    }
                }
            }

            UnityEngine.Object xmodel = Resources.Load(prefix + "Dice", typeof(GameObject));
            if (xmodel == null) {
                UnityEngine.Debug.LogError("dice load failture.");
            }
            _d1 = GameObject.Instantiate(xmodel) as GameObject;
            _d1.transform.SetParent(_cardsgo.transform);
            _d1.transform.localPosition = new Vector3(1.0f + 0.05f, 0.1f, 1.0f - 0.05f);
            _d2 = GameObject.Instantiate(xmodel) as GameObject;
            _d2.transform.SetParent(_cardsgo.transform);
            _d2.transform.localPosition = new Vector3(1.0f - 0.05f, 0.1f, 1.0f - 0.05f);

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

        public bool TakeCard(out Card card) {
            if (_service.GetPlayer(_curtake).TakeCard(out card)) {
                return true;
            } else {
                _takepoint++;
                if (_takepoint >= 6) {
                    // over
                    return false;
                } else {
                    _curtake++;
                    _curtake = _curtake > 4 ? 1 : _curtake;
                    return true;
                }
            }
            UnityEngine.Debug.Assert(card != null);
            return true;
        }

        public List<Card> TakeBlock() {
            try {
                if (_takeround == 4) {
                    List<Card> cards = new List<Card>();
                    Card card;
                    if (TakeCard(out card)) {
                        cards.Add(card);
                        return cards;
                    } else {
                        UnityEngine.Debug.Assert(false);
                    }
                } else {
                    List<Card> cards = new List<Card>();
                    Card card;
                    for (int i = 0; i < 4; i++) {
                        if (TakeCard(out card)) {
                            cards.Add(card);
                        } else {
                            UnityEngine.Debug.Assert(false);
                        }
                    }
                    return cards;
                }
                return null;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                return null;
            }
        }

        public SprotoTypeBase OnReady(SprotoTypeBase requestObj) {
            try {
                for (int i = 1; i <= 4; i++) {
                    _service.GetPlayer(i).Controller = this;
                }
                _oknum = 0;
                _scene.SetupPlayer();

                S2cSprotoType.ready.response responseObj = new S2cSprotoType.ready.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.ready.response responseObj = new S2cSprotoType.ready.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public void SendStep() {
            _oknum++;
            if (_oknum == 4) {
                _oknum = 0;
                UnityEngine.Debug.LogFormat("send step.");
                GameService service = (GameService)_ctx.QueryService(GameService.Name);
                C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                request.idx = service.MyIdx;
                _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
            }
        }

        public SprotoTypeBase OnShuffle(SprotoTypeBase requestObj) {
            _oknum = 0;
            S2cSprotoType.shuffle.request obj = requestObj as S2cSprotoType.shuffle.request;
            try {
                GameService service = (GameService)_ctx.QueryService(GameService.Name);
                Player player1 = service.GetPlayer(1);
                player1.Boxing(obj.p1, _cards);

                Player player2 = service.GetPlayer(2);
                player2.Boxing(obj.p2, _cards);

                Player player3 = service.GetPlayer(3);
                player3.Boxing(obj.p3, _cards);

                Player player4 = service.GetPlayer(4);
                player4.Boxing(obj.p4, _cards);

                S2cSprotoType.shuffle.response responseObj = new S2cSprotoType.shuffle.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.shuffle.response responseObj = new S2cSprotoType.shuffle.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public void BoxingCards(EventCmd e) {
            SendStep();
        }

        public SprotoTypeBase OnDice(SprotoTypeBase requestObj) {
            S2cSprotoType.dice.request obj = requestObj as S2cSprotoType.dice.request;
            try {
                _fistidx = obj.first;
                _fisttake = obj.firsttake;
                var player = _service.GetPlayer(obj.first);
                player.ThrowDice(obj.d1, obj.d2);

                S2cSprotoType.dice.response responseObj = new S2cSprotoType.dice.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.dice.response responseObj = new S2cSprotoType.dice.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        private void RenderThrowSDice(long d, GameObject go) {
            switch (d) {
                case 1:
                    go.transform.localRotation = Quaternion.AngleAxis(-180.0f, Vector3.forward);
                    break;
                case 2:
                    go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.right) * Quaternion.AngleAxis(270.0f, Vector3.forward);
                    break;
                case 3:
                    go.transform.localRotation = Quaternion.AngleAxis(-90.0f, Vector3.right) * Quaternion.AngleAxis(270.0f, Vector3.forward);
                    break;
                case 4:
                    go.transform.localRotation = Quaternion.AngleAxis(270.0f, Vector3.forward);
                    break;
                case 5:
                    go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.right) * Quaternion.AngleAxis(270.0f, Vector3.forward);
                    break;
                case 6:
                    go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.right);
                    break;
                default:
                    UnityEngine.Debug.Assert(false);
                    break;
            }
        }

        public void RenderThrowDice(long d1, long d2) {
            RenderThrowSDice(d1, _d1);
            RenderThrowSDice(d2, _d2);
            Command cmd = new Command(MyEventCmd.EVENT_THROWDICE);
            _ctx.Enqueue(cmd);
        }

        public void OnThrowDice(EventCmd e) {
            C2sSprotoType.step.request request = new C2sSprotoType.step.request();
            request.idx = _service.MyIdx;
            _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
        }

        public SprotoTypeBase OnDeal(SprotoTypeBase requestObj) {
            S2cSprotoType.deal.request obj = requestObj as S2cSprotoType.deal.request;
            try {
                _fistidx = obj.firstidx;
                _fisttake = obj.firsttake;
                _curidx = _fistidx;
                _curtake = _fisttake;
                _takeround = 1;
                _take1time = 1;

                _service.GetPlayer(1).CS = obj.p1;
                _service.GetPlayer(2).CS = obj.p2;
                _service.GetPlayer(3).CS = obj.p3;
                _service.GetPlayer(4).CS = obj.p4;

                _service.GetPlayer(_curidx).Deal();

                S2cSprotoType.deal.response responseObj = new S2cSprotoType.deal.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.deal.response responseObj = new S2cSprotoType.deal.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            }
        }

        public void OnTakeDeal(EventCmd e) {
            _take1time++;
            if (_take1time > 4) {
                _takeround++;
                _take1time = 1;
            }
            if (_takeround > 4) {
                for (int i = 1; i <= 4; i++) {
                    _service.GetPlayer(i).SortCards();
                }
                return;
            }
            _curidx += 1;
            if (_curidx > 4) {
                _curidx = 1;
            }
            var player = _service.GetPlayer(_curidx);
            player.Deal();
        }

        public void OnSortCards(EventCmd e) {
            SendStep();
        }

        public void OnUpdateClock(int past, int left) {
            _desk.UpdateClock(left);
        }

        public SprotoTypeBase OnTakeTurn(SprotoTypeBase requestObj) {
            S2cSprotoType.take_turn.request obj = requestObj as S2cSprotoType.take_turn.request;
            try {
                _curidx = obj.your_turn;
                _ctx.Countdown(Timer.CLOCK, (int)obj.countdown, OnUpdateClock, null);
                _service.GetPlayer(obj.your_turn).TakeTurn(obj.type, obj.card);

                S2cSprotoType.take_turn.response responseObj = new S2cSprotoType.take_turn.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (KeyNotFoundException ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.take_turn.response responseObj = new S2cSprotoType.take_turn.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public SprotoTypeBase OnCall(SprotoTypeBase requestObj) {
            S2cSprotoType.call.request obj = requestObj as S2cSprotoType.call.request;
            try {
                UnityEngine.Debug.Assert(obj.opcodes.Count > 0);
                for (int i = 0; i < obj.opcodes.Count; i++) {
                    CallInfo call = new CallInfo();
                    call.Card = obj.opcodes[i].card;
                    call.Peng = obj.opcodes[i].peng;
                    call.Gang = obj.opcodes[i].gang;
                    call.Hu = new CallInfo.HuInfo();
                    call.Hu.Code = obj.opcodes[i].hu.code;
                    call.Hu.Jiao = obj.opcodes[i].hu.jiao;
                    call.Hu.Dian = obj.opcodes[i].hu.dian;
                    call.Hu.Card = obj.opcodes[i].hu.card;

                    Player player = _service.GetPlayer(obj.opcodes[i].idx);
                    player.Call = call;
                    player.SetupCall(call.Card, obj.opcodes[i].countdown);
                }

                _ctx.Countdown(Timer.CLOCK, (int)obj.opcodes[0].countdown, OnUpdateClock, null);

                S2cSprotoType.call.response responseObj = new S2cSprotoType.call.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.call.response responseObj = new S2cSprotoType.call.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public SprotoTypeBase OnPeng(SprotoTypeBase requestObj) {
            S2cSprotoType.peng.request obj = requestObj as S2cSprotoType.peng.request;
            try {
                // 
                UnityEngine.Debug.Assert(obj.code == OpCodes.OPCODE_PENG);
                UnityEngine.Debug.Assert(obj.card == _lastCard.Value);

                _service.GetPlayer(obj.idx).Peng(obj.code, obj.card, obj.hor, _service.GetPlayer(_lastidx), _lastCard);

                S2cSprotoType.peng.response responseObj = new S2cSprotoType.peng.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (KeyNotFoundException ex) {
                UnityEngine.Debug.LogError(ex.Message);
                S2cSprotoType.peng.response responseObj = new S2cSprotoType.peng.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public void PengCard(EventCmd e) {
            C2sSprotoType.step.request request = new C2sSprotoType.step.request();
            request.idx = _service.MyIdx;
            _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
        }

        public SprotoTypeBase OnGang(SprotoTypeBase requestObj) {
            S2cSprotoType.gang.request obj = requestObj as S2cSprotoType.gang.request;
            try {
                if (OpCodes.OPCODE_ANGANG == obj.code) {
                    _service.GetPlayer(obj.idx).Gang(OpCodes.OPCODE_ANGANG, obj.card, obj.hor, _service.GetPlayer(obj.idx), _lastCard);
                } else if (OpCodes.OPCODE_ZHIGANG == obj.code) {
                    UnityEngine.Debug.Assert(_lastCard.Value == obj.card);
                    _service.GetPlayer(obj.idx).Gang(OpCodes.OPCODE_ZHIGANG, obj.card, obj.hor, _service.GetPlayer(_lastidx), _lastCard);
                } else if (OpCodes.OPCODE_BUGANG == obj.code) {
                    UnityEngine.Debug.Assert(_lastCard.Value == obj.card);
                    _service.GetPlayer(obj.idx).Gang(OpCodes.OPCODE_BUGANG, obj.card, obj.hor, _service.GetPlayer(_lastidx), _lastCard);
                }

                S2cSprotoType.gang.response responseObj = new S2cSprotoType.gang.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);

                S2cSprotoType.gang.response responseObj = new S2cSprotoType.gang.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            }
        }

        public void GangCard(EventCmd e) {
            C2sSprotoType.step.request request = new C2sSprotoType.step.request();
            request.idx = _service.MyIdx;
            _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
        }

        public SprotoTypeBase OnHu(SprotoTypeBase requestObj) {
            S2cSprotoType.hu.request obj = requestObj as S2cSprotoType.hu.request;
            try {
                _oknum = 0;
                _hus = obj.hus.Count;
                if (obj.hus.Count > 1) {
                    // 一炮多响
                }
                for (int i = 0; i < obj.hus.Count; i++) {
                    Player player = _service.GetPlayer(obj.hus[i].idx);
                    player.Hu(obj.hus[i].code, obj.hus[i].card, _lastCard, obj.hus[i].jiao);
                }

                S2cSprotoType.hu.response responseObj = new S2cSprotoType.hu.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);

                S2cSprotoType.hu.response responseObj = new S2cSprotoType.hu.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            }
        }

        public void HuCard(EventCmd e) {
            _oknum++;
            if (_oknum >= _hus) {
                C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                request.idx = _service.MyIdx;
                _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
            }
        }

        public SprotoTypeBase OnLead(SprotoTypeBase requestObj) {
            S2cSprotoType.lead.request obj = requestObj as S2cSprotoType.lead.request;
            try {
                _service.GetPlayer(obj.idx).Lead(obj.card);

                S2cSprotoType.lead.response responseObj = new S2cSprotoType.lead.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.lead.response responseObj = new S2cSprotoType.lead.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        private void OnLeadCard(EventCmd e) {
            C2sSprotoType.step.request request = new C2sSprotoType.step.request();
            request.idx = _service.MyIdx;
            _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
        }

        public SprotoTypeBase OnOver(SprotoTypeBase requestObj) {
            S2cSprotoType.over.request obj = requestObj as S2cSprotoType.over.request;
            try {

                S2cSprotoType.over.response responseObj = new S2cSprotoType.over.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.over.response responseObj = new S2cSprotoType.over.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public SprotoTypeBase OnRestart(SprotoTypeBase requestObj) {
            S2cSprotoType.restart.request obj = requestObj as S2cSprotoType.restart.request;
            try {

                S2cSprotoType.restart.response responseObj = new S2cSprotoType.restart.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.restart.response responseObj = new S2cSprotoType.restart.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public SprotoTypeBase OnTakeRestart(SprotoTypeBase requestObj) {
            try {
                S2cSprotoType.take_restart.response responseObj = new S2cSprotoType.take_restart.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.take_restart.response responseObj = new S2cSprotoType.take_restart.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public void OnSendRestart(EventCmd e) {
            UnityEngine.Debug.LogFormat("send restart.");
            C2sSprotoType.restart.request request = new C2sSprotoType.restart.request();
            request.idx = _service.MyIdx;
            _ctx.SendReq<C2sProtocol.restart>(C2sProtocol.restart.Tag, request);
        }

        public SprotoTypeBase OnTakeXuanPao(SprotoTypeBase requestObj) {
            S2cSprotoType.take_xuanpao.request obj = requestObj as S2cSprotoType.take_xuanpao.request;
            try {
                _ctx.Countdown(Timer.CLOCK, (int)obj.countdown, OnUpdateClock, null);

                _service.GetPlayer(_service.MyIdx).TakeXuanPao();

                S2cSprotoType.take_xuanpao.response responseObj = new S2cSprotoType.take_xuanpao.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.take_xuanpao.response responseObj = new S2cSprotoType.take_xuanpao.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public SprotoTypeBase OnXuanPao(SprotoTypeBase requestObj) {
            try {

                S2cSprotoType.xuanpao.response responseObj = new S2cSprotoType.xuanpao.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.xuanpao.response responseObj = new S2cSprotoType.xuanpao.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public SprotoTypeBase OnTakeXuanQue(SprotoTypeBase requestObj) {
            S2cSprotoType.take_xuanque.request obj = requestObj as S2cSprotoType.take_xuanque.request;
            try {
                _ctx.Countdown(Timer.CLOCK, (int)obj.countdown, OnUpdateClock, null);
                _service.GetPlayer(obj.your_turn).TakeFirsteCard(obj.card);

                S2cSprotoType.take_xuanque.response responseObj = new S2cSprotoType.take_xuanque.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.take_xuanque.response responseObj = new S2cSprotoType.take_xuanque.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public void OnTakeFirstCard(EventCmd e) {
            _service.GetPlayer(_service.MyIdx).TakeXuanQue();
        }

        public SprotoTypeBase OnXuanQue(SprotoTypeBase requestObj) {
            S2cSprotoType.xuanque.request obj = requestObj as S2cSprotoType.xuanque.request;
            try {

                _service.GetPlayer(obj.idx).XuanQue(obj.que);

                S2cSprotoType.xuanque.response responseObj = new S2cSprotoType.xuanque.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.xuanque.response responseObj = new S2cSprotoType.xuanque.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public void OnSendChatMsg(EventCmd e) {
            C2sSprotoType.rchat.request request = new C2sSprotoType.rchat.request();
            request.idx = _service.MyIdx;
            if ((int)e.Msg["type"] == 1) {
                request.type = 1;
                request.textid = (long)e.Msg["code"];
            } else if ((int)e.Msg["type"] == 2) {

            }
            _ctx.SendReq<C2sProtocol.rchat>(C2sProtocol.rchat.Tag, request);
        }

        public SprotoTypeBase OnRChat(SprotoTypeBase requestObj) {
            try {
                S2cSprotoType.rchat.response responseObj = new S2cSprotoType.rchat.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                UnityEngine.Debug.LogException(ex);
                S2cSprotoType.rchat.response responseObj = new S2cSprotoType.rchat.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

    }
}
