using Bacon;
using Maria;
using UnityEngine;
using System.Collections.Generic;

public class App : MonoBehaviour {

    public static App current = null;

    public StartBehaviour _start = null;
    private Bacon.App _app = null;

    void Awake() {
        if (current == null) {
            current = this;
        }
    }

    // Use this for initialization
    void Start() {
        DontDestroyOnLoad(this);
        _app = new Bacon.App(this);
        if (_start != null) {
            _start.SetupStartRoot();
        } else {
            throw new System.Exception("not imple");
        }
    }

    // Update is called once per frame
    void Update() {
        try {
            if (_app != null) {
                _app.Update();
            }
        } catch (System.Exception ex) {
            UnityEngine.Debug.LogException(ex);
        }
    }

    void OnApplicationFocus(bool isFocus) {
        if (_app != null) {
            _app.OnApplicationFocus(isFocus);
        }
    }

    void OnApplicationPause(bool isPause) {
        if (_app != null) {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
#else
            _app.OnApplicationPause(isPause);
#endif
        }
    }

    void OnApplicationQuit() {
        if (_app != null) {
            _app.OnApplicationQuit();
        }
        ABLoader.current.Unload();
    }

    public void Enqueue(Command cmd) {
        _app.Enqueue(cmd);
    }

}
