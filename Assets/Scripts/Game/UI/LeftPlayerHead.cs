using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LeftPlayerHead : PlayerHeadBase {

    // Use this for initialization
    void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        base.Update();
    }

    public override void ShowWAL(string value) {
        if (!_WAL.activeSelf) {
            _WAL.SetActive(true);
        }
        _WAL.GetComponent<Text>().text = value;
        _WAL.transform.localPosition = new Vector3(360.0f, 0.0f, 0.0f);
        _WAL.transform.DOLocalMoveY(10.0f, 1.0f);
    }

}
