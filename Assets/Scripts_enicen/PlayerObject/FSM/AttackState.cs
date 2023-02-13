using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateBase
{
    private int m_curPlayId = 0;
    float m_totalTimer = 0;
    protected float rotateSpeed = 2f;
    public AttackState(ObjectBase entity) : base(entity)
    {
    }

    public override void Release()
    {
        base.Release();
        m_curPlayId = 0;
        m_totalTimer = 0;
    }
    public override void Enter(object param)
    {
        base.Enter(param);
        CheckCanPlaySkill();
        m_totalTimer = 0.5f;
    }
    public override void Update()
    {
        base.Update();

        if (Vector3.Angle(m_entity.m_target.m_pos - m_entity.GetObjectInfo().m_pos, m_entity.GetObjectInfo().m_pos) > 0.2f)
        {
            m_entity.m_model.m_animator.transform.rotation = Quaternion.Slerp(m_entity.m_model.m_animator.transform.rotation,
                Quaternion.LookRotation(m_entity.m_target.m_pos - m_entity.GetObjectInfo().m_pos), rotateSpeed * Time.deltaTime);
        }

        m_totalTimer += Time.deltaTime;
        if (m_totalTimer >= 0.2f)
        {
            if (m_entity.m_target == null
                || (m_entity.m_target != null && m_entity.m_target.m_curType == FSMStateType.Die)
                || Vector3.Distance(m_entity.GetObjectInfo().m_pos, m_entity.m_target.m_pos) > m_entity.GetObjectInfo().m_cfgData.att_range)
            {
                m_entity.ChangeFSMState(FSMStateType.Idle);
                return;
            }
            m_totalTimer = 0;
        }
        CheckCanPlaySkill();
    }
    public override void Leave()
    {
        if (m_entity.m_curSkill != null)
        {
            m_entity.m_curSkill.EndSkill();
        }
        m_entity.m_curSkill = null;
        m_curPlayId = 0;
        base.Leave();
    }

    private int GetPlaySkillId() 
    {
        int id = 0;
        int repetition = 0;
        float mp = m_entity.GetObjectInfo().m_mp;
        if (m_entity.m_target.m_hp >= 0)
        {
            foreach (var item in m_entity.GetObjectInfo().m_skillList)
            {
                if ((TimerUtils.GetNowTimeStamp() - item.Value.m_endTime) >= item.Value.m_cfgData.cd && item.Value.m_cfgData.mp_cost <= mp)
                {
                    if (item.Value.m_cfgData.id == m_curPlayId)
                        repetition = m_curPlayId;
                    else
                        id = item.Value.m_cfgData.id;
                }
            }
            if (id == 0) id = repetition;
        }
        return id;
    }

    private void PlaySkill(int skillId) 
    {
        m_entity.m_curSkill = null;
        if (m_entity.GetObjectInfo().m_skillList.ContainsKey(skillId))
        {
            m_entity.PlaySkill(skillId);
        }
    }

    void CheckCanPlaySkill() 
    {
        if (!m_entity.isPlaySkill)
        {
            m_curPlayId = GetPlaySkillId();
            PlaySkill(m_curPlayId);
        }
    }
}
