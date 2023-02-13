using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFormationData : ConfigDataBase
{
    public string name = "";
    public int hero_id = 0;
    public int default_num = 0;
    public int default_select = 0;
}
public class HeroFormationNode : ConfigNodeBase
{
    public HeroFormationNode() : base()
    {
        m_cfgName = "D_HeroFormation";
        string[] str = GetData();
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].Length == 0 || i <= 1)
            {
                continue;
            }
            string[] values = str[i].Split(',');
            HeroFormationData data = new HeroFormationData();
            if (!string.IsNullOrEmpty(values[0])) data.id = int.Parse(values[0]);
            if (!string.IsNullOrEmpty(values[1])) data.name = values[1];
            if (!string.IsNullOrEmpty(values[2])) data.hero_id = int.Parse(values[2]);
            if (!string.IsNullOrEmpty(values[3])) data.default_num = int.Parse(values[3]);
            if (!string.IsNullOrEmpty(values[4])) data.default_select = int.Parse(values[4]);
            m_map.Add(data.id, data);
        }
    }
}
