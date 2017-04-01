using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRScMahjong : MonoBehaviour {

    private int _hujiaozhuanyi = 0;   // 0:关闭 1:启动
    private int _zimo = 2;            // 0:不加倍 1：加底 2：自摸加倍
    private int _dianganghua = 1;     // 0:自摸 1:点炮 
    private int _daiyaojiu = 1;       // 0:平胡番数，1:4番
    private int _duanyaojiu = 0;      // 0:平胡番数，1:2番
    private int _jiangdui = 0;        // 0:4番, 1:将对8番
    private int _tiandihu = 0;        // 0:平胡番数  1:32番
    private int _top = 8;             // 默认封顶8倍
    private int _ju = 8;              // 8局

    // Use this for initialization
    void Start() {
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
        } else {
            _hujiaozhuanyi = 0;
        }
    }

    public void OnZimobujiabeiChanged(bool value) {
        if (value) {
            _zimo = 0;
        }
    }

    public void OnZimojiadiChanged(bool value) {
        if (value) {
            _zimo = 1;
        }
    }

    public void OnZimojiabeiChanged(bool value) {
        if (value) {
            _zimo = 2;
        }
    }

    public void OnDianganghuazimoChanged(bool value) {
        if (value) {
            _dianganghua = 0;
        }
    }

    public void OnDianganghuadianpaoChanged(bool value) {
        if (value) {
            _dianganghua = 1;
        }
    }

    public void OnDaiyaojiuChanged(bool value) {
        if (value) {
            _daiyaojiu = 4;
        } else {
            _daiyaojiu = 1;
        }
    }

    public void OnDuanyaojiuChanged(bool value) {
        if (value) {
            _duanyaojiu = 2;
        } else {
            _duanyaojiu = 1;
        }
    }

    public void OnJiangduiChanged(bool value) {
        if (value) {
            _jiangdui = 8;
        } else {
            _jiangdui = 1;
        }
    }

    public void OnTiandihuChanged(bool value) {
        if (value) {
            _tiandihu = 32;
        } else {
            _tiandihu = 4;
        }
    }

    public void OnMulti8Changed(bool value) {
        if (value) {
            _top = 8;
        }
    }

    public void OnMulti16Changed(bool value) {
        if (value) {
            _top = 16;
        }
    }

    public void OnMulti32Changed(bool value) {
        if (value) {
            _top = 32;
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
