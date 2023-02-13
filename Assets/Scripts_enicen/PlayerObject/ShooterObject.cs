using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 投掷怪物
/// </summary>
public class ShooterObject : ObjectBase
{
    float m_totalTimer = 0f;
    public ShooterObject(ObjectInfoBase info) : base(info)
    {
        m_fsm = new FSM(this, m_info.m_cfgData);
        m_uihead = new UIHead(m_model.m_mountDic[MountType.UIHead], m_info);
    }
    public override void Update()
    {
        base.Update();
        if (m_info != null && m_info.m_cfgData!= null && m_info.m_cfgData.profession == 3)
        {
            m_totalTimer += Time.deltaTime;
            if (m_totalTimer >= 1f)
            {
                m_totalTimer = 0;
                SearchTarget();
            }
        }
    }

    ObjectInfoBase newTarget = null;
    void SearchTarget()
    {
        if (m_info != null)
        {
            if (m_fsm.m_type == FSMStateType.Idle || m_fsm.m_type == FSMStateType.Move)
            {
                newTarget = GameCore.GetInstance().m_gameLogic.CheckLockTarget(m_info);
            }
        }
        if (newTarget != null)
        {
            LockTarget(newTarget);
            newTarget = null;
        }
    }

    public override void LockTarget(ObjectInfoBase data)
    {
        base.LockTarget(data);
        if (m_fsm.m_type == FSMStateType.Move)  //切换目标，不换状态
        {
            return;
        }
        if (m_fsm.m_type != FSMStateType.Attack && m_fsm.m_type != FSMStateType.Die)
        {
            ChangeFSMState(FSMStateType.Move);
        }
    }
}
