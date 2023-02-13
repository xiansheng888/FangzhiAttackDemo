using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuffType : byte 
{
    Null,
    Summon = 1,//�ٻ�
    Value = 2,//�޸���ֵ ���� ����...
    ValueSustain = 3,//�����޸���ֵ  ���� ����...
    Abnormal = 4,   //�쳣״̬
}
/// <summary>
/// �����������
/// </summary>
public enum SkillComponentType : byte
{
    AnimClip = 1,//����Ƭ��
    Effect = 2,//��Ч
    Bullet = 3,//�ӵ�
    Audio = 4,//��Ч
    Move = 5,//λ��
    Line = 6,//����
    Ball = 7,//����
    PlayerBullet = 8,//��Ҽ����� ��������
    MagicCircle = 9,//���� ����buff
}
/// <summary>
/// ��ɫ�ҵ�
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
    Armor = 3, //����
    Atk = 4,    //������
    Speed = 5,  //�ٶ�
    AtkSpeed = 6,//����
    Frozen = 7, //����
    Silence = 8, //��Ĭ
}