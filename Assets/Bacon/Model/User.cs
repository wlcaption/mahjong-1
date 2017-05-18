using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;

namespace Bacon {
    class User : Model<User> {
        public string Name { get; set; }
        public long NameId { get; set; }
        public long RCard { get; set; }
        public long Sex { get; set; }
    }
}
