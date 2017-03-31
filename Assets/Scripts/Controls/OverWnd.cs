using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maria;
using Bacon;
using UnityEngine.UI;

public class OverWnd : MonoBehaviour {

    public GameObject _Bottom;
    public GameObject _Left;
    public GameObject _Top;
    public GameObject _Right;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnNext() {
        Close();
        Command cmd = new Command(MyEventCmd.EVENT_RESTART);
        GetComponent<FindApp>().App.Enqueue(cmd);
    }

    public void Show() {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }

    public void Close() {
        if (gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }

    public void SettleBottom(List<SettlementItem> li) {
        GameObject label = ABLoader.current.LoadRes<GameObject>("Prefabs/UI/OverLable");
        Transform content = _Bottom.transform.FindChild("Content");
        long chip = 0;
        for (int i = 0; i < li.Count; i++) {
            string tips = string.Empty;
            if (li[i].Gang != OpCodes.OPCODE_NONE) {
                if (li[i].Gang == OpCodes.OPCODE_BUGANG) {
                    tips += "补杠";
                    tips += string.Format("{0}", li[i].Chip);
                } else if (li[i].Gang == OpCodes.OPCODE_ANGANG) {
                    tips += "暗杠";
                    tips += string.Format("{0}", li[i].Chip);
                } else if (li[i].Gang == OpCodes.OPCODE_ZHIGANG) {
                    tips += "直杠";
                    tips += string.Format("{0}", li[i].Chip);
                }
                GameObject label1 = Instantiate<GameObject>(label);
                label1.GetComponent<Text>().text = tips;
                label1.transform.SetParent(content);
            } else {
                UnityEngine.Debug.Assert(li[i].HuCode != HuType.NONE);
                tips += HutypLangConfig.Instance.GetItem((int)li[i].HuCode).ch;
                tips += string.Format("{0}", li[i].Chip);
                GameObject label1 = Instantiate<GameObject>(label);
                label1.GetComponent<Text>().text = tips;
                label1.transform.SetParent(content);
            }
        }
    }

    public void SettleLeft(List<SettlementItem> li) {
        GameObject label = ABLoader.current.LoadRes<GameObject>("Prefabs/UI/OverLable");
        long chip = 0;
        for (int i = 0; i < li.Count; i++) {
            chip += li[i].Chip;
        }
        GameObject label1 = Instantiate<GameObject>(label);
        Transform content = _Left.transform.FindChild("Content");
        label1.transform.SetParent(content);
    }

    public void SettleTop(List<SettlementItem> li) {
        GameObject label = ABLoader.current.LoadRes<GameObject>("Prefabs/UI/OverLable");
        long chip = 0;
        for (int i = 0; i < li.Count; i++) {
            chip += li[i].Chip;
        }
        GameObject label1 = Instantiate<GameObject>(label);
        Transform content = _Top.transform.FindChild("Content");
        label1.transform.SetParent(content);
    }

    public void SettleRight(List<SettlementItem> li) {
        GameObject label = ABLoader.current.LoadRes<GameObject>("Prefabs/UI/OverLable");
        long chip = 0;
        for (int i = 0; i < li.Count; i++) {
            chip += li[i].Chip;
        }
        GameObject label1 = Instantiate<GameObject>(label);
        Transform content = _Right.transform.FindChild("Content");
        label1.transform.SetParent(content);
    }


}
