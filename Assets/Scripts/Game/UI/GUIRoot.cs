using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maria;
using Bacon;

public class GUIRoot : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject _RoomId;
    public GameObject _OverWnd;
    public GameObject _ChatWnd;
    public GameObject _HelpWnd;
    public GameObject _SettingWnd;
    public GameObject _ExitWnd;

    public Text _Time;
    public Text _Left;
    

	// Use this for initialization
	void Start () {
        Command cmd = new Maria.Command(MyEventCmd.EVENT_SETUP_GUIROOT, gameObject);
        _Root.App.Enqueue(cmd);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitUI(int id) {
        string name = string.Format("Room ID: {0}", id);
        if (_RoomId != null) {
            _RoomId.GetComponent<Text>().text = name;
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

    public  void OnHelp() {
        if (_HelpWnd != null) {
            _HelpWnd.GetComponent<RuleWnd>().Show();
            _HelpWnd.SetActive(true);
        }
    }

    public void OnSetting() {
        if (_SettingWnd != null) {
            _SettingWnd.GetComponent<SettingWnd>().Show();
        }
    }

    public void OnChat() {
        if (_ChatWnd != null) {
            _ChatWnd.GetComponent<ChatWnd>().Show();
        }
    }

    public void OnClose() {
        CloseOver();
    }

    public void ShowOver() {
        if (_OverWnd != null) {
            _OverWnd.SetActive(true);
        }
    }

    public void CloseOver() {
        if (_OverWnd != null) {
            _OverWnd.SetActive(false);
        }
    }

    public void OnRestart() {
        if (_OverWnd != null) {
            _OverWnd.SetActive(false);
        }
        Command cmd = new Command(MyEventCmd.EVENT_RESTART);
        _Root.App.Enqueue(cmd);
    }
}
