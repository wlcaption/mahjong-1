using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maria;
using Bacon;

public class GameRoot : MonoBehaviour {

    public RootBehaviour _Root;

    public GameObject _Top;
    public GameObject _Bottom;
    public GameObject _Left;
    public GameObject _Right;

	// Use this for initialization
	void Start () {
        var board = transform.FindChild("Board").gameObject;
        Command cmd1 = new Command(MyEventCmd.EVENT_SETUP_BOARD, board);
        Command cmd2 = new Command(MyEventCmd.EVENT_SETUP_SCENE, gameObject);
        _Root.App.Enqueue(cmd1);
        _Root.App.Enqueue(cmd2);
    }
	
	// Update is called once per frame
	void Update () { 	
	}

    public void SetupPlayer() {
        Command cmd1 = new Command(MyEventCmd.EVENT_SETUP_TOPPLAYER, _Top);
        Command cmd2 = new Command(MyEventCmd.EVENT_SETUP_BOTTOMPLAYER, _Bottom);
        Command cmd3 = new Command(MyEventCmd.EVENT_SETUP_RIGHTPLAYER, _Right);
        Command cmd4 = new Command(MyEventCmd.EVENT_SETUP_LEFTPLAYER, _Left);

        _Root.App.Enqueue(cmd1);
        _Root.App.Enqueue(cmd2);
        _Root.App.Enqueue(cmd3);
        _Root.App.Enqueue(cmd4);
    }
}
