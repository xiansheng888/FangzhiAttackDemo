using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum BulletMoveType 
{
    Line = 1,
    Parabola = 2,
}
public class SkillComponentBullet : SkillComponentBase
{
    Vector3 targetpos;
    float speed;
    BulletMoveType type = BulletMoveType.Line;
    string boomEffect = ""; //抛物线子弹落地特效

    public override void Trigger()
    {
        base.Trigger();
        if (m_obje != null && m_data != null)
        {
            if (m_obje.m_model.m_mountDic[MountType.Bullet])
            {
                int tmp;
                if (int.TryParse(m_data.param3, out tmp)) type = (BulletMoveType)int.Parse(m_data.param3);
                if (m_data.param6 == "1") targetpos = m_obje.m_target.m_pos; else targetpos = m_obje.m_target.m_strikePos;
                speed = float.Parse(m_data.param2);
                boomEffect = m_data.param4;
                int resolution = 10;//贝塞尔曲线 路径点数量
                Vector3[] _path = new Vector3[type == BulletMoveType.Parabola ? resolution : 2];
                if (type == BulletMoveType.Parabola)
                {
                    var bezierControlPoint = (m_obje.m_model.m_mountDic[MountType.Bullet].position + targetpos) * 0.5f + (Vector3.up * (string.IsNullOrEmpty(m_data.param5) ?10:float.Parse(m_data.param5)));
                    for (int i = 0; i < resolution; i++)
                    {
                        _path[i] = GetBezierPoint((i + 1) / (float)resolution, m_obje.m_model.m_mountDic[MountType.Bullet].position, bezierControlPoint, targetpos);
                    }
                }
                else
                {
                    _path[0] = m_obje.m_model.m_mountDic[MountType.Bullet].position;
                    _path[1] = targetpos;
                    
                }

                BulletManager.GetInstance().PlayBullet(m_data.param1, speed, _path, m_obje.GetObjectInfo(), m_obje.m_target, m_skillData, m_data,MoveEnd);
            }
            else
            {
                Debug.Log("挂点找不到 entityid:"+ m_obje.m_model.gameObject.name);
            }
        }
    }

    private void MoveEnd()
    {
        if (type == BulletMoveType.Parabola && !string.IsNullOrEmpty(boomEffect))
        {
            OneEffect eff = GameUtils.CreateOneEffect(boomEffect, GameScenesManager.GetInstance().m_effectRoot.transform);
            if(eff) eff.Play(targetpos, true);
        }
    }

    Vector3 GetBezierPoint(float t, Vector3 start, Vector3 center, Vector3 end)
    {
        return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void End()
    {
        base.End();
    }
    public override void Reset()
    {
        base.Reset();
    }
}
