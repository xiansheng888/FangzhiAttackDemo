using System.Collections.Generic;
using UnityEngine;

public class ObjectInfoBase
{
    public int m_entityId;
    private ObjectData _cfgData;
    public Vector3 m_pos;                                   //位置
    public Vector3 m_rotate;
    public Vector3 m_scale;
    public Vector3 m_strikePos;                             //打击点世界坐标
    public FSMStateType m_curType;                          //当前状态
    public float m_agentRadius = 1f;                        //寻路到达范围
    public int m_qualityPriority = 0;                       //集群寻路避让质量

    public float m_atk_extra = 0;                           //额外攻击 百分比
    public float m_speed_extra = 0;                         //额外速度 百分比
    public float m_atkSpeed_extra = 0;                      //额外攻速 具体值
    public float m_armor = 0;                               //护甲 抵消伤害
    public float m_mp_max;                                  //蓝上限
    public float m_hp_max;                                  //血上限

    private float _hp;
    private float _mp;

    public ObjectData m_cfgData //配置表数据
    {
        set
        {
            _cfgData = value;
            m_hp_max = _cfgData.hp;
            _hp = _cfgData.hp;
            m_mp_max = _cfgData.mp_max;
            _mp = _cfgData.mp_max;
        }
        get { return _cfgData; }
    }

    public float m_speed    //移速
    {
        get { return _cfgData.speed + _cfgData.speed * (m_speed_extra + GameUtils.Speed_Basics_extra); }
    }

    public float m_atkSpeed 
    {
        get { return GameUtils.AtkSpeed_basics + m_atkSpeed_extra; }
    }

    public float m_hp
    {
        get { return _hp; }
        set { _hp = value > m_hp_max ? m_hp_max : value; }
    }
    public float m_mp
    {
        get { return _mp; }
        set { _mp = value > m_mp_max ? m_mp_max : value; }
    }


    public Dictionary<int, SkillEntity> m_skillList = new Dictionary<int, SkillEntity>();
    public Dictionary<int, BuffEffectOnce> m_buffList = new Dictionary<int, BuffEffectOnce>();  //buff

    ~ObjectInfoBase() 
    {
        m_skillList.Clear();
        m_buffList.Clear();
    }
}
