using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkillComponent : ConfigDataBase
{
    public SkillComponentType type = 0;
    public float trigger_time = 0;
    public float life_time = 0;
    public string param1 = "";
    public string param2 = "";
    public string param3 = "";
    public string param4 = "";
    public string param5 = "";
    public string param6 = "";
}
public class SkillComponentNode : ConfigNodeBase
{
    public SkillComponentNode(string name) : base()
    {
        m_cfgName = name;
        string[] str = GetData();
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].Length == 0 || i <= 1)
            {
                continue;
            }
            string[] values = str[i].Split(',');
            SkillComponent data = new SkillComponent();
            if (!string.IsNullOrEmpty(values[0])) data.id = int.Parse(values[0]);
            if (!string.IsNullOrEmpty(values[1])) data.type = (SkillComponentType)int.Parse(values[1]);
            if (!string.IsNullOrEmpty(values[2])) data.trigger_time = float.Parse(values[2]);
            if (!string.IsNullOrEmpty(values[3])) data.life_time = float.Parse(values[3]);
            if (!string.IsNullOrEmpty(values[4])) data.param1 = values[4];
            if (!string.IsNullOrEmpty(values[5])) data.param2 = values[5];
            if (!string.IsNullOrEmpty(values[6])) data.param3 = values[6];
            if (!string.IsNullOrEmpty(values[7])) data.param4 = values[7];
            if (!string.IsNullOrEmpty(values[8])) data.param5 = values[8];
            if (!string.IsNullOrEmpty(values[9])) data.param6 = values[9];
            m_map.Add(data.id, data);
        }
    }
    protected override string[] GetData()
    {
        List<List<string>> data = new List<List<string>>();
        string text = ResourcesManager.LoadConfig(@Application.streamingAssetsPath + "/Config/SkillShow/" + m_cfgName + ".csv");
        string[] rows = text.Replace("\n", "").Split('\r');
        return rows;
    }
}
