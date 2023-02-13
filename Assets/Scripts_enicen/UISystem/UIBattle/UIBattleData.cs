using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattleData : Singleton<UIBattleData>
{
    Dictionary<int, int> m_useSkillCnt = new Dictionary<int, int>();
    
    public void UsePlayerSkill(int id) 
    {
        if (!m_useSkillCnt.ContainsKey(id)) m_useSkillCnt[id] = 0;
        m_useSkillCnt[id]++;
    }
    public float GetCostValue(int id)
    {
        PlayerSkillDataNode data = GameConfigManager.GetInstance().GetConfig<PlayerSkillDataNode>("D_PlayerSkill");
        foreach (var item in PlayerData.GetInstance().m_playerSkillCount)
        {
            if (id == item.Key && item.Value >0)
            {
                return 0;
            }
        }
        foreach (var item in data.m_map)
        {
            PlayerSkillData skilldata = (PlayerSkillData)item.Value;
            if (skilldata.skill_id == id)
            {
                if (skilldata.cost_energy.Count >= m_useSkillCnt[id])
                    return skilldata.cost_energy[m_useSkillCnt[id]];
                return skilldata.cost_energy[skilldata.cost_energy.Count - 1];
            }
        }
        return 0;
    }

    public void InitPlayerSkill() 
    {
        PlayerSkillDataNode data = GameConfigManager.GetInstance().GetConfig<PlayerSkillDataNode>("D_PlayerSkill");
        foreach (var item in data.m_map)
        {
            PlayerSkillData skilldata = (PlayerSkillData)item.Value;
            m_useSkillCnt[skilldata.skill_id] = 0;
        }
    }

    public void ClearCache()
    {
        m_useSkillCnt.Clear();
    }
}
