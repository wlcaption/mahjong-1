using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maria;
using Bacon;
using Bacon.Event;

public class MsgItem : MonoBehaviour {

    public enum Type {
        Sys,
        Ver,
    }

    public Image _Head;
    public Text _Title;
    public Text _Content;
    public Text _Date;

    private long _id;
    private Type _type;

	// Use this for initialization
	void Start () {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1100.0f);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80.0f);
        Vector3 pos = transform.localPosition;
        transform.localPosition = new Vector3(pos.x, pos.y, 0);
        transform.localScale = Vector3.one;
        gameObject.layer = transform.parent.gameObject.layer;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void SetType(Type t) {
        _type = t;
    }

    public void SetId(long id) {
        _id = id;  
    }

    public void SetTitle(string title) {
        _Title.text = title;
    }

    public void SetContent(string content) {
        _Content.text = content;
    }

    public void SetDateTime(long dt) { }

    public void OnView() {
        Message msg = new Message();
        msg["id"] = _id;
        msg["type"] = _type;
        Command cmd = new Command(MyEventCmd.EVENT_MUI_VIEWMAIL, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
    }
}
