using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maria;
using Bacon;

public class GUIRoot : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject _RoomId;
    public GameObject _Over;
    public GameObject _ChatPanel;
    public GameObject _HelpPanel;

    public Text _Time;
    public Text _Left;
    

	// Use this for initialization
	void Start () {
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_SETUP_GUIROOT, gameObject);
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

    }

    public  void OnHelp() {

    }

    public void OnSetting() {

    }

    public void OnChat() {
        if (_ChatPanel != null) {
            if (_ChatPanel.activeSelf) {
                _ChatPanel.SetActive(false);
            } else {
                _ChatPanel.SetActive(true);
                //_ChatPanel.transform.FindChild("")
            }
        }
    }

    public void OnClose() {
        CloseOver();
    }

    public void ShowOver() {
        if (_Over != null) {
            _Over.SetActive(true);
        }
    }

    public void CloseOver() {
        if (_Over != null) {
            _Over.SetActive(false);
        }
    }

    public void OnRestart() {
        if (_Over != null) {
            _Over.SetActive(false);
        }
        Command cmd = new Command(MyEventCmd.EVENT_RESTART);
        _Root.App.Enqueue(cmd);
    }
}
