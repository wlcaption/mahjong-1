using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Peng : MonoBehaviour {

    private Action _completed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play(Action cb) {
        _completed = cb;
    }

    public void OnCompleted() {
        _completed();
    }
}
