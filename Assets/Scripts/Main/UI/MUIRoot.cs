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
    public GameObject _MailWnd;
    public GameObject _RuleWnd;
    public GameObject _SettingPanel;

    public GameObject _Board;
    public GameObject _Adver;

    public GameObject _Tips;
    public GameObject _Title;

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
        Command cmd = new Command(MyEventCmd.EVENT_MUI_SHOWCREATE);
        _Root.App.Enqueue(cmd);
    }

    public void ShowCreate(int num) {
        if (_CreatePanel != null) {
            _CreatePanel.GetComponent<CreateRoom>().Show(num);
        }
    }

    public void OnJoin() {
        _JoinPanel.GetComponent<JoinRoom>().Show();
    }

    public void OnShare() {
        if (_SharePanel != null) {
            _SharePanel.GetComponent<ShareWnd>().Show();
        }
    }

    public void OnRecored() {
        if (_RecorePanel != null) {
            _RecorePanel.GetComponent<RecordWnd>().Show();
        }
    }

    public void OnMsg() {
        if (_MailWnd != null) {
            _MailWnd.GetComponent<MailWnd>().Show();
            Command cmd = new Command(MyEventCmd.EVENT_MUI_MSG);
            _Root.App.Enqueue(cmd);
        }
    }

    public void OnRule() {
        if (_RuleWnd != null) {
            _RuleWnd.GetComponent<RuleWnd>().Show();
        }
    }

    public void OnSetting() {
        if (_SettingPanel != null) {
            _SettingPanel.GetComponent<SettingWnd>().Show(SettingWnd.ExitType.EXIT_LOGIN);
        }
    }

    public void OnAdd() {
        if (_Tips != null) {
            _Tips.SetActive(true);
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

    public void ShowTips(string content) {
        //_Tips.GetComponent<TipsWnd>().
    }
}
