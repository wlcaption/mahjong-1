using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataItem : ITableItem {
    public int key;
    public string value;

    public int Key() {
        return key;
    }
}

public class DataConfig : TableManager<DataItem, DataConfig> {

    public override string TableName() {
        return "data";
    }
}
