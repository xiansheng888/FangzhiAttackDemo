using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : StateBase
{
    //idle和行走时都需要检查锁敌，所以具体逻辑写到具体实体update中
    bool isFollow = false;
    float agentStopSafeDis = 0.5f;//寻路停止，检测是否可攻击，安全距离
    public FollowState(ObjectBase entity) : base(entity)
    {
    }

    public override void Release()
    {
        base.Release();
        isFollow = false;
    }

    public override void Update()
    {
        base.Update();
        if (m_entity.m_target != null)
        {
            if (m_entity.m_model.m_agent.speed != m_entity.GetObjectInfo().m_speed) m_entity.m_model.m_agent.speed = m_entity.GetObjectInfo().m_speed;
            if (m_entity.m_model.m_agent.isOnNavMesh)
            {
                m_entity.m_model.m_agent.SetDestination(m_entity.GetObjectInfo().m_cfgData.profession == 1 ? GameScenesManager.GetInstance().m_mowang_target : m_entity.m_target.m_pos);
                if (Vector3.Distance(m_entity.GetObjectInfo().m_pos, m_entity.m_target.m_pos) <= m_entity.GetObjectInfo().m_cfgData.att_range)
                    m_entity.MoveCallBack();
                if (!m_entity.m_model.m_agent.pathPending && m_entity.m_model.m_agent.remainingDistance < m_entity.m_model.m_agent.stoppingDistance + m_entity.m_target.m_agentRadius - agentStopSafeDis)
                    m_entity.MoveCallBack();
            }
        }
    }

    public override void Enter(object param)
    {
        base.Enter(param);
        isFollow = true;
        m_entity.MoveBegin();
    }
    public override void Leave()
    {
        if (isFollow)
        {
            m_entity.StopRun();
        }
        base.Leave();
    }
}