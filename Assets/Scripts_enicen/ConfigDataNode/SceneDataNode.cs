using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : ConfigDataBase
{
    public int scene_type = 0;
    public string scene_name = "";
    public string action_group = "";
    public Vector3 camera_pos ;
    public Vector3 camera_pos_preview ;
    public Vector3 mowang_target;
}
public class SceneDataNode : ConfigNodeBase
{
    public SceneDataNode():base()
    {
        m_cfgName = "D_GameLevel";
        string[] str = GetData();
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i].Length == 0 || i<=1)
            {
                continue;
            }
            string[] values = str[i].Split(',');
            SceneData data = new SceneData();
            if (!string.IsNullOrEmpty(values[0]))data.id = int.Parse(values[0]);
            if (!string.IsNullOrEmpty(values[1])) data.scene_type = int.Parse(values[1]);
            if (!string.IsNullOrEmpty(values[2])) data.scene_name = values[2];
            if (!string.IsNullOrEmpty(values[3])) data.action_group = values[3];
            if (!string.IsNullOrEmpty(values[4])) 
            {
                string[] tmpstrs = values[4].Split('|');
                List<float> tmpflot = new List<float>();
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    tmpflot.Add(float.Parse(tmpstrs[j]));
                }
                data.camera_pos = new Vector3(tmpflot[0], tmpflot[1], tmpflot[2]);
            }
            if (!string.IsNullOrEmpty(values[5])) 
            {
                string[] tmpstrs = values[5].Split('|');
                List<float> tmpflot = new List<float>();
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    tmpflot.Add(float.Parse(tmpstrs[j]));
                }
                data.camera_pos_preview = new Vector3(tmpflot[0], tmpflot[1], tmpflot[2]);
            }
            if (!string.IsNullOrEmpty(values[6])) 
            {
                string[] tmpstrs = values[6].Split('|');
                List<float> tmpflot = new List<float>();
                for (int j = 0; j < tmpstrs.Length; j++)
                {
                    tmpflot.Add(float.Parse(tmpstrs[j]));
                }
                data.mowang_target = new Vector3(tmpflot[0], tmpflot[1], tmpflot[2]);
            }
            m_map.Add(data.id, data);
        }
    }
}
