using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsWnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClose() {
        gameObject.SetActive(false);
    }
}
