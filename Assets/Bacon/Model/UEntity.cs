using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;

namespace Bacon.Model {
    public class UEntity : Entity {
        public UEntity(Context ctx, uint uid) : base(ctx, uid) {
            AddComponent<UComUser>();
            AddComponent<UComSysInbox>();
            AddComponent<UComRecordMgr>();
        }
    }
}
