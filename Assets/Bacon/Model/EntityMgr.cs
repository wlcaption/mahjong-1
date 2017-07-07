using Maria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bacon.Model {
    public class EntityMgr {
        private Dictionary<uint, Entity> _entities = new Dictionary<uint, Entity>();
        private Entity _myentity = null;

        public EntityMgr() {
        }

        public Entity GetEntity(uint uid) {
            return _entities[uid];
        }

        public void AddEntity(Entity e) {
            _entities.Add(e.Uid, e);
        }

        public Entity MyEntity { get { return _myentity; } set { _myentity = value; } }
    }
}
