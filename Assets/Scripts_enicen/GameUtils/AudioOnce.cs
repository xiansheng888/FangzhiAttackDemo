using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOnce : MonoBehaviour
{
    float m_targetVolume = 0;
    float m_curVolume = 1;
    AudioSource m_as;
    bool isSetVolume = false;
    void Awake()
    {
        m_as = this.gameObject.AddComponent<AudioSource>();
    }

    public string CurSourceName
    {
        get
        {
            if (m_as)
            {
                return m_as.clip.name;
            }
            return "";
        }
    }
    public void Play(AudioClip clip,bool isLoop,float volume = 1,Action endcb = null) 
    {
        m_curVolume = volume;
        m_as.clip = clip;
        m_as.volume = volume;
        m_as.loop = isLoop;
        m_as.Play();
        if (!isLoop)
        {
            TimerUtils.StartTimer(clip.length, true, endcb);
        }
    }
    private void Update()
    {
        if (isSetVolume )
        {
            if (m_as && m_targetVolume != m_as.volume)
            {

                if (m_curVolume > m_targetVolume)
                {
                    m_as.volume -= Time.deltaTime * 0.6f;
                    m_curVolume = m_as.volume;
                    if (m_curVolume <= m_targetVolume) isSetVolume = false;
                }
                else
                {
                    m_as.volume += Time.deltaTime;
                    m_curVolume = m_as.volume * 0.6f;
                    if (m_curVolume >= m_targetVolume) isSetVolume = false;

                }
            }
            else 
            {
                isSetVolume = false;
            }
        }
    }

    public void SetVolume(float volume) 
    {
        m_targetVolume = volume;
        m_curVolume = m_as.volume;
        isSetVolume = true;
    }

    public bool isPlaying() { return m_as.isPlaying; }
}
