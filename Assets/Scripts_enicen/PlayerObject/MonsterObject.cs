using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//普通怪物
public class MonsterObject : ObjectBase
{
    float m_totalTimer = 1;
    public MonsterObject(ObjectInfoBase info):base(info) 
    {
        m_fsm = new FSM(this, m_info.m_cfgData);
        m_uihead = new UIHead(m_model.m_mountDic[MountType.UIHead], m_info);
    }

    public override void LockTarget(ObjectInfoBase data)
    {
        base.LockTarget(data);
        if (m_fsm != null)
        {
            if (m_fsm.m_type == FSMStateType.Move)  //切换目标，不换状态
            {
                return;
            }
            if (m_fsm.m_type != FSMStateType.Attack && m_fsm.m_type != FSMStateType.Die && m_fsm.m_type != FSMStateType.Init)
            {
                ChangeFSMState(FSMStateType.Move);
            }
        }
    }
    public override void Update()
    {
        base.Update();
        if (m_info != null && m_info.m_cfgData.profession == 2)
        {
            m_totalTimer += Time.deltaTime;
            if (m_totalTimer >= 1f)
            {
                m_totalTimer = 0;
                SearchTarget();
            }
        }
    }

    public void SearchTarget()
    {
        if (m_fsm != null && m_info != null && m_info.m_cfgData.profession == 2 )
        {
            if (m_fsm.m_type == FSMStateType.Idle || m_fsm.m_type == FSMStateType.Move)
            {
               GameCore.GetInstance().m_gameLogic.NavMeshCheckLockTarget(m_info,m_model.m_agent.areaMask,(tmpinfo)=> {
                    if (tmpinfo != null)
                    {
                        //Debug.Log("计算锁敌 发起人：" + m_info.m_entityId + "---被锁定目标：" + newTarget.m_entityId);
                        LockTarget(tmpinfo);
                    }
                });
            }
        }
        
    }

}
