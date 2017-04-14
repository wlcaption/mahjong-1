using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maria;
using Bacon;
using DG.Tweening;
using System;

public class Board : MonoBehaviour {

    public GameObject _Cover;
    public GameObject _Clock1;
    public GameObject _Clock2;
    public GameObject _Cursor;
    public GameObject _Dice1;
    public GameObject _Dice2;
    public GameObject _Dong;
    public GameObject _Nan;
    public GameObject _Xi;
    public GameObject _Bei;
    public GameObject _BottomGai;
    public GameObject _RightGai;
    public GameObject _TopGai;
    public GameObject _LeftGai;

    private int _oknum = 0;
    private bool _blink = false;
    private float _brightness = 1.0f;
    private int _gray = 0;
    private float _blinkinterval = 1.0f;

    // Use this for initialization
    void Start() {
        Command cmd = new Command(MyEventCmd.EVENT_SETUP_BOARD, gameObject);
        GetComponent<FindApp>().App.Enqueue(cmd);

        //Sequence mySequence = DOTween.Sequence();
        //mySequence.AppendCallback(() => {
        //    TakeTurnDong();
        //}).AppendInterval(20.0f).AppendCallback(() => {
        //    TakeTurnNan();
        //}).AppendInterval(20.0f).AppendCallback(() => {
        //    TakeTurnXi();
        //}).AppendInterval(20.0f).AppendCallback(() => {
        //    TakeTurnBei();
        //});

    }

    // Update is called once per frame
    void Update() {

    }

    public void ShowCountdown(int pt) {
        int c1 = pt / 10;
        int c2 = pt % 10;
        if (_Clock1 != null) {
            if (!_Clock1.activeSelf) {
                _Clock1.SetActive(true);
            }
            _Clock1.GetComponent<TextMesh>().text = string.Format("{0}", c1);
        }
        if (_Clock2 != null) {
            if (!_Clock2.activeSelf) {
                _Clock2.SetActive(true);
            }
            _Clock2.GetComponent<TextMesh>().text = string.Format("{0}", c2);
        }
    }

    public void ChangeCursor(Vector3 pos) {
        if (_Cursor != null) {
            if (!_Cursor.activeSelf) {
                _Cursor.SetActive(true);
            }
            _Cursor.transform.localPosition = pos;
        }
    }

    public void ThrowDice(long d1, long d2) {
        _oknum = 0;
        ThrowSDice(d1, _Dice1);
        ThrowSDice(d2, _Dice2);
    }

    private void ThrowSDice(long d, GameObject go) {
        Action act = delegate () {
            Command cmd = new Command(MyEventCmd.EVENT_THROWDICE);
            GetComponent<FindApp>().App.Enqueue(cmd);
            _Cover.SetActive(true);
            _Clock1.SetActive(true);
            _Clock2.SetActive(true);
        };
        switch (d) {
            case 1:
                go.GetComponent<Dice>().Play(() => {
                    go.transform.localRotation = Quaternion.AngleAxis(-180.0f, Vector3.forward);
                    _oknum++;
                    if (_oknum >= 2) {
                        act();
                    }
                });
                break;
            case 2:
                go.GetComponent<Dice>().Play(() => {
                    go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.right) * Quaternion.AngleAxis(270.0f, Vector3.forward);
                    _oknum++;
                    if (_oknum >= 2) {
                        act();
                    }
                });
                break;
            case 3:
                go.GetComponent<Dice>().Play(() => {
                    go.transform.localRotation = Quaternion.AngleAxis(-90.0f, Vector3.right) * Quaternion.AngleAxis(270.0f, Vector3.forward);
                    _oknum++;
                    if (_oknum >= 2) {
                        act();
                    }
                });
                break;
            case 4:
                go.GetComponent<Dice>().Play(() => {
                    go.transform.localRotation = Quaternion.AngleAxis(270.0f, Vector3.forward);
                    _oknum++;
                    if (_oknum >= 2) {
                        act();
                    }
                });
                break;
            case 5:
                go.GetComponent<Dice>().Play(() => {
                    go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.right) * Quaternion.AngleAxis(270.0f, Vector3.forward);
                    _oknum++;
                    if (_oknum >= 2) {
                        act();
                    }
                });
                break;
            case 6:
                go.GetComponent<Dice>().Play(() => {
                    go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.right);
                    _oknum++;
                    if (_oknum >= 2) {
                        act();
                    }
                });
                break;
            default:
                UnityEngine.Debug.Assert(false);
                break;
        }
    }

    public void ShowBottomSlot(Action cb) {
        _BottomGai.transform.localPosition = new Vector3(1.0f, -0.15f, 1.0f);
        _BottomGai.transform.DOLocalMoveY(0.0f, 1.0f);
    }

    public void CloseBottomSlot(Action cd) {
        cd();
    }

    public void ShowRightSlot(Action cd) {
        _RightGai.transform.localPosition = new Vector3(1.0f, -0.15f, 1.0f);
        _RightGai.transform.DOLocalMoveY(0.0f, 1.0f);
    }

    public void CloseRightSlot(Action cb) {
        cb();
    }

    public void ShowTopSlot(Action cd) {
        _TopGai.transform.localPosition = new Vector3(1.0f, -0.15f, 1.0f);
        _TopGai.transform.DOLocalMoveY(0.0f, 1.0f);
    }

    public void CloseTopSlot(Action cb) {
        cb();
    }

    public void ShowLeftSlot(Action cb) {
        _LeftGai.transform.localPosition = new Vector3(1.0f, -0.15f, 1.0f);
        _LeftGai.transform.DOLocalMoveY(0.0f, 1.0f);
    }
    public void CloseLeftSlot(Action cb) {
        cb();
    }

    public void SetDongAtRight() {
        //_Dong.transform.localPosition = new Vector3(1, -0.06f, 1);
        _Dong.transform.localRotation = Quaternion.Euler(-90.0f, 90.0f, 90);
    }

    public void SetDongAtTop() {
        _Dong.transform.localPosition = new Vector3(1, 0.0f, 1);
        _Dong.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 90.0f);
    }

    public void SetDongAtLeft() {
        _Dong.transform.localPosition = new Vector3(1.01f, 0.0f, 1.0f);
        _Dong.transform.localRotation = Quaternion.Euler(-90.0f, -90.0f, 90.0f);
    }

    public void SetDongAtBottom() {
        //_Dong.transform.localPosition = new Vector3(1.0f, -0.05f, 1.010f);
        _Dong.transform.localRotation = Quaternion.Euler(-90.0f, 180.0f, 90.0f);
    }

    public void SetNanAtRight() {
        //_Nan.transform.localPosition = new Vector3(1, -0.04f, 1);
        _Nan.transform.localRotation = Quaternion.Euler(-90.0f, -90.0f, 0);
    }

    public void SetNanAtTop() {
        //_Nan.transform.localPosition = new Vector3(1, -0.05f, 1);
        _Nan.transform.localRotation = Quaternion.Euler(-90.0f, 180.0f, 0.0f);
    }

    public void SetNanAtLeft() {
        //_Nan.transform.localPosition = new Vector3(1, -0.05f, 1);
        _Nan.transform.localRotation = Quaternion.Euler(-90.0f, 90.0f, 0.0f);
    }

    public void SetNanAtBottom() {
        //_Nan.transform.localPosition = new Vector3(1.009f, -0.05f, 1.007f);
        _Nan.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
    }

    public void SetXiAtRight() {
        //_Xi.transform.localPosition = new Vector3(1.01f, -0.05f, 1.007f);
        _Xi.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
    }

    public void SetXiAtTop() {
        //_Xi.transform.localPosition = new Vector3(1.00f, -0.04f, 1.000f);
        _Xi.transform.localRotation = Quaternion.Euler(-90.0f, -90.0f, 0.0f);
    }

    public void SetXiAtLeft() {
        //_Xi.transform.localPosition = new Vector3(1.00f, -0.06f, 1.000f);
        _Xi.transform.localRotation = Quaternion.Euler(-90.0f, 180.0f, 0.0f);
    }

    public void SetXiAtBottom() {
        //_Xi.transform.localPosition = new Vector3(1.01f, -0.06f, 1.000f);
        _Xi.transform.localRotation = Quaternion.Euler(-90.0f, 90.0f, 0.0f);
    }

    public void SetBeiAtRight() {
        _Bei.transform.localRotation = Quaternion.Euler(-90.0f, 90.0f, 0.0f);
    }

    public void SetBeiAtTop() {
        //_Bei.transform.localPosition = new Vector3(1.00f, -0.05f, 1.000f);
        _Bei.transform.localRotation = Quaternion.Euler(-90.0f, 0.0f, 0.0f);
    }

    public void SetBeiAtLeft() {
        _Bei.transform.localRotation = Quaternion.Euler(-90.0f, -90.0f, 0.0f);
    }

    public void SetBeiAtBottom() {
        //_Bei.transform.localPosition = new Vector3(1.01f, -0.07f, 1.000f);
        _Bei.transform.localRotation = Quaternion.Euler(-90.0f, 180.0f, 0.0f);
    }

    public void TakeOnDong(bool blink) {
        _blink = blink;
        _gray = 0;
        _brightness = 0.9f;

        _Dong.GetComponent<Renderer>().material.SetInt("_Gray", _gray);
        _Dong.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);

        if (_blink) {
            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(_blinkinterval)
                .AppendCallback(() => {
                    if (_brightness == 1.0f) {
                        _brightness = 0.9f;
                        _Dong.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);
                    } else {
                        _brightness = 1.0f;
                        _Dong.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);
                    }

                    if (!_blink) {
                        mySequence.SetLoops(0);
                    }
                }).SetLoops(-1);
        }
    }

    public void TakeOffDong() {
        _blink = false;
        _gray = 1;
        _brightness = 1.0f;
        _Dong.GetComponent<Renderer>().material.SetInt("_Gray", _gray);
        _Dong.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);
    }

    public void TakeTurnDong(bool blink = true) {
        TakeOnDong(true);
        TakeOffNan();
        TakeOffXi();
        TakeOffBei();
    }

    public void TakeOnNan(bool blink) {
        _blink = blink;
        _gray = 0;
        _brightness = 0.9f;

        _Nan.GetComponent<Renderer>().material.SetInt("_Gray", _gray);
        _Nan.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);

        if (_blink) {
            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(_blinkinterval)
                .AppendCallback(() => {
                    if (_brightness == 1.0f) {
                        _brightness = 0.9f;
                        _Nan.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);
                    } else {
                        _brightness = 1.0f;
                        _Nan.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);
                    }

                    if (!_blink) {
                        mySequence.SetLoops(0);
                    }
                }).SetLoops(-1);
        }
    }

    public void TakeOffNan() {
        _blink = false;
        _gray = 1;
        _brightness = 1.0f;

        _Nan.GetComponent<Renderer>().material.SetInt("_Gray", _gray);
        _Nan.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);
    }

    public void TakeTurnNan(bool blink = true) {
        TakeOffDong();
        TakeOnNan(blink);
        TakeOffXi();
        TakeOffBei();
    }

    public void TakeOnXi(bool blink) {
        _blink = blink;
        _gray = 0;
        _brightness = 0.9f;

        _Xi.GetComponent<Renderer>().material.SetInt("_Gray", _gray);
        _Xi.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);

        if (_blink) {
            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(_blinkinterval)
                .AppendCallback(() => {
                    if (_brightness == 1.0f) {
                        _brightness = 0.9f;
                        _Xi.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);
                    } else {
                        _brightness = 1.0f;
                        _Xi.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);
                    }

                    if (!_blink) {
                        mySequence.SetLoops(0);
                    }
                }).SetLoops(-1);
        }
    }

    public void TakeOffXi() {
        _blink = false;
        _gray = 1;
        _brightness = 1.0f;

        _Xi.GetComponent<Renderer>().material.SetInt("_Gray", _gray);
        _Xi.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);
    }

    public void TakeTurnXi(bool blink = true) {
        TakeOffDong();
        TakeOffNan();
        TakeOnXi(blink);
        TakeOffBei();
    }

    public void TakeOnBei(bool blink) {
        _blink = blink;
        _gray = 0;
        _brightness = 0.9f;

        _Bei.GetComponent<Renderer>().material.SetInt("_Gray", _gray);
        _Bei.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);

        if (_blink) {
            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(_blinkinterval)
                .AppendCallback(() => {
                    if (_brightness == 1.0f) {
                        _brightness = 0.9f;
                        _Bei.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);
                    } else {
                        _brightness = 1.0f;
                        _Bei.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);
                    }

                    if (!_blink) {
                        mySequence.SetLoops(0);
                    }
                }).SetLoops(-1);
        }
    }

    public void TakeOffBei() {
        _blink = false;
        _gray = 1;
        _brightness = 1.0f;

        _Bei.GetComponent<Renderer>().material.SetInt("_Gray", _gray);
        _Bei.GetComponent<Renderer>().material.SetFloat("_Brightness", _brightness);
    }

    public void TakeTurnBei(bool blink = true) {
        TakeOffDong();
        TakeOffNan();
        TakeOffXi();
        TakeOnBei(true);
    }

}
