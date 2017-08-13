using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maria;
using Bacon;
using Bacon.Event;
using Maria.Util;

namespace Bacon.GL.Main.UI { 
public class JoinRoom : MonoBehaviour {

    public Text _RoomNum;
    private int _count = 0;
    private const int _max = 6;
    private int _num = 0;
    private string _numstr = string.Empty;
    private bool _sended;

    private string _tips = "请输入六位数字";

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    void OnEnable() {
        _RoomNum.text = _tips;
        _count = 0;
        _num = 0;
        _numstr = string.Empty;
        _sended = false;
    }

    void OnDisable() {

    }

    private void AddNum(int num) {
        if (_count >= _max) {
            return;
        }
        _num *= 10;
        _num += num;
        _numstr += string.Format("{0}", num);
        _RoomNum.text = _numstr;
        _count++;
    }

    public void Show() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }

    public void OnClose() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }

    public void OnBtn1() {
        AddNum(1);
    }

    public void OnBtn2() {
        AddNum(2);
    }

    public void OnBtn3() {
        AddNum(3);
    }

    public void OnBtn4() {
        AddNum(4);
    }

    public void OnBtn5() {
        AddNum(5);
    }

    public void OnBtn6() {
        AddNum(6);
    }

    public void OnBtn7() {
        AddNum(7);
    }

    public void OnBtn8() {
        AddNum(8);
    }

    public void OnBtn9() {
        AddNum(9);
    }

    public void OnBtn0() {
        AddNum(0);
    }

    public void OnBtnDel() {
        if (_count > 0) {
            _num /= 10;
            _numstr = _numstr.Remove(_numstr.Length - 1);
            _RoomNum.text = _numstr;
            _count--;
            if (_count <= 0) {
                _RoomNum.text = _tips;
            }
        }
    }

    public void OnBtnClr() {
        
        _RoomNum.text = _tips;
        _count = 0;
        _num = 0;
        _numstr = string.Empty;
    }

    public void OnJoin() {
        if (_RoomNum == null) {
            return;
        }
        if (_sended) {
            return;
        }
        if (_count == _max) {
           
            Maria.Message msg = new Maria.Message();
            msg["roomid"] = _num;

            Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_MUI_JOIN, gameObject, msg);
            GetComponent<FindApp>().App.Enqueue(cmd);
            _sended = true;
        }
    }
}
}