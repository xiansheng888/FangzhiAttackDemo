using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillComponentAnimClip : SkillComponentBase
{
    bool isTrigger = false;
    public override void Reset()
    {
        base.Reset();
    }
    public override void Trigger()
    {
        isTrigger = true;
        base.Trigger();
        if (m_obje != null)
        {
            m_obje.m_model.PlayAnim(m_data.param1,m_speed,(name)=> {
                if (m_data != null && name == m_data.param1 && isTrigger)
                {
                    m_obje.PlayIdle();
                }
            });
        }
    }
    public override void End()
    {
        isTrigger = false;
        base.End();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void SetSpeed(float speed)
    {
        base.SetSpeed(speed);
        if (speed <= 0.02f)
        {
            isTrigger = false;
        }
        m_obje.m_model.SetSpeed(speed);
    }
}
