using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public bool isActive = false;
    ObjectInfoBase m_attacker;
    ObjectInfoBase m_target;
    Vector3 m_targetPos;
    SkillData m_skillData;
    SkillComponent m_componentData;

    Vector3 m_bulletTarget;
    Action m_endCB;
    
    public void ResetData(ObjectInfoBase attacker,ObjectInfoBase target,SkillData skilldata ,SkillComponent componentdata) 
    {
        m_attacker = attacker;
        m_target = target;
        m_skillData = skilldata;
        m_componentData = componentdata;
    }
    
    /// <summary>
    /// 播放
    /// </summary>
    /// <param name="speed">速度或时长</param>
    /// <param name="isFixedTime">是否固定时间到达</param>
    public void PlayNormal(Vector3[] path,float speed, Action endCb)
    {
        m_endCB = endCb;
        isActive = true;
        m_bulletTarget = path[path.Length - 1];
        Move(speed, path);
    }

    private void Move(float time,Vector3[] path)
    {
        if (transform)
        {
            transform.position = path[0];
            m_targetPos = path[path.Length - 1];
            gameObject.SetActive(true);
            BulletMoveType type = BulletMoveType.Line;
            int movetype;
            if (int.TryParse(m_componentData.param3, out movetype))
                type = (BulletMoveType)movetype;
            float speed = Vector3.Distance(transform.position, m_targetPos) / time;
            //DOTween.Sequence().Append(transform.DOPath(path, speed, type == BulletMoveType.Line ?PathType.Linear: PathType.CatmullRom).SetEase(Ease.Linear).SetLookAt(0.01f)).AppendCallback(MoveEnd);
            transform.DOPath(path, speed, type == BulletMoveType.Line ? PathType.Linear : PathType.CatmullRom).SetEase(Ease.Linear).SetLookAt(0.01f).OnComplete(MoveEnd);
        }
    }

    private void MoveEnd()
    {
        if (m_skillData.scope_buttle > 0)
        {
            float range_bullet = m_skillData.scope_buttle;
            GameCore.GetInstance().m_gameLogic.AttackByRange(m_targetPos, m_attacker, m_skillData, range_bullet);
        }
        else
        {
            if (m_target != null && Vector3.Distance(m_target.m_strikePos, m_bulletTarget) <= 0.3f)
            {
                GameCore.GetInstance().m_gameLogic.AttackByPoint(m_attacker, m_target, m_skillData);
            }
        }
        isActive = false;
        if (m_endCB != null)
        {
            m_endCB();
        }
        if (this.gameObject)
        {
            this.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject);
        }
    }
    float time = 0;
    private void Update()
    {
        //if (isActive && transform.position.y <= 0)
        //{
        //    isActive = false;
        //}
        time += Time.deltaTime;
        if (time > 5f)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    public void OnDestroy()
    {
        m_attacker = null;
        m_target = null;
        m_skillData = null;
        m_componentData = null;
        m_endCB = null;
    }
}
