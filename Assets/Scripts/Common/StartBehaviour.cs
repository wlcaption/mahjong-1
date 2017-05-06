using UnityEngine;
using System.Collections;
using Maria;
using Bacon;
using UnityEngine.UI;

public class StartBehaviour : MonoBehaviour {

    public RootBehaviour _root = null;

    public GameObject _Slider;
    public GameObject _Tips;

    private float _progress = 0.0f;

    // Use this for initialization
    void Start() {
        _progress = 0.0f;
    }

    // Update is called once per frame
    void Update() {
        if (_progress <= 1.0f) {
            _progress += 0.01f;

            _Slider.GetComponent<Slider>().value = _progress > 1 ? 1 : _progress;
            _Tips.transform.FindChild("Text").GetComponent<Text>().text = string.Format("%{0}", Mathf.FloorToInt((_progress > 1 ? 1 : _progress) * 100));

            if (_progress > 1.0f) {
                Command cmd = new Command(MyEventCmd.EVENT_UPdATERES);
                _root.App.Enqueue(cmd);
            }
        }
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
        //ABLoader.current.LoadAssetAsync<AudioClip>("Sound/Man", "peng", (AudioClip clip) => {
        //    UnityEngine.Debug.Log("ok");
        //    //Command cmd = new Command(MyEventCmd.EVENT_UPdATERES);
        //    //_root.App.Enqueue(cmd);
        //});

        //ABLoader.current.LoadAssetAsync<AudioClip>("Sound/Woman", "bam1", (AudioClip clip) => {
        //    UnityEngine.Debug.Log("ok");
        //});

    }
}
