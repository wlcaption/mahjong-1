﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria.Lua {

    [XLua.CSharpCallLua]
    public interface ClientSock {
        void recv(string package);
    }
}
