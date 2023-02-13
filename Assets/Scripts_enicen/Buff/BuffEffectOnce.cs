using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEffectOnce : BuffChekerBase
{
    Timer m_timer;
    PropertyType proptype;
    float value = 0;
    bool isTrigger = false;
    List<ObjectInfoBase> entitys_influenced = new List<ObjectInfoBase>();
    List<OneEffect> effects = new List<OneEffect>();
    public BuffEffectOnce(int id,ObjectInfoBase data,Vector3 pos) : base(id,data, pos) {}
    public override void Start()
    {
        base.Start();
        int type = 0;
        if (int.TryParse(m_data.param1, out type))
        {
            proptype = (PropertyType)type;
        }
        if (float.TryParse(m_data.param2, out value))
        {
            value = float.Parse(m_data.param2);
        }
        else 
        {
            value = 1;
        }

        ShowBuffEffect();
        StartCheck();
    }
    public void ShowBuffEffect()
    {
        if (!string.IsNullOrEmpty(m_data.obj_show))
        {
            if (!string.IsNullOrEmpty(m_data.superposition))
            {
                GameScenesManager.GetInstance().PlayEntityBuffEffect(m_info.m_entityId, m_data.superposition, m_data.obj_show, true,Vector3.zero,Vector3.zero,Vector3.one);
            }
            else
            {
                OneEffect one = GameUtils.CreateOneEffect(m_data.obj_show, GameScenesManager.GetInstance().m_effectRoot.transform);
                effects.Add(one);
                one.Play(m_scenePos, true, m_data.time_duration < 0 ? 9999 : m_data.time_duration);
            }
        }
    }

    public void StartCheck() 
    {
        float frequency = 0;
        float.TryParse(m_data.param3, out frequency);
        if (frequency > 0)
        {
            m_timer = TimerUtils.StartTimer(frequency, false, () =>
            {
                bool isTrigger = true;
                if (m_type == BuffType.Summon && m_data.distance > 0)
                {
                    isTrigger = GameCore.GetInstance().m_gameLogic.CheckRangeEntityByCamp(m_info.m_pos, m_data.distance, m_info.m_cfgData.camp);
                }
                if (isTrigger) Trigger();
            });
        }
        else
        {
            Trigger();
        }

        if (m_data.time_duration > 0)
        {
            TimerUtils.StartTimer(m_data.time_duration, true, () =>
            {
                if (m_timer) m_timer.Stop();
                m_timer = null;
                End();
            });
        }
    }

    public override void Trigger()
    {
        base.Trigger();
        isTrigger = true;
        if (m_type == BuffType.Summon)
        {
            if(m_info!= null) GameCore.GetInstance().m_gameLogic.SummonCreateEntity(m_data.param1,m_info.m_pos);
        }
        else
        {
            if (m_data.lock_type == 2 && m_data.distance>0)
            {
                entitys_influenced = GameCore.GetInstance().m_gameLogic.GetEntitysByPointRange(m_info != null ? m_info.m_pos : m_scenePos, m_data.distance, m_data.camp);
            }
            else
            {
                if(m_info != null) entitys_influenced.Add(m_info);
            }
            for (int i = 0; i < entitys_influenced.Count; i++)
            {
                AddPropty(proptype, value, entitys_influenced[i]);
            }
        }
    }

    private void AddPropty(PropertyType type, float value,ObjectInfoBase target) 
    {
        if (type == PropertyType.HP)
        {
            target.m_hp += value;
        }
        if (type == PropertyType.Armor)
        {
            target.m_armor += value;
        }
        if (type == PropertyType.Atk)
        {
            target.m_atk_extra += value;
        }
        if (type == PropertyType.Speed)
        {
            target.m_speed_extra += value;
        }
        if (type == PropertyType.AtkSpeed)
        {
            target.m_atkSpeed_extra += value;
        }
        if (type == PropertyType.Frozen || type == PropertyType.Silence)
        {
            Messenger.GetInstance().Broadcast(new Battle_BuffAbnormal(target.m_entityId, m_data, value > 0, proptype));
        }
        if (type != PropertyType.Nil)
        {
            Messenger.GetInstance().Broadcast(new Battle_InfoChange(target.m_entityId, proptype, value));
        }
    }
    public override void End()
    {
        if (m_info != null && !string.IsNullOrEmpty(m_data.obj_show)) GameScenesManager.GetInstance().PlayEntityBuffEffect(m_info.m_entityId, "", m_data.obj_show, false,Vector3.zero, Vector3.zero, Vector3.one);
        if (m_timer) m_timer.Stop();
        m_timer = null;
        if ( m_data != null)
        {
            if (isTrigger)
            {
                if (m_data.is_reset == 1)
                {
                    if (proptype == PropertyType.Speed || proptype == PropertyType.Atk) //临时增长的属性，在结束buff的时候返回属性
                    {
                        for (int i = 0; i < entitys_influenced.Count; i++)
                        {
                            if(entitys_influenced[i].m_curType != FSMStateType.Die) AddPropty(proptype, value * -1, entitys_influenced[i]);
                        }
                    }
                }
                if (m_type == BuffType.Abnormal)
                {
                    
                }
            }
            GameCore.GetInstance().m_gameLogic.RemoveBuff(m_info!= null ? m_info.m_entityId : -1, m_data,m_info != null);
        }
        entitys_influenced.Clear();
        for (int i = 0; i < effects.Count; i++)
        {
            if(effects[i]) effects[i].Release();
        }
        effects.Clear();
        base.End();
    }

}
