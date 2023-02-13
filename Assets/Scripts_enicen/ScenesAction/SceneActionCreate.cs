using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneActionCreate : ScenesActionBase
{
    string m_entityGroup = "";
    List<int> m_curEntityList = null; //当前生成器生成的所有实体

    public override void Trigger()
    {
        base.Trigger();
        m_entityGroup = m_data.param1;
        m_nextIndex = m_data.action_next;
        m_curEntityList = new List<int>();
        Messenger.GetInstance().AddMessenge(MessgeType.EntityDie, EntityDie);
        StartCreate();
    }
    private void EntityDie(NotifyData _data) 
    {
        EntityDie data = (EntityDie)_data;
        if (m_curEntityList.Contains(data.entityId))
        {
            m_curEntityList.Remove(data.entityId);
            Checker();
        }
    }
    private void StartCreate() 
    {
        EntityGroupNode group_node = GameConfigManager.GetInstance().GetEntityGruopNode(m_entityGroup);
        List<ConfigDataBase> datas = new List<ConfigDataBase>(group_node.m_map.Values) { };
        for (int i = 0; i < datas.Count ; i++)
        {
            EntityGroupData data = (EntityGroupData)datas[i];
            int entityId = GameCore.GetInstance().m_gameLogic.CreateObjectEntityByInit(data);
            m_curEntityList.Add(entityId);
        }
    }

    public override void Checker()
    {
        base.Checker();
        if (m_curEntityList.Count == 0)
        {
            Leave();
            if (m_data != null && m_data.action_next != null && m_data.action_next.Count > 0)
            {
                for (int i = 0; i < m_data.action_next.Count; i++)
                {
                    m_entity.TriggerNodeById(m_data.action_next[i]);
                }
            }
            
        }
    }

    public override void Leave()
    {
        base.Leave();
        Messenger.GetInstance().RemoveMessenge(MessgeType.EntityDie, EntityDie);
        if(m_curEntityList != null) m_curEntityList.Clear();
        m_curEntityList = null;
    }

    public override void Release()
    {
        Leave();
        base.Release();
    }
}
