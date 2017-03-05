using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria {
    public class Timer {

        public static string CLOCK = "CLOCK";

        public delegate void CountdownCb();
        public delegate void CountdownDeltaCb(int past, int left);


        public string Name { get; set; }
        public bool Enable { get; set; }
        public int ST { get; set; }
        public int PT { get; set; }
        public int CD { get; set; }
        public CountdownCb CB { get; set; }
        public CountdownDeltaCb DCB { get; set; }

    }
}
