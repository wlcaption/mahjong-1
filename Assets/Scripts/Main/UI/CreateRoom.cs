using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bacon;

public class CreateRoom : MonoBehaviour {

    public RootBehaviour _Root;
    public GameObject _SCPanel;
    public GameObject _SXPanel;

    private uint _provice = Provice.Sichuan;            // 0:四川 1:陕西
    #region sichuan
    private int _hujiaozhuanyi = 0;   // 0:关闭 1:启动
    private int _zimo = 2;            // 0:不加倍 1：加底 2：自摸加倍
    private int _dianganghua = 1;     // 0:自摸 1:点炮 
    private int _daiyaojiu = 1;       // 0:平胡番数，1:4番
    private int _duanyaojiu = 0;      // 0:平胡番数，1:2番
    private int _jiangdui = 0;        // 0:4番, 1:将对8番
    private int _tiandihu = 0;        // 0:平胡番数  1:32番
    private int _top = 8;             // 默认封顶8倍
    #endregion

    #region shanxi
    private int _sxhuqidui = 1;        // 0:不可以胡七对，1可以七对不加番，2胡七对加饭
    private int _sxqiyise = 0;         // 0:清一色不加番，1清一色加番
    #endregion


    private int _ju = 8;              // 8局

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    #region sichuan
    public void OnSc(bool value) {
        if (value) {
            _provice = Provice.Sichuan;
            _SCPanel.SetActive(true);
        } else {
            _SCPanel.SetActive(false);
        }

    }

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

    #endregion

    #region shanxi

    public void OnSx(bool value) {
        if (value) {
            _provice = Provice.Shaanxi;
            _SXPanel.SetActive(true);
        } else {
            _SXPanel.SetActive(false);
        }
    }

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
    #endregion

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

    public void OnCreate() {
        Maria.Message msg = new Maria.Message();
        if (_provice == Provice.Sichuan) {
            msg[CrCode.provice] = Provice.Sichuan;
            msg[CrCode.hujiaozhuanyi] = _hujiaozhuanyi;
            msg[CrCode.zimo] = _zimo;
            msg[CrCode.dianganghua] = _dianganghua;
            msg[CrCode.daiyaojiu] = _daiyaojiu;
            msg[CrCode.duanyaojiu] = _duanyaojiu;
            msg[CrCode.jiangdui] = _jiangdui;
            msg[CrCode.tiandihu] = _tiandihu;
            msg[CrCode.top] = _top;
            msg[CrCode.ju] = _ju;
        } else if (_provice == Provice.Shaanxi) {
            msg[CrCode.provice] = Provice.Shaanxi;
            msg[CrCode.sxqidui] = _sxhuqidui;
            msg[CrCode.sxqingyise] = _sxqiyise;
            msg[CrCode.ju] = _ju;
        }
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_MUI_CREATE, gameObject, msg);
        _Root.App.Enqueue(cmd);
    }

}
