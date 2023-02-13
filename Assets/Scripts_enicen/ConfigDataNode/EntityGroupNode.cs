using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityGroupData : ConfigDataBase
{
    public List<float> pos = null;
    public List<float> rotate = null;
    public List<float> scale = null;
    public int object_id = 0;

}

public class EntityGroupNode :ConfigNodeBase
{
    public EntityGroupNode(string name) : base()
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
            EntityGroupData data = new EntityGroupData();
            if (!string.IsNullOrEmpty(values[0])) data.id = int.Parse(values[0]);
            if (!string.IsNullOrEmpty(values[1]))
            {
                string[] tmpstrs = values[1].Split('|');
                data.pos = new List<float>();
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    data.pos.Add(float.Parse(tmpstrs[j]));
                }

            }
            if (!string.IsNullOrEmpty(values[2]))
            {
                string[] tmpstrs = values[2].Split('|');
                data.rotate = new List<float>();
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    data.rotate.Add(float.Parse(tmpstrs[j]));
                }

            }if (!string.IsNullOrEmpty(values[3]))
            {
                string[] tmpstrs = values[3].Split('|');
                data.scale = new List<float>();
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    data.scale.Add(float.Parse(tmpstrs[j]));
                }

            }
            if (!string.IsNullOrEmpty(values[4])) data.object_id = int.Parse(values[4]);
            m_map.Add(data.id, data);
        }
    }
    protected override string[] GetData()
    {
        List<List<string>> data = new List<List<string>>();
        string text = ResourcesManager.LoadConfig(@Application.streamingAssetsPath + "/Config/EntityGroup/" + m_cfgName + ".csv");
        string[] rows = text.Replace("\n", "").Split('\r');
        return rows;
    }
}
