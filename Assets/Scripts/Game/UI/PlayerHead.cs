using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHead : MonoBehaviour {

    public Text _Gold;
    public GameObject _Leave;
    public GameObject _Mark;
    public GameObject _Say;
    public GameObject _Head;

    // Use this for initialization
    void Start() {
        SetGold(0);
        _Leave.SetActive(false);
        _Mark.SetActive(false);
        _Say.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
    }

    public void SetGold(int num) {
        string txt = string.Format("{0}", num);
        _Gold.text = txt;
    }

    public void SetLeave(bool value) {
        if (value) {
            _Leave.SetActive(true);
        } else {
            _Leave.SetActive(false);
        }
    }

    public void SetMark(string m) {
        if (m == string.Empty) {
            _Mark.SetActive(false);
        } else {
            if (!_Mark.activeSelf) {
                _Mark.SetActive(true);
                _Mark.GetComponent<Text>().text = m;
            }
        }
    }

    public void SetSay(string value) {
        if (value == string.Empty) {
            _Say.SetActive(false);
        } else {
            if (!_Say.activeSelf) {
                _Say.SetActive(true);
                _Say.GetComponent<Text>().text = value;
            }
        }
    }


}
