using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主角用技能表现，自上向下范围伤害
/// </summary>
public class SkillComponentPlayerBullet : SkillComponentBase
{
    Vector3 target;
    float speed;

    public SkillComponentPlayerBullet()
    {
    }

    public override void Trigger()
    {
        base.Trigger();
        Vector3[] path = new Vector3[2];
        path[0] = new Vector3(target.x, target.y + 30, target.z);
        path[1] = target;
        speed = float.Parse(m_data.param2);
        BulletManager.GetInstance().PlayBullet(m_data.param1, speed, path, null, null, m_skillData, m_data, () => {
            MoveEnd();
        });
    }

    public override void Update()
    {
        base.Update();
    }
    public override void SetTarget(Vector3 pos, SkillData data)
    {
        base.SetTarget(pos, data);
        target = pos;
    }
    void MoveEnd() 
    {
        if (!string.IsNullOrEmpty( m_data.param3))
        {
            OneEffect eff = GameUtils.CreateOneEffect(m_data.param3, GameScenesManager.GetInstance().m_effectRoot.transform);
            eff.Play(target, true);
        }
    }

    public override void End()
    {
        base.End();
    }
}
