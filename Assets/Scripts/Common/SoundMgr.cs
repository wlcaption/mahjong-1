using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour {

    public static SoundMgr current = null;

    private float _music = 1.0f;
    private float _sound = 1.0f;
    private AudioClip _musicClip;
    private GameObject _soundGo;

    void Awake() {
        if (current == null) {
            current = this;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMusic(float value) {
        if (_music > 0.0f) {
            if (value == 0.0f) {
                _music = value;
                GetComponent<AudioSource>().Stop();
            } else {
                _music = value;
                //GetComponent<AudioSource>().
            }
        } else {
            _music = value;
        }
    }

    public void SetSound(float value) {
        if (_sound > 0.0f) {
            if (value == 0.0f) {
                _sound = value;
                _soundGo.GetComponent<AudioSource>().Stop();
            }
        } else {
            _sound = value;
        }
    }


    public void PlayMusic(AudioClip clip) {
        _musicClip = clip;
        gameObject.GetComponent<AudioSource>().clip = clip;
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void PlaySound(GameObject go) {
        go.GetComponent<AudioSource>().Play();
    }

}
