using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maria;
using Bacon;

public class XuanQue : MonoBehaviour {

    // Use this for initialization
    void Start() {
        transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {

    }

    public void Show() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
            transform.localPosition = Vector3.zero;
        }
    }

    public void Close() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }

    public void OnCrak() {
        Message msg = new Message();
        msg["cardtype"] = Card.CardType.Crak;
        Command cmd = new Command(MyEventCmd.EVENT_XUANQUE, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
        Close();
    }

    public void OnBam() {
        Message msg = new Message();
        msg["cardtype"] = Card.CardType.Bam;
        Command cmd = new Command(MyEventCmd.EVENT_XUANQUE, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
        Close();
    }

    public void OnDot() {
        Message msg = new Message();
        msg["cardtype"] = Card.CardType.Dot;
        Command cmd = new Command(MyEventCmd.EVENT_XUANQUE, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
        Close();
    }
}
