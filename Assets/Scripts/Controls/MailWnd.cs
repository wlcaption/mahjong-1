using Bacon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailWnd : MonoBehaviour {

    public GameObject _OfficialPage;
    public GameObject _VersionPage;
    public GameObject _InfoPage;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

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

    public void OnOfficalChanged(bool value) {
        if (value) {
            _OfficialPage.SetActive(true);
        } else {
            _OfficialPage.SetActive(false);
        }
    }

    public void OnVersionChanged(bool value) {
        if (value) {
            _VersionPage.SetActive(true);
        } else {
            _VersionPage.SetActive(false);
        }
    }

    public void ShowMsg(SysInbox inbox) {
        //for (int i = 0; i < length; i++) {

        //}
        Transform content = _OfficialPage.transform.FindChild("Viewport").FindChild("Content");
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MsgItem"));
        go.transform.SetParent(content);
    }

    public void ShowMailInfo(string title, string content) {
        if (!_InfoPage.activeSelf) {
            _InfoPage.SetActive(true);
        }
    }

    public void OnCloseMailInfo() {
        if (_InfoPage.activeSelf) {
            _InfoPage.SetActive(false);
        }
    }
}
