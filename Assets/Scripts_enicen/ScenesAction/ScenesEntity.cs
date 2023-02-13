using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScenesActionBase
{
    protected ScenesEntity m_entity = null;
    protected List<int> m_nextIndex = null;
    public ActionData m_data;
    public bool m_isActive = false;
    public virtual void Trigger() {
        m_isActive = true;
    }
    public virtual void Checker() {}
    public virtual void Leave()
    {
        m_isActive = false;
        if (m_entity != null) m_entity.NodeFinish(m_data);
        m_entity = null;
        m_nextIndex = null;
        m_data = null;
    }

    public void SetSceneEntityAndData(ScenesEntity entity, ActionData data) 
    {
        m_entity = entity;
        m_data = data;
    }
    public virtual void Release()
    {
        m_entity = null;
        m_nextIndex = null;
        m_data = null;
        m_isActive = false;
    }
}

public class ScenesEntity
{
    Dictionary<int,ObjectBase> m_objectList = new Dictionary<int,ObjectBase>();
    List<ScenesActionBase> m_actionList = new List<ScenesActionBase>();
    //Dictionary<string, List<OneEffect>> m_effects = new Dictionary<string, List<OneEffect>>();
    List<int> m_releaseEntityId = new List<int>();
    SceneActionNode m_data;
    List<int> m_checkAllNode = new List<int>();//所有都完成
    List<int> m_checkOneNode = new List<int>();//只需要达成1个即完成
    List<int> m_finishNode = new List<int>();

    bool m_isStartBattle = false; //判断是否处于侦查阶段
    GameObject m_heroPland;
    bool isHeroPlandShow = false;
    ObjectInfoBase m_plandFoll = null;
    Action m_endCb;

    Dictionary<string, Func<ActionData, ScenesActionBase>> m_typeToActionIns = new Dictionary<string, Func<ActionData, ScenesActionBase>>
    {
        ["start"] = (data)=>
        {
            return new SceneActionStart();
        } ,
        ["time"] = (data) => 
        {
            return new SceneActionTime();
        },
        ["creat"] = (data) =>
        {
            return new SceneActionCreate();
        },
        //["end"] = (data) =>
        //{
        //    return new SceneActionEnd();
        //},

    }; 
    public ScenesEntity(SceneData data,Action cb) 
    {
        m_endCb = cb;
        m_data = GameConfigManager.GetInstance().GetSceneActionNode(data.action_group);
        List<ConfigDataBase> datas = new List<ConfigDataBase> (m_data.m_map.Values) {};
        for (int i = 0; i < datas.Count; i++)
        {
            ActionData oneActData = (ActionData)datas[i];
            if ("end" == oneActData.action_type)
            {
                if (!string.IsNullOrEmpty(oneActData.param1))
                {
                    string[] tmpstr = oneActData.param1.Split('|');
                    for (int j = 0; j < tmpstr.Length; j++)
                    {
                        m_checkAllNode.Add(int.Parse(tmpstr[j]));
                    }
                }

                if (!string.IsNullOrEmpty(oneActData.param2))
                {
                    string[] tmpstr = oneActData.param2.Split('|');
                    for (int j = 0; j < tmpstr.Length; j++)
                    {
                        m_checkOneNode.Add(int.Parse(tmpstr[j]));
                    }
                }
                continue;
            }
            ScenesActionBase actIns = m_typeToActionIns[oneActData.action_type](oneActData);
            actIns.SetSceneEntityAndData(this, oneActData);
            m_actionList.Add(actIns);
        }
        Messenger.GetInstance().AddMessenge(MessgeType.Battle_Create, CreateObject);
        Messenger.GetInstance().AddMessenge(MessgeType.EntityDie, EntityDie);
        Messenger.GetInstance().AddMessenge(MessgeType.Battle_Strike, PlayHindEffect);
        Messenger.GetInstance().AddMessenge(MessgeType.Battle_BuffAbnormal, BattleBuffAbnormal);
        Messenger.GetInstance().AddMessenge(MessgeType.Battle_InfoChange, BattleInfoChange);
        Messenger.GetInstance().AddMessenge(MessgeType.BuffChange, BuffChange);

    }

    private void BuffChange(NotifyData obj) 
    {
        BuffChange data = (BuffChange)obj;
        ObjectBase objbase;
        if (m_objectList.TryGetValue(data.m_entityid, out objbase))
        {
            if (objbase.GetObjectInfo().m_curType != FSMStateType.Die && objbase.m_uihead!= null)
            {
                objbase.m_uihead.RefreshBuff();
            }
            //if (data.m_isAdd)
            //{
                
            //}
        }
    }
    private void BattleInfoChange(NotifyData obj)
    {
        Battle_InfoChange data = (Battle_InfoChange)obj;
        ObjectBase objbase;
        if (m_objectList.TryGetValue(data.entityId,out objbase))
        {
            if (data.type == PropertyType.Speed)
            {
                objbase.PlayRun();
            }
            if (objbase.GetObjectInfo().m_curType != FSMStateType.Die && objbase.m_uihead != null)
            {
                objbase.m_uihead.InfoChange(data.type);
            } 
        }
    }
    private void BattleBuffAbnormal(NotifyData obj)
    {
        Battle_BuffAbnormal data = (Battle_BuffAbnormal)obj;
        ObjectBase objbase;
        if (m_objectList.TryGetValue(data.entityid, out objbase))
        {
            if (objbase.GetObjectInfo().m_curType != FSMStateType.Die)
            {
                objbase.SetAbnormalState(data.proptype,data.data,data.active);
            }
        }
    }
    private void EntityDie(NotifyData data)
    {
        EntityDie diedata = (EntityDie)data;
        if (m_objectList.ContainsKey(diedata.entityId))
        {
            m_objectList[diedata.entityId].ChangeFSMState(FSMStateType.Die);
            m_releaseEntityId.Add(diedata.entityId);
        }
    }
    private void CreateObject(NotifyData data) 
    {
        Battle_Create createdata = (Battle_Create)data;
        ObjectBase entity = GameUtils.CreateEntityInstance(createdata.infobase);
        bool isInto = createdata.infobase.m_cfgData.camp == 1 && createdata.infobase.m_cfgData.profession != 1;
        entity.ChangeFSMState(isInto ? FSMStateType.Init : FSMStateType.Idle);
        m_objectList.Add(createdata.infobase.m_entityId,entity);
        RefreshAllEntityQualityPriority();
    }
    private void PlayHindEffect(NotifyData data) 
    {
        Battle_Strike effdata = (Battle_Strike)data;
        PlayEffect(effdata.effectname,effdata.pos);
    }
    private void PlayEffect(string name,Vector3 pos,float time = 10) 
    {
        //if (! m_effects.ContainsKey(name))
        //{
        //    m_effects[name] = new List<OneEffect>();
        //}
        OneEffect eff = null;
        //for (int i = 0; i < m_effects[name].Count; i++)
        //{
        //    if (!m_effects[name][i].m_isPlay)
        //    {
        //        eff = m_effects[name][i];
        //        break;
        //    }
        //}
        if (eff == null)
        {
            eff = GameUtils.CreateOneEffect(name, GameScenesManager.GetInstance().m_effectRoot.transform);
            //m_effects[name].Add(eff);
        }
        eff.Play(pos,true, time);
    }
    private void RefreshAllEntityQualityPriority() 
    {
        foreach (var item in m_objectList)
        {
            item.Value.RefreshQualityPriority();
        }
    }
    //进入一个副本
    public void Enter() 
    {
        for (int i = 0; i < m_actionList.Count; i++)
        {
            if (m_actionList[i].m_data.action_head == 1)
            {
                m_actionList[i].Trigger();
            }
        }
        m_isStartBattle = false;
        GameAudioManager.GetInstance().SetVolume("bgm_1", 0.2f);
    }
    //释放副本实体
    public void Release()
    {
        GameAudioManager.GetInstance().SetVolume("bgm_1", 1f);
        Messenger.GetInstance().RemoveMessenge(MessgeType.BuffChange, BuffChange);
        Messenger.GetInstance().RemoveMessenge(MessgeType.Battle_InfoChange, BattleInfoChange);
        Messenger.GetInstance().RemoveMessenge(MessgeType.Battle_BuffAbnormal, BattleBuffAbnormal);
        Messenger.GetInstance().RemoveMessenge(MessgeType.Battle_Create, CreateObject);
        Messenger.GetInstance().RemoveMessenge(MessgeType.EntityDie, EntityDie);
        Messenger.GetInstance().RemoveMessenge(MessgeType.Battle_Strike, PlayHindEffect);
        foreach (var item in m_objectList)
        {
            item.Value.Release();
        }
        m_objectList.Clear();
        for (int i = 0; i < m_actionList.Count; i++)
        {
            m_actionList[i].Release();
        }
        //foreach (var item in m_effects)
        //{
        //    for (int i = 0; i < item.Value.Count; i++)
        //    {
        //        item.Value[i].Release();
        //    }
        //}
        //m_effects.Clear();
        m_checkAllNode.Clear();
        m_checkOneNode.Clear();
        m_finishNode.Clear();
        m_actionList.Clear();
        m_actionList = null;
        m_data = null;
        m_checkAllNode = null;
        m_checkOneNode = null;
        m_finishNode = null;
        m_isStartBattle = false;
        GameCore.GetInstance().m_gameLogic.ReleaseEntityData();
        BulletManager.GetInstance().Release();
    }
    //根据ID执行下一步骤
    public void TriggerNodeById(int id) 
    {
        for (int i = 0; i < m_actionList.Count; i++)
        {
            if (id == m_actionList[i].m_data.id)
            {
                if (!m_isStartBattle && m_actionList[i].m_data.battle_execute != 0)
                {
                    continue;
                }
                m_actionList[i].Trigger();
            }
        }
    }
    //每当有节点结束通知一下 用来判断是否结算
    public void NodeFinish(ActionData data)
    {
        bool isOneFinish = m_checkOneNode.Contains(data.id);
        bool isAllFinish = false;
        if (m_checkAllNode.Count > 0)
        {
            m_finishNode.Add(data.id);
            int cnt = 0;
            for (int j = 0; j < m_finishNode.Count; j++)
            {
                if (m_checkAllNode.Contains(m_finishNode[j]))
                {
                    cnt++;
                }
            }
            isAllFinish = cnt == m_checkAllNode.Count;
        }
        
        if (isOneFinish || isAllFinish)
        {

            Action cb = () =>
            {
                if (m_endCb != null)
                {
                    m_endCb();
                }
            };
            foreach (var item in m_objectList)
            {
                if (item.Value.GetObjectInfo().m_curType != FSMStateType.Die)
                {
                    item.Value.ChangeFSMState(FSMStateType.Abnormal);
                }
            }
            GameUIManager.GetInstance().ShowPanel<UIBattleResult>( cb);
        }
    }

    public void ShowHeroPland(bool active, GameObject root, int followId)
    {
        if (active && m_heroPland == null)
        {
            m_heroPland = GameObject.Instantiate(ResourcesManager.LoadGameObject("com_heropland"), root.transform);
        }
        m_plandFoll = m_objectList[followId].GetObjectInfo();
        isHeroPlandShow = active;
        m_heroPland.SetActive(active);
    }
    Dictionary<int, SkillEntity>.Enumerator enumerator;
    public void Update() 
    {
        if (m_objectList != null && m_objectList.Count > 0)
        {
            foreach (var item in m_objectList)
            {
                if (item.Value != null)
                {
                    item.Value.Update();
                }
            }
        }
        if (m_releaseEntityId.Count > 0)
        {
            for (int i = 0; i < m_releaseEntityId.Count; i++)
            {
                m_objectList.Remove(m_releaseEntityId[i]);
            }
            m_releaseEntityId.Clear();
            RefreshAllEntityQualityPriority();
        }
        if (isHeroPlandShow && m_plandFoll!= null)
        {
            m_heroPland.transform.position = m_plandFoll.m_pos;
        }

        if (PlayerData.GetInstance().m_playerskill != null)
        {
            enumerator = PlayerData.GetInstance().m_playerskill.GetEnumerator();
            while (enumerator.MoveNext())
            {
                enumerator.Current.Value.Update();
            }
        }
    }
    public void StartBattle()
    {
        m_isStartBattle = true;
        GameUIManager.GetInstance().ShowPanel<UIBattle>();
        for (int i = 0; i < m_actionList.Count; i++)
        {
            if (m_actionList[i].m_data!= null && m_actionList[i].m_data.battle_execute == 1)
            {
                m_actionList[i].Trigger();
            }
        }
    }
    public ObjectBase GetDemonKingEntity() 
    {
        return m_objectList[GameCore.GetInstance().m_gameLogic.m_myDemonKingInfo.m_entityId];
    }

    public void PlayEntityBuffEffect(int id,string mount,string res,bool isactive, Vector3 pos, Vector3 rotate, Vector3 scale) 
    {
        if (m_objectList.ContainsKey(id)) m_objectList[id].PlayEffect(mount, res, pos, rotate, scale, true, isactive);
    }

}
