using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoom : MonoBehaviour {

    public RootBehaviour _Root;
    public Text _RoomNum;
    private int _count = 0;
    private const int _max = 6;
    private string _num = string.Empty;
    private bool _sended;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    void OnEnable() {
        _count = 0;
        _sended = false;
    }

    void OnDisable() {

    }

    public void OnBtn1() {
        if (_count >= _max) {
            return;
        }
        _num += string.Format("{0}", 1);
        _RoomNum.text = _num;
        _count++;
    }

    public void OnBtn2() {
        if (_count >= _max) {
            return;
        }
        _num += string.Format("{0}", 2);
        _RoomNum.text = _num;
        _count++;
    }

    public void OnBtn3() {
        if (_count >= _max) {
            return;
        }
        _num += string.Format("{0}", 3);
        _RoomNum.text = _num;
        _count++;
    }

    public void OnBtn4() {
        if (_count >= _max) {
            return;
        }
        _num += string.Format("{0}", 4);
        _RoomNum.text = _num;
        _count++;
    }

    public void OnBtn5() {
        if (_count >= _max) {
            return;
        }
        _num += string.Format("{0}", 5);
        _RoomNum.text = _num;
        _count++;
    }

    public void OnBtn6() {
        if (_count >= _max) {
            return;
        }
        _num += string.Format("{0}", 6);
        _RoomNum.text = _num;
        _count++;
    }

    public void OnBtn7() {
        if (_count >= _max) {
            return;
        }
        _num += string.Format("{0}", 7);
        _RoomNum.text = _num;
        _count++;
    }

    public void OnBtn8() {
        if (_count >= _max) {
            return;
        }
        _num += string.Format("{0}", 8);
        _RoomNum.text = _num;
        _count++;
    }

    public void OnBtn9() {
        if (_count >= _max) {
            return;
        }
        _num += string.Format("{0}", 9);
        _RoomNum.text = _num;
        _count++;
    }

    public void OnBtn0() {
        if (_count >= _max) {
            return;
        }
        _num += string.Format("{0}", 0);
        _RoomNum.text = _num;
        _count++;
    }

    public void OnBtnDel() {
        if (_count > 0) {
            _num = _num.Remove(_count - 1);
            _RoomNum.text = _num;
            _count--;
        }
    }

    public void OnBtnClr() {
        _num = string.Empty;
        _RoomNum.text = _num;
        _count = 0;
    }

    public void OnJoin() {
        if (_RoomNum == null) {
            return;
        }
        if (_sended) {
            return;
        }
        if (_count == 6) {
            int res = 0;
            for (int i = 0; i < _num.Length; i++) {
                res += res * 10 + int.Parse(_num[i].ToString());
            }

            Maria.Message msg = new Maria.Message();
            msg["roomid"] = res;

            Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_MUI_JOIN, gameObject, msg);
            _Root.App.Enqueue(cmd);
            _sended = true;
        }
    }
}
