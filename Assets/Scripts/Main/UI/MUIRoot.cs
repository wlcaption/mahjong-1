using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maria;
using Bacon;
using DG.Tweening;

public class MUIRoot : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject _CreatePanel;
    public GameObject _JoinPanel;
    public GameObject _SharePanel;
    public GameObject _RecorePanel;
    public GameObject _MsgPanel;
    public GameObject _RulePanel;
    public GameObject _SettingPanel;

    public GameObject _Board;
    public GameObject _Adver;

    public GameObject _Name;
    public GameObject _NameId;
    public GameObject _RCard;
    public GameObject _Tips;

    // Use this for initialization
    void Start() {
        Command cmd = new Command(MyEventCmd.EVENT_SETUP_MUI, gameObject);
        _Root.App.Enqueue(cmd);
        GameObject go = _Adver.transform.FindChild("Mask").FindChild("Text").gameObject;

        go.transform.localPosition.Set(750.0f, 0.0f, 0.0f);
        Sequence s = DOTween.Sequence();
        Tween t = go.transform.DOMoveX(-750.0f, 10.0f);
        s.Append(t).SetLoops(-1);

    }

    // Update is called once per frame
    void Update() {

    }

    #region 回掉接口
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
            Command cmd = new Command(MyEventCmd.EVENT_MUI_MSG);
            _Root.App.Enqueue(cmd);
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

    public void OnAdd() {
        if (_Tips != null) {
            _Tips.SetActive(true);
        }
    }

    public void OnTipsClose() {
        if (_Tips != null) {
            _Tips.SetActive(false);
        }
    }
    #endregion

    public void SetBoard(string board) {
        Text content = _Board.transform.FindChild("Content").GetComponent<Text>();
        content.text = board;
    }

    public void SetAdver(string adver) {
        Text content = _Adver.transform.FindChild("Mask").FindChild("Text").GetComponent<Text>();
        content.text = adver;
    }

    public void SetName(string name) {
        _Name.GetComponent<Text>().text = name;
    }

    public void SetNameId(string nameid) {
        _NameId.GetComponent<Text>().text = nameid;
    }

    public void SetRCard(string rcard) {
        _RCard.GetComponent<Text>().text = rcard;
    }

}
