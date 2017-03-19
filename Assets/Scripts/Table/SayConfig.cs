using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maria;

public class SayItem : ITableItem {
    public int id;
    public int code;
    public string text;
    public string sound;

    public int Key() {
        return id;
    }
}

public class SayConfig : TableManager<SayItem, SayConfig> {

    public override string TableName() {
        return "say";
    }
}

