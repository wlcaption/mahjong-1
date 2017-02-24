using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour {

    public RootBehaviour _Root;

	// Use this for initialization
	void Start () {
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_SETUP_SCENE, gameObject);
        _Root.App.Enqueue(cmd);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
