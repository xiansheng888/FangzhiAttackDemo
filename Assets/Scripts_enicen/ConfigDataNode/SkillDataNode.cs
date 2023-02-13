using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ConfigDataBase
{
    public string show_path = "";
    public float time = 0;
    public float att_timestamp = 0;
    public float cd = 0;
    public string att_effect = "";
    public string show_icon = "";
    public float att_value = 0;
    public float att_interval = 0;
    public List<float> add_prob = new List<float>();//给目标加
    public List<int> add_id = new List<int>();
    public List<float> add_prob_self = new List<float>();//给自己加
    public List<int> add_id_self = new List<int>();
    public List<int> target_type = new List<int>();//目标类型
    public float scope = 0; //溅射范围
    public float mp_cost = 0;
    public string skill_aim = "";
    public float scope_buttle = 0;
}

public class SkillDataNode : ConfigNodeBase
{
    public SkillDataNode() : base()
    {
        m_cfgName = "D_Skill";
        string[] str = GetData();
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].Length == 0 || i <= 1)
            {
                continue;
            }
            string[] values = str[i].Split(',');
            SkillData data = new SkillData();
            if (!string.IsNullOrEmpty(values[0])) data.id = int.Parse(values[0]);
            if (!string.IsNullOrEmpty(values[1])) data.show_path = values[1];
            if (!string.IsNullOrEmpty(values[2])) data.time = float.Parse(values[2]);
            if (!string.IsNullOrEmpty(values[3])) data.att_timestamp = float.Parse( values[3]);
            if (!string.IsNullOrEmpty(values[4])) data.cd = float.Parse( values[4]);
            if (!string.IsNullOrEmpty(values[5])) data.att_effect = values[5];
            if (!string.IsNullOrEmpty(values[6])) data.show_icon = values[6];
            if (!string.IsNullOrEmpty(values[7])) data.att_value = float.Parse(values[7]);
            if (!string.IsNullOrEmpty(values[8])) data.att_interval = float.Parse(values[8]);
            if (!string.IsNullOrEmpty(values[9]))
            {
                string[] tmpstrs = values[9].Split('|');
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    data.add_prob.Add(float.Parse(tmpstrs[j]));
                }
            }
            if (!string.IsNullOrEmpty(values[10]))
            {
                string[] tmpstrs = values[10].Split('|');
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    data.add_id.Add(int.Parse(tmpstrs[j]));
                }
            }
            if (!string.IsNullOrEmpty(values[11]))
            {
                string[] tmpstrs = values[11].Split('|');
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    data.add_prob.Add(float.Parse(tmpstrs[j]));
                }
            }
            if (!string.IsNullOrEmpty(values[12]))
            {
                string[] tmpstrs = values[12].Split('|');
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    data.add_id.Add(int.Parse(tmpstrs[j]));
                }
            }
            if (!string.IsNullOrEmpty(values[13]))
            {
                string[] tmpstrs = values[13].Split('|');
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    data.target_type.Add(int.Parse(tmpstrs[j]));
                }
            }
            if (!string.IsNullOrEmpty(values[14])) data.scope = float.Parse(values[14]);
            if (!string.IsNullOrEmpty(values[15])) data.mp_cost = float.Parse(values[15]);
            if (!string.IsNullOrEmpty(values[16])) data.skill_aim = values[16];
            if (!string.IsNullOrEmpty(values[17])) data.scope_buttle = float.Parse(values[17]);

            m_map.Add(data.id, data);
        }
    }
}
