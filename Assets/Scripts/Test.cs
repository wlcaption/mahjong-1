using UnityEngine;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

public class Test : MonoBehaviour {

    //[DllImport("test", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    //public static extern int fntest();

    private GameObject _go;
    private Vector3 _lastMousePostion;

    // Use this for initialization
    void Start() {
        //byte[] xx = Maria.Encrypt.Crypt.randomkey();
        //UnityEngine.Debug.Log(ASCIIEncoding.ASCII.GetString(xx));

        //int r = fntest();
        //UnityEngine.Debug.Log(r);

        //transform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.up);
        //transform.localPosition = new Vector3(10.0f, 0.0f, 0.0f);

        //transform.localPosition = new Vector3(20.0f, 0.0f, 0.0f);
        //transform.localRotation = Quaternion.AngleAxis(60.0f, Vector3.up);

    }

    // Update is called once per frame
    void Update() {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        if (Input.GetMouseButtonDown(0)) {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(r, out hitInfo, 100)) {
                _go = hitInfo.transform.gameObject;
                _lastMousePostion = Input.mousePosition;
            }
        } else if (Input.GetMouseButton(0)) {
            if (_go != null) {
                Vector3 now = Input.mousePosition;
                Vector3 delta = now - _lastMousePostion;
                Debug.LogFormat("delta x: {0}, delta y: {1}", delta.x, delta.y);

                float v = Input.GetAxis("Vertical");
                float h = Input.GetAxis("Horizontal");
                Debug.LogFormat(" v: {0},  h: {1}", v, h);

                _go.transform.localPosition = _go.transform.localPosition + new Vector3(delta.x / 1000.0f, 0.0f, delta.y / 1000.0f);
            }
        } else if (Input.GetMouseButtonUp(0)) {
            if (_go != null) {
                _go = null;
            }
        }
#endif

#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0) {
            UnityStandardAssets.CrossPlatformInput.TouchPad
            if (Input.touches[0].phase == TouchPhase.Began) {
                
                RaycastHit hitInfo;
                if (Physics.Raycast(r, out hitInfo, 100)) {
                    _go = hitInfo.transform.gameObject;
                }
            } else if (Input.touches[0].phase == TouchPhase.Moved) {
                if (_go) {
                    Vector2 delta = Input.touches[0].deltaPosition * 10;
                    _go.transform.localPosition = _go.transform.localPosition + new Vector3(delta.x, 0, delta.y);
                }
            } else if (Input.touches[0].phase == TouchPhase.Ended) {
                if (_go) {
                    _go = null;
                }
            }
        }
#endif
    }
}
