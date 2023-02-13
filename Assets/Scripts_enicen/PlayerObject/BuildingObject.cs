using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可被打破的墙
public class BuildingObject : ObjectBase
{
    public BuildingObject(ObjectInfoBase info) : base(info) 
    {
        m_fsm = new FSM(this, m_info.m_cfgData);
        m_uihead = new UIHead(m_model.m_mountDic[MountType.UIHead], m_info);
    }
    public override void PlayDie()
    {
        GameUtils.CreateOneEffect("effect_pohuai_hit", GameScenesManager.GetInstance().m_effectRoot.transform).Play(m_model.m_mountDic[MountType.Position].position, true, 5f);
        base.PlayDie();
        GameCore.GetInstance().m_camerashake.DoShake();
    }
}
