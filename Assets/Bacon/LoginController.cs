﻿using Bacon;
using Maria;
using UnityEngine;

namespace Bacon {
    class LoginController : Controller {
        private string _server;
        private string _username;
        private string _password;

        private LoginActor _loginActor;

        public LoginController(Context ctx) : base(ctx) {
            _name = "login";
            _loginActor = new LoginActor(_ctx, this);
        }

        public override void Enter() {
            base.Enter();

            InitService service = _ctx.QueryService<InitService>(InitService.Name);
            if (service != null) {
                SMActor actor = service.SMActor;
                actor.LoadScene("login");
            }
        }

        public override void Exit() {
            base.Exit();
        }

        public void LoginAuth(string server, string username, string password) {
            if (((AppConfig)_ctx.Config).VTYPE == AppConfig.VERSION_TYPE.TEST) {
                _ctx.Push(typeof(MainController));
            } else {
                _server = server;
                _username = username;
                _password = password;
                _ctx.LoginAuth(server, username, password);
            }
        }

        public override void OnGateAuthed(int code) {
            base.OnGateAuthed(code);
            if (code == 200) {
                _ctx.Push(typeof(MainController));
            } else {
                _loginActor.EnableCommitOk();
            }
        }

        public override void OnGateDisconnected() {
            base.OnGateDisconnected();
        }
    }
}
