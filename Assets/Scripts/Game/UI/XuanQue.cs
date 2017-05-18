using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maria;
using Bacon;
using Bacon.Game;
using Bacon.Event;

public class XuanQue : MonoBehaviour {

    public GameObject _Crak;
    public GameObject _Bam;
    public GameObject _Dot;

    // Use this for initialization
    void Start() {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update() {

    }

    public void Show() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
        transform.localPosition = Vector3.zero;
        if (!_Crak.activeSelf) {
            _Crak.SetActive(true);
        }
        if (!_Bam.activeSelf) {
            _Bam.SetActive(true);
        }
        if (!_Dot.activeSelf) {
            _Dot.SetActive(true);
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
        if (_Bam.activeSelf) {
            _Bam.SetActive(false);
        }
        if (_Dot.activeSelf) {
            _Dot.SetActive(false);
        }
    }

    public void OnBam() {
        Message msg = new Message();
        msg["cardtype"] = Card.CardType.Bam;
        Command cmd = new Command(MyEventCmd.EVENT_XUANQUE, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
        if (_Crak.activeSelf) {
            _Crak.SetActive(false);
        }
        if (_Dot.activeSelf) {
            _Dot.SetActive(false);
        }
    }

    public void OnDot() {
        Message msg = new Message();
        msg["cardtype"] = Card.CardType.Dot;
        Command cmd = new Command(MyEventCmd.EVENT_XUANQUE, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
        if (_Crak.activeSelf) {
            _Crak.SetActive(false);
        }
        if (_Bam.activeSelf) {
            _Bam.SetActive(false);
        }
    }
}
