using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MUIRoot : MonoBehaviour {

    public RootBehaviour _Root;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMatch() {
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_MUI_MATCH, gameObject);
        _Root.App.Enqueue(cmd);
    }

    public void OnCreate() {

    }

    public void OnJoin() {

    }
}
