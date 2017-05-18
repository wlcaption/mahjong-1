using Maria;
using System;
using UnityEngine;
using DG.Tweening;
using Bacon.Service;
using Bacon.Event;

namespace Bacon.Game {
    class BottomPlayer : Player {

        private Card _leadcard1 = null;
        private GameObject _xuanpao = null;
        private GameObject _xuanque = null;
        private global::BottomPlayer _com;

        public BottomPlayer(Context ctx, GameService service) : base(ctx, service) {
            _upv = Quaternion.AngleAxis(0.0f, Vector3.up);
            _uph = Quaternion.AngleAxis(-90.0f, Vector3.up);
            _downv = Quaternion.AngleAxis(180.0f, Vector3.forward);
            _backv = Quaternion.Euler(-25.0f, 0.0f, 0.0f);

            _ori = Orient.BOTTOM;
            _takeleftoffset = 0.5f;
            _takebottomoffset = 0.42f;

            _leftoffset = 0.5f;
            _bottomoffset = 0.2f;

            _leadcardoffset = new Vector3(0.05f, 0.0f, 0.0f);
            _leadleftoffset = 0.8f;
            _leadbottomoffset = 0.8f;

            _putbottomoffset = 0.235f - Card.Length / 2.0f;
            _putrightoffset = 0.25f - Card.Width / 2.0f;

            // 手
            _rhandinitpos = new Vector3(0.4f, -1.0f, -1.8f);
            _rhandinitrot = Quaternion.Euler(30.0f, 0.0f, 0.0f);
            _rhanddiuszoffset = new Vector3(0.154f, -1.965f, 0.123f);
            _rhandtakeoffset = new Vector3(-0.39f, -1.445f, -1.546f);
            _rhandleadoffset = new Vector3(-0.592f, -1.954f, -1.111f);
            _rhandnaoffset = new Vector3(-0.413f, -1.633f, -1.456f);
            _rhandpgoffset = Vector3.zero;
            _rhandhuoffset = Vector3.zero;

            _lhandinitpos = new Vector3(0.4f, -1.0f, -1.8f);
            _lhandinitrot = Quaternion.Euler(30.0f, 0.0f, 0.0f);
            _lhandhuoffset = Vector3.zero;

            EventListenerCmd listener1 = new EventListenerCmd(MyEventCmd.EVENT_SETUP_BOTTOMPLAYER, OnSetup);
            _ctx.EventDispatcher.AddCmdEventListener(listener1);

            EventListenerCmd listener2 = new EventListenerCmd(MyEventCmd.EVENT_PENG, OnSendPeng);
            _ctx.EventDispatcher.AddCmdEventListener(listener2);

            EventListenerCmd listener3 = new EventListenerCmd(MyEventCmd.EVENT_GANG, OnSendGang);
            _ctx.EventDispatcher.AddCmdEventListener(listener3);

            EventListenerCmd listener4 = new EventListenerCmd(MyEventCmd.EVENT_HU, OnSendHu);
            _ctx.EventDispatcher.AddCmdEventListener(listener4);

            EventListenerCmd listener5 = new EventListenerCmd(MyEventCmd.EVENT_GUO, OnSendGuo);
            _ctx.EventDispatcher.AddCmdEventListener(listener5);

            EventListenerCmd listener6 = new EventListenerCmd(MyEventCmd.EVENT_LEAD, OnSendLead);
            _ctx.EventDispatcher.AddCmdEventListener(listener6);

            EventListenerCmd listener7 = new EventListenerCmd(MyEventCmd.EVENT_XUANQUE, OnSendQue);
            _ctx.EventDispatcher.AddCmdEventListener(listener7);

            EventListenerCmd listener8 = new EventListenerCmd(MyEventCmd.EVENT_XUANPAO, OnSendPao);
            _ctx.EventDispatcher.AddCmdEventListener(listener8);
        }

        public override void Init() {
            base.Init();
            if (_sex == 1) {

            }
        }

        protected override void RenderPlayFlameCountdown() {
            _com.Head.PlayFlameCountdown(_cd);
        }

        protected override void RenderStopFlame() {
            _com.Head.StopFlame();
        }

        private void OnSetup(EventCmd e) {
            _go = e.Orgin;
            _ctx.EnqueueRenderQueue(RenderSetup);
        }

        private void RenderSetup() {
            _com = _go.GetComponent<global::BottomPlayer>();
            _com.ShowUI();
            _com.Head.SetGold(_chip);
            UnityEngine.Debug.Log("2");
            // 设置头像
        }

        protected override Vector3 CalcPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            float x = _leftoffset + Card.Width * pos + Card.Width / 2.0f;
            float y = Card.Length / 2.0f + Card.HeightMZ;
            y = 0.1f;
            float z = _bottomoffset + Card.Height / 2.0f;
            z = 0.235f;

            return new Vector3(x, y, z);
        }

        protected override Vector3 CalcLeadPos(int pos) {
            Desk desk = ((GameController)_controller).Desk;
            //int row = (pos + 1) / 6;
            //int col = (pos + 1) % 6;
            int row = (pos) / 6;
            int col = (pos) % 6;

            float x = _leadleftoffset + (Card.Width * col) + (Card.Width / 2.0f);
            float y = Card.Height / 2.0f + Card.HeightMZ;
            float z = _leadbottomoffset - (Card.Length * row) - (Card.Length / 2.0f);

            return new Vector3(x, y, z);
        }

        private void OnSendPeng(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.op = new C2sSprotoType.opinfo();
            request.op.idx = _idx;

            request.op.card = Call.Card;
            request.op.dian = Call.Dian;
            request.op.guo = OpCodes.OPCODE_NONE;
            request.op.peng = Call.Peng;
            request.op.gang = OpCodes.OPCODE_NONE;

            request.op.hu = new C2sSprotoType.huinfo();
            request.op.hu.idx = _idx;
            request.op.hu.code = HuType.NONE;

            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendGang(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.op = new C2sSprotoType.opinfo();
            request.op.idx = _idx;
            request.op.card = Call.Card;
            request.op.dian = Call.Dian;

            request.op.guo = OpCodes.OPCODE_NONE;
            request.op.peng = OpCodes.OPCODE_NONE;
            request.op.gang = Call.Gang;

            request.op.hu = new C2sSprotoType.huinfo();
            request.op.hu.idx = _idx;
            request.op.hu.code = HuType.NONE;

            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendGuo(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.op = new C2sSprotoType.opinfo();
            request.op.idx = _idx;
            request.op.card = Call.Card;
            request.op.dian = Call.Dian;

            request.op.guo = OpCodes.OPCODE_GUO;
            request.op.peng = OpCodes.OPCODE_NONE;
            request.op.gang = OpCodes.OPCODE_NONE;

            request.op.hu = new C2sSprotoType.huinfo();
            request.op.hu.idx = _idx;
            request.op.hu.code = OpCodes.OPCODE_NONE;

            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendHu(EventCmd e) {
            C2sSprotoType.call.request request = new C2sSprotoType.call.request();
            request.op = new C2sSprotoType.opinfo();
            request.op.idx = _idx;
            request.op.card = Call.Card;
            request.op.dian = Call.Dian;

            request.op.guo = OpCodes.OPCODE_NONE;
            request.op.peng = OpCodes.OPCODE_NONE;
            request.op.gang = OpCodes.OPCODE_NONE;

            request.op.hu = new C2sSprotoType.huinfo();
            request.op.hu.idx = _idx;
            request.op.hu.card = Call.Hu.Card;
            request.op.hu.code = Call.Hu.Code;
            request.op.hu.jiao = Call.Hu.Jiao;
            request.op.hu.gang = Call.Hu.Gang;
            request.op.hu.dian = Call.Hu.Dian;

            _ctx.SendReq<C2sProtocol.call>(C2sProtocol.call.Tag, request);
        }

        private void OnSendLead(EventCmd e) {
            Card card = null;
            for (int i = 0; i < _cards.Count; i++) {
                if (_cards[i].Go == e.Orgin) {
                    card = _cards[i];
                }
            }
            if (card == null) {
                UnityEngine.Debug.Assert(e.Orgin == _holdcard.Go);
                card = _holdcard;
            }

            _leadcard1 = card;

            C2sSprotoType.lead.request request = new C2sSprotoType.lead.request();
            request.idx = _idx;
            request.card = card.Value;
            _ctx.SendReq<C2sProtocol.lead>(C2sProtocol.lead.Tag, request);
        }

        private void OnSendQue(EventCmd e) {
            C2sSprotoType.xuanque.request request = new C2sSprotoType.xuanque.request();
            request.idx = _idx;
            request.que = (long)(Card.CardType)e.Msg["cardtype"];
            _ctx.SendReq<C2sProtocol.xuanque>(C2sProtocol.xuanque.Tag, request);
        }

        private void OnSendPao(EventCmd e) {
            C2sSprotoType.xuanpao.request request = new C2sSprotoType.xuanpao.request();
            request.idx = _idx;
            request.fen = (long)e.Msg["fen"];
            _ctx.SendReq<C2sProtocol.xuanpao>(C2sProtocol.xuanpao.Tag, request);
        }

        protected override void RenderFixDirMark() {
            if (_idx == 1) {
                ((GameController)_controller).Desk.RenderSetDongAtBottom();
                ((GameController)_controller).Desk.RenderTakeOnDong();
            } else if (_idx == 2) {
                ((GameController)_controller).Desk.RenderSetNanAtBottom();
                ((GameController)_controller).Desk.RenderTakeOnNan();
            } else if (_idx == 3) {
                ((GameController)_controller).Desk.RenderSetXiAtBottom();
                ((GameController)_controller).Desk.RenderTakeOnXi();
            } else if (_idx == 4) {
                ((GameController)_controller).Desk.RenderSetBeiAtBottom();
                ((GameController)_controller).Desk.RenderTakeOnBei();
            } else {
                UnityEngine.Debug.Assert(false);
            }

            if (_sex == 1) {
                GameObject rori = ABLoader.current.LoadAsset<GameObject>("Prefabs/Hand", "boyrhand");
                _rhand = GameObject.Instantiate<GameObject>(rori);

                GameObject lori = ABLoader.current.LoadAsset<GameObject>("Prefabs/Hand", "boylhand");
                _lhand = GameObject.Instantiate<GameObject>(lori);
            } else {
                GameObject rori = ABLoader.current.LoadAsset<GameObject>("Prefabs/Hand", "girlrhand");
                _rhand = GameObject.Instantiate<GameObject>(rori);

                GameObject lori = ABLoader.current.LoadAsset<GameObject>("Prefabs/Hand", "girllhand");
                _lhand = GameObject.Instantiate<GameObject>(lori);
            }

            _rhand.transform.SetParent(_go.transform);
            _rhand.transform.localPosition = _rhandinitpos;
            _rhand.transform.localRotation = _rhandinitrot;


            _lhand.transform.SetParent(_go.transform);
            _lhand.transform.localPosition = _lhandinitpos;
            _lhand.transform.localRotation = _lhandinitrot;
        }

        protected override void RenderBoxing() {
            int count = 0;
            Desk desk = ((GameController)_controller).Desk;
            desk.RenderShowBottomSlot(() => {
            });

            for (int i = 0; i < _takecards.Count; i++) {
                int idx = i / 2;
                float x = desk.Width - (_takeleftoffset + idx * Card.Width + Card.Width / 2.0f);
                float y = Card.HeightMZ + Card.Height / 2.0f;
                float z = _takebottomoffset;
                if (i % 2 == 0) {
                    y = Card.HeightMZ + Card.Height + Card.Height / 2.0f;
                } else if (i % 2 == 1) {
                    y = Card.HeightMZ + Card.Height / 2.0f;
                }
                Card card = _takecards[i];
                card.Go.transform.localRotation = _downv;

                card.Go.transform.localPosition = new UnityEngine.Vector3(x, y - _takemove, z);
                Tween t = card.Go.transform.DOLocalMoveY(y, _takemovedelta);

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(t)
                .AppendCallback(() => {
                    count++;
                    if (count == _takecards.Count) {
                        count = 0;
                        Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_BOXINGCARDS);
                        _ctx.Enqueue(cmd);
                    }
                });
            }
        }

        protected override void RenderThrowDice() {

            // 1.0 浼告墜
            Desk desk = ((GameController)_controller).Desk;

            Animator animator = _rhand.GetComponent<Animator>();
            _rhand.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            Tween t = _rhand.transform.DOLocalMove(_rhanddiuszoffset, _diushaizishendelta);
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(t)
                .AppendCallback(() => {
                    // 2.0 涓㈠暐瀛?
                    Hand hand = _rhand.GetComponent<Hand>();
                    hand.Rigster(Hand.EVENT.DIUSHAIZI_COMPLETED, () => {
                        // 3.1
                        UnityEngine.Debug.Log("bottom diu saizi ");
                        ((GameController)_controller).RenderThrowDice(_d1, _d2);

                        // 3.2 鏀舵墜
                        Tween t32 = _rhand.transform.DOLocalMove(_rhandinitpos, _diushaizishoudelta);
                        Sequence mySequence32 = DOTween.Sequence();
                        mySequence32.Append(t32)
                        .AppendCallback(() => {
                            // 4.0
                            _rhand.transform.localRotation = _rhandinitrot;
                            animator.SetBool("Idle", true);
                        });
                    });
                    animator.SetBool("Diushaizi", true);
                });
        }

        protected override void RenderDeal() {
            // 播放声音
            //SoundMgr.current.PlaySound(_go, "Sound/common", "changescene");

            _oknum = 0;
            int count = 0;
            int i = 0;
            if (_cards.Count == 13) {
                i = 12;
                count = 1;
            } else {
                i = _cards.Count - 4;
                count = 4;
            }

            for (; i < _cards.Count; i++) {
                Vector3 dst = CalcPos(i);
                var card = _cards[i];
                card.Go.transform.localPosition = dst;
                card.Go.transform.localRotation = Quaternion.AngleAxis(-90.0f, Vector3.right);
                Tween t = card.Go.transform.DOLocalRotateQuaternion(Quaternion.AngleAxis(-25.0f, Vector3.right), _dealcarddelta);
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(t)
                    .AppendCallback(() => {

                        _oknum++;
                        if (_oknum >= count) {
                            _oknum = 0;

                            Command cmd = new Command(MyEventCmd.EVENT_TAKEDEAL);
                            _ctx.Enqueue(cmd);
                        }
                    });
            }
        }

        protected override void RenderSortCards() {
            int count = 0;
            for (int i = 0; i < _cards.Count; i++) {
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(-120.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        for (int j = 0; j < _cards.Count; j++) {
                            Vector3 dst = CalcPos(j);
                            _cards[j].Go.transform.localPosition = dst;
                            _go.GetComponent<global::BottomPlayer>().Add(_cards[j]);
                        }
                    })
                    .Append(_cards[i].Go.transform.DORotateQuaternion(Quaternion.AngleAxis(-25.0f, Vector3.right), _sortcardsdelta))
                    .AppendCallback(() => {
                        count++;
                        if (count >= _cards.Count) {
                            UnityEngine.Debug.LogFormat("bottom player send event sortcards");
                            Command cmd = new Command(MyEventCmd.EVENT_SORTCARDSAFTERDEAL);
                            _ctx.Enqueue(cmd);
                        }
                    });
            }
        }

        protected override void RenderTakeXuanPao() {
            if (_xuanpao == null) {
                ABLoader.current.LoadAssetAsync<GameObject>("Prefabs/UI", "SxXuanPao", (GameObject go) => {
                    GameObject inst = GameObject.Instantiate<GameObject>(go);
                    if (inst) {
                        _xuanpao = inst;
                        _xuanpao.transform.SetParent(_go.GetComponent<global::BottomPlayer>()._Canvas.transform);
                    }
                });
            } else {
                _xuanpao.GetComponent<XuanPao>().Show();
            }
        }

        protected override void RenderXuanPao() {
            _go.GetComponent<global::BottomPlayer>().Head.ShowMark(string.Format("{0}", _fen));
        }

        protected override void RenderTakeFirstCard() {
            UnityEngine.Debug.Assert(_takefirst);
            _com.HoldCard = _holdcard.Go;

            _holdcard.RenderQueBrightness();
            RenderTakeCard(() => {
                Command cmd = new Command(MyEventCmd.EVENT_TAKEFIRSTCARD);
                _ctx.Enqueue(cmd);
            });
        }

        protected override void RenderTakeXuanQue() {
            if (_xuanque == null) {
                ABLoader.current.LoadAssetAsync<GameObject>("Prefabs/Controls", "ScXuanQue", (GameObject go) => {
                    GameObject inst = GameObject.Instantiate<GameObject>(go);
                    if (inst) {
                        _xuanque = inst;
                        _xuanque.transform.SetParent(_go.GetComponent<global::BottomPlayer>()._Canvas.transform);
                    }
                });
            } else {
                _xuanque.GetComponent<XuanQue>().Show();
            }
        }

        protected override void RenderXuanQue() {
            if (_xuanque.activeSelf) {
                _xuanque.SetActive(false);
            }

            if (_que == Card.CardType.Bam) {
                _go.GetComponent<global::BottomPlayer>().Head.ShowMark("条");
            } else if (_que == Card.CardType.Crak) {
                _go.GetComponent<global::BottomPlayer>().Head.ShowMark("万");
            } else if (_que == Card.CardType.Dot) {
                _go.GetComponent<global::BottomPlayer>().Head.ShowMark("筒");
            }
            RenderSortCardsToDo(0.0f, () => {
                for (int i = 0; i < _cards.Count; i++) {
                    _cards[i].RenderQueBrightness();
                }
            });
        }

        protected override void RenderTakeTurn() {
            base.RenderTakeTurn();

            if (_turntype == 1) {
                _com.HoldCard = _holdcard.Go;
                _holdcard.RenderQueBrightness();

                RenderTakeCard(() => {
                    _com.SwitchOnTouch();
                });
            } else if (_turntype == 0) {
                Vector3 dst = CalcPos(_cards.Count + 1);
                _holdcard.Go.transform.localRotation = _backv;

                // 
                _com.Remove(_holdcard);
                _com.HoldCard = _holdcard.Go;

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_holdcard.Go.transform.DOLocalMove(dst, _holdafterpengdelta))
                    .AppendCallback(() => {
                        UnityEngine.Debug.Assert(_hashu == false);
                        _go.GetComponent<global::BottomPlayer>().SwitchOnTouch();
                    });
            } else if (_turntype == 2) {
                if (_xuanque) {
                    _xuanque.GetComponent<XuanQue>().Close();
                }

                UnityEngine.Debug.Assert(_hashu == false);
                _com.Head.ShowTips("你是庄家请先出牌");
                //_com.HoldCard = _holdcard.Go;
                _com.SwitchOnTouch();
            } else if (_turntype == 3) {
                UnityEngine.Debug.Assert(_hashu == false);
                //_com.HoldCard = _holdcard.Go;
                if (_hashu) {
                    Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_LEAD, _holdcard.Go);
                    _ctx.Enqueue(cmd);
                } else {
                    _com.SwitchOnTouch();
                }
            }
        }

        protected override void RenderInsert(Action cb) {
            base.RenderInsert(cb);
        }

        protected override void RenderSortCardsAfterFly(Action cb) {
            base.RenderSortCardsAfterFly(cb);
        }

        protected override void RenderFly(Action cb) {
            base.RenderFly(cb);
        }

        protected override void RenderLead() {
            base.RenderLead();

            if (_leadcard.Value == _holdcard.Value) {
                _com.HoldCard = null;
            } else {
                _com.Remove(_leadcard);
                _com.Add(_holdcard);
                _com.HoldCard = null;
            }

            _leadcard.RenderQueBrightness();

            _rhand.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            RenderLead1(RenderLead1Cb);
        }

        protected override void RenderLead1Cb() {
            base.RenderLead1Cb();
        }

        protected override void RenderCall() {
            if (Call.Gang == OpCodes.OPCODE_ANGANG ||
                Call.Gang == OpCodes.OPCODE_BUGANG ||
                (Call.Hu.Code != HuType.NONE && Call.Hu.Jiao == JiaoType.ZIMO) ||
                (Call.Hu.Code != HuType.NONE && Call.Hu.Jiao == JiaoType.DIANGANGHUA) ||
                (Call.Hu.Code != HuType.NONE && Call.Hu.Jiao == JiaoType.ZIGANGHUA)) {
                Vector3 dst = CalcPos(_cards.Count + 1);
                _holdcard.Go.transform.localPosition = dst + _holdnaoffset;
                _holdcard.Go.transform.localRotation = _backv;

                _com.HoldCard = _holdcard.Go;
                _holdcard.RenderQueBrightness();

                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(_holdcard.Go.transform.DOLocalMove(dst, _holddowndelta))
                    .AppendCallback(() => {
                        if (_hashu) {
                            if (Call.Hu.Code != HuType.NONE) {
                                Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_HU);
                                _ctx.Enqueue(cmd);
                            } else {
                                Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_GUO);
                                _ctx.Enqueue(cmd);
                            }
                        } else {
                            if (Call.Peng == OpCodes.OPCODE_PENG) {
                                _go.GetComponent<global::BottomPlayer>().ShowPeng();
                            }
                            if (Call.Gang == OpCodes.OPCODE_ANGANG || Call.Gang == OpCodes.OPCODE_BUGANG || Call.Gang == OpCodes.OPCODE_ZHIGANG) {
                                _go.GetComponent<global::BottomPlayer>().ShowGang();
                            }
                            if (Call.Hu.Code != HuType.NONE) {
                                _go.GetComponent<global::BottomPlayer>().ShowHu();
                            }
                            _go.GetComponent<global::BottomPlayer>().ShowGuo();
                        }
                    });

            } else {
                if (_hashu) {
                    if (Call.Hu.Code != HuType.NONE) {
                        Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_HU);
                        _ctx.Enqueue(cmd);
                    } else {
                        Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_GUO);
                        _ctx.Enqueue(cmd);
                    }
                } else {
                    if (Call.Peng == OpCodes.OPCODE_PENG) {
                        _go.GetComponent<global::BottomPlayer>().ShowPeng();
                    }
                    if (Call.Gang == OpCodes.OPCODE_ANGANG || Call.Gang == OpCodes.OPCODE_BUGANG || Call.Gang == OpCodes.OPCODE_ZHIGANG) {
                        _go.GetComponent<global::BottomPlayer>().ShowGang();
                    }
                    if (Call.Hu.Code != HuType.NONE) {
                        _go.GetComponent<global::BottomPlayer>().ShowHu();
                    }
                    _go.GetComponent<global::BottomPlayer>().ShowGuo();
                }
            }
        }

        protected override void RenderClearCall() {

            _com.CloseAll();
            _com.Head.CloseTips();
            _com.Head.CloseWAL();
        }

        protected override void RenderPeng() {
            base.RenderPeng();

            Desk desk = ((GameController)_controller).Desk;
            PGCards pg = _putcards[_putidx];
            UnityEngine.Debug.Assert(pg.Cards.Count == 3);
            for (int i = 0; i < pg.Cards.Count - 1; i++) {
                _com.Remove(pg.Cards[i]);
            }
            float offset = _putrightoffset;
            for (int i = 0; i < _putidx; i++) {
                UnityEngine.Debug.Assert(_putcards[i].Width > 0.0f);
                offset += _putcards[i].Width + _putmargin;
            }

            _putmove = new Vector3(0.1f, 0.0f, 0.0f);
            for (int i = 0; i < pg.Cards.Count; i++) {
                float x = 0.0f;
                float y = Card.Height / 2.0f + Card.HeightMZ;
                float z = _putbottomoffset;
                if (i == pg.Hor) {
                    x = desk.Width - (offset + Card.Length / 2.0f);
                    z = _putbottomoffset + Card.Width / 2.0f;
                    offset += Card.Length;
                    pg.Width += Card.Length;
                    pg.Cards[i].Go.transform.localRotation = _uph;
                } else {
                    x = desk.Width - (offset + Card.Width / 2.0f);
                    z = _putbottomoffset + Card.Length / 2.0f;
                    offset += Card.Width;
                    pg.Width += Card.Width;
                    pg.Cards[i].Go.transform.localRotation = _upv;
                }
                pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z) - _putmove;
            }

            RenderPeng1();
        }

        protected override void RenderGang() {
            base.RenderGang();

            Desk desk = ((GameController)_controller).Desk;
            PGCards pg = _putcards[_putidx];
            UnityEngine.Debug.Assert(pg.Cards.Count == 4);

            float offset = _putrightoffset;
            for (int i = 0; i < _putidx; i++) {
                UnityEngine.Debug.Assert(_putcards[i].Width > 0.0f);
                offset += _putcards[i].Width + _putmargin;
            }

            _putmove = new Vector3(0.1f, 0.0f, 0.0f);

            if (pg.Opcode == OpCodes.OPCODE_ZHIGANG) {
                for (int i = 0; i < _cards.Count - 1; i++) {
                    _com.Remove(_cards[i]);
                }

                for (int i = 0; i < pg.Cards.Count; i++) {
                    float x = 0.0f;
                    float y = Card.Height / 2.0f + Card.HeightMZ;
                    float z = _putbottomoffset;
                    if (i == pg.Hor) {
                        x = desk.Width - (offset + Card.Length / 2.0f);
                        z = _putbottomoffset + Card.Width / 2.0f;
                        offset += Card.Length;
                        pg.Width += Card.Length;
                        pg.Cards[i].Go.transform.localRotation = _uph;
                    } else {
                        x = desk.Width - (offset + Card.Width / 2.0f);
                        z = _putbottomoffset + Card.Length / 2.0f;
                        offset += Card.Width;
                        pg.Width += Card.Width;
                        pg.Cards[i].Go.transform.localRotation = _upv;
                    }
                    pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z) - _putmove;
                }

                RenderGang1(() => {
                    RenderSortCardsToDo(_pgsortcardsdelta, () => {
                        Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                        _ctx.Enqueue(cmd);
                    });
                });
            } else if (pg.Opcode == OpCodes.OPCODE_ANGANG) {
                for (int i = 0; i < pg.Cards.Count; i++) {
                    float x = desk.Width - (offset + Card.Width / 2.0f);
                    float y = Card.Height / 2.0f + Card.HeightMZ;
                    float z = _putbottomoffset + Card.Length / 2.0f;
                    offset += Card.Width;
                    pg.Width += Card.Width;

                    if (i == pg.Hor) {
                        pg.Cards[i].Go.transform.localRotation = _upv;
                    } else {
                        pg.Cards[i].Go.transform.localRotation = _backv;
                    }
                    pg.Cards[i].Go.transform.localPosition = new Vector3(x, y, z) - _putmove;
                }
                RenderGang1(() => {

                    Vector3 dst = _putcards[_putidx].Cards[3].Go.transform.localPosition;
                    ((GameController)_controller).Desk.RenderChangeCursor(new Vector3(dst.x, dst.y + desk.CurorMH, dst.z));

                    if (pg.Cards[3].Value == _holdcard.Value) {
                        // 娓呴櫎go
                        for (int j = 0; j < pg.Cards.Count - 1; j++) {
                            _com.Remove(pg.Cards[j]);
                        }
                        _com.HoldCard = null;

                        RenderSortCardsToDo(_pgsortcardsdelta, () => {
                            Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                            _ctx.Enqueue(cmd);
                        });
                    } else {
                        // 娓呴櫎go,鍔犲叆holdcard
                        for (int j = 0; j < pg.Cards.Count; j++) {
                            _com.Remove(pg.Cards[j]);
                        }
                        _com.Add(_holdcard);

                        if (_holdcard.Pos == (_cards.Count - 1)) {
                            RenderSortCardsToDo(_pgsortcardsdelta, () => {
                                Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                _ctx.Enqueue(cmd);
                            });
                        } else {
                            RenderFly(() => {
                                Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                _ctx.Enqueue(cmd);
                            });
                        }
                    }
                });
            } else if (pg.Opcode == OpCodes.OPCODE_BUGANG) {

                float x = desk.Width - (offset + (Card.Width * pg.Hor) + (Card.Length / 2.0f));
                float y = Card.Height / 2.0f;
                float z = _putbottomoffset + Card.Width + Card.Width / 2.0f;
                pg.Cards[3].Go.transform.localPosition = new Vector3(x, y, z) - _putmove;
                pg.Cards[3].Go.transform.localRotation = _uph;

                RenderGang1(() => {
                    Vector3 dst = _putcards[_putidx].Cards[3].Go.transform.localPosition;
                    ((GameController)_controller).Desk.RenderChangeCursor(new Vector3(dst.x, dst.y + desk.CurorMH, +dst.z));

                    if (_holdcard.Value == pg.Cards[3].Value) {
                        _com.HoldCard = null;
                        _holdcard = null;
                        Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                        _ctx.Enqueue(cmd);
                    } else {
                        _com.Remove(pg.Cards[3]);
                        _com.Add(_holdcard);

                        if (_holdcard.Pos == (_cards.Count - 1)) {
                            RenderSortCardsToDo(_pgsortcardsdelta, () => {
                                Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                _ctx.Enqueue(cmd);
                            });
                        } else {
                            RenderFly(() => {
                                Command cmd = new Command(MyEventCmd.EVENT_GANGCARD);
                                _ctx.Enqueue(cmd);
                            });
                        }
                    }
                });
            } else {
                UnityEngine.Debug.Assert(false);
            }
        }

        protected override void RenderGangSettle() {
            long chip = 0;
            long left = 0;
            if (_settle.Count > 0) {
                for (int i = 0; i < _settle.Count; i++) {
                    chip += _settle[i].Chip;
                    left = _settle[i].Left > left ? _settle[i].Left : left;
                }
                _chip = (int)left;
                _com.Head.SetGold(_chip);
                _com.Head.ShowWAL(string.Format("{0}", chip));
            }
        }

        protected override void RenderHu() {
            base.RenderHu();

            int idx = _hucards.Count - 1;
            Card card = _hucards[idx];
            Desk desk = ((GameController)_controller).Desk;

            float x = desk.Width - (_hurightoffset + (Card.Width / 2.0f) + (Card.Width * idx));
            float y = Card.Height / 2.0f;
            float z = _hubottomoffset + Card.Length / 2.0f;
            card.Go.transform.localPosition = new Vector3(x, y, z);
            card.Go.transform.localRotation = _upv;
            ((GameController)_controller).Desk.RenderChangeCursor(new Vector3(x, y + desk.CurorMH, z));

            _com.Head.SetHu(true);

            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(1.0f)
                .AppendCallback(() => {
                    Command cmd = new Command(MyEventCmd.EVENT_HUCARD);
                    _ctx.Enqueue(cmd);
                });

        }

        protected override void RenderHuSettle() {
            long chip = 0;
            long left = 0;
            if (_settle.Count > 0) {
                for (int i = 0; i < _settle.Count; i++) {
                    chip = _settle[i].Chip;
                    left = _settle[i].Left > left ? _settle[i].Left : left;
                }
                _com.Head.SetGold((int)left);
                _com.Head.ShowWAL(string.Format("{0}", chip));
            }
        }

        protected override void RenderOver() {
            RenderOverShen((Action act) => {
                _oknum = 0;
                Desk desk = ((GameController)_controller).Desk;
                for (int i = 0; i < _cards.Count; i++) {
                    float x = _leftoffset + Card.Width * i + Card.Width / 2.0f;
                    float y = Card.Height / 2.0f + Card.HeightMZ;
                    float z = _bottomoffset + Card.Length / 2.0f;

                    Tween t1 = _cards[i].Go.transform.DOLocalMove(new Vector3(x, y, z), 1.0f);
                    Tween t2 = _cards[i].Go.transform.DOLocalRotateQuaternion(_upv, 1.0f);

                    Sequence mySequence = DOTween.Sequence();
                    mySequence.Append(t2)
                    .AppendCallback(() => {
                        _oknum++;
                        if (_oknum >= _cards.Count) {
                            act();
                        }
                    });
                }
            }, () => { });
        }

        protected override void RenderSettle() {
            long chip = 0;
            long left = 0;
            if (_settle.Count > 0) {
                SettlementItem item = _settle[0];
                if (item.TuiSui == 1) {
                    _com.Head.SetGold((int)left);
                    _com.Head.ShowWAL("退税");

                    Sequence mySequence = DOTween.Sequence();
                    mySequence.AppendInterval(1.0f)
                        .AppendCallback(() => {
                            _com.Head.ShowWAL(string.Format("{0}", chip));
                        })
                    .AppendInterval(1.0f)
                    .AppendCallback(() => {
                        Command cmd = new Command(MyEventCmd.EVENT_SETTLE_NEXT);
                        _ctx.Enqueue(cmd);
                    });
                } else {
                    Sequence mySequence = DOTween.Sequence();
                    mySequence.AppendInterval(1.0f)
                        .AppendCallback(() => {
                            Command cmd = new Command(MyEventCmd.EVENT_SETTLE_NEXT);
                            _ctx.Enqueue(cmd);
                        });
                }
            }
        }

        protected override void RenderFinalSettle() {
            _com.Head.SetHu(false);
            _com.Head.CloseWAL();
            GameService service = _ctx.QueryService<GameService>(GameService.Name);
            _com.OverWnd.SettleBottom(_idx, (int)service.Max, _settle);
        }

        protected override void RenderRestart() {
            _com.Head.SetHu(false);
            _com.Head.CloseWAL();
            _com.Head.SetReady(true);
        }

        protected override void RenderTakeRestart() {
            _com.Head.SetReady(false);
        }

        protected override void RenderSay() {
            _com.Say(_say);
        }
    }
}
