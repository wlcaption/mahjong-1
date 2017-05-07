using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsWnd : MonoBehaviour {

    public GameObject _Content;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Show(string content) {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
        if (_Content != null) {
            _Content.GetComponent<Text>().text = content;
        }
    }

    public void OnClose() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }
}
