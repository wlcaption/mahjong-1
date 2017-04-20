using System;
using System.Collections.Generic;
using System.Text;

namespace Maria
{
    public class User
    {
        public string Server { get; set; }    // 没有啥用
        public string Username { set; get; }  // 没有啥用
        public string Password { set; get; }  // 没有啥用
        public int Uid { get; set; }
        public int Subid { set; get; }
        public byte[] Secret { set; get; }
    }
}
