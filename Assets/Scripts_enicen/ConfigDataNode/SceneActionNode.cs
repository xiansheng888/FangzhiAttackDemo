using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ActionData : ConfigDataBase
{
    public string action_type = "";
    public int action_head = 0;
    public List<int> action_next = null;
    public string param1 = "";
    public string param2 = "";
    public string param3 = "";
    public int battle_execute = 0;//选角完 进入战斗执行
}
public class SceneActionNode : ConfigNodeBase
{
    public SceneActionNode(string name):base()
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
            ActionData data = new ActionData();
            if (!string.IsNullOrEmpty(values[0])) data.id = int.Parse(values[0]);
            if (!string.IsNullOrEmpty(values[1])) data.action_type = values[1];
            if (!string.IsNullOrEmpty(values[2])) data.action_head = int.Parse(values[2]);
            if (!string.IsNullOrEmpty(values[3])) 
            {
                string[] action_next_strs = values[3].Split('|');
                data.action_next = new List<int>();
                for (int j = 0; j < action_next_strs.Length; j++)
                {
                    data.action_next.Add(int.Parse(action_next_strs[j]));
                }

            }
            if (!string.IsNullOrEmpty(values[4])) data.param1 = values[4];
            if (!string.IsNullOrEmpty(values[5])) data.param2 = values[5];
            if (!string.IsNullOrEmpty(values[6])) data.param3 = values[6];
            if (!string.IsNullOrEmpty(values[7])) data.battle_execute = int.Parse(values[7]);

            m_map.Add(data.id, data);
        }
    }
    protected override string[] GetData()
    {
        List<List<string>> data = new List<List<string>>();
        string text = ResourcesManager.LoadConfig(@Application.streamingAssetsPath + "/Config/SceneActions/" + m_cfgName + ".csv");
        string[] rows = text.Replace("\n", "").Split('\r');
        return rows;
    }
}
