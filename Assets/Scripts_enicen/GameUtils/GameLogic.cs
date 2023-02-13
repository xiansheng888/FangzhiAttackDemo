using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameLogic : MonoBehaviour
{
    Dictionary<int, ObjectInfoBase> m_EntityDatas = new Dictionary<int, ObjectInfoBase>();
    List<int> m_entityQualityList = new List<int>();//集群寻路实体的质量列表 index就是Priority值

    public ObjectInfoBase m_myDemonKingInfo = null;
    public ObjectInfoBase m_enemyDemonKingInfo = null;
    public List<BuffEffectOnce> m_sceneBuffs = new List< BuffEffectOnce>();//特殊场景buff
    int m_curEntityId = -1;

    public void ResetEntityMP(int camp) 
    {
        foreach (var item in m_EntityDatas)
        {
            if (item.Value.m_cfgData.camp == camp)
            {
                item.Value.m_mp = item.Value.m_mp_max;
            }
        }
    }

    public void ReleaseEntityData() 
    {
        m_curEntityId = -1;
        m_EntityDatas.Clear();
        m_entityQualityList.Clear();
    }

    public void AddOneEntity(int entityid,int cfgid,Vector3 pos,Vector3 rotate,Vector3 scale) 
    {
        ObjectInfoBase data = new ObjectInfoBase();
        data.m_entityId = entityid;
        data.m_pos = pos;
        data.m_rotate = rotate;
        data.m_scale = scale;
        data.m_cfgData = GameConfigManager.GetInstance().GetItem<ObjectData>("D_ObjectBase", cfgid);
        m_EntityDatas[entityid] = data;
        if (data.m_cfgData.move_type == 1)
        {
            if (data.m_cfgData.profession == 1)
            {
                m_entityQualityList.Insert(m_entityQualityList.Count, entityid);
            }
            else
                m_entityQualityList.Insert(0, entityid);
        }
        Messenger.GetInstance().Broadcast(new Battle_Create(m_EntityDatas[entityid]));
    }
    /// <summary>
    /// 召唤物
    /// </summary>
    private int CreateObjectEntityBySummon(EntityGroupData data,Vector3 pos) 
    {
        m_curEntityId++;
        int entityid = m_curEntityId;
        pos.x += UnityEngine.Random.Range(-3, 4);
        pos.z += UnityEngine.Random.Range(-3, 4);
        AddOneEntity(entityid, data.object_id, pos, new Vector3(data.rotate[0], data.rotate[1], data.rotate[2]), new Vector3(data.scale[0], data.scale[1], data.scale[2]));
        return entityid;
    }
    /// <summary>
    /// 初始化场景
    /// </summary>
    public int CreateObjectEntityByInit(EntityGroupData data)
    {
        m_curEntityId++;
        int entityid = m_curEntityId;
        AddOneEntity(entityid, data.object_id, new Vector3(data.pos[0], data.pos[1], data.pos[2]), new Vector3(data.rotate[0], data.rotate[1], data.rotate[2]), new Vector3(data.scale[0], data.scale[1], data.scale[2]));
        if (m_EntityDatas[entityid].m_cfgData.profession == 1)
        {
            if (m_EntityDatas[entityid].m_cfgData.camp == 1)
            {
                m_myDemonKingInfo = m_EntityDatas[entityid];
            }
            else
            {
                m_enemyDemonKingInfo = m_EntityDatas[entityid];
            }
            m_EntityDatas[entityid].m_qualityPriority = 99;
        }
        return entityid;
    }
    /// <summary>
    /// 种英雄
    /// </summary>
    public int CreateObjectHero(int id,Vector3 pos,Vector3 scale)
    {
        ObjectData cfg = GameConfigManager.GetInstance().GetItem<ObjectData>("D_ObjectBase", id);
        bool isCreate = PlayerData.GetInstance().m_heroCreateCount[cfg.id] > 0 || PlayerData.GetInstance().m_energy >= cfg.energy_cost;
        if (isCreate)
        {
            m_curEntityId++;
            int entityid = m_curEntityId;
            if (!GameUtils.createhero_free)
            {
                if (PlayerData.GetInstance().m_heroCreateCount[cfg.id] > 0)
                    PlayerData.GetInstance().m_heroCreateCount[cfg.id]--;
                else
                    PlayerData.GetInstance().m_energy -= cfg.energy_cost;
            }
            AddOneEntity(entityid, id, pos, cfg.drag_rotate, scale);
            if (m_EntityDatas[entityid].m_cfgData.buff_passive != null)
            {
                for (int i = 0; i < m_EntityDatas[entityid].m_cfgData.buff_passive.Count; i++)
                {
                    AddBuff(m_EntityDatas[entityid].m_cfgData.buff_passive[i], entityid);
                }
            }
            return entityid;
        }
        return -1;
    }

    public void SummonCreateEntity(string groupname,Vector3 pos) 
    {
        EntityGroupNode group_node = GameConfigManager.GetInstance().GetEntityGruopNode(groupname);
        List<ConfigDataBase> datas = new List<ConfigDataBase>(group_node.m_map.Values) { };
        for (int i = 0; i < datas.Count; i++)
        {
            EntityGroupData data = (EntityGroupData)datas[i];
            int entityId = GameCore.GetInstance().m_gameLogic.CreateObjectEntityBySummon(data,pos);
        }
    }
    /// <summary>
    /// 弹弹球
    /// </summary>
    public List<ObjectInfoBase> BallCheckLockTargets(ObjectInfoBase data,int cnt,float range, List<int> target_type) 
    {
        List<ObjectInfoBase> datas = new List<ObjectInfoBase>();
        datas.Add(data);
        ObjectInfoBase cur = data;

        Func<int, bool> IsHave = (entityid) =>
         {
             bool ishave = false;
             for (int i = 0; i < datas.Count; i++)
             {
                 if (entityid == datas[i].m_entityId)
                 {
                     ishave = true;
                 }
             }
             return ishave;
         };

        float lastdis = -1f;
        float curdis = 0f;
        ObjectInfoBase target = null;
        for (int i = 0; i < cnt-1; i++)
        {
            lastdis = -1f;
            target = null;
            foreach (var item in m_EntityDatas)
            {
                curdis = Vector3.Distance(cur.m_pos, item.Value.m_pos);
                if (cur.m_cfgData.camp == item.Value.m_cfgData.camp && 
                    (lastdis == -1 || curdis < lastdis) &&
                    cur.m_entityId != item.Value.m_entityId &&
                    !IsHave(item.Value.m_entityId)&&
                    curdis <= range && target_type.Contains(item.Value.m_cfgData.type))
                {
                    target = item.Value;
                    lastdis = curdis;
                }
            }
            if (target != null)
            {
                cur = target;
                datas.Add(target);
            }
        }
        //Debug.Log("球锁敌-----------------------");
        //for (int i = 0; i < datas.Count; i++)
        //{
        //    Debug.Log(string.Format("球锁敌 " + datas[i].m_entityId));
        //}
        return datas;
    }
    public ObjectInfoBase TowerChekLockTarget(ObjectInfoBase data, List<int> target_type) 
    {
        float range = -1;
        float currange = 0;
        ObjectInfoBase target = null;
        foreach (var item in m_EntityDatas)
        {
            if (item.Value.m_cfgData.camp != data.m_cfgData.camp && item.Value.m_hp > 0 && target_type.Contains(item.Value.m_cfgData.type))
            {
                currange = Vector3.Distance(data.m_pos, item.Value.m_pos);
                if (currange <= data.m_cfgData.att_range)
                {
                    if (range == -1 || currange<range)
                    {
                        target = item.Value;
                    }
                }
            }
        }
        return target;
    }
    public ObjectInfoBase CheckLockTarget(ObjectInfoBase data) 
    {
        ObjectInfoBase target = null;
        float lastdis = -1;
        if ((data.m_curType != FSMStateType.Init && data.m_curType != FSMStateType.Die))
        {
            List<int> types = new List<int>();
            if (data.m_skillList.Count > 0)
            {
                foreach (var item1 in data.m_skillList)
                {
                    if (types.Count == 0) types = item1.Value.m_cfgData.target_type;
                }
            }
            
            foreach (var item in m_EntityDatas)
            {
                if (item.Value.m_curType != FSMStateType.Die && item.Value.m_hp > 0 && item.Value.m_cfgData.camp != data.m_cfgData.camp&& types.Contains(item.Value.m_cfgData.type))
                {
                    float dis = Vector3.Distance(data.m_pos, item.Value.m_pos);
                    if (dis <= 20 && (lastdis < 0 || dis < lastdis))
                    {
                        lastdis = dis;
                        target = item.Value;
                    }
                }
            }
        }
        if (target == null)
        {
            target = data.m_cfgData.camp == 1 ? m_enemyDemonKingInfo : m_myDemonKingInfo;
        }
        return target;
    }

    public void NavMeshCheckLockTarget(ObjectInfoBase data,int areaMask, Action<ObjectInfoBase> end) 
    {
        Dictionary<int, ObjectInfoBase> entitys = new Dictionary<int, ObjectInfoBase>();
        float lastdis = -1;
        if ((data.m_curType != FSMStateType.Init && data.m_curType != FSMStateType.Die))
        {
            List<int> types = new List<int>();
            if (data.m_skillList.Count > 0)
            {
                foreach (var item1 in data.m_skillList)
                {
                    if (types.Count == 0) types = item1.Value.m_cfgData.target_type;
                }
            }

            foreach (var item in m_EntityDatas)
            {
                if (item.Value.m_curType != FSMStateType.Die && item.Value.m_hp > 0 && item.Value.m_cfgData.camp != data.m_cfgData.camp && types.Contains(item.Value.m_cfgData.type))
                {
                    if (Vector3.Distance( data.m_pos,item.Value.m_pos)<=(data.m_cfgData.att_range < 25 ? 25:data.m_cfgData.att_range))
                    {
                        entitys.Add(item.Value.m_entityId, item.Value);
                    }
                }
            }
        }
        if (entitys.Count == 0)
        {
            end(m_enemyDemonKingInfo);
            return;
        }
        GameObject go = new GameObject("NavmeshPath");
        NavMeshPathUtlis tmp = go.AddComponent<NavMeshPathUtlis>();
        tmp.SetPoint(data,areaMask, entitys, end);
    }

    public bool CheckRangeEntityByCamp(Vector3 pos,float range,int camp) 
    {
        foreach (var item in m_EntityDatas)
        {
            if (item.Value.m_cfgData.camp != camp && Vector3.Distance(item.Value.m_pos,pos) <= range)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckAttack(ObjectInfoBase attacker, ObjectInfoBase target) 
    {
        float dis = Vector3.Distance(attacker.m_pos, target.m_pos);
        float AtkDis = attacker.m_cfgData.att_range + target.m_agentRadius + attacker.m_agentRadius;
        //Debug.Log("attacker:"+attacker.m_pos + "target:"+target.m_pos);
        //Debug.Log("表显距离 " + attacker.m_cfgData.att_range + "  自身半径 " + attacker.m_agentRadius + "  目标半径 " + target.m_agentRadius);
        //Debug.Log("判断攻击  真实:" + dis + " 攻击:" + AtkDis + "  --- " + attacker.m_entityId + "->" + target.m_entityId);
        return target != null && dis <= AtkDis && attacker.m_curType != FSMStateType.Die && target.m_curType != FSMStateType.Die;
    }
    /// <summary>
    /// 群体数值改变buff支持
    /// </summary>
    /// <returns></returns>
    public List<ObjectInfoBase> GetEntitysByPointRange(Vector3 pos,float range,int camp) 
    {
        List<ObjectInfoBase> entitys = new List<ObjectInfoBase>();
        foreach (var item in m_EntityDatas)
        {
            if (item.Value.m_cfgData.camp == camp && Vector3.Distance( item.Value.m_pos,pos)<= range)
            {
                entitys.Add(item.Value);
            }
        }
        return entitys;
    }
    public bool GetCanUseEnergy(int type,float energy,int id) 
    {
        int freeCnt = type == 1 ? 
            PlayerData.GetInstance().m_heroCreateCount[id] 
            : PlayerData.GetInstance().m_playerSkillCount[id];
        if (freeCnt <= 0)
        {
            return PlayerData.GetInstance().m_energy >= energy;
        }
        return true;
    }
    public void UsePlayerSKill(SkillData data)
    {
        if (PlayerData.GetInstance().m_playerSkillCount[data.id] > 0)
            PlayerData.GetInstance().m_playerSkillCount[data.id]--;
        else
        {
            PlayerData.GetInstance().m_energy -= UIBattleData.GetInstance().GetCostValue(data.id);
            UIBattleData.GetInstance().UsePlayerSkill(data.id);
        }
    }

    /// <summary>
    /// 打击一个目标
    /// </summary>
    public void AttackByPoint(ObjectInfoBase attacker, ObjectInfoBase target, SkillData data) 
    {
        RealAttackByPoint(attacker, target, data);
    }
    /// <summary>
    /// 范围伤害
    /// </summary>
    public void AttackByRange(Vector3 atkpoint,ObjectInfoBase attacker,SkillData data,float range) 
    {
        foreach (var item in m_EntityDatas)
        {
            bool isAttack = attacker != null ? (item.Value.m_cfgData.camp != attacker.m_cfgData.camp) : (item.Value.m_cfgData.camp == 2);
            if (isAttack)
            {
                if (Vector3.Distance(atkpoint, item.Value.m_pos) <= range)
                {
                    RealAttackByPoint(attacker, item.Value, data);
                }
            }
        }
    }
    private void RealAttackByPoint(ObjectInfoBase attacker, ObjectInfoBase target, SkillData data)
    {
        if (!data.target_type.Contains(target.m_cfgData.type))return;
        //Debug.Log("计算攻击 攻击者：" + attacker == null ? "玩家" :attacker.m_entityId + "---受击者:" + target.m_entityId + "---技能:" + data.id);
        if (attacker != null && data.mp_cost>0)
        {
            bool isMpCost = attacker.m_cfgData.camp == 1 ? !GameUtils.self_mp_free : !GameUtils.other_mp_free;
            if (isMpCost)
            {
                attacker.m_mp -= data.mp_cost;
                Messenger.GetInstance().Broadcast(new Battle_InfoChange(attacker.m_entityId, PropertyType.MP));
            }
        }
        if (target != null && target.m_curType != FSMStateType.Die)
        {
            float real_atk = data.att_value + data.att_value * (attacker != null ? attacker.m_atk_extra : 1); //计算攻击力
            float real_harm = real_atk - target.m_armor; //扣除抵消伤害
            if (real_harm < 0) real_harm = 0;
            if ((GameUtils.self_invincible && target.m_cfgData.camp == 1) || (GameUtils.other_invincible && target.m_cfgData.camp == 2)) 
                real_harm = 0;    
            target.m_hp -= (real_harm + (target.m_cfgData.camp == 1?GameUtils.other_atk : GameUtils.self_atk)); //最终伤害
            if (real_harm > 0)
            {
                Messenger.GetInstance().Broadcast(new Battle_InfoChange(target.m_entityId, PropertyType.HP));
            }
            if (!string.IsNullOrEmpty(data.att_effect))
            {
                Messenger.GetInstance().Broadcast(new Battle_Strike(data.att_effect, target.m_strikePos));
            }
            CheckBuff(data.add_prob,data.add_id, target);
            CheckBuff(data.add_prob_self,data.add_id_self, attacker);
            if (target.m_hp <= 0.01f)
            {
                TimerUtils.StartTimer(0.1f, true, () => {
                    EntityDie(target.m_entityId);
                });
            }
        }
    }
    private void CheckBuff(List<float> probs,List<int> ids,ObjectInfoBase check_target) 
    {
        bool isExist = check_target != null && check_target.m_hp > 0;
        if (isExist && probs.Count > 0 && ids.Count > 0)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                float result = UnityEngine.Random.Range(0.01f, 1.01f);
                if (true)
                {
                    if (result >= (1 - probs[i]))
                    {
                        AddBuff(ids[i] ,check_target.m_entityId);
                    }
                }
            }
        }
    }
    private void EntityDie(int id)
    {
        bool ismove = true;
        if (m_EntityDatas.ContainsKey(id)) ismove = m_EntityDatas[id].m_cfgData.move_type == 1;
        if (m_entityQualityList.Contains(id) && ismove) m_entityQualityList.Remove(id);
        if (m_EntityDatas.ContainsKey(id)) m_EntityDatas.Remove(id);
        Messenger.GetInstance().Broadcast(new EntityDie(id));
    }

    public void AddBuff( int buffid, int entityid = -1, Vector3 pos = default(Vector3)) 
    {
        Debug.Log("给" + entityid + "+ buff " + buffid);
        BuffEffectOnce one = null;
        if (entityid > 0)
        {
            if (m_EntityDatas.ContainsKey(entityid))
            {
                if (m_EntityDatas[entityid].m_buffList.ContainsKey(buffid))
                {
                    m_EntityDatas[entityid].m_buffList[buffid].End();
                }
                one = new BuffEffectOnce(buffid, m_EntityDatas[entityid], Vector3.zero);
                one.Start();
                m_EntityDatas[entityid].m_buffList[buffid] = one;
            }
        }
        else 
        {
            one = new BuffEffectOnce(buffid, null, pos);
            one.Start();
            m_sceneBuffs.Add(one);
        }
        Messenger.GetInstance().Broadcast(new BuffChange(entityid, true, one.m_data));
    }

    public void RemoveBuff(int entityid, BuffData buffdata,bool isEntityBuff)
    {
        if (isEntityBuff)
        {
            if (m_EntityDatas.ContainsKey(entityid))
            {
                m_EntityDatas[entityid].m_buffList.Remove(buffdata.id);
            }
        }
        else 
        {
            int index = -1;
            for (int i = 0; i < m_sceneBuffs.Count; i++)
            {
                if (m_sceneBuffs[i].m_data.id == buffdata.id)
                {
                    index = i;
                    break;
                }
            }
            if (index > -1)
            {
                m_sceneBuffs.RemoveAt(index);
            }
        }
        Messenger.GetInstance().Broadcast(new BuffChange(entityid, false, buffdata));
    }


    public void StartBattle()
    {
        PlayerData.GetInstance().RefreshDefCount();
        foreach (var item in m_EntityDatas)
        {
            if (item.Value.m_cfgData.buff_passive != null)
            {
                for (int i = 0; i < item.Value.m_cfgData.buff_passive.Count; i++)
                {
                    AddBuff(item.Value.m_cfgData.buff_passive[i],item.Value.m_entityId);
                }
            }
        }
    }

    public int GetQualityPriorityByEntityid(int id) 
    {
        for (int i = 0; i < m_entityQualityList.Count; i++)
        {
            if (id==m_entityQualityList[i])
            {
                return 99 - i;
            }
        }
        return 99;
    }


}
