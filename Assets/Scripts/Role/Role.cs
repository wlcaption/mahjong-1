using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Role : MonoBehaviour {

    public GameObject _BUICanvas;

    private bool _rotation = false;
    private Vector2 _lastPos = Vector2.zero;
    private float _past = 0.0f;

    // Use this for initialization
    protected void Start () {
        _past = 10.0f;
	}
	
	// Update is called once per frame
	protected void Update () {
        if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject == null) {
            if (Input.touchSupported) {
                if (Input.touches[0].phase == TouchPhase.Began) {
                    _rotation = true;
                    _lastPos = Input.touches[0].position;
                } else if (Input.touches[0].phase == TouchPhase.Moved) {
                    if (_rotation) {
                        float deg = Input.touches[0].deltaPosition.x / 60;
                        transform.Rotate(Vector3.up, deg);
                    }
                } else if (Input.touches[1].phase == TouchPhase.Ended) {
                    _rotation = false;
                }
            } else {
                if (Input.GetMouseButtonDown(0)) {
                    _rotation = true;
                    _lastPos = Input.mousePosition;
                } else if (Input.GetMouseButton(0)) {
                    if (_rotation) {
                        Vector2 now = Input.mousePosition;
                        Vector2 delta = now - _lastPos;
                        float deg = delta.x / 60;
                        transform.Rotate(Vector3.up, deg);
                    }
                } else if (Input.GetMouseButtonUp(0)) {
                }
            }
        }

        _past += Time.deltaTime;
        if (_past > 12.0f) {
            _past = 0.0f;
            int r = Random.Range(1, 3);
            if (r >= 2) {
                GetComponent<Animator>().SetTrigger("Idle01");
            } else if (r >= 1) {
                GetComponent<Animator>().SetTrigger("Idle02");
            }
        }
    }
}
