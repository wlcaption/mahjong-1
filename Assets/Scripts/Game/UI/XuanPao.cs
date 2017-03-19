using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maria;
using Bacon;

public class XuanPao : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void OnFen1() {
        Message msg = new Message();
        msg["fen"] = 1;
        Command cmd = new Command(MyEventCmd.EVENT_XUANPAO, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
        gameObject.SetActive(false);
    }

    public void OnFen2() {
        Message msg = new Message();
        msg["fen"] = 2;
        Command cmd = new Command(MyEventCmd.EVENT_XUANPAO, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
        gameObject.SetActive(false);
    }

    public void OnFen3() {
        Message msg = new Message();
        msg["fen"] = 3;
        Command cmd = new Command(MyEventCmd.EVENT_XUANPAO, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
        gameObject.SetActive(false);
    }

    public void OnFen4() {
        Message msg = new Message();
        msg["fen"] = 4;
        Command cmd = new Command(MyEventCmd.EVENT_XUANPAO, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
        gameObject.SetActive(false);
    }
}
