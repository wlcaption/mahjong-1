using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HutypeLangItem : ITableItem {
    public int id;
    public string ch;

    public int Key() {
        return id;
    }
}

public class HutypLangConfig : TableManager<HutypeLangItem, HutypLangConfig> {
    public override string TableName() {
        return "hutype_lang";
    }
}
