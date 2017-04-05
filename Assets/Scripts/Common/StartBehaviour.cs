using UnityEngine;
using System.Collections;
using Maria;
using Bacon;

public class StartBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetupStartRoot() {
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_SETUP_STARTROOT, gameObject);
        _root.App.Enqueue(cmd);
    }

    public void UpdateRes() {
        ABLoader.current.LoadPath();
        ABLoader.current.FetchVersion(() => {
            Command cmd = new Command(MyEventCmd.EVENT_UPdATERES);
            _root.App.Enqueue(cmd);
        });
    }

    public void TestRes() {
        ABLoader.current.LoadPath();
        ABLoader.current.LoadAssetAsync<AudioClip>("Sound/Man", "peng", (AudioClip clip) => {
            UnityEngine.Debug.Log("ok");
            Command cmd = new Command(MyEventCmd.EVENT_UPdATERES);
            _root.App.Enqueue(cmd);
        });

        ABLoader.current.LoadAssetAsync<AudioClip>("Sound/Woman", "bam1", (AudioClip clip) => {
            UnityEngine.Debug.Log("ok");
        });
    }
}
