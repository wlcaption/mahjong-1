using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleWnd : MonoBehaviour {

    public GameObject _ScPage;
    public GameObject _SxPage;

    public GameObject _XiuLiu;
    public GameObject _XiuZhan;
    public GameObject _Point;

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

    public void OnScXiuLiu(bool value) {
        if (value) {
            //_ScPage.SetActive(true);
            Vector3 xpos = transform.worldToLocalMatrix * _XiuLiu.transform.position;
            Vector3 pos = _Point.transform.localPosition;
            _Point.transform.localPosition = new Vector3(pos.x, xpos.y, pos.z);
        } else {
            //_ScPage.SetActive(false);
        }
    }

    public void OnScXiuZhan(bool value) {
        if (value) {
            //_ScPage.SetActive(true);
            Vector3 xpos = transform.worldToLocalMatrix * _XiuZhan.transform.position;
            Vector3 pos = _Point.transform.localPosition;
            _Point.transform.localPosition = new Vector3(pos.x, xpos.y, pos.z);
        } else {
            //_ScPage.SetActive(false);
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
