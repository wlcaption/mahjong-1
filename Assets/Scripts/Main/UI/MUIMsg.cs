using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bacon;

public class MUIMsg : MonoBehaviour {
    public GameObject _OfficialContent;
    public GameObject _VersionContent;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void ShowMsg(SysInbox inbox) {
        //for (int i = 0; i < length; i++) {

        //}
        Transform content = _OfficialContent.transform.FindChild("Viewport").FindChild("Content");
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/UI/MsgItem"));
        go.transform.SetParent(content);
    }

    public void OnOfficalChanged(bool value) {
        if (value) {
            _OfficialContent.SetActive(true);
        } else {
            _OfficialContent.SetActive(false);
        }
    }

    public void OnVersionChanged(bool value) {
        if (value) {
            _VersionContent.SetActive(true);
        } else {
            _VersionContent.SetActive(false);
        }
    }
}
