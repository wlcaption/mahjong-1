using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maria;
using Bacon;
using Bacon.Event;

public class MsgItemInfo : MonoBehaviour {

    public Text _Title;
    public Text _Content;

    private MsgItem.Type _type;
    private long _id;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    public void Show(MsgItem.Type type, long id, string title, string content) {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
        _type = type;
        _id = id;
        _Title.text = title;
        _Content.text = content;
    }

    public void OnClose() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
        Message msg = new Message();
        msg.AddField<MsgItem.Type>("type", _type);
        msg.AddField<long>("id", _id);
        Command cmd = new Command(MyEventCmd.EVENT_MUI_VIEWEDMAIL, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
    }
}
