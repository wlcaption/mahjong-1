using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon.GL.Controls { 
public class ShareWnd : MonoBehaviour {

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

    public void OnShareFriend() {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
#elif UNITY_IOS || UNITY_ANDROID
        try {
            AndroidJavaClass c = new AndroidJavaClass("com.emberfarkas.mahjong.wxapi.WXEntryActivity");
            AndroidJavaObject o = c.GetStatic<AndroidJavaObject>("currentWXActivity");
            o.Call("ahareApp", 1);
        } catch (System.Exception ex) {
            UnityEngine.Debug.LogException(ex);
        }
#endif
    }

    public void OnShareSocial() {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
#elif UNITY_IOS || UNITY_ANDROID
        try {
            AndroidJavaClass c = new AndroidJavaClass("com.emberfarkas.mahjong.wxapi.WXEntryActivity");
            AndroidJavaObject o = c.GetStatic<AndroidJavaObject>("currentWXActivity");
            o.Call("ahareApp", 2);
        } catch (System.Exception ex) {
            UnityEngine.Debug.LogException(ex);
        }
#endif

    }

}
}