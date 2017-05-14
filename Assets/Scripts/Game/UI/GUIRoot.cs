using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maria;
using Bacon;
using System;

public class GUIRoot : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject _Canvas;
    public GameObject _RoomId;
    public GameObject _OverWnd;
    public GameObject _ChatWnd;
    public GameObject _HelpWnd;
    public GameObject _SettingWnd;
    public GameObject _ExitWnd;

    public Text _Time;
    public Text _Left;


    // Use this for initialization
    void Start() {
        Command cmd = new Maria.Command(MyEventCmd.EVENT_SETUP_GUIROOT, gameObject);
        _Root.App.Enqueue(cmd);
    }

    // Update is called once per frame
    void Update() {
        _Time.text = DateTime.Now.ToShortTimeString();
    }

    public void InitUI(int id) {
        string name = string.Format("房间号: {0:000000}", id);
        if (_RoomId != null) {
            _RoomId.GetComponent<TextMesh>().text = name;
        }
    }

    public void OnExit() {
        if (_ExitWnd != null) {
            _ExitWnd.SetActive(true);
        }
    }

    public void OnExitClose() {
        if (_ExitWnd != null) {
            _ExitWnd.SetActive(false);
        }
    }

    public void OnHelp() {
        if (_HelpWnd != null) {
            _HelpWnd.GetComponent<RuleWnd>().Show();
            _HelpWnd.SetActive(true);
        }
    }

    public void OnSetting() {
        Command cmd = new Maria.Command(MyEventCmd.EVENT_GAME_OPENSETTING, gameObject);
        _Root.App.Enqueue(cmd);
    }

    public void ShowSetting(SettingWnd.ExitType et) {
        if (_SettingWnd != null) {
            _SettingWnd.GetComponent<SettingWnd>().Show(et);
        }
    }

    public void OnChat() {
        if (_ChatWnd != null) {
            _ChatWnd.GetComponent<ChatWnd>().Show();
        }
    }

    public void OnInvite() {

    }

    public void ShowOver() {
        _OverWnd.GetComponent<OverWnd>().Show();
    }

    public void CloseOver() {
        _OverWnd.GetComponent<OverWnd>().Close();
    }
}
