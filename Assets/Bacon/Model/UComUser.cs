﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;
using Sproto;

namespace Bacon.Model {
    class UComUser : Component {
        public UComUser(Entity entity) : base(entity) {
        }

        public string Name { get; set; }
        public long NameId { get; set; }
        public long RCard { get; set; }
        public long Sex { get; set; }

        public void Fetch() {
            AppContext ctx = _entity.Ctx as AppContext;
            ctx.SendReq<C2sProtocol.first>(C2sProtocol.first.Tag, null);
        }

        public void First(SprotoTypeBase responseObj) {
            C2sSprotoType.first.response obj = responseObj as C2sSprotoType.first.response;

            Name = obj.name;
            NameId = obj.nameid;
            RCard = obj.rcard;
            Sex = obj.sex;

            UnityEngine.Debug.LogFormat("name = {0}, nameid = {1}, rcard = {2}, sex = {3}", Name, NameId, RCard, Sex);
            var controller = _entity.Ctx.Peek<MainController>();
            if (controller != null) {
                _entity.Ctx.EnqueueRenderQueue(controller.RenderFirst);
            }
        }

       
    }
}
