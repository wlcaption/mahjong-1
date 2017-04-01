using Bacon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maria;
using Bacon;

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
        Command cmd = new Command(MyEventCmd.EVENT_MUI_MSGCLOSED);
        GetComponent<FindApp>().App.Enqueue(cmd);
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

    public void ShowSysMsg(SysInbox inbox) {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }

        Transform content = _OfficialPage.transform.FindChild("Viewport").FindChild("Content");
        if (content.childCount > 0) {
            content.DetachChildren();
        }
        if (content.childCount == 0) {
            foreach (var item in inbox) {
                GameObject ori = ABLoader.current.LoadAsset<GameObject>("Prefabs/Controls", "MailItem");
                GameObject go = GameObject.Instantiate<GameObject>(ori);
                go.GetComponent<MsgItem>().SetType(MsgItem.Type.Sys);
                go.GetComponent<MsgItem>().SetId(item.Id);
                go.GetComponent<MsgItem>().SetTitle(item.Title);
                go.GetComponent<MsgItem>().SetContent(item.Content);
                go.GetComponent<MsgItem>().SetDateTime(item.DateTime);
                go.transform.SetParent(content);
            }
        }
    }

    public void ShowVerMsg(VerInbox inbox) {

    }
}
