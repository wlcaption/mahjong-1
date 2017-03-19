// Generated by sprotodump. DO NOT EDIT!
using System;
using Sproto;
using System.Collections.Generic;

public class S2cProtocol : ProtocolBase {
	public static  S2cProtocol Instance = new S2cProtocol();
	private S2cProtocol() {
		Protocol.SetProtocol<call> (call.Tag);
		Protocol.SetRequest<S2cSprotoType.call.request> (call.Tag);
		Protocol.SetResponse<S2cSprotoType.call.response> (call.Tag);

		Protocol.SetProtocol<deal> (deal.Tag);
		Protocol.SetRequest<S2cSprotoType.deal.request> (deal.Tag);
		Protocol.SetResponse<S2cSprotoType.deal.response> (deal.Tag);

		Protocol.SetProtocol<dice> (dice.Tag);
		Protocol.SetRequest<S2cSprotoType.dice.request> (dice.Tag);
		Protocol.SetResponse<S2cSprotoType.dice.response> (dice.Tag);

		Protocol.SetProtocol<gang> (gang.Tag);
		Protocol.SetRequest<S2cSprotoType.gang.request> (gang.Tag);
		Protocol.SetResponse<S2cSprotoType.gang.response> (gang.Tag);

		Protocol.SetProtocol<handshake> (handshake.Tag);
		Protocol.SetResponse<S2cSprotoType.handshake.response> (handshake.Tag);

		Protocol.SetProtocol<hu> (hu.Tag);
		Protocol.SetRequest<S2cSprotoType.hu.request> (hu.Tag);
		Protocol.SetResponse<S2cSprotoType.hu.response> (hu.Tag);

		Protocol.SetProtocol<join> (join.Tag);
		Protocol.SetRequest<S2cSprotoType.join.request> (join.Tag);
		Protocol.SetResponse<S2cSprotoType.join.response> (join.Tag);

		Protocol.SetProtocol<lead> (lead.Tag);
		Protocol.SetRequest<S2cSprotoType.lead.request> (lead.Tag);
		Protocol.SetResponse<S2cSprotoType.lead.response> (lead.Tag);

		Protocol.SetProtocol<leave> (leave.Tag);
		Protocol.SetRequest<S2cSprotoType.leave.request> (leave.Tag);
		Protocol.SetResponse<S2cSprotoType.leave.response> (leave.Tag);

		Protocol.SetProtocol<match> (match.Tag);
		Protocol.SetRequest<S2cSprotoType.match.request> (match.Tag);
		Protocol.SetResponse<S2cSprotoType.match.response> (match.Tag);

		Protocol.SetProtocol<over> (over.Tag);
		Protocol.SetRequest<S2cSprotoType.over.request> (over.Tag);
		Protocol.SetResponse<S2cSprotoType.over.response> (over.Tag);

		Protocol.SetProtocol<peng> (peng.Tag);
		Protocol.SetRequest<S2cSprotoType.peng.request> (peng.Tag);
		Protocol.SetResponse<S2cSprotoType.peng.response> (peng.Tag);

		Protocol.SetProtocol<radio> (radio.Tag);
		Protocol.SetRequest<S2cSprotoType.radio.request> (radio.Tag);
		Protocol.SetResponse<S2cSprotoType.radio.response> (radio.Tag);

		Protocol.SetProtocol<rcard> (rcard.Tag);
		Protocol.SetRequest<S2cSprotoType.rcard.request> (rcard.Tag);
		Protocol.SetResponse<S2cSprotoType.rcard.response> (rcard.Tag);

		Protocol.SetProtocol<rchat> (rchat.Tag);
		Protocol.SetRequest<S2cSprotoType.rchat.request> (rchat.Tag);
		Protocol.SetResponse<S2cSprotoType.rchat.response> (rchat.Tag);

		Protocol.SetProtocol<ready> (ready.Tag);
		Protocol.SetResponse<S2cSprotoType.ready.response> (ready.Tag);

		Protocol.SetProtocol<restart> (restart.Tag);
		Protocol.SetRequest<S2cSprotoType.restart.request> (restart.Tag);
		Protocol.SetResponse<S2cSprotoType.restart.response> (restart.Tag);

		Protocol.SetProtocol<shuffle> (shuffle.Tag);
		Protocol.SetRequest<S2cSprotoType.shuffle.request> (shuffle.Tag);
		Protocol.SetResponse<S2cSprotoType.shuffle.response> (shuffle.Tag);

		Protocol.SetProtocol<take_restart> (take_restart.Tag);
		Protocol.SetResponse<S2cSprotoType.take_restart.response> (take_restart.Tag);

		Protocol.SetProtocol<take_turn> (take_turn.Tag);
		Protocol.SetRequest<S2cSprotoType.take_turn.request> (take_turn.Tag);
		Protocol.SetResponse<S2cSprotoType.take_turn.response> (take_turn.Tag);

		Protocol.SetProtocol<take_xuanpao> (take_xuanpao.Tag);
		Protocol.SetResponse<S2cSprotoType.take_xuanpao.response> (take_xuanpao.Tag);

		Protocol.SetProtocol<take_xuanque> (take_xuanque.Tag);
		Protocol.SetRequest<S2cSprotoType.take_xuanque.request> (take_xuanque.Tag);
		Protocol.SetResponse<S2cSprotoType.take_xuanque.response> (take_xuanque.Tag);

		Protocol.SetProtocol<xuanpao> (xuanpao.Tag);
		Protocol.SetRequest<S2cSprotoType.xuanpao.request> (xuanpao.Tag);
		Protocol.SetResponse<S2cSprotoType.xuanpao.response> (xuanpao.Tag);

		Protocol.SetProtocol<xuanque> (xuanque.Tag);
		Protocol.SetRequest<S2cSprotoType.xuanque.request> (xuanque.Tag);
		Protocol.SetResponse<S2cSprotoType.xuanque.response> (xuanque.Tag);

	}

	public class call {
		public const int Tag = 9;
	}

	public class deal {
		public const int Tag = 13;
	}

	public class dice {
		public const int Tag = 11;
	}

	public class gang {
		public const int Tag = 7;
	}

	public class handshake {
		public const int Tag = 1;
	}

	public class hu {
		public const int Tag = 8;
	}

	public class join {
		public const int Tag = 3;
	}

	public class lead {
		public const int Tag = 12;
	}

	public class leave {
		public const int Tag = 4;
	}

	public class match {
		public const int Tag = 2;
	}

	public class over {
		public const int Tag = 15;
	}

	public class peng {
		public const int Tag = 6;
	}

	public class radio {
		public const int Tag = 31;
	}

	public class rcard {
		public const int Tag = 30;
	}

	public class rchat {
		public const int Tag = 18;
	}

	public class ready {
		public const int Tag = 14;
	}

	public class restart {
		public const int Tag = 16;
	}

	public class shuffle {
		public const int Tag = 10;
	}

	public class take_restart {
		public const int Tag = 17;
	}

	public class take_turn {
		public const int Tag = 5;
	}

	public class take_xuanpao {
		public const int Tag = 19;
	}

	public class take_xuanque {
		public const int Tag = 21;
	}

	public class xuanpao {
		public const int Tag = 20;
	}

	public class xuanque {
		public const int Tag = 22;
	}

}