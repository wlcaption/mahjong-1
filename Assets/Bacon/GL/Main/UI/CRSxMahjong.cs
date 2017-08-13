using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bacon.GL.Main.UI { 
public class CRSxMahjong : MonoBehaviour {

    private int _sxhuqidui = 1;        // 0:不可以胡七对，1可以七对不加番，2胡七对加饭
    private int _sxqiyise = 0;         // 0:清一色不加番，1清一色加番
    private int _ju = 8;              // 8局

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public int SxHuQiDui { get { return _sxhuqidui; } }
    public int SxQingYiSe { get { return _sxqiyise; } }
    public int Ju { get { return _ju; } }

    public void OnBukehuqiduiChanged(bool value) {
        if (value) {
            _sxhuqidui = 0;
        }
    }

    public void OnHuqiduijiafanChanged(bool value) {
        if (value) {
            _sxhuqidui = 2;
        }
    }

    public void OnHuqiduibujiafanChanged(bool value) {
        if (value) {
            _sxhuqidui = 1;
        }
    }

    public void OnQingyisejiafanChanged(bool value) {
        if (value) {
            _sxqiyise = 1;
        } else {
            _sxqiyise = 0;
        }
    }

    public void OnJu8Changed(bool value) {
        if (value) {
            _ju = 8;
        }
    }

    public void OnJu16Changed(bool value) {
        if (value) {
            _ju = 16;
        }
    }
}
}