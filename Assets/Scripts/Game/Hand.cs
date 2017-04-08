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
    }

    protected Animator _animator;
    protected Dictionary<EVENT, Action> _callback = new Dictionary<EVENT, Action>();

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

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
}
