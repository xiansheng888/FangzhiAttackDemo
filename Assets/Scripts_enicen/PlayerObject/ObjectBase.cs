using System;
using System.Collections.Generic;
using UnityEngine;


public class ObjectBase
{
    protected GameObject m_go;
    protected ObjectInfoBase m_info;
    protected FSM m_fsm;
    protected bool IsActive = false;
    public bool isPlaySkill = false;

    public UIHead m_uihead;
    //public Dictionary<int, SkillEntity> m_cacheSkill = new Dictionary<int, SkillEntity>();
    public ModelControl m_model;
    public ObjectInfoBase m_target;
    public SkillEntity m_curSkill;


    public ObjectBase(ObjectInfoBase info)
    {
        m_info = info;
        CreateObject();
    }
    public void CreateObject()
    {
        GameObject obj = ResourcesManager.LoadGameObject(/*Application.dataPath + "/Res/BuildAssets/Model/" +*/ m_info.m_cfgData.res_name /*+ ".prefab"*/);
        m_go = GameObject.Instantiate(obj, GameScenesManager.GetInstance().m_entityRoot.transform);
        m_go.transform.position = m_info.m_pos;
        m_go.transform.localScale = m_info.m_scale;
        m_go.transform.Rotate(m_info.m_rotate, Space.Self);
        m_go.name = m_info.m_entityId.ToString();
        //Debug.Log("创建 obj :" + m_info.m_cfgData.res_name + "  实体名称：" + (m_go ? m_go.name : "-"));
        m_model = m_go.AddComponent<ModelControl>();
        m_model.InitComponent(m_info.m_cfgData.anim_path);
        GameObject tmpMountGo = GameUtils.FindChild(m_model.gameObject, "point_head");
        m_model.m_mountDic[MountType.UIHead] = tmpMountGo ? tmpMountGo.transform : null;
        tmpMountGo = GameUtils.FindChild(m_model.gameObject, m_info.m_cfgData.bullet_point);
        m_model.m_mountDic[MountType.Bullet] = tmpMountGo? tmpMountGo.transform : null;
        tmpMountGo = GameUtils.FindChild(m_model.gameObject, "point_strike");
        m_model.m_mountDic[MountType.Strike] = tmpMountGo ? tmpMountGo.transform : null;
        tmpMountGo = GameUtils.FindChild(m_model.gameObject, "point_position");
        m_model.m_mountDic[MountType.Position] = tmpMountGo ? tmpMountGo.transform : null;
        if (m_model.m_mountDic[MountType.Position]) m_info.m_pos = m_model.m_mountDic[MountType.Position].position;
        if (m_model.m_mountDic[MountType.Strike]) m_info.m_strikePos = m_model.m_mountDic[MountType.Strike].position;
        if (m_model.m_agent)
        {
            m_model.m_agent.enabled = m_info.m_cfgData.move_type == 4 ? false : true;
            m_model.m_agent.autoRepath = false;
            m_info.m_agentRadius = m_model.m_agent.radius;
            //m_model.m_agent.stoppingDistance = 2f;//m_info.m_cfgData.att_range > 2 ? m_info.m_cfgData.att_range : 2f;
        }
        InitSkills();
        IsActive = true;
    }
    private void InitSkills()
    {
        if (m_info.m_cfgData.skill_id != null && m_info.m_cfgData.skill_id.Count > 0)
        {
            for (int i = 0; i < m_info.m_cfgData.skill_id.Count; i++)
            {
                m_info.m_skillList[m_info.m_cfgData.skill_id[i]] = new SkillEntity(m_info.m_cfgData.skill_id[i], this);
            }
        }
    }
    public virtual void Update()
    {
        if (!IsActive) return;
        if (!m_model) return;

        if (m_info != null && m_go && m_model.m_mountDic[MountType.Position])
        {
            if (m_model.m_mountDic[MountType.Position]) m_info.m_pos = m_model.m_mountDic[MountType.Position].position;
            if (m_model.m_mountDic[MountType.Strike]) m_info.m_strikePos = m_model.m_mountDic[MountType.Strike].position;
        }
        if (m_curSkill != null) m_curSkill.Update();
        if (m_uihead != null) m_uihead.Update();
        if (m_fsm != null) m_fsm.Update();
    }
    public void ChangeFSMState(FSMStateType type, object param = null)
    {
        //Debug.Log("切换状态" +type.ToString()+"   id" + m_info.m_entityId);
        if (m_fsm != null && m_info != null)
        {
            m_fsm.ChangeState(type, param);
        }
    }
    public void ChangeLastFSMState()
    {
        if (m_fsm != null)
        {
            ChangeFSMState(m_fsm.m_lastType);
        }
    }

    public float PlayInto()
    {
        m_model.PlayAnim(m_info.m_cfgData.into);
        AnimationClip clip = m_model.GetClicp(m_info.m_cfgData.into);
        if (clip)
        {
            return clip.length;
        }
        return 0;
    }
    public void PlayIdle()
    {
        if (m_model) m_model.PlayAnim(m_info.m_cfgData.idle);
    }
    public void PlayRun()
    {
        if (m_model) m_model.PlayAnim(m_info.m_cfgData.run);
    }
    public virtual void PlayDie()
    {
        StopRun();
        m_model.PlayAnim(m_info.m_cfgData.die);
    }
    public void PlayEffect(string mount, string respath, Vector3 pos, Vector3 rotate, Vector3 scale, bool isFollow, bool active)
    {
        if (m_model)
        {
            if (active)
                m_model.PlayEffect(mount, respath, pos, rotate, scale, isFollow);
            else
                m_model.StopEffect(respath);
        }
    }

    public void PlayLineBegin()
    {
        if (m_model.m_line!= null && m_target != null)
        {
            for (int i = 0; i < m_model.m_line.Length; i++)
            {
                m_model.m_line[i].gameObject.SetActive(true);
                m_model.m_line[i].enabled = true;
                m_model.m_line[i].positionCount = 2;
            }
        }
    }

    public void PlayLine()
    {
        if (m_model.m_line!= null && m_target != null)
        {
            for (int i = 0; i < m_model.m_line.Length; i++)
            {
                m_model.m_line[i].SetPosition(0, m_model.m_mountDic[MountType.Bullet].position);
                m_model.m_line[i].SetPosition(1, m_target.m_strikePos);
            }
        }
    }
    public void EndLine()
    {
        if (m_model.m_line != null && m_target != null)
        {
            for (int i = 0; i < m_model.m_line.Length; i++)
            {
                m_model.m_line[i].gameObject.SetActive(false);
                m_model.m_line[i].enabled = false;
            }
        }
    }
    public void PlaySkill(int skillId)
    {
        isPlaySkill = true;
        m_curSkill = m_info.m_skillList[skillId];
        m_curSkill.PlaySkill( m_info.m_atkSpeed);
    }
    public void EndSkillCallBack() 
    {
        isPlaySkill = false;
    }
    public virtual void MoveBegin()
    {
        if (m_info != null)
        {
            PlayRun();
            //m_model.m_agent.speed = m_info.m_speed;
            m_model.m_agent.enabled = true;
        }
    }
    public virtual void MoveCallBack()
    {
        if (GameCore.GetInstance().m_gameLogic.CheckAttack(m_info, m_target))
        {
            ChangeFSMState(FSMStateType.Attack);
        }
        //m_model.m_agent.enabled = false;
    }
    public virtual void LockTarget(ObjectInfoBase data)
    {
        m_target = data;
    }
    public virtual void Release()
    {
        IsActive = false;
        GameObject.Destroy(m_go);
        if (m_fsm != null) m_fsm.Release();
        m_fsm = null;
        if (m_uihead != null) m_uihead.Release();
        m_uihead = null;
        if (m_model) m_model.Release();
        m_model = null;
        if (m_curSkill != null)
        {
            m_curSkill.EndSkill();
        }
        if (m_info != null)
        {
            foreach (var item in m_info.m_skillList)
            {
                item.Value.ResetSkill();
            }
            m_info.m_skillList.Clear();
            List<BuffEffectOnce> all = new List<BuffEffectOnce>(m_info.m_buffList.Values);
            if (all.Count > 0)
            {
                for (int i = 0; i < all.Count; i++)
                {
                    all[i].End();
                }
            }
            m_info.m_buffList.Clear();
        }

        m_info = null;
        m_curSkill = null;
        m_target = null;
    }
    public void StopRun()
    {
        if (m_model.m_agent && m_model.m_agent.enabled)
        {
            m_model.m_agent.speed = 0;
            m_model.m_agent.velocity = Vector3.zero;
            //m_model.m_agent.enabled = false;
        }
        m_model.PlayAnim(m_info.m_cfgData.idle);
    }
    public void SetAbnormalState(PropertyType type, BuffData data, bool active)
    {
        if (active && (type == PropertyType.Frozen || type == PropertyType.Silence))
        {
            if (m_curSkill != null) m_curSkill.EndSkill();
            if (m_info.m_curType != FSMStateType.Abnormal)
            {
                ChangeFSMState(FSMStateType.Abnormal);
            }
            m_fsm.SetCurData(data);
        }
    }
    public ObjectInfoBase GetObjectInfo()
    {
        return m_info;
    }

    public void RefreshQualityPriority() 
    {
        if (m_model && m_info != null)
        {
            if (m_model.m_agent)
            {
                m_model.m_agent.avoidancePriority = GameCore.GetInstance().m_gameLogic.GetQualityPriorityByEntityid(m_info.m_entityId);
            }
        }
    }
}
