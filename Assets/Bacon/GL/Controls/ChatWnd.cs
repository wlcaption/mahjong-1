﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Bacon.DataSet;
using Maria.Util;

namespace Bacon.GL.Controls { 
public class ChatWnd : MonoBehaviour {

    public GameObject _Page;

    // Use this for initialization
    void Start() {
        Transform pa = _Page.transform.Find("Viewport").Find("Content");
        var data = SayDataSet.Instance.GetSays();
        foreach (var kv in data) {

            SayDataSet.SayItem item = kv.Value;
            //ResourceManager.Instance.LoadAssetAsync<GameObject>("Prefabs/UI/ChatItem.prefab", "ChatItem", (GameObject go) => {
            //    GameObject inst = Instantiate<GameObject>(go);
            //    inst.GetComponent<ChatItem>().Init(item.code, item.text);
            //    inst.transform.SetParent(pa);
            //});
            ABLoader.current.LoadAssetAsync<GameObject>("Prefabs/UI", "ChatItem", (GameObject go) => {
                GameObject inst = Instantiate<GameObject>(go);
                inst.GetComponent<ChatItem>().Init((int)item.code, item.text);
                inst.transform.SetParent(pa);
            });
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            if (EventSystem.current.IsPointerOverGameObject()) {
#elif UNITY_IOS || UNITY_ANDROID
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
#endif
                if (EventSystem.current.currentSelectedGameObject == gameObject) {
                    if (gameObject.activeSelf) {
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void Show() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }

}
}