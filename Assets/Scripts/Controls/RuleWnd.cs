using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleWnd : MonoBehaviour {

    public GameObject _ScPage;
    public GameObject _SxPage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

    public void OnLookSc(bool value) {
        if (value) {
            _ScPage.SetActive(true);
        } else {
            _ScPage.SetActive(false);
        }
    }

    public void OnLookSx(bool value) {
        if (value) {
            _ScPage.SetActive(true);
        } else {
            _ScPage.SetActive(false);
        }
    }
}
