using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : ConfigDataBase
{
    public int profession = 0; //1魔王 2近战 3远程 4塔 5无技能有血阻挡物
    public int type = 0;        //种族
    public int move_type = 0; // 1移动 4固定
    public string res_name = "";
    public List<int> skill_id = null;
    public float att_range = 0f;
    public List<int> buff_passive = null;
    public float hp = 0f;
    public float speed = 0f;
    public string idle = "";
    public string run = "";
    public string die = "";
    public string into = "";
    public int camp = 0;
    public string bullet_point = "";//子弹挂点
    public string anim_path = "";//动画管理器节点
    public float energy_cost = 0;
    public float energy_acquire = 0;
    public float mp_max = 0;
    public string icon_ui = "";
    public List<Dictionary<int, float>> drop = new List<Dictionary<int, float>>();
    public float cd_create = 0;//种怪冷却
    public int look_rotateY = 0;
    public Vector3 drag_rotate = Vector3.zero;
    public string name = "";
    public string into_audio = "";
}

public class ObjectDataNode : ConfigNodeBase
{
    public ObjectDataNode() : base()
    {
        m_cfgName = "D_ObjectBase";
        string[] str = GetData();
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].Length == 0 || i <= 1)
            {
                continue;
            }
            string[] values = str[i].Split(',');
            ObjectData data = new ObjectData();
            if (!string.IsNullOrEmpty(values[0])) data.id = int.Parse(values[0]);
            if (!string.IsNullOrEmpty(values[1])) data.profession = int.Parse(values[1]);
            if (!string.IsNullOrEmpty(values[2])) data.type = int.Parse(values[2]);
            if (!string.IsNullOrEmpty(values[3])) data.move_type = int.Parse(values[3]);
            if (!string.IsNullOrEmpty(values[4])) data.res_name = values[4];
            if (!string.IsNullOrEmpty(values[5])) 
            {
                data.skill_id = new List<int>();
                string[] tmpstrs = values[5].Split('|');
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    data.skill_id.Add(int.Parse( tmpstrs[j]));
                }
            }
            if (!string.IsNullOrEmpty(values[6])) data.att_range = float.Parse(values[6])<2f ? 2f : float.Parse(values[6]);
            if (!string.IsNullOrEmpty(values[7])) 
            {
                data.buff_passive = new List<int>();
                string[] tmpstrs = values[7].Split('|');
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    data.buff_passive.Add(int.Parse(tmpstrs[j]));
                }
            }
            if (!string.IsNullOrEmpty(values[8])) data.hp = float.Parse(values[8]);
            if (!string.IsNullOrEmpty(values[9])) data.speed = float.Parse(values[9]);
            if (!string.IsNullOrEmpty(values[10])) data.idle = values[10];
            if (!string.IsNullOrEmpty(values[11])) data.run = values[11];
            if (!string.IsNullOrEmpty(values[12])) data.die = values[12];
            if (!string.IsNullOrEmpty(values[13])) data.into = values[13];
            if (!string.IsNullOrEmpty(values[14])) data.camp = int.Parse(values[14]);
            if (!string.IsNullOrEmpty(values[15])) data.bullet_point = values[15];
            if (!string.IsNullOrEmpty(values[16])) data.anim_path = values[16];
            if (!string.IsNullOrEmpty(values[17])) data.energy_cost = float.Parse( values[17]);
            if (!string.IsNullOrEmpty(values[18])) data.energy_acquire = float.Parse( values[18]);
            if (!string.IsNullOrEmpty(values[19])) data.mp_max = float.Parse( values[19]);
            if (!string.IsNullOrEmpty(values[20])) 
            {
                string[] tmp = values[20].Split(";");
                for (int j = 0; j < tmp.Length; j++)
                {
                    data.drop.Add(new Dictionary<int, float>());
                    string[] tmpstr = tmp[j].Split('|');
                    data.drop[j].Add(int.Parse(tmpstr[0]), float.Parse(tmpstr[1]));
                }
            }

            if (!string.IsNullOrEmpty(values[21])) data.cd_create = float.Parse( values[21]);
            if (!string.IsNullOrEmpty(values[22])) data.icon_ui = values[22];
            if (!string.IsNullOrEmpty(values[23])) data.look_rotateY = int.Parse(values[23]);
            if (!string.IsNullOrEmpty(values[24])) 
            {
                string[] tmpstrs = values[24].Split('|');
                if (tmpstrs.Length > 3)
                    data.drag_rotate = new Vector3(float.Parse(tmpstrs[0]), float.Parse(tmpstrs[1]), float.Parse(tmpstrs[2]));
            }

            if (!string.IsNullOrEmpty(values[25])) data.name = values[25];
            if (!string.IsNullOrEmpty(values[26])) data.into_audio = values[26];

            m_map.Add(data.id, data);
        }
    }
}
