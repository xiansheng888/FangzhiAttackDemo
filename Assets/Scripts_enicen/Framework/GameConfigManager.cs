using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigDataBase //单行
{
    public int id;
}

public class ConfigNodeBase //整表
{

    public Dictionary<int, ConfigDataBase> m_map = new Dictionary<int, ConfigDataBase>();
    public string m_cfgName;
    public ConfigNodeBase() 
    {
    }

    protected virtual string[] GetData()
    {
        List<List<string>> data = new List<List<string>>();
        string text = ResourcesManager.LoadConfig(@Application.streamingAssetsPath + "/Config/" + m_cfgName + ".csv");
        string[] rows = text.Replace("\n", "").Split('\r');
        return rows;
    }
    
}

public class GameConfigManager : Singleton<GameConfigManager>
{
    Dictionary<string, ConfigNodeBase> m_dataMap = new Dictionary<string, ConfigNodeBase>();//系统表

    public void InitConfigData() 
    {
        ConfigNodeBase data = new SceneDataNode();
        m_dataMap.Add(data.m_cfgName, data);
        data = new ObjectDataNode();
        m_dataMap.Add(data.m_cfgName,data);
        data = new SkillDataNode();
        m_dataMap.Add(data.m_cfgName, data);
        data = new BuffDataNode();
        m_dataMap.Add(data.m_cfgName, data);
        data = new PlayerSkillDataNode();
        m_dataMap.Add(data.m_cfgName, data);
        data = new HeroFormationNode();
        m_dataMap.Add(data.m_cfgName, data);
    }

    public T GetItem<T>(string cfgname, int id) where T : ConfigDataBase
    {
        if (m_dataMap.ContainsKey(cfgname))
        {
            if (m_dataMap[cfgname].m_map.ContainsKey(id))
            {
                T data = m_dataMap[cfgname].m_map[id] as T;
                return data;
            }
        }
        return null;
    }
    public T GetConfig<T>(string cfgname) where T : ConfigNodeBase 
    {
        if (m_dataMap.ContainsKey(cfgname))
        {
            if (m_dataMap[cfgname] is T)
            {
                return (T)m_dataMap[cfgname];
            }
        }
        return null;
    }

    public SceneActionNode GetSceneActionNode(string name)
    {
        return new SceneActionNode(name);
    }

    public EntityGroupNode GetEntityGruopNode(string name) 
    {
        return new EntityGroupNode(name);
    }

    public SkillComponentNode GetSkillComponentNode(string name) 
    {
        return new SkillComponentNode(name);
    }
}
