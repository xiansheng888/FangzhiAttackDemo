using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneEffect:MonoBehaviour
{
    public string m_effectName;
    public bool m_isPlay;
    public float m_speed;
    bool m_isStopDestory = false;
    float m_time = 10;

    public void Play(Vector3 pos,bool isStopDestory = false ,float time = 10)
    {
        m_totalTime = 0;
        m_isPlay = true;
        transform.position = pos;
        m_isStopDestory = isStopDestory;
        m_time = time;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    float m_totalTime = 0;
    private void Update()
    {
        if (m_isPlay)
        {
            m_totalTime += Time.deltaTime;
            if (m_totalTime >= m_time)
            {
                if (m_isStopDestory) Release(); else Stop();
            }
        }
    }

    public void Stop() 
    {
        m_isPlay = false;
        m_totalTime = 0;
    }

    public void Release()
    {
        Stop();
        m_effectName = string.Empty;
        if (gameObject) GameObject.Destroy(gameObject);
    }
}
