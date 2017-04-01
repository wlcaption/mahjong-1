using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
    class MyEventCmd : EventCmd {

        public static uint EVENT_SETUP_STARTROOT = 51;
        public static uint EVENT_UPdATERES = 52;
        public static uint EVENT_TSETRES = 53;

        public static uint EVENT_ONBORN = 109;
        public static uint EVENT_ONJOIN = 110;
        public static uint EVENT_SETUP_UIROOT = 111;

        // 主界面
        public static uint EVENT_SETUP_MUI = 120;
        public static uint EVENT_MUI_MATCH = 121;
        public static uint EVENT_MUI_CREATE = 122;
        public static uint EVENT_MUI_JOIN = 123;
        public static uint EVENT_MUI_MSG = 124;
        public static uint EVENT_MUI_VIEWMAIL = 125;
        public static uint EVENT_MUI_VIEWEDMAIL = 126;
        public static uint EVENT_MUI_MSGCLOSED = 127;
        public static uint EVENT_MUI_SHOWCREATE = 128;


        // 游戏界面

        public static uint EVENT_SETUP_SCENE = 201;
        public static uint EVENT_SETUP_CARD = 202;
        public static uint EVENT_SETUP_GUIROOT = 203;
        public static uint EVENT_SETUP_BOARD = 204;

        public static uint EVENT_SETUP_BOTTOMPLAYER = 205;
        public static uint EVENT_SETUP_TOPPLAYER = 206;
        public static uint EVENT_SETUP_LEFTPLAYER = 207;
        public static uint EVENT_SETUP_RIGHTPLAYER = 208;

        public static uint EVENT_PENG = 211;
        public static uint EVENT_GANG = 212;
        public static uint EVENT_HU = 213;
        public static uint EVENT_GUO = 214;

        public static uint EVENT_LOADEDCARDS = 215;
        public static uint EVENT_BOXINGCARDS = 216;
        public static uint EVENT_THROWDICE = 217;
        public static uint EVENT_TAKEDEAL = 218;

        public static uint EVENT_LEAD = 219;
        public static uint EVENT_PENGCARD = 220;
        public static uint EVENT_GANGCARD = 221;
        public static uint EVENT_HUCARD = 222;

        public static uint EVENT_SORTCARDS = 223;
        public static uint EVENT_LEADCARD = 224;

        public static uint EVENT_RESTART = 225;

        public static uint EVENT_XUANPAO = 226;
        public static uint EVENT_XUANQUE = 227;

        public static uint EVENT_SENDCHATMSG = 228;
        public static uint EVENT_TAKEFIRSTCARD = 229;

        public MyEventCmd(uint cmd) : base(cmd) {
        }
    }
}
