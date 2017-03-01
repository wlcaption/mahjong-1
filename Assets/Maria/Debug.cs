using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Maria {
    public class Debug {

        private enum Type {
            Log,
            Log1,
            LogError,
            LogError1,
            LogException,
            LogException1,
        }

        private struct Info {
            public Type type;
            public object message;
            public UnityEngine.Object context;
        }

        private Context _ctx = null;
        private Queue<Info> _queue = new Queue<Info>();

        public Debug(Context ctx) {
            _ctx = ctx;
        }

        public void Log(object message) {
            Info i = new Info();
            i.type = Type.Log;
            i.message = message;
            i.context = null;
            lock (_queue) {
                _queue.Enqueue(i);
            }
            _ctx.EnqueueRenderQueue(RenderLog);
        }

        public void Log(object message, UnityEngine.Object context) {
            Info i = new Info();
            i.type = Type.Log;
            i.message = message;
            i.context = context;
            lock (_queue) {
                _queue.Enqueue(i);
            }
            _ctx.EnqueueRenderQueue(RenderLog);
        }

        public void Assert(bool condition) {
            if (condition) {
            } else {
                throw new Exception("assert failtur.");
            }
        }

        public void LogError(object message) {
            Info i = new Info();
            i.type = Type.Log;
            i.message = message;
            i.context = null;
            lock (_queue) {
                _queue.Enqueue(i);
            }
            _ctx.EnqueueRenderQueue(RenderLog);
        }

        public void LogError(object message, UnityEngine.Object context) {
            Info i = new Info();
            i.type = Type.Log;
            i.message = message;
            i.context = context;
            lock (_queue) {
                _queue.Enqueue(i);
            }
            _ctx.EnqueueRenderQueue(RenderLog);
        }

        protected void RenderLog() {
            lock (_queue) {
                while (_queue.Count > 0) {
                    Info i = _queue.Dequeue();
                    switch (i.type) {
                        case Type.Log:
                            UnityEngine.Debug.Log(i.message);
                            break;
                        case Type.Log1:
                            UnityEngine.Debug.Log(i.message, i.context);
                            break;
                        case Type.LogError:
                            UnityEngine.Debug.LogError(i.message);
                            break;
                        case Type.LogError1:
                            UnityEngine.Debug.LogError(i.message, i.context);
                            break;
                        case Type.LogException:
                            break;
                        case Type.LogException1:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
