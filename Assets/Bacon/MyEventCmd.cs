using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
    class MyEventCmd : EventCmd {

        public static uint EVENT_SETUP_STARTROOT = 108;

        public static uint EVENT_ONBORN = 109;
        public static uint EVENT_ONJOIN = 110;
        public static uint EVENT_SETUP_UIROOT = 111;

        public static uint EVENT_MUI_MATCH = 112;
        public static uint EVENT_MUI_CREATE = 113;
        public static uint EVENT_MUI_JOIN = 114;

        public static uint EVENT_PENG = 200;
        public static uint EVENT_GANG = 201;
        public static uint EVENT_HU = 202;
        public static uint EVENT_GUO = 203;

        public static uint EVENT_SETUP_SCENE = 204;
        public static uint EVENT_SETUP_CARD = 205;
        public static uint EVENT_SETUP_GUIROOT = 206;

        public static uint EVENT_SETUP_BOTTOMPLAYER = 207;
        public static uint EVENT_SETUP_TOPPLAYER = 208;
        public static uint EVENT_SETUP_LEFTPLAYER = 209;
        public static uint EVENT_SETUP_RIGHTPLAYER = 210;

        public static uint EVENT_LOADEDCARDS = 211;
        public static uint EVENT_BOXINGCARDS = 212;
        public static uint EVENT_THROWDICE = 213;
        public static uint EVENT_TAKEDEAL = 214;

        public static uint EVENT_LEAD = 215;
        public static uint EVENT_PENGCARD = 216;
        public static uint EVENT_GANGCARD = 217;
        public static uint EVENT_HUCARD = 218;

        public static uint EVENT_SORTCARDS = 219;
        public static uint EVENT_LEADCARD = 220;

        public static uint EVENT_SETUP_BOARD = 204;

        public MyEventCmd(uint cmd) : base(cmd) {
        }
    }
}
