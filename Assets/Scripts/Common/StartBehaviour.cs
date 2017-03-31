using UnityEngine;
using System.Collections;
using Maria;
using Bacon;

public class StartBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;

    // Use this for initialization
    void Start() {
        ABLoader.current.LoadABAsync<AudioClip>("sound/man.normal", "peng", (AudioClip clip) => {
            UnityEngine.Debug.Log("ok");
        });
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetupStartRoot() {
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_SETUP_STARTROOT, gameObject);
        _root.App.Enqueue(cmd);
    }

    public void UpdateRes() {
        ABLoader.current.FetchVersion(() => {
            Command cmd = new Command(MyEventCmd.EVENT_UPdATERES);
            _root.App.Enqueue(cmd);
        });
    }
}
