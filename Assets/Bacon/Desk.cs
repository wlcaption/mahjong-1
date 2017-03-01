using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;

namespace Bacon {
    public class Desk : Actor {

        private float _width = 2.0f;
        private float _length = 2.0f;
        private float _height = 2.0f;

        public Desk(Context ctx, Controller controller)
            : base(ctx, controller, null) {
        }

        public float Width { get { return _width; } }
        public float Length { get { return _length; } }
        public float Height { get { return _height; } }

    }
}
