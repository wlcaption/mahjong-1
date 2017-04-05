using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TopPlayerHead : MonoBehaviour {

    public Text _Gold;
    public GameObject _Leave;
    public GameObject _Mark;
    public GameObject _Say;
    public GameObject _Head;
    public GameObject _Hu;
    public GameObject _WAL;
    public GameObject _Ready;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    public void Init() {
        SetGold(0);
        _Leave.SetActive(false);
        _Mark.SetActive(false);
        _Say.SetActive(false);
        _Hu.SetActive(false);
        _WAL.SetActive(false);
        _Ready.SetActive(false);
    }

    public void Show() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }

    public void Close() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
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
                _Mark.transform.FindChild("Content").GetComponent<Text>().text = m;
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

    public void SetHu(bool value) {
        if (value) {
            if (!_Hu.activeSelf) {
                //_Hu.GetComponent<RectTransform>().localPosition.Set(-300.0f, -50.0f, 0.0f);
                _Hu.SetActive(true);
            }
        } else {
            if (_Hu.activeSelf) {
                _Hu.SetActive(false);
            }
        }
    }

    public void SetReady(bool value) {
        if (value) {
            if (!_Ready.activeSelf) {
                _Ready.SetActive(true);
            }
        } else {
            if (_Ready.activeSelf) {
                _Ready.SetActive(false);
            }
        }
    }

    public void ShowWAL(string value) {
        if (!_WAL.activeSelf) {
            _WAL.SetActive(true);
        }
        _WAL.GetComponent<Text>().text = value;
        _WAL.transform.localPosition = new Vector3(-500, -110.0f, 0.0f);
        _WAL.transform.DOLocalMoveY(-100.0f, 1.0f);
    }

    public void CloseWAL() {
        if (_WAL.activeSelf) {
            _WAL.SetActive(false);
        }
    }

    public void Clear() {
        _Leave.SetActive(false);
        _Mark.SetActive(false);
        _Say.SetActive(false);
        _Hu.SetActive(false);
        _WAL.SetActive(false);
    }
}
