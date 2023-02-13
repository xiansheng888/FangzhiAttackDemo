using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MessgeType
{
    EntityDie = 1,
    SceneTime = 3,  //场景倒计时
    BuffChange = 4, //buff新增、移除

    Battle_Create = 100,//创建
    Battle_InfoChange = 101,//属性变化
    Battle_Strike = 102,//受击
    Battle_BuffAbnormal = 103,

}
public class EntityDie : NotifyData
{
    public int entityId;
    public EntityDie(int id) : base(MessgeType.EntityDie)
    {
        entityId = id;
    }
}


public class SceneTime : NotifyData 
{
    public float m_time;

    public SceneTime(float time) : base( MessgeType.SceneTime)
    {
        m_time = time;
    }
}


public class Battle_Create : NotifyData 
{
    public ObjectInfoBase infobase;
    public Battle_Create(ObjectInfoBase info):base(MessgeType.Battle_Create) 
    {
        infobase = info;
    }
}



public class Battle_InfoChange : NotifyData 
{
    public int entityId;
    public PropertyType type = PropertyType.Nil;
    public object value = null;
    public Battle_InfoChange(int id,PropertyType _type,object _value = null) : base(MessgeType.Battle_InfoChange) 
    {
        entityId = id;
        type = _type;
        value = _value;
    }
}


public class Battle_Strike : NotifyData
{
    public Vector3 pos;
    public string effectname;
    public Battle_Strike(string resname,Vector3 _pos) : base(MessgeType.Battle_Strike)
    {
        effectname = resname;
        pos = _pos;
    }
}

public class Battle_BuffAbnormal : NotifyData //切入异常状态
{
    public int entityid;
    public BuffData data;
    public bool active;
    public PropertyType proptype;

    public Battle_BuffAbnormal(int _entityid, BuffData _data, bool _active, PropertyType _proptype) : base(MessgeType.Battle_BuffAbnormal)
    {
        entityid = _entityid;
        data = _data;
        active = _active;
        proptype = _proptype;
    }
}

public class BuffChange : NotifyData
{
    public int m_entityid;
    public bool m_isAdd;
    public BuffData m_buffData;
    public BuffChange(int entityid,bool isAdd, BuffData buffData) : base(MessgeType.BuffChange)
    {
        m_entityid = entityid;
        m_isAdd = isAdd;
        m_buffData = buffData;
    }
}