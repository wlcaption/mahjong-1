using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHeadBase : MonoBehaviour {

    public Text _Gold;
    public GameObject _Leave;
    public GameObject _Mark;
    public GameObject _Say;
    public GameObject _Head;
    public GameObject _Hu;
    public GameObject _Peng;
    public GameObject _WAL;
    public GameObject _Ready;
    public GameObject _Flame;

    // Use this for initialization
    protected void Start() {
    }

    // Update is called once per frame
    protected void Update() {

    }

    public virtual void Init() {
        SetGold(0);
        _Leave.SetActive(false);
        _Mark.SetActive(false);
        _Say.SetActive(false);
        _Hu.SetActive(false);
        _Peng.SetActive(false);
        _WAL.SetActive(false);
        _Ready.SetActive(false);
    }

    public virtual void Show() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }

    public virtual void Close() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }

    public virtual void SetGold(int num) {
        string txt = string.Format("{0}", num);
        UnityEngine.Debug.Log(txt);
        _Gold.text = txt;
    }

    public virtual void SetLeave(bool value) {
        if (value) {
            _Leave.SetActive(true);
        } else {
            _Leave.SetActive(false);
        }
    }

    public virtual void ShowMark(string m) {
        if (!_Mark.activeSelf) {
            _Mark.SetActive(true);
        }
        _Mark.transform.FindChild("Content").GetComponent<Text>().text = m;
    }

    public virtual void CloseMark() {
        if (_Mark.activeSelf) {
            _Mark.SetActive(false);
        }
    }

    public virtual void ShowSay(string value) {
        if (!_Say.activeSelf) {
            _Say.SetActive(true);
        }
        _Say.GetComponent<Text>().text = value;
    }

    public virtual void CloseSay() {
        if (_Say.activeSelf) {
            _Say.SetActive(false);
        }
    }

    public virtual void SetHu(bool value) {
        if (value) {
            if (!_Hu.activeSelf) {
                _Hu.SetActive(true);
                var hu = _Hu.GetComponent<Hu>();
                hu.Play(() => { });
            }
        } else {
            if (_Hu.activeSelf) {
                _Hu.SetActive(false);
            }
        }
    }

    public virtual void SetPeng(bool value) {
        if (value) {
            if (!_Peng.activeSelf) {
                _Peng.SetActive(true);
                var peng = _Peng.GetComponent<Peng>();
                peng.Play(() => { });
            }
        } else {
            if (_Peng.activeSelf) {
                _Peng.SetActive(false);
            }
        }
    }

    public virtual void ShowWAL(string value) {
        if (!_WAL.activeSelf) {
            _WAL.SetActive(true);
        }
    }

    public virtual void CloseWAL() {
        if (_WAL.activeSelf) {
            _WAL.SetActive(false);
        }
    }

    public virtual void SetReady(bool value) {
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

    public virtual void ShowFlameCountdown(float cd) {
        _Flame.GetComponent<FlameThrower>().Play(cd);
    }

    public virtual void Clear() {
        _Leave.SetActive(false);
        _Mark.SetActive(false);
        _Say.SetActive(false);
        _Hu.SetActive(false);
        _Peng.SetActive(false);
        _WAL.SetActive(false);
        _Ready.SetActive(false);
    }
}
