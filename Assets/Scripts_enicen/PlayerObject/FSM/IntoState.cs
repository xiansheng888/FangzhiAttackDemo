using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntoState : StateBase
{
    private float m_totalTimer;
    private float m_intoTime = -1;
    public IntoState(ObjectBase entity) : base(entity)
    {
        m_intoTime = m_entity.PlayInto() - 0.5f;
        GameAudioManager.GetInstance().Play(m_entity.GetObjectInfo().m_cfgData.into_audio, false);
    }

    public override void Release()
    {
        base.Release();
        m_totalTimer = 0;
        m_intoTime = -1;
    }
    public override void Update()
    {
        base.Update();
        if (m_intoTime > -1)
        {
            m_totalTimer += Time.deltaTime;
            if (m_totalTimer >= m_intoTime)
            {
                m_totalTimer = 0;
                m_intoTime = -1;
                m_entity.ChangeFSMState(FSMStateType.Idle);
            }
        }
    }
}