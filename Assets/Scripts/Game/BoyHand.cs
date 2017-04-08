using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyHand : Hand {

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
            }
        }
    }

}
