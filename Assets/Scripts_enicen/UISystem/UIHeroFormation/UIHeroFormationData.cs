using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeroFormationData : Singleton<UIHeroFormationData>
{
    private const int formationCnt = 2;
    public List<ObjectData> m_allHero = new List<ObjectData>();
    public UIHeroFormationData()
    {
        ObjectDataNode map = GameConfigManager.GetInstance().GetConfig<ObjectDataNode>("D_ObjectBase");
        foreach (var item in map.m_map)
        {
            ObjectData data = (ObjectData)item.Value;
            if (data.camp == 1 && data.profession != 1)
            {
                m_allHero.Add(data);
            }
        }
    }
    public List<ObjectData> GetFormations() 
    {
        return PlayerData.GetInstance().m_formationHero;
    }

    public List<SkillData> GetSkills() 
    {
        return PlayerData.GetInstance().m_formationSkill;
    }
}
