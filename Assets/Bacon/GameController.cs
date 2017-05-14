using Maria;
using Maria.Network;
using Sproto;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon {
    public class GameController : Controller {

        public static readonly string Name = "game";

        private GameService _service = null;
        private GUIRootActor _ui = null;
        private GameObject _cardsgo = null;

        private Dictionary<long, Card> _cards = new Dictionary<long, Card>();

        private Scene _scene = null;
        private Desk _desk = null;

        private int _type;

        // 游戏数据
        private long _fistidx = 0;
        private long _fisttake = 0;

        private long _curidx = 0;
        private long _curtake = 0;

        private int _huscount = 0;
        private int _oknum = 0;
        private int _take1time = 0;
        private int _takeround = 0;
        private int _takepoint = 0;  // 最多是6 

        private long _lastidx = 0;
        private Card _lastCard = null;

        private List<S2cSprotoType.settle> _settles = null;
        private int _settlesidx = 0;

        public GameController(Context ctx) : base(ctx) {

            _service = _ctx.QueryService<GameService>(GameService.Name);
            _ui = new GUIRootActor(_ctx, this);
            _type = GameType.GAME;

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_SCENE, SetupScene);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_BOXINGCARDS, BoxingCards);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);

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

            EventListenerCmd listener10 = new EventListenerCmd(MyEventCmd.EVENT_SORTCARDSAFTERDEAL, OnSortCardsAfterDeal);
            _ctx.EventDispatcher.AddCmdEventListener(listener10);

            EventListenerCmd listener11 = new EventListenerCmd(MyEventCmd.EVENT_LEADCARD, OnLeadCard);
            _ctx.EventDispatcher.AddCmdEventListener(listener11);

            EventListenerCmd listener12 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_BOARD, SetupMap);
            _ctx.EventDispatcher.AddCmdEventListener(listener12);

            EventListenerCmd listener13 = new EventListenerCmd(MyEventCmd.EVENT_SENDCHATMSG, OnSendChatMsg);
            _ctx.EventDispatcher.AddCmdEventListener(listener13);

            EventListenerCmd listener14 = new EventListenerCmd(MyEventCmd.EVENT_TAKEFIRSTCARD, OnTakeFirstCard);
            _ctx.EventDispatcher.AddCmdEventListener(listener14);

            EventListenerCmd listener15 = new EventListenerCmd(MyEventCmd.EVENT_SETTLE_NEXT, OnSettleNext);
            _ctx.EventDispatcher.AddCmdEventListener(listener15);

        }

        public override void Update(float delta) {
            base.Update(delta);
            if (_scene != null) {
                _scene.Update(delta);
            }
        }

        public override void OnEnter() {
            base.OnEnter();
            InitService service = _ctx.QueryService<InitService>(InitService.Name);
            if (service != null) {
                SMActor actor = service.SMActor;
                actor.LoadScene("game");
            }
        }

        public override void OnExit() {
            base.OnExit();
        }

        public Desk Desk { get { return _desk; } }
        public Scene Scene { get { return _scene; } }

        public long CurIdx { get { return _curidx; } set { _curidx = value; } }
        public long LastIdx { get { return _lastidx; } set { _lastidx = value; } }
        public Card LastCard { get { return _lastCard; } set { _lastCard = value; } }
        public int TakeRound { get { return _takeround; } }
        public int Type { get { return _type; } set { _type = value; } }

        public void OnUpdateClock(int past, int left) {
            _desk.UpdateClock(left);
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
                    _curtake--;
                    if (_curtake <= 0) {
                        _curtake = _service.Max;
                    }
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

        public void SetupMap(EventCmd e) {
            GameObject map = e.Orgin;
            _desk = new Desk(_ctx, this, map);
        }

        public void SetupScene(EventCmd e) {
            GameObject word = e.Orgin;
            _scene = new Scene(_ctx, this, word);
            _ctx.EnqueueRenderQueue(RenderLoadCard);
        }

        private void RenderLoadCard() {
            _cardsgo = _scene.Go.transform.FindChild("cards").gameObject;
            string path = "Prefabs/Mahjongs";
            for (int i = 1; i < 4; i++) {
                string prefix = string.Empty;
                if (i == (int)Card.CardType.Crak) {
                    prefix = "Crak_";
                } else if (i == (int)Card.CardType.Bam) {
                    prefix = "Bam_";
                } else if (i == (int)Card.CardType.Dot) {
                    prefix = "Dot_";
                }
                for (int j = 1; j < 10; j++) {
                    string name = prefix + string.Format("{0}", j);
                    GameObject ori = ABLoader.current.LoadAsset<GameObject>(path, name);
                    if (ori == null) {
                        UnityEngine.Debug.LogErrorFormat("Type:{0}, Num:{1} load failed.", i, j);
                        continue;
                    } else {
                        for (int k = 1; k < 5; k++) {
                            GameObject go = GameObject.Instantiate<GameObject>(ori);
                            go.transform.SetParent(_cardsgo.transform);
                            go.transform.localPosition = new Vector3(-1.0f, 0.0f, -1.0f);
                            //go.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

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
            }

            Command cmd = new Command(MyEventCmd.EVENT_LOADEDCARDS);
            _ctx.Enqueue(cmd);
        }

        public void FlushUI() {
            _ctx.EnqueueRenderQueue(RenderFlushUI);
        }

        private void RenderFlushUI() {
            _ui.RenderRoomId((int)_service.RoomId);
        }

        public SprotoTypeBase OnReady(SprotoTypeBase requestObj) {
            try {
                _oknum = 0;

                _service.Foreach((Player player) => {
                    player.FixDirMark();
                });

                C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                request.idx = _service.MyIdx;
                _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);

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

        public SprotoTypeBase OnShuffle(SprotoTypeBase requestObj) {
            S2cSprotoType.shuffle.request obj = requestObj as S2cSprotoType.shuffle.request;
            try {
                _oknum = 0;
                foreach (var item in _cards) {
                    item.Value.Clear();
                }

                GameService service = _ctx.QueryService<GameService>(GameService.Name);
                UnityEngine.Debug.Assert(obj.p1.Count == 28);
                Player player1 = service.GetPlayer(1);
                player1.Boxing(obj.p1, _cards);

                UnityEngine.Debug.Assert(obj.p2.Count == 28);
                Player player2 = service.GetPlayer(2);
                player2.Boxing(obj.p2, _cards);

                UnityEngine.Debug.Assert(obj.p3.Count == 26);
                Player player3 = service.GetPlayer(3);
                player3.Boxing(obj.p3, _cards);

                UnityEngine.Debug.Assert(obj.p4.Count == 26);
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
            _oknum++;
            if (_oknum == 4) {
                _oknum = 0;
                if (_type == GameType.GAME) {
                    UnityEngine.Debug.LogFormat("send step after boxing.");
                    GameService service = _ctx.QueryService<GameService>(GameService.Name);
                    C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                    request.idx = service.MyIdx;
                    _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
                }
            }
        }

        public SprotoTypeBase OnDice(SprotoTypeBase requestObj) {
            S2cSprotoType.dice.request obj = requestObj as S2cSprotoType.dice.request;
            try {
                _fistidx = obj.first;
                _fisttake = obj.firsttake;

                long min = Math.Min(obj.d1, obj.d2);
                _service.GetPlayer(_fisttake).Takecardsidx = (int)(min * 2);

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

        public void RenderThrowDice(long d1, long d2) {
            _desk.RenderThrowDice(d1, d2);
        }

        public void OnThrowDice(EventCmd e) {
            if (_type == GameType.GAME) {
                C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                request.idx = _service.MyIdx;
                _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
            }
        }

        public SprotoTypeBase OnDeal(SprotoTypeBase requestObj) {
            S2cSprotoType.deal.request obj = requestObj as S2cSprotoType.deal.request;
            try {
                _oknum = 0;
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

        public void OnSortCardsAfterDeal(EventCmd e) {
            _oknum++;
            if (_oknum == 4) {
                _oknum = 0;
                if (_type == GameType.GAME) {
                    UnityEngine.Debug.LogFormat("send step after sort cards.");
                    GameService service = _ctx.QueryService<GameService>(GameService.Name);
                    C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                    request.idx = service.MyIdx;
                    _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
                }
            }
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
                _desk.ShowCountdown();

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

        public SprotoTypeBase OnTakeTurn(SprotoTypeBase requestObj) {
            S2cSprotoType.take_turn.request obj = requestObj as S2cSprotoType.take_turn.request;
            try {
                _service.Foreach((Player player) => {
                    player.ClearCall();
                });

                _curidx = obj.your_turn;
                _ctx.Countdown(Timer.CLOCK, (int)obj.countdown, OnUpdateClock, null);
                _service.GetPlayer(obj.your_turn).TakeTurn(obj.type, obj.card, obj.countdown);

                // flame
                _service.Foreach((Player player) => {
                    if (player.Idx == obj.your_turn) {
                        player.PlayFlameCountdown();
                    } else {
                        player.StopFlame();
                    }
                });

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
                _service.Foreach((Player player) => {
                    player.ClearCall();
                });

                UnityEngine.Debug.Assert(obj.opcodes.Count > 0);
                for (int i = 0; i < obj.opcodes.Count; i++) {
                    CallInfo call = new CallInfo();
                    call.Card = obj.opcodes[i].card;
                    call.Dian = obj.opcodes[i].dian;
                    call.Peng = obj.opcodes[i].peng;
                    call.Gang = obj.opcodes[i].gang;

                    call.Hu = new CallInfo.HuInfo();
                    call.Hu.Card = obj.opcodes[i].hu.card;
                    call.Hu.Code = obj.opcodes[i].hu.code;
                    call.Hu.Jiao = obj.opcodes[i].hu.jiao;
                    call.Hu.Gang = obj.opcodes[i].hu.gang;
                    call.Hu.Dian = obj.opcodes[i].hu.dian;

                    Player player = _service.GetPlayer(obj.opcodes[i].idx);
                    player.Call = call;
                    player.SetupCall(obj.opcodes[i].take, obj.opcodes[i].countdown);
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

                _service.Foreach((Player player) => {
                    player.ClearCall();
                });

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
            if (_type == GameType.GAME) {
                C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                request.idx = _service.MyIdx;
                _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
            }
        }

        public SprotoTypeBase OnGang(SprotoTypeBase requestObj) {
            S2cSprotoType.gang.request obj = requestObj as S2cSprotoType.gang.request;
            try {
                _service.Foreach((Player player) => {
                    player.ClearCall();
                    player.ClearSettle();
                });

                if (OpCodes.OPCODE_ANGANG == obj.code) {
                    _service.GetPlayer(obj.idx).Gang(OpCodes.OPCODE_ANGANG, obj.card, obj.hor, _service.GetPlayer(obj.idx), _lastCard);
                } else if (OpCodes.OPCODE_ZHIGANG == obj.code) {
                    UnityEngine.Debug.Assert(_lastCard.Value == obj.card);
                    _service.GetPlayer(obj.idx).Gang(OpCodes.OPCODE_ZHIGANG, obj.card, obj.hor, _service.GetPlayer(_lastidx), _lastCard);
                } else if (OpCodes.OPCODE_BUGANG == obj.code) {
                    _service.GetPlayer(obj.idx).Gang(OpCodes.OPCODE_BUGANG, obj.card, obj.hor, _service.GetPlayer(_lastidx), _lastCard);
                }

                _settles = obj.settles;
                _settlesidx = 0;

                _service.Foreach((Player player) => {
                    S2cSprotoType.settlementitem si = null;
                    S2cSprotoType.settle settle = _settles[_settlesidx];
                    long idx = 0;
                    if (player.Idx == 1) {
                        idx = 1;
                        if (settle.p1 != null) {
                            si = settle.p1;
                        }
                    } else if (player.Idx == 2) {
                        idx = 2;
                        if (settle.p2 != null) {
                            si = settle.p2;
                        }
                    } else if (player.Idx == 3) {
                        idx = 3;
                        if (settle.p3 != null) {
                            si = settle.p3;
                        }
                    } else if (player.Idx == 4) {
                        idx = 4;
                        if (settle.p4 != null) {
                            si = settle.p4;
                        }
                    }
                    if (si != null) {
                        SettlementItem item = new SettlementItem();
                        item.Idx = si.idx;
                        item.Chip = si.chip;  // 有正负
                        item.Left = si.left;  // 以次值为准

                        item.Win = si.win;
                        item.Lose = si.lose;

                        item.Gang = si.gang;
                        item.HuCode = si.hucode;
                        item.HuJiao = si.hujiao;
                        item.HuGang = si.hugang;
                        item.HuaZhu = si.huazhu;
                        item.DaJiao = si.dajiao;
                        item.TuiSui = si.tuisui;

                        _service.GetPlayer(idx).AddSettle(item);
                        _service.GetPlayer(idx).GangSettle();
                    }
                });

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
            if (_type == GameType.GAME) {
                C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                request.idx = _service.MyIdx;
                _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
            }
        }

        public SprotoTypeBase OnHu(SprotoTypeBase requestObj) {
            S2cSprotoType.hu.request obj = requestObj as S2cSprotoType.hu.request;
            try {
                _service.Foreach((Player player) => {
                    player.ClearCall();
                    player.ClearSettle();
                });

                _oknum = 0;
                _huscount = obj.hus.Count;
                if (obj.hus.Count > 1) {
                    // 一炮多响
                }

                long dian = 0;
                for (int i = 0; i < obj.hus.Count; i++) {
                    S2cSprotoType.huinfo huinfo = obj.hus[i];
                    Card card = _lastCard;
                    if (dian == 0) {
                        dian = huinfo.dian;
                    } else {
                        UnityEngine.Debug.Assert(dian == huinfo.dian);
                    }

                    Player player = _service.GetPlayer(huinfo.idx);
                    player.Hu(obj.hus[i].code, obj.hus[i].card, obj.hus[i].jiao, obj.hus[i].gang, obj.hus[i].dian, _service.GetPlayer(obj.hus[i].dian), card);
                }

                _settles = obj.settles;
                _settlesidx = 0;

                _service.Foreach((Player player) => {
                    for (int i = 0; i < _settles.Count; i++) {
                        S2cSprotoType.settlementitem si = null;
                        S2cSprotoType.settle settle = _settles[i];
                        long idx = 0;
                        if (player.Idx == 1) {
                            idx = 1;
                            if (settle.p1 != null) {
                                si = settle.p1;
                            }
                        } else if (player.Idx == 2) {
                            idx = 2;
                            if (settle.p2 != null) {
                                si = settle.p2;
                            }
                        } else if (player.Idx == 3) {
                            idx = 3;
                            if (settle.p3 != null) {
                                si = settle.p3;
                            }
                        } else if (player.Idx == 4) {
                            idx = 4;
                            if (settle.p4 != null) {
                                si = settle.p4;
                            }
                        }
                        if (si != null) {
                            SettlementItem item = new SettlementItem();
                            item.Idx = si.idx;
                            item.Chip = si.chip;  // 有正负
                            item.Left = si.left;  // 以次值为准

                            item.Win = si.win;
                            item.Lose = si.lose;

                            item.Gang = si.gang;
                            item.HuCode = si.hucode;
                            item.HuJiao = si.hujiao;
                            item.HuGang = si.hugang;
                            item.HuaZhu = si.huazhu;
                            item.DaJiao = si.dajiao;
                            item.TuiSui = si.tuisui;

                            _service.GetPlayer(idx).AddSettle(item);
                            _service.GetPlayer(idx).HuSettle();
                        }
                    }
                });

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
            if (_oknum >= _huscount) {
                if (_type == GameType.GAME) {
                    C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                    request.idx = _service.MyIdx;
                    _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
                }
            }
        }

        public SprotoTypeBase OnLead(SprotoTypeBase requestObj) {
            S2cSprotoType.lead.request obj = requestObj as S2cSprotoType.lead.request;
            try {

                _ctx.Countdown(Timer.CLOCK, -1, null, null);
                _desk.UpdateClock(0);

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
            if (_type == GameType.GAME) {
                C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                request.idx = _service.MyIdx;
                _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
            }
        }

        public SprotoTypeBase OnOver(SprotoTypeBase requestObj) {
            try {
                _service.Foreach((Player player) => {
                    player.Over();
                });

                C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                request.idx = _service.MyIdx;
                _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);

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

        public SprotoTypeBase OnSettle(SprotoTypeBase requestObj) {
            S2cSprotoType.settle.request obj = requestObj as S2cSprotoType.settle.request;
            try {
                _settles = obj.settles;
                _settlesidx = 0;

                _service.Foreach((Player player) => {
                    player.ClearSettle();
                    OnSettleNext(null);
                });

                S2cSprotoType.settle.response responseObj = new S2cSprotoType.settle.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                S2cSprotoType.settle.response responseObj = new S2cSprotoType.settle.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        private void OnSettleNext(EventCmd e) {
            _oknum++;
            if (_oknum >= _service.Max) {
                if (_settlesidx >= _settles.Count) {
                    C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                    request.idx = _service.MyIdx;
                    _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
                    return;
                }

                _service.Foreach((Player player) => {
                    player.ClearSettle();

                    S2cSprotoType.settlementitem si = null;
                    S2cSprotoType.settle settle = _settles[_settlesidx];
                    long idx = 0;
                    if (player.Idx == 1) {
                        idx = 1;
                        if (settle.p1 != null) {
                            si = settle.p1;
                        }
                    } else if (player.Idx == 2) {
                        idx = 2;
                        if (settle.p2 != null) {
                            si = settle.p2;
                        }
                    } else if (player.Idx == 3) {
                        idx = 3;
                        if (settle.p3 != null) {
                            si = settle.p3;
                        }
                    } else if (player.Idx == 4) {
                        idx = 4;
                        if (settle.p4 != null) {
                            si = settle.p4;
                        }
                    }
                    if (si != null) {
                        SettlementItem item = new SettlementItem();
                        item.Idx = si.idx;
                        item.Chip = si.chip;  // 有正负
                        item.Left = si.left;  // 以次值为准

                        item.Win = si.win;
                        item.Lose = si.lose;

                        item.Gang = si.gang;
                        item.HuCode = si.hucode;
                        item.HuJiao = si.hujiao;
                        item.HuGang = si.hugang;
                        item.HuaZhu = si.huazhu;
                        item.DaJiao = si.dajiao;
                        item.TuiSui = si.tuisui;

                        _service.GetPlayer(idx).AddSettle(item);
                        _service.GetPlayer(idx).Settle();
                    }
                });
                _settlesidx++;
            }
        }

        public SprotoTypeBase OnFinalSettle(SprotoTypeBase requestObj) {
            try {
                S2cSprotoType.final_settle.request obj = requestObj as S2cSprotoType.final_settle.request;

                _ui.ShowOver();

                _service.Foreach((Player player) => {
                    player.ClearSettle();
                });

                _service.Foreach((Player player) => {
                    List<S2cSprotoType.settlementitem> settle = null;
                    long idx = 0;
                    if (player.Idx == 1) {
                        idx = 1;
                        if (obj.p1 != null) {
                            settle = obj.p1;
                        }
                    } else if (player.Idx == 2) {
                        idx = 2;
                        if (obj.p2 != null) {
                            settle = obj.p2;
                        }
                    } else if (player.Idx == 3) {
                        idx = 3;
                        if (obj.p3 != null) {
                            settle = obj.p3;
                        }
                    } else if (player.Idx == 4) {
                        idx = 4;
                        if (obj.p4 != null) {
                            settle = obj.p4;
                        }
                    }

                    if (settle != null && settle.Count > 0) {
                        for (int i = 0; i < settle.Count; i++) {
                            SettlementItem item = new SettlementItem();
                            item.Idx = settle[i].idx;
                            item.Chip = settle[i].chip;  // 有正负
                            item.Left = settle[i].left;  // 以次值为准

                            item.Win = settle[i].win;
                            item.Lose = settle[i].lose;

                            item.Gang = settle[i].gang;
                            item.HuCode = settle[i].hucode;
                            item.HuJiao = settle[i].hujiao;
                            item.HuGang = settle[i].hugang;
                            item.HuaZhu = settle[i].huazhu;
                            item.DaJiao = settle[i].dajiao;
                            item.TuiSui = settle[i].tuisui;

                            _service.GetPlayer(idx).AddSettle(item);
                        }

                        _service.GetPlayer(idx).FinalSettle();
                    }

                });

                S2cSprotoType.final_settle.response responseObj = new S2cSprotoType.final_settle.response();
                responseObj.errorcode = Errorcode.SUCCESS;
                return responseObj;
            } catch (Exception ex) {
                S2cSprotoType.final_settle.response responseObj = new S2cSprotoType.final_settle.response();
                responseObj.errorcode = Errorcode.FAIL;
                return responseObj;
            }
        }

        public SprotoTypeBase OnRestart(SprotoTypeBase requestObj) {
            S2cSprotoType.restart.request obj = requestObj as S2cSprotoType.restart.request;
            try {

                _service.GetPlayer(obj.idx).Restart();

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
                _fistidx = 0;
                _fisttake = 0;

                _curidx = 0;
                _curtake = 0;

                _huscount = 0;
                _oknum = 0;
                _take1time = 0;
                _takeround = 0;
                _takepoint = 0;  // 最多是6 

                _lastidx = 0;
                _lastCard = null;

                foreach (var item in _cards) {
                    item.Value.Clear();
                }

                _service.Foreach((Player player) => {
                    player.TakeRestart();
                });

                {
                    C2sSprotoType.step.request request = new C2sSprotoType.step.request();
                    request.idx = _service.MyIdx;
                    _ctx.SendReq<C2sProtocol.step>(C2sProtocol.step.Tag, request);
                }

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

        public void OnSendChatMsg(EventCmd e) {
            if (_type == GameType.GAME) {
                C2sSprotoType.rchat.request request = new C2sSprotoType.rchat.request();
                request.idx = _service.MyIdx;
                if ((int)e.Msg["type"] == 1) {
                    request.type = 1;
                    request.textid = (long)e.Msg["code"];
                } else if ((int)e.Msg["type"] == 2) {

                }
                _ctx.SendReq<C2sProtocol.rchat>(C2sProtocol.rchat.Tag, request);
            }
        }

        public SprotoTypeBase OnRChat(SprotoTypeBase requestObj) {
            S2cSprotoType.rchat.request obj = requestObj as S2cSprotoType.rchat.request;
            try {

                _service.GetPlayer(obj.idx).Say(obj.textid);

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
