using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPlayer : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject _UI;

    // Use this for initialization
    void Start() {
        //Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_SETUP_RIGHTPLAYER, gameObject);
        //_Root.App.Enqueue(cmd);
    }

    // Update is called once per frame
    void Update() {

    }

    public void ShowUI() {
        _UI.SetActive(true);
    }
    public void HideUI() {
        _UI.SetActive(false);
    }
}
