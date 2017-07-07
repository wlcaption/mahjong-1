using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maria {
    public class Entity {

        private uint _uid = 0;
        private uint _subid = 0;
        private Dictionary<string, Component> _components = new Dictionary<string, Component>();
        private Context _ctx;

        public Entity(Context ctx, uint uid) {
            _ctx = ctx;
            _uid = uid;
        }

        public T GetComponent<T>() where T : Component {
            Type t = typeof(T);
            return _components[t.FullName] as T;
        }

        public void AddComponent<T>() where T : Component {
            Component o = Activator.CreateInstance(typeof(T), this) as T;
            string name = o.GetType().FullName;
            _components[name] = o;
        }

        public void RemoveComponent<T>() where T : Component {
            Type t = typeof(T);
            string name = t.FullName;
            _components.Remove(name);
        }



        public Context Ctx { get { return _ctx; } }
        public uint Uid { get { return _uid; } set { _uid = value; } }
        public uint Subid { get { return _uid; } set { _subid = value; } }

    }
}
