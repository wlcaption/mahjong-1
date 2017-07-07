using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bacon;
using Bacon.Model;

public class RecordWnd : MonoBehaviour {

    public GameObject _Content;

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

    //public void Show(RecordMgr mgr) {
    //    if (!gameObject.activeSelf) {
    //        gameObject.SetActive(true);
    //    }
    //    foreach (var item in mgr) {
    //        GameObject ori = ABLoader.current.LoadAsset<GameObject>("Prefabs/Main/UI", "RecordItem");
    //        GameObject go = Instantiate<GameObject>(ori);
    //        go.transform.SetParent(_Content.transform);
    //    }
    //}

    //public void OnClose() {
    //    if (gameObject.activeSelf) {
    //        gameObject.SetActive(false);
    //    }
    //}
}
