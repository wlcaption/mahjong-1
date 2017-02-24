using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Maria {
    public class Debug {
        private Context _ctx = null;
        private Queue<object> _queue = new Queue<object>();

        public Debug(Context ctx) {
            _ctx = ctx;
        }

        public void Log(object message) {
            lock (_queue) {
                _queue.Enqueue(message);
            }
            _ctx.EnqueueRenderQueue(RenderLog);
        }

        protected void RenderLog() {
            lock (_queue) {
                while (_queue.Count > 0) {
                    UnityEngine.Debug.Log(_queue.Dequeue());
                }
            }
        }

        public void Assert(bool condition) {
            if (condition) {
            } else {
                throw new Exception("assert failtur.");
            }
        }
    }
}
