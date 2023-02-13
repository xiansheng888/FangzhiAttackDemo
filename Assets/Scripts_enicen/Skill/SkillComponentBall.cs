using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillComponentBall : SkillComponentBase
{
    List<ObjectInfoBase> m_targets = new List<ObjectInfoBase>();
    int curindex = -1;
    float speed = 0f;
    Vector3[] _path = new Vector3[2];

    bool isNext = false;
    public override void Trigger()
    {
        base.Trigger();
        speed = float.Parse(m_data.param2);
        //Debug.Log("球锁敌 技能开始 锁敌id" + m_obje.m_target.m_entityId);
        m_targets = GameCore.GetInstance().m_gameLogic.BallCheckLockTargets(m_obje.m_target, int.Parse(m_data.param4), float.Parse( m_data.param3),m_skillData.target_type);
        curindex = 0;
        StartMove();
    }
    private void StartMove()
    {
        if (m_targets.Count > curindex)
        {
            _path[0] = curindex == 0 ? m_obje.GetObjectInfo().m_strikePos : m_targets[curindex - 1].m_strikePos;
            _path[1] = m_targets[curindex].m_strikePos;

            BulletManager.GetInstance().PlayBullet(m_data.param1, speed, _path, m_obje.GetObjectInfo(), m_targets[curindex], m_skillData, m_data, () =>
            {
                curindex++;
                isNext = true;
            });
        }
        else 
        {
            m_targets.Clear();
        }
    }

    public override void Update()
    {
        base.Update();
        if (isNext && curindex > 0)
        {
            StartMove();
            isNext = false;
        }
        //if (curindex >= 0)
        //{
        //    var direction = m_targets[curindex].m_strikePos - m_buttle.transform.position;
        //    m_buttle.transform.Translate(direction.normalized * Time.deltaTime * speed, Space.World);
        //    var turn = Vector3.Cross(m_buttle.transform.forward, direction).y >= 0 ? 1f : -1f;
        //    m_buttle.transform.Rotate(m_buttle.transform.up, Vector3.Angle(m_buttle.transform.forward, direction) * Time.deltaTime * speed * turn, Space.World);
        //    if (Vector3.Distance(m_buttle.transform.position, m_targets[curindex].m_strikePos) <= 0.2f)
        //    {
        //        GameCore.GetInstance().m_gameLogic.AttackByPoint(m_obje.GetObjectInfo(), m_targets[curindex], m_skillData);
        //        curindex++;
        //        if (curindex >= m_targets.Count)
        //        {
        //            curindex = -1;
        //            m_buttle.SetActive(false);
        //        }
        //    }
        //}
        
    }

    public override void End()
    {
        curindex = -1;
        speed = 0f;
        base.End();
    }
    public override void Reset()
    {
        base.Reset();
    }

}
