using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public GameObject _Clock1;
    public GameObject _Clock2;
    public GameObject _Cursor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowCountdown(int pt) {
        int c1 = pt / 10;
        int c2 = pt % 10;
        if (_Clock1 != null) {
            if (!_Clock1.activeSelf) {
                _Clock1.SetActive(true);
            }
            _Clock1.GetComponent<TextMesh>().text = string.Format("{0}", c1);
        }
        if (_Clock2 != null) {
            if (!_Clock2.activeSelf) {
                _Clock2.SetActive(true);
            }
            _Clock2.GetComponent<TextMesh>().text = string.Format("{0}", c2);
        }
    }

    public void ChangeCursor(Vector3 pos) {
        if (_Cursor != null) {
            if (!_Cursor.activeSelf) {
                _Cursor.SetActive(true);
            }
            _Cursor.transform.localPosition = pos;
        }
    }

}
