﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bacon;

public class BottomPlayer : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject _Canvas;
    
    public GameObject _Gang;
    public GameObject _Peng;
    public GameObject _Hu;
    public GameObject _Guo;

    public BottomPlayerHead Head;
    public OverWnd OverWnd;

    private List<GameObject> _cards = new List<GameObject>();

    private bool _touch = false;
    private GameObject _hitGo = null;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    private Vector3 _lastMousePostion;
#elif UNITY_IOS || UNITY_ANDROID
#endif

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (_touch) {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            if (Input.GetMouseButtonDown(0)) {
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(r, out hitInfo, 20)) {
                    _hitGo = hitInfo.transform.gameObject;
                    _lastMousePostion = Input.mousePosition;
                }
            } else if (Input.GetMouseButton(0)) {
                if (_hitGo != null) {
                    Vector3 delta = Input.mousePosition - _lastMousePostion;
                    //_hitGo.transform.localPosition = _hitGo.transform.localPosition + new Vector3(delta.x / 1000.0f, 0.0f, delta.y / 1000.0f);
                }
            } else if (Input.GetMouseButtonUp(0)) {
                if (_hitGo != null) {
                    bool flag = false;
                    for (int i = 0; i < _cards.Count; i++) {
                        if (_cards[i] == _hitGo) {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag) {
                        if (HoldCard == _hitGo) {
                            flag = true;
                        }
                    }
                    if (flag) {
                        _touch = false;
                        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_LEAD, _hitGo);
                        _Root.App.Enqueue(cmd);
                    }
                    _hitGo = null;
                }
            }
#elif UNITY_IOS || UNITY_ANDROID
                       if (Input.touchCount > 0) {
                if (Input.touches[0].phase == TouchPhase.Began) {
                    Ray r = Camera.main.ScreenPointToRay(Input.touches[0].position);
                    RaycastHit hitInfo;
                    Physics.Raycast(r, out hitInfo, 50);


                    //hitInfo.transform.gameObject;

                }
            }
#endif
        }
    }

    public void Init() {
        Head.Init();
    }

    public GameObject HoldCard { get; set; }

    public void CloseAll() {
        _Gang.SetActive(false);
        _Peng.SetActive(false);
        _Hu.SetActive(false);
        _Guo.SetActive(false);
    }

    public void OnPeng() {
        CloseAll();
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_PENG);
        _Root.App.Enqueue(cmd);
    }

    public void OnGang() {
        CloseAll();
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_GANG);
        _Root.App.Enqueue(cmd);
    }

    public void OnHu() {
        CloseAll();
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_HU);
        _Root.App.Enqueue(cmd);
    }

    public void OnGuo() {
        CloseAll();
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_GUO);
        _Root.App.Enqueue(cmd);
    }

    public void ShowPeng() {
        _Peng.SetActive(true);
    }

    public void ShowGang() {
        _Gang.SetActive(true);
    }

    public void ShowHu() {
        _Hu.SetActive(true);
    }

    public void ShowGuo() {
        _Guo.SetActive(true);
    }

    public void SwitchOnTouch() {
        _touch = true;
    }

    public void SwitchOffTouch() {
        _touch = false;
    }

    public void Add(Card card) {
        _cards.Add(card.Go);
    }

    public void Remove(Card card) {
        _cards.Remove(card.Go);
    }

    public void ShowUI() {
        Head.Show();
    }
    public void HideUI() {
        Head.Close();
    }

    public void Say(long code) {
        SayItem item = SayConfig.Instance.GetItem((int)code);
        Head.SetSay(item.text);
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
