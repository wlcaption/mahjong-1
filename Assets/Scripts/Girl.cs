using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : MonoBehaviour {

    private Vector2 _lastPos = Vector2.zero;
    private float _past;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _past += Time.deltaTime;
        if (_past > 60) {
            _past = 0.0f;
            GetComponent<Animator>().SetBool("Idle", true);
        }
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        if (Input.GetMouseButtonDown(0)) {
            _lastPos = Input.mousePosition;
            
        } else if (Input.GetMouseButton(0)) {
            Vector2 now = Input.mousePosition;
            Vector2 delta = now - _lastPos;
            float deg = delta.x / 60;
            transform.Rotate(Vector3.up, deg);
        } else if (Input.GetMouseButtonUp(0)) {
        }
#elif UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0) {
            if (Input.touches[0].phase == TouchPhase.Began) {
                _lastPos = Input.touches[0].position;
            } else if (Input.touches[0].phase == TouchPhase.Moved) {
                float deg = Input.touches[0].deltaPosition.x / 60;
                transform.Rotate(Vector3.up, deg);
            } else if (Input.touches[1].phase == TouchPhase.Ended) {
            }
        }
#endif

    }
}
