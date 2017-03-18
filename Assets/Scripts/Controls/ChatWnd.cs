using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChatWnd : MonoBehaviour {

    // Use this for initialization
    void Start() {

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
