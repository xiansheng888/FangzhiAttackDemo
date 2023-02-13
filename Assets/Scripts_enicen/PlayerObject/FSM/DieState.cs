using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : StateBase
{
    string m_clipName;
    Timer timer;
    public DieState(ObjectBase entity) : base(entity)
    {
        m_clipName = m_entity.GetObjectInfo().m_cfgData.die;
    }

    public override void Release()
    {
        base.Release();
        m_clipName = string.Empty;
        timer = null;
    }
    public override void Enter(object param)
    {
        base.Enter(param);
        m_entity.m_uihead.Release();
        float time = 0f;
        AnimationClip clip = m_entity.m_model.GetClicp(m_clipName);
        if (clip)
        {
            time = clip.length;
        }
        if (time > 0)
        {
            m_entity.PlayDie();
            timer = TimerUtils.StartTimer(time, true, () => {
                Leave(); 
            });
        }
        else 
        {
            Leave();
        }
    }
    public override void Leave()
    {
        if (timer)
        {
            timer.Stop();
            timer = null;
        }
        base.Leave();
        m_entity.Release();
    }

}
