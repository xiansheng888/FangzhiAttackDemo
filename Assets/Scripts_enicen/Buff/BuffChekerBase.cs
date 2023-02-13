using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffChekerBase
{
    public ObjectInfoBase m_info;
    public BuffData m_data;
    public BuffType m_type = BuffType.Null;
    public Vector3 m_scenePos = Vector3.zero;
    public BuffChekerBase(int id, ObjectInfoBase _info,Vector3 _scenePos) 
    {
        m_info = _info;
        m_scenePos = _scenePos;
        m_data = GameConfigManager.GetInstance().GetItem<BuffData>("D_Buff", id);
        m_type = (BuffType)m_data.type;
    }

    public virtual void Start() { }

    public virtual void Trigger() { }

    public virtual void End() 
    {
        m_info = null;
        m_data = null;
    }
}
