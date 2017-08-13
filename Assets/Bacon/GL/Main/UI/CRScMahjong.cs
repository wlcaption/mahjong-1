using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bacon.GL.Main.UI { 
public class CRScMahjong : MonoBehaviour {

    public GameObject _HuJiaoZhuanYi;
    public GameObject _ZiMoBuJiaBei;
    public GameObject _ZiMoJiaDi;
    public GameObject _ZiMoJiaBei;
    public GameObject _DianGangHuaZiMo;
    public GameObject _DianGangHuaDianPao;

    public GameObject _DaiYaoJiux4;
    public GameObject _DuanYaoJiux2;
    public GameObject _JiangDuix8;
    public GameObject _TianDiHux32;
    public GameObject _Top8;
    public GameObject _Top16;
    public GameObject _Top32;

    public GameObject _Ju8;
    public GameObject _Ju16;

    public GameObject _HuJiaoZhuanYiLable;
    public GameObject _ZiMoBuJiaBeiLable;
    public GameObject _ZiMoJiaDiLable;
    public GameObject _ZiMoJiaBeiLabel;
    public GameObject _DianGangHuaZiMoLable;
    public GameObject _DianGangHuaDianPaoLable;

    public GameObject _DaiYaoJiux4Lable;
    public GameObject _DuanYaoJiux2Lable;
    public GameObject _JiangDuix8Lable;
    public GameObject _TianDiHux32Lable;

    public GameObject _Top8Lable;
    public GameObject _Top16Lable;
    public GameObject _Top32Lable;

    public GameObject _Ju8Lable;
    public GameObject _Ju16Lable;

    private int _hujiaozhuanyi = 0;   // 0:关闭 1:启动
    private int _zimo = 2;            // 0:不加倍 1：加底 2：自摸加倍
    private int _dianganghua = 1;     // 0:自摸 1:点炮 
    private int _daiyaojiu = 1;       // 0:平胡番数，1:4番
    private int _duanyaojiu = 0;      // 0:平胡番数，1:2番
    private int _jiangdui = 0;        // 0:4番, 1:将对8番
    private int _tiandihu = 0;        // 0:平胡番数  1:32番
    private int _top = 8;             // 默认封顶8倍
    private int _ju = 8;              // 8局

    private readonly Color _normal = new Color(123.0f / 255.0f, 87.0f / 255.0f, 9.0f / 255.0f);
    private readonly Color _pressed = new Color(168.0f / 255.0f, 39.0f / 255.0f, 7.0f / 255.0f);

    // Use this for initialization
    void Start() {
        _HuJiaoZhuanYi.SetActive(false);
        _HuJiaoZhuanYi.GetComponent<Toggle>().isOn = true;
        _HuJiaoZhuanYi.SetActive(true);

        _ZiMoBuJiaBei.GetComponent<Toggle>().isOn = true;
        _ZiMoJiaDi.GetComponent<Toggle>().isOn = false;
        _ZiMoJiaBei.GetComponent<Toggle>().isOn = false;

        _DianGangHuaZiMo.GetComponent<Toggle>().isOn = false;
        _DianGangHuaDianPao.GetComponent<Toggle>().isOn = true;

        _DaiYaoJiux4.GetComponent<Toggle>().isOn = true;
        _DuanYaoJiux2.GetComponent<Toggle>().isOn = false;
        _JiangDuix8.GetComponent<Toggle>().isOn = false;
        _TianDiHux32.GetComponent<Toggle>().isOn = false;

        _Top8.GetComponent<Toggle>().isOn = true;
        _Top16.GetComponent<Toggle>().isOn = false;
        _Top32.GetComponent<Toggle>().isOn = false;

        _Ju8.GetComponent<Toggle>().isOn = true;
        _Ju16.GetComponent<Toggle>().isOn = false;
    }

    // Update is called once per frame
    void Update() {
    }

    public int HuJiaoZhuanYi { get { return _hujiaozhuanyi; } }
    public int ZiMo { get { return _zimo; } }
    public int DianGangHua { get { return _dianganghua; } }
    public int DaiYaoJiu { get { return _daiyaojiu; } }
    public int DuanYaoJiu { get { return _duanyaojiu; } }
    public int JiangDui { get { return _jiangdui; } }
    public int TianDiHU { get { return _tiandihu; } }
    public int Top { get { return _top; } }
    public int Ju { get { return _ju; } }

    public void OnHujiaozhuanyiChanged(bool value) {
        if (value) {
            _hujiaozhuanyi = 1;
            _HuJiaoZhuanYiLable.GetComponent<Text>().color = _pressed;
        } else {
            _hujiaozhuanyi = 0;
            _HuJiaoZhuanYiLable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnZimobujiabeiChanged(bool value) {
        if (value) {
            _zimo = 0;
            _ZiMoBuJiaBeiLable.GetComponent<Text>().color = _pressed;
        } else {
            _ZiMoBuJiaBeiLable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnZimojiadiChanged(bool value) {
        if (value) {
            _zimo = 1;
            _ZiMoJiaDiLable.GetComponent<Text>().color = _pressed;
        } else {
            _ZiMoJiaDiLable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnZimojiabeiChanged(bool value) {
        if (value) {
            _zimo = 2;
            _ZiMoJiaBeiLabel.GetComponent<Text>().color = _pressed;
        } else {
            _ZiMoJiaBeiLabel.GetComponent<Text>().color = _normal;
        }
    }

    public void OnDianganghuazimoChanged(bool value) {
        if (value) {
            _dianganghua = 0;
            _DianGangHuaZiMoLable.GetComponent<Text>().color = _pressed;
        } else {
            _DianGangHuaZiMoLable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnDianganghuadianpaoChanged(bool value) {
        if (value) {
            _dianganghua = 1;
            _DianGangHuaDianPaoLable.GetComponent<Text>().color = _pressed;
        } else {
            _DianGangHuaDianPaoLable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnDaiyaojiuChanged(bool value) {
        if (value) {
            _daiyaojiu = 4;
            _DaiYaoJiux4Lable.GetComponent<Text>().color = _pressed;
        } else {
            _daiyaojiu = 1;
            _DaiYaoJiux4Lable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnDuanyaojiuChanged(bool value) {
        if (value) {
            _duanyaojiu = 2;
            _DuanYaoJiux2Lable.GetComponent<Text>().color = _pressed;
        } else {
            _duanyaojiu = 1;
            _DuanYaoJiux2Lable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnJiangduiChanged(bool value) {
        if (value) {
            _jiangdui = 8;
            _JiangDuix8Lable.GetComponent<Text>().color = _pressed;
        } else {
            _jiangdui = 1;
            _JiangDuix8Lable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnTiandihuChanged(bool value) {
        if (value) {
            _tiandihu = 32;
            _TianDiHux32Lable.GetComponent<Text>().color = _pressed;
        } else {
            _tiandihu = 4;
            _TianDiHux32Lable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnMulti8Changed(bool value) {
        if (value) {
            _top = 8;
            _Top8Lable.GetComponent<Text>().color = _pressed;
        } else {
            _Top8Lable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnMulti16Changed(bool value) {
        if (value) {
            _top = 16;
            _Top16Lable.GetComponent<Text>().color = _pressed;
        } else {
            _Top16Lable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnMulti32Changed(bool value) {
        if (value) {
            _top = 32;
            _Top32Lable.GetComponent<Text>().color = _pressed;
        } else {
            _Top32Lable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnJu8Changed(bool value) {
        if (value) {
            _ju = 8;
            _Ju8Lable.GetComponent<Text>().color = _pressed;
        } else {
            _Ju8Lable.GetComponent<Text>().color = _normal;
        }
    }

    public void OnJu16Changed(bool value) {
        if (value) {
            _ju = 16;
            _Ju16Lable.GetComponent<Text>().color = _pressed;
        } else {
            _Ju16Lable.GetComponent<Text>().color = _normal;
        }
    }
}
}