using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour {

    public RootBehaviour _Root;

    private int _type = 0;            // 0:四川 1:陕西
    #region sichuan
    private int _hujiaozhuanyi = 0;   // 0:关闭 1:启动
    private int _zimo = 0;            // 0:不加倍 1：加底 2：自摸加倍
    private int _dianganghua = 0;     // 0:自摸 1:点炮 
    private int _daiyaojiu = 1;       // 带幺九4番
    private int _duanyaojiu = 1;      // 断幺九2番
    private int _jiangdui = 1;        // 将对8番
    private int _tiandihu = 1;       // 天地胡32番
    private int _top = 8;             // 默认封顶8倍
    #endregion

    #region shanxi
    private int _sxhuqidui = 0;        // 0:不可以胡七对，1可以七对不加番，2胡七对加饭
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
    public void OnSc() {
        _type = 0;
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

    public void OnSx() {
        _type = 1;
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
        msg["hujiaozhuanyi"] = _hujiaozhuanyi;
        msg["zimo"] = _zimo;
        msg["dianganghua"] = _dianganghua;
        msg["daiyaojiu"] = _daiyaojiu;
        msg["duanyaojiu"] = _duanyaojiu;
        msg["jiangdui"] = _jiangdui;
        msg["tiandihu"] = _tiandihu;
        msg["top"] = _top;
        msg["ju"] = _ju;
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_MUI_CREATE, gameObject, msg);
        _Root.App.Enqueue(cmd);
    }

}
