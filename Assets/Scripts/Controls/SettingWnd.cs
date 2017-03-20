using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWnd : MonoBehaviour {

    public Slider _MusicSlider;
    public Slider _SoundSlider;
    public Toggle _MusicToggle;
    public Toggle _SoundToggle;

    private float _music = 1.0f;
    private float _sound = 1.0f;

    // Use this for initialization
    void Start() {
        SoundMgr.current.SetMusic(_music);
        SoundMgr.current.SetSound(_sound);
    }

    // Update is called once per frame
    void Update() {

    }

    public void Show() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }

    public void OnClose() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }

    public void OnMusicChanged(bool value) {
        if (value) {
            _music = _MusicSlider.maxValue;
            _MusicSlider.value = _MusicSlider.maxValue;
        } else {
            _music = _MusicSlider.minValue;
            _MusicSlider.value = _MusicSlider.minValue;
        }
        SoundMgr.current.SetMusic(_music);
    }

    public void OnSoundChanged(bool value) {
        if (value) {
            _sound = _SoundSlider.maxValue;
            _SoundSlider.value = _SoundSlider.maxValue;
        } else {
            _sound = _SoundSlider.minValue;
            _SoundSlider.value = _SoundSlider.minValue;
        }
        SoundMgr.current.SetSound(_sound);
    }

    public void OnMusicSliderChanged(float value) {
        _music = value;
        if (value == 0.0f) {
            if (_MusicToggle.isOn) {
                _MusicToggle.isOn = false;
            }
        } else {
            if (!_MusicToggle.isOn) {
                _MusicToggle.isOn = true;
            }
        }
        SoundMgr.current.SetMusic(_music);
    }

    public void OnSoundSliderChanged(float value) {
        _sound = value;
        if (value == 0.0f) {
            if (_SoundToggle.isOn) {
                _SoundToggle.isOn = false;
            }
        } else {
            if (!_SoundToggle.isOn) {
                _SoundToggle.isOn = true;
            }
        }
        SoundMgr.current.SetSound(_sound);
    }
}
