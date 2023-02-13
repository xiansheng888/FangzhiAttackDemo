using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffData : ConfigDataBase
{
    public string name = "";
    public string icon = "";
    public string desc = "";
    public int icon_show = 0;
    public int type = 0;    //类型 1召唤 2修改属性 3持续修改属性 4减速
    public string param1 = "";
    public string param2 = "";
    public string param3 = "";
    public float time_duration = -1;
    public string obj_show = "";
    public string superposition = "";
    public int lock_type = 0;
    public int is_reset = 0;
    public float distance = 0;
    public int camp = 0;
}

public class BuffDataNode : ConfigNodeBase
{
    public BuffDataNode() : base()
    {
        m_cfgName = "D_Buff";
        string[] str = GetData();
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].Length == 0 || i <= 1)
            {
                continue;
            }
            string[] values = str[i].Split(',');
            BuffData data = new BuffData();
            if (!string.IsNullOrEmpty(values[0])) data.id = int.Parse(values[0]);
            if (!string.IsNullOrEmpty(values[1])) data.name = values[1];
            if (!string.IsNullOrEmpty(values[2])) data.icon = values[2];
            if (!string.IsNullOrEmpty(values[3])) data.desc = values[3];
            if (!string.IsNullOrEmpty(values[4])) data.icon_show = int.Parse(values[4]);
            if (!string.IsNullOrEmpty(values[5])) data.type = int.Parse(values[5]);
            if (!string.IsNullOrEmpty(values[6])) data.param1 = values[6];
            if (!string.IsNullOrEmpty(values[7])) data.param2 = values[7];
            if (!string.IsNullOrEmpty(values[8])) data.param3 = values[8];
            if (!string.IsNullOrEmpty(values[9])) data.time_duration = float.Parse(values[9]);
            if (!string.IsNullOrEmpty(values[10])) data.obj_show = values[10];
            if (!string.IsNullOrEmpty(values[11])) data.superposition = values[11];
            if (!string.IsNullOrEmpty(values[12])) data.lock_type = int.Parse(values[12]); 
            if (!string.IsNullOrEmpty(values[13])) data.is_reset = int.Parse(values[13]); 
            if (!string.IsNullOrEmpty(values[14])) data.distance = float.Parse(values[14]); 
            if (!string.IsNullOrEmpty(values[15])) data.camp = int.Parse(values[15]); 
            m_map.Add(data.id, data);
        }
    }
}
