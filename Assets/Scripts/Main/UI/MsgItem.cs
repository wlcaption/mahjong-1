using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maria;
using Bacon;

public class MsgItem : MonoBehaviour {

    public Image _Head;
    public Text _Title;
    public Text _Content;
    public Text _Date;

	// Use this for initialization
	void Start () {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1100.0f);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 80.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnView() {
        Command cmd = new Command(Bacon.MyEventCmd.EVENT_MUI_VIEWMAIL);
        GetComponent<FindApp>().App.Enqueue(cmd);
    }
}
