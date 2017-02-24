using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject _8Toggle;
    public GameObject _16Toggle;
    public GameObject _RoomNum;

    private int _count = 8;
    private int _rule = 1;  // 平胡

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void On8ToggleChanged(bool value) {

    }

    public void On16ToggleChanged(bool value) {

    }

    public void OnCreate() {
        Maria.Message msg = new Maria.Message();
        msg["count"] = _count;
        msg["rule"] = _rule;
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_MUI_CREATE, gameObject, msg);
        _Root.App.Enqueue(cmd);
    }

    public void OnJoin() {
        if (_RoomNum == null) {
            return;
        }

        string num = _RoomNum.GetComponent<InputField>().text;
        int res = 0;
        if (int.TryParse(num, out res)) {
            Maria.Message msg = new Maria.Message();
            msg["roomid"] = res;

            Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_MUI_JOIN, gameObject, msg);
            _Root.App.Enqueue(cmd);
        }
    }

}
