using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPlayer : MonoBehaviour {

    public RootBehaviour _Root;
    public TopPlayerHead Head;
    public OverWnd OverWnd;

    // Use this for initialization
    void Start() {
        //Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_SETUP_TOPPLAYER, gameObject);
        //_Root.App.Enqueue(cmd);
    }

    // Update is called once per frame
    void Update() {

    }

    public void Init() {
        Head.Init();
    }

    public void ShowUI() {
        Head.Show();
    }

    public void HideUI() {
        Head.Close();
    }

    public void Say(long code) {
        SayItem i = SayConfig.Instance.GetItem((int)code);
        Head.SetSay(i.text);
        string path = i.sound;
        int idx = path.IndexOf('.');
        if (idx != -1) {
            path = path.Remove(idx);
        }

        AudioClip clip = ABLoader.current.LoadRes<AudioClip>(path);
        SoundMgr.current.PlaySound(gameObject, clip);
    }
}
