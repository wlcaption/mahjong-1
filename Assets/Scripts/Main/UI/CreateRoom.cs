using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bacon;

public class CreateRoom : MonoBehaviour {

    public GameObject _SCPanel;
    public GameObject _SXPanel;
    public GameObject _RCard;

    private int _provice = Provice.Sichuan;            // 0:四川 1:陕西
    private int _overtype = OverType.XUELIU;


    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    public void Show(int num) {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
        _RCard.GetComponent<Text>().text = string.Format("已有房卡{0}张", num);
    }

    public void OnClose() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }

    public void OnScXL(bool value) {
        if (value) {
            _provice = Provice.Sichuan;
            _overtype = OverType.XUELIU;
            _SCPanel.SetActive(true);
        } else {
            _SCPanel.SetActive(false);
        }

    }

    public void OnScXZ(bool value) {
        if (value) {
            _provice = Provice.Sichuan;
            _overtype = OverType.XUELIU;
            _SCPanel.SetActive(true);
        } else {
            _SCPanel.SetActive(false);
        }
    }

    public void OnSx(bool value) {
        if (value) {
            _provice = Provice.Shaanxi;
            _SXPanel.SetActive(true);
        } else {
            _SXPanel.SetActive(false);
        }
    }

    public void OnCreate() {
        Maria.Message msg = new Maria.Message();
        if (_provice == Provice.Sichuan) {
            var com = _SCPanel.GetComponent<CRScMahjong>();
            msg[CrCode.provice] = Provice.Sichuan;
            msg[CrCode.hujiaozhuanyi] = com.HuJiaoZhuanYi;
            msg[CrCode.zimo] = com.ZiMo;
            msg[CrCode.dianganghua] = com.DianGangHua;
            msg[CrCode.daiyaojiu] = com.DaiYaoJiu;
            msg[CrCode.duanyaojiu] = com.DuanYaoJiu;
            msg[CrCode.jiangdui] = com.JiangDui;
            msg[CrCode.tiandihu] = com.TianDiHU;
            msg[CrCode.top] = com.Top;
            msg[CrCode.ju] = com.Ju;
            msg[CrCode.overtype] = _overtype;
        } else if (_provice == Provice.Shaanxi) {
            var com = _SXPanel.GetComponent<CRSxMahjong>();
            msg[CrCode.provice] = Provice.Shaanxi;
            msg[CrCode.sxqidui] = com.SxHuQiDui;
            msg[CrCode.sxqingyise] = com.SxQingYiSe;
            msg[CrCode.ju] = com.Ju;
            msg[CrCode.overtype] = _overtype;
        }
        Maria.Command cmd = new Maria.Command(Bacon.MyEventCmd.EVENT_MUI_CREATE, gameObject, msg);
        GetComponent<FindApp>().App.Enqueue(cmd);
    }

}
