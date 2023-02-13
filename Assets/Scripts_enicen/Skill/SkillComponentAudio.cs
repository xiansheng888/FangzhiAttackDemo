using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillComponentAudio : SkillComponentBase
{
    public override void Trigger()
    {
        base.Trigger();
        GameAudioManager.GetInstance().Play(m_data.param1, false, float.Parse(m_data.param2));
    }

    public override void End()
    {
        base.End();
    }
}
