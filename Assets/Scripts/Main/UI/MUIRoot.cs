using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MUIRoot : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject _CreatePanel;
    public GameObject _JoinPanel;
    public GameObject _SharePanel;
    public GameObject _RecorePanel;
    public GameObject _MsgPanel;
    public GameObject _RulePanel;
    public GameObject _SettingPanel;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnMatch() {
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_MUI_MATCH, gameObject);
        _Root.App.Enqueue(cmd);
    }

    public void OnCreate() {
        if (_CreatePanel != null) {
            _CreatePanel.SetActive(true);
            string txt = string.Format("已有房卡x{0}", 0);
            _CreatePanel.transform.FindChild("RCard").GetComponent<Text>().text = txt;
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

    public void OnShare() {
        if (_SharePanel != null) {
            _SharePanel.SetActive(true);
        }
    }

    public void OnShareClose() {
        if (_SharePanel != null) {
            _SharePanel.SetActive(false);
        }
    }

    public void OnRecored() {
        if (_RecorePanel != null) {
            _RecorePanel.SetActive(true);
        }
    }

    public void OnRecoredClose() {
        if (_RecorePanel != null) {
            _RecorePanel.SetActive(false);
        }
    }

    public void OnMsg() {
        if (_MsgPanel != null) {
            _MsgPanel.SetActive(true);
        }
    }

    public void OnMsgClose() {
        if (_MsgPanel != null) {
            _MsgPanel.SetActive(false);
        }
    }

    public void OnRule() {
        if (_RulePanel != null) {
            _RulePanel.SetActive(true);
        }
    }

    public void OnRuleClose() {
        if (_RulePanel != null) {
            _RulePanel.SetActive(false);
        }
    }

    public void OnSetting() {
        if (_SettingPanel != null) {
            _SettingPanel.SetActive(true);
        }
    }

    public void OnSettingClose() {
        if (_SettingPanel != null) {
            _SettingPanel.SetActive(false);
        }
    }
}
