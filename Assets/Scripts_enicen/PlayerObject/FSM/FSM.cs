using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase 
{
    protected ObjectBase m_entity;
    public StateBase(ObjectBase entity) 
    {
        m_entity = entity;
    }

    public virtual void SetData(object data) 
    {
        
    }
    public virtual void Enter(object param) 
    {
    
    }

    public virtual void Update() 
    {
        
    }

    public virtual void Leave() 
    {
        
    }

    public virtual void Release() 
    {
        m_entity = null;
    }
}

public enum FSMStateType
{
    Nil,
    Init,//我放英雄放置时切init
    Idle,
    Move,
    Attack,
    Die,
    Abnormal,//异常
}

public class FSM
{
    Dictionary<FSMStateType, StateBase> m_stateList = new Dictionary<FSMStateType, StateBase>();
    StateBase m_cur;
    public FSMStateType m_type = FSMStateType.Nil;
    public FSMStateType m_lastType = FSMStateType.Nil;
    public ObjectInfoBase m_data;

    bool isRelease = false;
    public FSM(ObjectBase entity, ObjectData data)
    {
        m_data = entity.GetObjectInfo();
        //1魔王 2近战 3远程 4召唤师 5无技能有血阻挡物
        if (data.camp == 1 && data.profession != 1)
        {
            m_stateList.Add(FSMStateType.Init, new IntoState(entity));
        }
        if (data.skill_id != null && data.move_type != 4 && data.skill_id.Count > 0)
        {
            m_stateList.Add(FSMStateType.Move, new FollowState(entity));
        }
        if (data.skill_id != null && data.skill_id.Count >0)
        {
            m_stateList.Add(FSMStateType.Attack, new AttackState(entity));
        }
        if (data.profession != 5)
        {
            m_stateList.Add(FSMStateType.Abnormal, new AbnormalState(entity));
        }

        m_stateList.Add(FSMStateType.Idle, new IdleState(entity));
        m_stateList.Add(FSMStateType.Die, new DieState(entity));
    }
    public void SetCurData(object data) 
    {
        if (m_cur != null)
        {
            m_cur.SetData(data);
        }
    }
    public bool ChangeState(FSMStateType type,object param) 
    {
        bool isSuc = m_stateList.ContainsKey(type);
        if (isSuc && m_type != type)
        {
            m_lastType = m_type;
            m_type = type;
            m_data.m_curType = type;
            if (m_cur != null)m_cur.Leave();
            m_cur = m_stateList[type];
            m_cur.Enter(param);
        }
        return isSuc;
    }
    public void Update() 
    {
        if (m_cur != null)
        {
            m_cur.Update();
        }
        if (isRelease)
        {
            isRelease = false;
            if (m_stateList.Count > 0)
            {
                foreach (var item in m_stateList)
                {
                    item.Value.Release();
                }
            }
            m_stateList.Clear();
            m_stateList = null;
            m_data = null;
        }
    }
    public void Release() 
    {
        isRelease = true;
    }
}
