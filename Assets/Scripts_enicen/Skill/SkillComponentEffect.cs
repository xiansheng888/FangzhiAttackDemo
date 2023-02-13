using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillComponentEffect : SkillComponentBase
{
    public override void Trigger()
    {
        base.Trigger();
        Vector3 pos = Vector3.zero;
        if (!string.IsNullOrEmpty( m_data.param3))
        {
            string[] tmp = m_data.param3.Split('|');
            pos = new Vector3(float.Parse(tmp[0]), float.Parse(tmp[1]), float.Parse(tmp[2]));
        }
        Vector3 rotate = Vector3.zero;
        if (!string.IsNullOrEmpty(m_data.param4))
        {
            string[] tmp = m_data.param4.Split('|');
            rotate = new Vector3(float.Parse(tmp[0]), float.Parse(tmp[1]), float.Parse(tmp[2]));
        }
        Vector3 scale = Vector3.one;
        if (!string.IsNullOrEmpty(m_data.param5))
        {
            string[] tmp = m_data.param5.Split('|');
            scale = new Vector3(float.Parse(tmp[0]), float.Parse(tmp[1]), float.Parse(tmp[2]));
        }
        m_obje.PlayEffect(m_data.param2, m_data.param1, pos, rotate, scale, m_data.param6 == "1",true);
    }

    public override void Update()
    {
        base.Update();
    }
    public override void End()
    {
        m_obje.PlayEffect(m_data.param2, m_data.param1, Vector3.zero, Vector3.zero, Vector3.zero, false, false);
        base.End();
    }
    public override void Reset()
    {
        base.Reset();
    }
}
