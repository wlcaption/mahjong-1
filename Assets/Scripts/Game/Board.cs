using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maria;
using Bacon;

public class Board : MonoBehaviour {

    public GameObject _Clock1;
    public GameObject _Clock2;
    public GameObject _Cursor;
    public GameObject _Dice1;
    public GameObject _Dice2;

	// Use this for initialization
	void Start () {
        Command cmd = new Command(MyEventCmd.EVENT_SETUP_BOARD, gameObject);
        GetComponent<FindApp>().App.Enqueue(cmd);
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

    public void ThrowDice(long d1, long d2) {
        ThrowSDice(d1, _Dice1);
        ThrowSDice(d2, _Dice2);
        Command cmd = new Command(MyEventCmd.EVENT_THROWDICE);
        GetComponent<FindApp>().App.Enqueue(cmd);
    }

    private void ThrowSDice(long d, GameObject go) {
        switch (d) {
            case 1:
                go.transform.localRotation = Quaternion.AngleAxis(-180.0f, Vector3.forward);
                break;
            case 2:
                go.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.right) * Quaternion.AngleAxis(270.0f, Vector3.forward);
                break;
            case 3:
                go.transform.localRotation = Quaternion.AngleAxis(-90.0f, Vector3.right) * Quaternion.AngleAxis(270.0f, Vector3.forward);
                break;
            case 4:
                go.transform.localRotation = Quaternion.AngleAxis(270.0f, Vector3.forward);
                break;
            case 5:
                go.transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.right) * Quaternion.AngleAxis(270.0f, Vector3.forward);
                break;
            case 6:
                go.transform.localRotation = Quaternion.AngleAxis(0.0f, Vector3.right);
                break;
            default:
                UnityEngine.Debug.Assert(false);
                break;
        }
    }
}
