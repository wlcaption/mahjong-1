using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject _LoginPanel;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnLogin() {
        if (_LoginPanel == null) {
            return;
        }

        var ungo = _LoginPanel.transform.FindChild("username").gameObject;
        string username = ungo.GetComponentInChildren<InputField>().text;
        var pwgo = _LoginPanel.transform.FindChild("password").gameObject;
        string password = pwgo.GetComponentInChildren<InputField>().text;

        if (username != null && password != null) {

            Maria.Message msg = new Maria.Message();
            msg["username"] = username;
            msg["password"] = password;
            msg["server"] = "sample";
            Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_LOGIN, gameObject, msg);
            _Root.App.Enqueue(cmd);
        }

    }
}
