using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 模型控制器
/// </summary>
public class ModelControl : MonoBehaviour
{
    public Animator m_animator;
    public Dictionary<MountType, Transform> m_mountDic = new Dictionary<MountType, Transform>(); //挂点
    public Dictionary<string, Transform> m_skillMountDic = new Dictionary<string, Transform>();//技能特效挂点
    public Dictionary<string, GameObject> m_effectDic = new Dictionary<string, GameObject>();
    public NavMeshAgent m_agent;
    public LineRenderer[] m_line;
    Timer m_timer;
    public void InitComponent(string animPath = "")
    {
        Transform animGo = this.transform;
        if (!string.IsNullOrEmpty(animPath))
        {
            animGo = transform.Find(animPath);
        }
        if (!animGo)
        {
            Debug.Log("找不到动画控制器节点"+gameObject.name);
        }
        m_animator = animGo.GetComponentInChildren<Animator>();
        m_agent = GetComponent<NavMeshAgent>();
        Transform linego = transform.Find("skill_line");
        if (linego)
        {
            m_line = linego.transform.GetComponentsInChildren<LineRenderer>();
            for (int i = 0; i < m_line.Length; i++)
            {
                m_line[i].gameObject.SetActive(false);
                m_line[i].enabled = false;
            }
        }
    }

    public void PlayAnim(string name,float speed = 1,Action<string> endcb = null)
    {
        if (m_timer) m_timer.Stop();
        m_timer = null;
        if (!m_animator)
        {
            Debug.Log("让我看看谁没有动画控制器"+this.gameObject.name);
        }
        m_animator.speed = speed;
        m_animator.CrossFade(name, 0.2f,0,0);
        if (endcb != null)
        {
            AnimationClip clip = GetClicp(name);
            if (clip)
            {
                m_timer = TimerUtils.StartTimer(clip.length-0.2f, true, () => { endcb(name); });
            }
        }
    }

    public void StopAnim() 
    {
        
    }

    public void SetSpeed(float speed) 
    {
        m_animator.speed = speed;
    }
    public void StopEffect( string respath) 
    {
        if (m_effectDic.ContainsKey(respath) && m_effectDic[respath])
        {
            m_effectDic[respath].SetActive(false);
            GameObject.Destroy(m_effectDic[respath]);
            m_effectDic.Remove(respath);
        }
    }
    public void PlayEffect(string mount, string respath, Vector3 pos, Vector3 rotate, Vector3 scale, bool isFollow) 
    {
        if (!string.IsNullOrEmpty(mount) && !string.IsNullOrEmpty(respath))
        {
            if (!m_skillMountDic.ContainsKey(mount))
            {
                GameObject go = GameUtils.FindChild(gameObject, mount);
                m_skillMountDic[mount] = go.transform;
            }
            if (!m_effectDic.ContainsKey(respath))
            {
                GameObject effect = GameObject.Instantiate(ResourcesManager.LoadGameObject(respath));
                effect.name = respath;
                m_effectDic[respath] = effect;
            }
            if (isFollow)
            {
                m_effectDic[respath].transform.SetParent(m_skillMountDic[mount]);
                m_effectDic[respath].transform.localPosition = pos;
            }
            else 
            {
                m_effectDic[respath].transform.SetParent(GameScenesManager.GetInstance().m_effectRoot.transform);
                m_effectDic[respath].transform.position = m_skillMountDic[mount].position + pos;
            }
            m_effectDic[respath].transform.localEulerAngles = rotate;
            m_effectDic[respath].transform.localScale = scale;

            m_effectDic[respath].SetActive(false);
            m_effectDic[respath].SetActive(true);
        }
    }

    public AnimationClip GetClicp(string clipname)
    {
        if (m_animator)
        {
            AnimationClip[] clips = m_animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name == clipname)
                {
                    return clips[i];
                }
            }
        }
        return null;
    }


    public void Release()
    {
        m_animator = null;
        m_agent = null;
        m_mountDic = null;
        m_skillMountDic.Clear();
        foreach (var item in m_effectDic)
        {
            if (item.Value)
            {
                GameObject.Destroy(item.Value);
            }
        }
        m_line = null;
    }
}
