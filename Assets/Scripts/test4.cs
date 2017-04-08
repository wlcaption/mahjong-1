using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test4 : MonoBehaviour {

    public GameObject _Hand;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnChupai() {
        _Hand.GetComponent<Animator>().SetBool("Chupai", true);
    }

    public void OnChutuipai() {
        _Hand.GetComponent<Animator>().SetBool("Chutuipai", true);
    }

    public void OnDiushaizi() {
        _Hand.GetComponent<Animator>().SetBool("Diushaizi", true);
    }
}
