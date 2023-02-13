using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSkillData : ConfigDataBase
{
    public string name = "";
    public int skill_id = 0;
    public int num = 0;
    public List<float> cost_energy = new List<float>();

}
public class PlayerSkillDataNode : ConfigNodeBase
{
    public PlayerSkillDataNode() : base()
    {
        m_cfgName = "D_PlayerSkill";
        string[] str = GetData();
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].Length == 0 || i <= 1)
            {
                continue;
            }
            string[] values = str[i].Split(',');
            PlayerSkillData data = new PlayerSkillData();
            if (!string.IsNullOrEmpty(values[0])) data.id = int.Parse(values[0]);
            if (!string.IsNullOrEmpty(values[1])) data.name = values[1];
            if (!string.IsNullOrEmpty(values[2])) data.skill_id = int.Parse(values[2]);
            if (!string.IsNullOrEmpty(values[3])) data.num = int.Parse(values[3]);
            if (!string.IsNullOrEmpty(values[4])) 
            {
                string[] tmp = values[4].Split('|');
                for (int j = 0; j < tmp.Length; j++)
                {
                    data.cost_energy.Add(float.Parse(tmp[j]));
                }
            }
            m_map.Add(data.id, data);
        }
    }
}
