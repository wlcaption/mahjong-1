using UnityEngine;
using Maria;
using Bacon.Event;

public class GameRoot : MonoBehaviour {

    public RootBehaviour _Root;

    public GameObject _Top;
    public GameObject _Bottom;
    public GameObject _Left;
    public GameObject _Right;

    // Use this for initialization
    void Start() {
        _Top.GetComponent<TopPlayer>().Init();
        _Bottom.GetComponent<BottomPlayer>().Init();
        _Left.GetComponent<LeftPlayer>().Init();
        _Right.GetComponent<RightPlayer>().Init();

        Command cmd = new Command(MyEventCmd.EVENT_SETUP_SCENE, gameObject);
        _Root.App.Enqueue(cmd);
    }

    // Update is called once per frame
    void Update() {
    }

    public void SetupBottomPlayer() {
        Command cmd2 = new Command(MyEventCmd.EVENT_SETUP_BOTTOMPLAYER, _Bottom);
        _Root.App.Enqueue(cmd2);
    }

    public void SetupLeftPlayer() {
        Command cmd4 = new Command(MyEventCmd.EVENT_SETUP_LEFTPLAYER, _Left);
        _Root.App.Enqueue(cmd4);
    }

    public void SetupTopPlayer() {
        Command cmd1 = new Command(MyEventCmd.EVENT_SETUP_TOPPLAYER, _Top);
        _Root.App.Enqueue(cmd1);
    }

    public void SetupRightPlayer() {
        Command cmd3 = new Command(MyEventCmd.EVENT_SETUP_RIGHTPLAYER, _Right);
        _Root.App.Enqueue(cmd3);
    }

}
