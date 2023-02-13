using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ѣ��
/// </summary>
public class AbnormalState : StateBase
{
    BuffData m_buffdate = null;
    float m_time = 0;
    public AbnormalState(ObjectBase entity) : base(entity){}
    public override void SetData(object data)
    {
        base.SetData(data);
        m_buffdate = (BuffData)data;
        m_time = 0;
    }
    public override void Enter(object param)
    {
        base.Enter(param);
        Debug.Log("�����쳣״̬");
        m_entity.m_model.m_animator.speed = 0;
    }
    public override void Update()
    {
        base.Update();
        if (m_buffdate != null && m_buffdate.time_duration>0)
        {
            m_time += Time.deltaTime;
            if (m_time >= m_buffdate.time_duration)
            {
                m_entity.ChangeLastFSMState();
            }
        }
    }
    public override void Leave()
    {
        m_time = 0;
        m_buffdate = null;
        m_entity.m_model.m_animator.speed = 1;
        base.Leave();
        Debug.Log("�뿪�쳣״̬");
    }

    public override void Release()
    {
        m_time = 0;
        m_buffdate = null;
        base.Release();
    }
}

