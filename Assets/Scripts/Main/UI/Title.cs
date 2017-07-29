using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour {

    public GameObject _UIRoot;
    public GameObject _Name;
    public GameObject _NameId;
    public GameObject _RCard;
    public GameObject _MsgRed;
    public GameObject _RecordRed;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    public void SetName(string name) {
        _Name.GetComponent<Text>().text = name;
    }

    public void SetNameId(string nameid) {
        _NameId.GetComponent<Text>().text = nameid;
    }

    public void SetRCard(string rcard) {
        _RCard.GetComponent<Text>().text = rcard;
    }

    public void SetMsgRed(int num) {
        if (num > 0) {
            if (!_MsgRed.activeSelf) {
                _MsgRed.SetActive(true);
            }
            string text = string.Format("{0}", num);
            _MsgRed.transform.Find("Text").GetComponent<Text>().text = text;
        } else {
            if (_MsgRed.activeSelf) {
                _MsgRed.SetActive(false);
            }
        }
    }

    public void ShowRecordRed(int num) {
        if (!_RecordRed.activeSelf) {
            _RecordRed.SetActive(true);
        }
        string text = string.Format("{0}", num);
        _RecordRed.transform.Find("Text").GetComponent<Text>().text = text;
    }

    public void CloseRecordRed() {
        if (_RecordRed.activeSelf) {
            _RecordRed.SetActive(false);
        }
    }

    public void OnShare() {
        _UIRoot.GetComponent<MUIRoot>().OnShare();
    }

    public void OnRecored() {
        _UIRoot.GetComponent<MUIRoot>().OnRecored();
    }

    public void OnMsg() {
        _UIRoot.GetComponent<MUIRoot>().OnMsg();
    }

    public void OnRule() {
        _UIRoot.GetComponent<MUIRoot>().OnRule();
    }

    public void OnSetting() {
        _UIRoot.GetComponent<MUIRoot>().OnSetting();
    }

    public void OnAdd() {
        _UIRoot.GetComponent<MUIRoot>().OnAdd();
    }
}
