using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Bacon.GL.Game { 

public class Hu : MonoBehaviour {

    private Action _completed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play(Action cb) {
        _completed = cb;
        Animator animator = GetComponent<Animator>();
    }

    public void OnCompleted() {

    }
}
}