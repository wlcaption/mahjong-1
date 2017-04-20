using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon {
    public class Sysmail : IComparable<Sysmail> {

        public long Id { get; set; }
        public long DateTime { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int CompareTo(Sysmail other) {
            return (int)(this.Id - other.Id);
        }


    }
}
