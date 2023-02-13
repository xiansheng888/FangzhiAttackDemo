using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuffType : byte 
{
    Null,
    Summon = 1,//召唤
    Value = 2,//修改数值 减速 减防...
    ValueSustain = 3,//持续修改数值  灼烧 穿刺...
    Abnormal = 4,   //异常状态
}
/// <summary>
/// 技能组件类型
/// </summary>
public enum SkillComponentType : byte
{
    AnimClip = 1,//动画片段
    Effect = 2,//特效
    Bullet = 3,//子弹
    Audio = 4,//音效
    Move = 5,//位移
    Line = 6,//连线
    Ball = 7,//弹球
    PlayerBullet = 8,//玩家技能用 自上向下
    MagicCircle = 9,//法阵 关联buff
}
/// <summary>
/// 角色挂点
/// </summary>
public enum MountType : byte
{
    Position,
    UIHead,
    LeftHand,
    Bullet,
    Strike,
}

public enum PropertyType : byte 
{
    Nil = 0,
    HP = 1,
    MP = 2,
    Armor = 3, //护甲
    Atk = 4,    //攻击力
    Speed = 5,  //速度
    AtkSpeed = 6,//攻速
    Frozen = 7, //冰冻
    Silence = 8, //沉默
}