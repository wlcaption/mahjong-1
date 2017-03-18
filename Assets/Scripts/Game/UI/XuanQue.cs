using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maria;
using Bacon;

public class XuanQue : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCrak( ) {

        gameObject.SetActive(false);
    }

    public void OnBam() {
        gameObject.SetActive(false);
    }

    public void OnDot() {
        gameObject.SetActive(false);

    }
}
