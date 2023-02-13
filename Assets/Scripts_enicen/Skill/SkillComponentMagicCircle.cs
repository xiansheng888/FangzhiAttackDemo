using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ä§·¨Õó
/// </summary>
public class SkillComponentMagicCircle : SkillComponentBase
{
    Vector3 m_target;
    public override void Trigger()
    {
        base.Trigger();
        int buffid = int.Parse(m_data.param1);
        GameCore.GetInstance().m_gameLogic.AddBuff(buffid, -1, m_target);
    }
    public override void SetTarget(Vector3 pos, SkillData data)
    {
        base.SetTarget(pos, data);
        m_target = pos;
    }
    public override void End()
    {
        base.End();
    }
}
