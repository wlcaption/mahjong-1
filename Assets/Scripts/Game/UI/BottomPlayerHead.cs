using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BottomPlayerHead : PlayerHeadBase {

    public GameObject _Tips;

    // Use this for initialization
    void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        base.Update();
    }

    public override void Init() {
        base.Init();
        _Tips.SetActive(false);
    }

    public override void ShowWAL(string value) {
        if (!_WAL.activeSelf) {
            _WAL.SetActive(true);
        }
        _WAL.GetComponent<Text>().text = value;
        _WAL.GetComponent<RectTransform>().localPosition = new Vector3(600, 110, 0);
        _WAL.transform.DOLocalMoveY(120.0f, 1.0f);
    }

    public void ShowTips(string content) {
        if (_Tips != null) {
            if (!_Tips.activeSelf) {
                _Tips.SetActive(true);
            }
            _Tips.GetComponent<Text>().text = content;
        }
    }

    public void CloseTips() {
        if (_Tips != null) {
            if (_Tips.activeSelf) {
                _Tips.SetActive(false);
            }
        }
    }

    public override void Clear() {
        base.Clear();
        _Tips.SetActive(false);
    }
}
