using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SkillEntity
{
    private Dictionary<SkillComponentType, Func<SkillComponentBase>> SkillTypeToComponent = new Dictionary<SkillComponentType, Func<SkillComponentBase>>()
    {
        [SkillComponentType.AnimClip] = () => { return new SkillComponentAnimClip(); },
        [SkillComponentType.Effect] = () => { return new SkillComponentEffect(); },
        [SkillComponentType.Audio] = () => { return new SkillComponentAudio(); },
        [SkillComponentType.Bullet] = () => { return new SkillComponentBullet(); },
        [SkillComponentType.Line] = () => { return new SkillComponentLine(); },
        [SkillComponentType.Ball] = () => { return new SkillComponentBall(); },
        [SkillComponentType.PlayerBullet] = () => { return new SkillComponentPlayerBullet(); },
        [SkillComponentType.MagicCircle] = () => { return new SkillComponentMagicCircle(); },

    };

    public SkillData m_cfgData = null;
    public List<SkillComponentBase> m_componentData = null;
    public long m_endTime = 0;
    private float _speed = GameUtils.AtkSpeed_basics; //1 正常速度
    public float m_speed 
    {
        set {
            _speed = value;
            for (int i = 0; i < m_componentData.Count; i++)
            {
                m_componentData[i].SetSpeed(_speed);
            }
        }
        get { return _speed; }
    }

    private ObjectBase m_entity;
    private float m_time = 0;
    private bool m_attack = false;
    private float m_lastAtkTime = 0;
    public SkillEntity(int id, ObjectBase entity)
    {
        m_entity = entity;
        m_componentData = new List<SkillComponentBase>() { };
        m_cfgData = GameConfigManager.GetInstance().GetItem<SkillData>("D_Skill", id);
        SkillComponentNode node = GameConfigManager.GetInstance().GetSkillComponentNode(m_cfgData.show_path);
        List<ConfigDataBase> datas = new List<ConfigDataBase>(node.m_map.Values);
        for (int i = 0; i < datas.Count; i++)
        {
            SkillComponent data = (SkillComponent)datas[i];
            m_componentData.Add(SkillTypeToComponent[data.type]());
            m_componentData[i].SetObjectEntityAndData(entity, data);
        }
    }

    public void ResetSkill()
    {
        m_attack = false;
        m_time = 0;
        m_lastAtkTime = 0;
        for (int i = 0; i < m_componentData.Count; i++)
        {
            m_componentData[i].Reset();
        }
    }

    public void EndSkill()
    {
        for (int i = 0; i < m_componentData.Count; i++)
        {
            m_componentData[i].End();
        }
        m_attack = false;
        m_time = 0;
        m_lastAtkTime = 0;
        if(m_entity != null) m_entity.EndSkillCallBack();
    }

    public void PlaySkill(float speed = 1, Vector3 plandPos = default(Vector3))
    {
        m_time = 0;
        m_endTime = TimerUtils.GetNowTimeStamp();
        _speed = speed;
        for (int i = 0; i < m_componentData.Count; i++)
        {
            m_componentData[i].SetSpeed(_speed);
            m_componentData[i].SetTarget(plandPos, m_cfgData);
            m_componentData[i].Enter();
        }
    }

    public void Update()
    {
        for (int i = 0; i < m_componentData.Count; i++)
        {
            m_componentData[i].Update();
        }
        m_time += Time.deltaTime * _speed;
        AtkStampCheck();
        AtkEndCheck();
    }
    private void SendAtkRequest() 
    {
        if (m_entity != null)
        {
            ObjectInfoBase m_info = m_entity.GetObjectInfo();
            if (m_cfgData.scope > 0)
                GameCore.GetInstance().m_gameLogic.AttackByRange(m_info.m_pos, m_info, m_cfgData, m_cfgData.scope + m_info.m_cfgData.att_range);
            else
                GameCore.GetInstance().m_gameLogic.AttackByPoint(m_info, m_entity.m_target, m_cfgData);

        }
    }
    private void AtkStampCheck() 
    {
        if (m_cfgData.att_interval > 0)
        {
            if (m_time - m_lastAtkTime >= m_cfgData.att_interval)
            {
                m_lastAtkTime = m_time;
                SendAtkRequest();
            }
        }
        else if (!m_attack && m_cfgData.att_timestamp >= 0)
        {
            if (m_time >= m_cfgData.att_timestamp)
            {
                m_attack = true;
                SendAtkRequest();
            }
        }
        
    }
    private void AtkEndCheck() 
    {
        if (m_cfgData.time > 0)
        {
            if (m_time >= m_cfgData.time)
            {
                EndSkill();
            }
        }
    }
}
