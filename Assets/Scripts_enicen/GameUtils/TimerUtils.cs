using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Timer :MonoBehaviour
{
    Action m_cb;
    bool isonce = false;
    bool isstop = false;
    public void SetData(float time, bool once, Action cb,float oneinvoke) 
    {
        isonce = once;
        m_cb = cb;
        if (once)
        {
            Invoke("InvokeCB", time);
        }
        else
        {
            InvokeRepeating("InvokeCB", oneinvoke, time);
        }
    }

    public void Stop()
    {
        isstop = true;
        m_cb = null;
        GameObject.Destroy(this);
    }

    void InvokeCB()
    {
        if (m_cb != null)
        {
            m_cb();
        }
        if (isonce && !isstop)
        {
            Stop();
        }
    }
}

public class TimerUtils
{
    static public Timer StartTimer(float time, bool once, Action cb,float oneinvoke = 0)
    {
        Timer timer = GameCore.GetInstance().m_timerRoot.AddComponent<Timer>();
        timer.SetData(time,once,cb, oneinvoke);
        return timer;
    }

    static public long GetNowTimeStamp() 
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }
}
