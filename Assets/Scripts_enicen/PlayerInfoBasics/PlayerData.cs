using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    public Dictionary<int, SkillEntity> m_playerskill = new Dictionary<int, SkillEntity>();
    public List<ObjectData> m_formationHero = new List<ObjectData>();
    public List<SkillData> m_formationSkill = new List<SkillData>();

    public Dictionary<int, int> m_heroCreateCount = new Dictionary<int, int>();
    public Dictionary<int, int> m_playerSkillCount = new Dictionary<int, int>();

    public float m_energyMax = GameUtils.energy_max_basics;
    private float _energy = GameUtils.energy_basics;
    public float m_energy
    {
        get { return _energy; }
        set
        {
            if (value >= GameUtils.energy_max_basics)
                _energy = GameUtils.energy_max_basics;
            else if (value <= 0)
                _energy = 0;
            else
                _energy = value;
        }
    }

    public PlayerData()
    {
        PlayerSkillDataNode datanode = GameConfigManager.GetInstance().GetConfig<PlayerSkillDataNode>("D_PlayerSkill");
        foreach (var item in datanode.m_map)
        {
            PlayerSkillData data = (PlayerSkillData)item.Value;
            if (!m_playerskill.ContainsKey(data.id))
            {
                m_playerskill[data.id] = new SkillEntity(data.skill_id, null);
            }
        }

        HeroFormationNode map = GameConfigManager.GetInstance().GetConfig<HeroFormationNode>("D_HeroFormation");
        foreach (var item in map.m_map)
        {
            HeroFormationData data = (HeroFormationData)item.Value;
            if (data.default_select == 1)
            {
                m_formationHero.Add(GameConfigManager.GetInstance().GetItem<ObjectData>("D_ObjectBase",data.hero_id));
            }
        }

        PlayerSkillDataNode map1 = GameConfigManager.GetInstance().GetConfig<PlayerSkillDataNode>("D_PlayerSkill");
        foreach (var item in map1.m_map)
        {
            PlayerSkillData data = (PlayerSkillData)item.Value;
            m_formationSkill.Add(GameConfigManager.GetInstance().GetItem<SkillData>("D_Skill", data.skill_id));
        }
    }

    public void RefreshDefCount() 
    {
        _energy = GameUtils.energy_basics;
        m_heroCreateCount.Clear();
        HeroFormationNode map = GameConfigManager.GetInstance().GetConfig<HeroFormationNode>("D_HeroFormation");
        Dictionary<int, HeroFormationData> allhero = new Dictionary<int, HeroFormationData>();
        foreach (var item in map.m_map)
        {
            HeroFormationData data = (HeroFormationData)item.Value;
            allhero[data.hero_id] = data;
        }
        for (int i = 0; i < m_formationHero.Count; i++)
        {
            if (allhero.ContainsKey(m_formationHero[i].id))
            {
                m_heroCreateCount[m_formationHero[i].id] = allhero[m_formationHero[i].id].default_num;
            }
        }
        m_playerSkillCount.Clear();
        PlayerSkillDataNode skillmap = GameConfigManager.GetInstance().GetConfig<PlayerSkillDataNode>("D_PlayerSkill");
        foreach (var item in skillmap.m_map)
        {
            PlayerSkillData data = (PlayerSkillData)item.Value;
            m_playerSkillCount[data.skill_id] = data.num;
        }
    }

    public void RefreshHeroBySlot(ObjectData data,int slot) 
    {
        int upSlot = -1;
        for (int i = 0; i < m_formationHero.Count; i++)
        {
            if (m_formationHero[i].id == data.id)
            {
                upSlot = i;
            }
        }
        m_formationHero[slot] = data;
        if (upSlot >= 0) m_formationHero[upSlot] = null;
    }

    public bool GetHeroFormationState(int id) 
    {
        for (int i = 0; i < m_formationHero.Count; i++)
        {
            if (m_formationHero[i].id == id)
            {
                return true;
            }
        }
        return false;
    }
}
