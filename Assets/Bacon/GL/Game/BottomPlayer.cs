﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bacon;
using Bacon.Game;
using Bacon.Event;
using Bacon.DataSet;
using Maria.Util;
using Bacon.GL.Game.UI;
using Bacon.GL.Common;
using Bacon.GL.Controls;

namespace Bacon.GL.Game {
    public class BottomPlayer : MonoBehaviour {

        public RootBehaviour _Root;
        public GameObject _Canvas;

        public GameObject _Gang;
        public GameObject _Peng;
        public GameObject _Hu;
        public GameObject _Guo;

        public BottomPlayerHead Head;
        public OverWnd OverWnd;

        private bool _lead = false;          // 启动lead flag
        private bool _touch = true;         // 总是true
        private GameObject _hitGo = null;
        private GameObject _selectedGo = null;
        private List<GameObject> _cards = new List<GameObject>();

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
                        GameObject hitGo = hitInfo.transform.gameObject;
                        Vector3 lastMousePostion = Input.mousePosition;
                        if (hitGo != null) {
                            _hitGo = hitGo;
                            if (_selectedGo != null && _selectedGo == _hitGo && _lead) {
                                _lead = false;
                                _hitGo = _selectedGo = null;
                                Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_LEAD, _hitGo);
                                _Root.App.Enqueue(cmd);
                            }
                        }
                    }
                } else if (Input.GetMouseButton(0)) {
                    if (_hitGo != null) {
                        UnityEngine.Debug.LogFormat(Input.mousePosition.ToString());
                        Vector3 delta = Input.mousePosition - _lastMousePostion;
                        //_hitGo.transform.localPosition = _hitGo.transform.localPosition + new Vector3(delta.x / 1000.0f, 0.0f, delta.y / 1000.0f);
                    }
                } else if (Input.GetMouseButtonUp(0)) {
                    if (_hitGo != null && _cards.Contains(_hitGo)) {
                        _selectedGo = _hitGo; // trs
                        var dest = _selectedGo.transform.localPosition + new Vector3(0.0f, 0.025f, 0.0f);
                        Matrix4x4 mat = _selectedGo.transform.worldToLocalMatrix * Matrix4x4.Translate(new Vector3( 0.0f, 0.025f, 0.0f));
                        _selectedGo.transform.right = mat.GetRow(0);
                        _selectedGo.transform.up = mat.GetRow(1);
                        _selectedGo.transform.forward = mat.GetRow(2);
                    }
                    _hitGo = null;
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
            Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_PENG);
            _Root.App.Enqueue(cmd);
        }

        public void OnGang() {
            CloseAll();
            Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_GANG);
            _Root.App.Enqueue(cmd);
        }

        public void OnHu() {
            CloseAll();
            Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_HU);
            _Root.App.Enqueue(cmd);
        }

        public void OnGuo() {
            CloseAll();
            Maria.Command cmd = new Maria.Command(MyEventCmd.EVENT_GUO);
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
}