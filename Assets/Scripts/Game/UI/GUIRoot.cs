using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIRoot : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject _RoomId;

	// Use this for initialization
	void Start () {
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_SETUP_GUIROOT, gameObject);
        _Root.App.Enqueue(cmd);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitUI(int id) {
        string name = string.Format("Room ID: {0}", id);
        if (_RoomId != null) {
            _RoomId.GetComponent<Text>().text = name;
        }
    }
}
