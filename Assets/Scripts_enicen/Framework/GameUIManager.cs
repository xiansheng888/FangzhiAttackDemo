using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager:Singleton<GameUIManager>
{
    private Dictionary<Type, UIBase> m_uiDic = new Dictionary<Type, UIBase>();

    public void Init()
    {

    }


    public void ShowPanel<T>(params object[] data) where T : UIBase
    {
        Type type = typeof(T);
        if (!m_uiDic.ContainsKey(type))
        {
            T instance = Activator.CreateInstance(type) as T;
            m_uiDic.Add(type, instance);
            m_uiDic[type].SetJumpData(data);
            m_uiDic[type].CreatePanel();
            m_uiDic[type].SetActive(true);
        }
        else
        {
            m_uiDic[type].SetJumpData(data);
            m_uiDic[type].SetActive(true);
        }

        if (m_uiDic[type].m_type == PanelType.Window)
        {
            List<Type> removeList = new List<Type>();
            foreach (var item in m_uiDic)
            {
                if (item.Key != type && item.Value.m_type == PanelType.Window && item.Value.m_IsActive)
                {
                    removeList.Add(item.Key);
                    item.Value.OnDestory();
                }
            }
            for (int i = 0; i < removeList.Count; i++)
            {
                m_uiDic.Remove(removeList[i]);
            }
        }
    }

    public void DestoryPanel<T>() where T : UIBase
    {
        if (m_uiDic.ContainsKey(typeof(T)))
        {
            m_uiDic[typeof(T)].OnDestory();
            m_uiDic.Remove(typeof(T));
        } 
    }
    Dictionary<Type, UIBase>.Enumerator enumerator;
    public void Update()
    {
        enumerator = m_uiDic.GetEnumerator();

        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Value.m_IsActive)
            {
                enumerator.Current.Value.Update();
            }
        }

    }

}
