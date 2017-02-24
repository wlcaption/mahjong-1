using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MUIRoot : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject    _CreatePanel;
    public GameObject    _JoinPanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMatch() {
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_MUI_MATCH, gameObject);
        _Root.App.Enqueue(cmd);
    }

    public void OnCreate() {
        if (_CreatePanel != null) {
            _CreatePanel.SetActive(true);
        }
    }

    public void OnCreateClose() {
        if (_CreatePanel != null) {
            _CreatePanel.SetActive(false);
        }
    }

    public void OnJoin() {
        if (_JoinPanel != null) {
            _JoinPanel.SetActive(true);
        } else {
            UnityEngine.Debug.Assert(false);
        }
    }

    public void OnJoinClose() {
        if (_JoinPanel != null) {
            _JoinPanel.SetActive(false);
        } else {
            UnityEngine.Debug.Assert(false);
        }
    }
}
