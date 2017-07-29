using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maria;
using Bacon;
using Bacon.Event;

public class SettingWnd : MonoBehaviour {

    public enum ExitType {
        EXIT_LOGIN,
        JIESHAN_ROOM,
        EXIT_ROOM,
    }

    public Slider _MusicSlider;
    public Slider _SoundSlider;
    public Toggle _MusicToggle;
    public Toggle _SoundToggle;
    public GameObject _ExitBtn;


    private float _music = 1.0f;
    private float _sound = 1.0f;

    private ExitType _et;

    // Use this for initialization
    void Start() {
        SoundMgr.current.SetMusic(_music);
        SoundMgr.current.SetSound(_sound);
    }

    // Update is called once per frame
    void Update() {

    }

    public void Show(ExitType et) {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
        _et = et;
        if (et == ExitType.EXIT_LOGIN) {
            var txt = _ExitBtn.transform.Find("Text").transform;
            txt.GetComponent<Text>().text = "退出登录";
        } else if (et == ExitType.EXIT_ROOM) {
            var txt = _ExitBtn.transform.Find("Text").transform;
            txt.GetComponent<Text>().text = "退出房间";
        } else if (et == ExitType.JIESHAN_ROOM) {
            var txt = _ExitBtn.transform.Find("Text").transform;
            txt.GetComponent<Text>().text = "解散房间";
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

    public void OnExit() {
        if (_et == ExitType.EXIT_LOGIN) {
            Command cmd = new Command(MyEventCmd.EVENT_MUI_EXITLOGIN);
            GetComponent<FindApp>().App.Enqueue(cmd);
        } else if (_et == ExitType.EXIT_ROOM) {
            Command cmd = new Command(MyEventCmd.EVENT_EXITROOM);
            GetComponent<FindApp>().App.Enqueue(cmd);
        } else if (_et == ExitType.JIESHAN_ROOM) {
            Command cmd = new Command(MyEventCmd.EVENT_EXITROOM);
            GetComponent<FindApp>().App.Enqueue(cmd);
        }
    }
}
