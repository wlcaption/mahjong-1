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
        SayItem item = SayConfig.Instance.GetItem((int)code);
        Head.ShowSay(item.text);
        string path = item.sound;
        int idx = path.IndexOf('.');
        if (idx != -1) {
            path = path.Remove(idx);
        }
        idx = path.LastIndexOf('/');
        string name = string.Empty;
        for (int i = idx + 1; i < path.Length; i++) {
            name += path[i];
        }
        path.Remove(idx);
        
        AudioClip clip = ABLoader.current.LoadAsset<AudioClip>(path, name);
        SoundMgr.current.PlaySound(gameObject, clip);
    }
}
