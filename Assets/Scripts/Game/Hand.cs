using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public enum EVENT {
        CHUPAI_COMPLETED,
        DIUSHAIZI_COMPLETED,
        NAPAI_COMPLETED,
        FANGPAI_COMPLETED,
        HUPAI_COMPLETED,
        PENGGANG_COMPLETED,
    }

    protected Animator _animator;
    protected Dictionary<EVENT, Action> _callback = new Dictionary<EVENT, Action>();

    // Use this for initialization
    void Start() {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (_animator != null) {
            AnimatorStateInfo si = _animator.GetCurrentAnimatorStateInfo(0);
            if (si.IsName("Base Layer.Chupai")) {
                _animator.SetBool("Chupai", false);
            } else if (si.IsName("Base Layer.Chutuipai")) {
                _animator.SetBool("Chutuipai", false);
            } else if (si.IsName("Base Layer.Diushaizi")) {
                _animator.SetBool("Diushaizi", false);
            } else if (si.IsName("Base Layer.Hupai")) {
                _animator.SetBool("Hupai", false);
            } else if (si.IsName("Base Layer.Napai")) {
                _animator.SetBool("Napai", false);
            } else if (si.IsName("Base Layer.Fangpai")) {
                _animator.SetBool("Fangpai", false);
            } else if (si.IsName("Base Layer.Penggang")) {
                _animator.SetBool("Penggang", false);
            } else if (si.IsName("Base Layer.Idle")) {
                _animator.SetBool("Idle", false);
            }
        }
    }

    public void Rigster(EVENT name, Action cb) {
        _callback.Add(name, cb);
    }

    protected void OnChupaiCompleted() {
        if (_callback.ContainsKey(EVENT.CHUPAI_COMPLETED)) {
            var cb = _callback[EVENT.CHUPAI_COMPLETED];
            cb();
        }
        _callback.Clear();
    }

    protected void OnDiushaiziCompleted() {
        if (_callback.ContainsKey(EVENT.DIUSHAIZI_COMPLETED)) {
            var cb = _callback[EVENT.DIUSHAIZI_COMPLETED];
            cb();
        }
        _callback.Clear();
    }

    protected void OnNapaiCompleted() {
        if (_callback.ContainsKey(EVENT.NAPAI_COMPLETED)) {
            var cb = _callback[EVENT.NAPAI_COMPLETED];
            cb();
        }
        _callback.Clear();
    }

    protected void OnFangpaiCompleted() {
        if (_callback.ContainsKey(EVENT.FANGPAI_COMPLETED)) {
            var cb = _callback[EVENT.FANGPAI_COMPLETED];
            cb();
        }
        _callback.Clear();
    }

    protected void OnHupaiCompleted() {
        if (_callback.ContainsKey(EVENT.HUPAI_COMPLETED)) {
            var cb = _callback[EVENT.HUPAI_COMPLETED];
            cb();
        }
        _callback.Clear();
    }

    protected void OnPengGangCompleted() {
        if (_callback.ContainsKey(EVENT.PENGGANG_COMPLETED)) {
            var cb = _callback[EVENT.PENGGANG_COMPLETED];
            cb();
        }
        _callback.Clear();
    }
}
