using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateBase
{
    //idle和行走时都需要检查锁敌，所以具体逻辑写到具体实体update中
    public IdleState(ObjectBase entity) : base(entity){}
    public override void Enter(object param)
    {
        base.Enter(param);
        m_entity.PlayIdle();
    }

    public override void Update()
    {
        base.Update();
    }
    public override void Leave()
    {
        base.Leave();
    }
    public override void Release()
    {
        base.Release();
    }
}
