using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillComponentBase 
{
    protected SkillComponent m_data;
    protected ObjectBase m_obje;
    protected float m_speed = 1;
    private float m_time = 0;
    private bool m_isActive = false;
    protected bool m_isTrigger = false;
    protected SkillData m_skillData;
    public void Enter() 
    {
        m_isActive = true;
    }

    public virtual void Update() 
    {
        if (m_isActive)
        {
            m_time += Time.deltaTime * m_speed;
            if (!m_isTrigger && m_time >= m_data.trigger_time )
            {
                Trigger();
            }
            if (m_data.life_time > 0 && m_time >= m_data.life_time)
            {
                End();
            }
        }
    }

    public virtual void End() 
    {
        m_isActive = false;
        m_isTrigger = false;
        m_time = 0;
    }

    public virtual void Trigger() 
    {
        m_isTrigger = true;
    }

    public void SetObjectEntityAndData(ObjectBase entity, SkillComponent data) 
    {
        m_obje = entity;
        m_data = data;
        m_time = 0;
    }

    public virtual void Reset() 
    {
        m_data = null;
        m_obje = null;
        m_skillData = null;
        m_isActive = false;
        m_isTrigger = false;
    }
    public virtual void SetSpeed(float speed) 
    {
        m_speed = speed;
    }

    public virtual void SetTarget(Vector3 pos,SkillData data) 
    {
        m_skillData = data;
    }
}