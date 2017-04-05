using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class test3 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        transform.DOLocalMove(new Vector3(0.0f, 100.0f, 0.0f), 20.0f);
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
