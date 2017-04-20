using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject _LoginPanel;

    private bool _logined;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnEnable() {
        _logined = false;
    }

    public void OnLogin() {
        if (_LoginPanel == null) {
            return;
        }
        if (_logined) {
            return;
        }
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        string code = string.Format("{0}", Random.Range(100, 10000));
        Login(code);
#elif UNITY_IOS || UNITY_ANDROID
        try {
            AndroidJavaClass c = new AndroidJavaClass("com.emberfarkas.mahjong.wxapi.WXEntryActivity");
            AndroidJavaObject o = c.GetStatic<AndroidJavaObject>("currentWXActivity");
            o.Call("login");
        } catch (System.Exception ex) {
            UnityEngine.Debug.LogException(ex);
        }
#endif
        _logined = true;
    }

    void Login(string code) {
        UnityEngine.Debug.Log(code);

        Maria.Message msg = new Maria.Message();
        msg["username"] = code;
        msg["password"] = "Password";
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        msg["server"] = "sample1";
#elif UNITY_IOS || UNITY_ANDROID
        msg["server"] = "sample";
#endif
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_LOGIN, gameObject, msg);
        _Root.App.Enqueue(cmd);
    }
}
