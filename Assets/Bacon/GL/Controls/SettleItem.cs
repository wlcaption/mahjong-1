using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bacon.GL.Controls { 
public class SettleItem : MonoBehaviour {

    public GameObject _Cause;
    public GameObject _Multiple;
    public GameObject _Fen;
    public GameObject _Who;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(string cause, string multiple, string fen, string who) {
        _Cause.GetComponent<Text>().text = cause;
        _Multiple.GetComponent<Text>().text = multiple;
        _Fen.GetComponent<Text>().text = fen;
        _Who.GetComponent<Text>().text = who;
    }
}
}