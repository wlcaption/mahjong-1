using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maria;
using Bacon;
using Bacon.Event;

public class ChatItem : MonoBehaviour {

    public GameObject _Content;

    private string _content = string.Empty;
    private int _code = 0;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public void Init(int code, string content) {
        _code = code;
        _content = content;

        if (_Content != null) {
            _Content.GetComponent<Text>().text = _content;
        }
    }

    public void OnClick() {
        Message msg = new Message();
        msg["type"] = 1;
        msg["code"] = _code;
        Command cmd = new Command(MyEventCmd.EVENT_SENDCHATMSG, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
    }
}
