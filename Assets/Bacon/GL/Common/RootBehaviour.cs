using UnityEngine;
using Maria;

namespace Bacon.GL.Common { 
public class RootBehaviour : MonoBehaviour {
    private Maria.Util.App _app = null;

    // Use this for initialization
    void Start() {
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update() {
    }

    protected Maria.Util.App InitApp() {
        if (_app == null) {
            _app = GameObject.Find("App").GetComponent<Maria.Util.App>();
            if (_app == null) {
                UnityEngine.Debug.Assert(false, "why ");
                return null;
            } else {
                return _app;
            }
        } else {
            return _app;
        }
    }

    public Maria.Util.App App {
        get {
            return InitApp();
        }
        set {
            InitApp();
        }
    }
}
}