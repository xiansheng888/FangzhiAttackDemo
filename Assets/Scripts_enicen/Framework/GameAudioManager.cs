using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameAudioManager : Singleton<GameAudioManager>
{
    List<AudioOnce> audios = new List<AudioOnce>();
    
    public void Play(string audio,bool isloop,float volume = 1,Action endcb = null) 
    {
        AudioOnce once = GetNothingAudio();
        if (once == null)
        {
            GameObject audiogo = new GameObject("audio");
            audiogo.transform.SetParent(GameCore.GetInstance().m_audioroot.transform);
            once = audiogo.AddComponent<AudioOnce>();
            audios.Add(once);
        }
        once.Play(ResourcesManager.Load<AudioClip>(audio),isloop, volume, endcb);
    }

    AudioOnce GetNothingAudio() 
    {
        for (int i = 0; i < audios.Count; i++)
        {
            if ( !audios[i].isPlaying())
            {
                return audios[i];
            }
        }
        return null;
    }


    public void SetVolume(string name,float value) 
    {
        for (int i = 0; i < audios.Count; i++)
        {
            if (name == audios[i].CurSourceName)
            {
                audios[i].SetVolume(value);
                break;
            }
        }
    }
}
