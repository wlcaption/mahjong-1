using Bacon.DataSet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPlayer : MonoBehaviour {

    public RootBehaviour _Root;

    public LeftPlayerHead Head;
    public OverWnd OverWnd;

    // Use this for initialization
    void Start() {
        //Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_SETUP_LEFTPLAYER, gameObject);
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
        Head.Show();
    }

    public void Say(long code) {
        SayDataSet.SayItem item = SayDataSet.Instance.GetSayItem(code);
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
