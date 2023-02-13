using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils 
{

    static public float AtkSpeed_basics = 1;    
    static public float Speed_Basics_extra = 0;//专门为gm服务的额外移速 百分比

    static public float self_atk = 0;
    static public float other_atk = 0;

    static public bool self_mp_free = false;
    static public bool other_mp_free = false;

    static public bool createhero_free = false;
    static public bool playerskill_free = false;

    static public bool self_invincible = false;
    static public bool other_invincible = false;

    static public float energy_basics = 100;
    static public float energy_max_basics = 100;



    static public OneEffect CreateOneEffect(string path, Transform parent)
    {
        GameObject effres = ResourcesManager.LoadGameObject(path);
        if (!effres) Debug.LogError("加载资源错误，没有资源 " + path);
        if (effres)
        {
            OneEffect onece = GameObject.Instantiate(effres, parent).AddComponent<OneEffect>();
            onece.gameObject.name = path;
            return onece;
        }
        return null;
    }

    //profession 1魔王 2近战 3远程 4塔 5无技能有血阻挡物
    static public Dictionary<int, Func<ObjectInfoBase, ObjectBase>> m_createFunc = new Dictionary<int, Func<ObjectInfoBase, ObjectBase>>()
    {
        [1] = (data) => { return new DemonKingObject(data); },
        [2] = (data) => { return new MonsterObject(data); },
        [3] = (data) => { return new ShooterObject(data); },
        [4] = (data) => { return new TowerObject(data); },
        [5] = (data) => { return new BuildingObject(data); },
    };
    static public ObjectBase CreateEntityInstance(ObjectInfoBase data)
    {
        ObjectBase entity = m_createFunc[data.m_cfgData.profession](data);
        return entity;
    }
    static public GameObject FindChild(GameObject go,string name) 
    {
        Transform child = go.transform.Find(name);
        if (child)
        {
            return child.gameObject;
        }
        for (int i = 0; i < go.transform.childCount; i++)
        {
            GameObject tmp = FindChild(go.transform.GetChild(i).gameObject,name);
            if (tmp != null)
            {
                return tmp;
            }
        }
        return null;
    }


}
