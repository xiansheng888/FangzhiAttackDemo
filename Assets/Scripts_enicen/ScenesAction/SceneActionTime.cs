using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneActionTime : ScenesActionBase
{
    float m_time;
    float m_timecnt;
    Timer m_timer;
    public override void Trigger()
    {
        base.Trigger();
        m_time = float.Parse(m_data.param1);
        m_timecnt = 0;
        Messenger.GetInstance().Broadcast(new SceneTime(m_time));
        m_timer = TimerUtils.StartTimer(1, false, CountDown,1);
    }
    private void CountDown()
    {
        m_timecnt += 1;
        if (m_timecnt >= m_time)
        {
            m_timer.Stop();
            Leave();
        }
    }
    public override void Checker()
    {
        base.Checker();
    }

    public override void Leave()
    {
        m_timer = null;
        m_time = 0;
        m_timecnt = 0;
        base.Leave();
    }
    public override void Release()
    {
        if(m_timer != null) m_timer.Stop();
        m_timer = null;
        m_time = 0;
        m_timecnt = 0;
        base.Release();
    }

}
