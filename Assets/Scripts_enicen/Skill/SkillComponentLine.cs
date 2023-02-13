using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillComponentLine : SkillComponentBase
{
    public override void Trigger()
    {
        base.Trigger();
        m_obje.PlayLineBegin();
    }

    public override void Reset()
    {
        base.Reset();
    }

    public override void Update()
    {
        base.Update();
        if (m_isTrigger)
        {
            m_obje.PlayLine();
        }
    }
    public override void End()
    {
            Debug.Log("�������������!!!");
        base.End();
        m_obje.EndLine();
    }
}
