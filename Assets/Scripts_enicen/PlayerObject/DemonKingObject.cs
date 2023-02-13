using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//魔王
public class DemonKingObject : ObjectBase
{
    float m_totalTimer = 0;
    public DemonKingObject(ObjectInfoBase info) : base(info) 
    {
        m_fsm = new FSM(this,m_info.m_cfgData);
        m_uihead = new UIHead(m_model.m_mountDic[ MountType.UIHead],m_info);
    }

    public override void Update()
    {
        base.Update();
        if (m_info != null && m_info.m_cfgData.camp == 1)
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
    public void SearchTarget()
    {
        if (m_info == null || m_info.m_cfgData.camp == 2 || m_info.m_cfgData.profession != 1)
        {
            return;
        }
        if (m_target != null && GameCore.GetInstance().m_gameLogic.CheckAttack(m_info, m_target))
        {
            ChangeFSMState(FSMStateType.Attack);
            return;
        }
        if (m_fsm.m_type == FSMStateType.Idle || m_fsm.m_type == FSMStateType.Move)
        {
            newTarget = GameCore.GetInstance().m_gameLogic.CheckLockTarget(m_info);
        }
        if (newTarget != null)
        {
            LockTarget(newTarget);
            newTarget = null;
        }
    }
    public override void LockTarget(ObjectInfoBase data)
    {
        if (m_target == null || data.m_entityId != m_target.m_entityId)
        {
            base.LockTarget(data);
            if (m_fsm.m_type != FSMStateType.Move)
            {
                ChangeFSMState(FSMStateType.Move);
            }
        }
    }
}
