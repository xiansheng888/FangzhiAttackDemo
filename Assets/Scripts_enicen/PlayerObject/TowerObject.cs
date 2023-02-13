using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 塔
/// </summary>
public class TowerObject : ObjectBase
{
    float m_totalTimer = 0f;
    public TowerObject(ObjectInfoBase info) : base(info)
    {
        m_fsm = new FSM(this, m_info.m_cfgData);
        m_uihead = new UIHead(m_model.m_mountDic[MountType.UIHead], m_info);
    }
    public override void Update()
    {
        base.Update();
        if (m_info != null && m_info.m_cfgData.profession == 4)
        {
            m_totalTimer += Time.deltaTime;
            if (m_totalTimer >= 0.5f)
            {
                m_totalTimer = 0;
                SearchTarget();
            }
        }
        
    }
    //Quaternion des;
    //Vector3 dir;
    //protected override void LookTargetFunc()
    //{
    //    if (m_info.m_cfgData.look_rotateY == 0)
    //    {
    //        base.LookTargetFunc();
    //    }
    //    else
    //    {
    //        if (m_fsm.m_type == FSMStateType.Attack && m_target != null) //攻击状态转向到目标
    //        {
    //            if (Vector3.Angle(m_target.m_strikePos - m_model.m_mountDic[MountType.Bullet].transform.position, m_model.m_mountDic[MountType.Bullet].transform.position) > 0.02f)
    //            {
    //                dir = m_target.m_strikePos - m_model.m_mountDic[MountType.Bullet].transform.position;
    //                des = Quaternion.LookRotation(dir + Vector3.down * 8);
    //                m_model.m_animator.transform.rotation = Quaternion.Slerp(m_model.m_animator.transform.rotation, des , rotateSpeed * Time.deltaTime);
    //            }
    //        }
    //    }
    //}
    ObjectInfoBase newTarget = null;
    public virtual void SearchTarget()
    {
        if (m_info != null && m_info.m_cfgData.profession == 4)
        {
            if (m_fsm.m_type == FSMStateType.Idle || m_target == null)
            {
                if (m_info.m_skillList.Count > 0)
                {
                    List<int> types = new List<int>();
                    foreach (var item in m_info.m_skillList)
                    {
                        if (types.Count == 0) types = item.Value.m_cfgData.target_type;
                    }
                    newTarget = GameCore.GetInstance().m_gameLogic.TowerChekLockTarget(m_info, types);
                }
            }
            if (newTarget != null)
            {
                LockTarget(newTarget);
            }
            newTarget = null;
        }
    }
    public override void LockTarget(ObjectInfoBase data)
    {
        base.LockTarget(data);
        ChangeFSMState(m_target != null ? FSMStateType.Attack : FSMStateType.Idle);
    }
    public override void Release()
    {
        base.Release();
        m_totalTimer = 0f;
        newTarget = null;
    }
}
