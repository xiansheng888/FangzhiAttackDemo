using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelType:byte
{
    Window = 1, //互斥
    Dialog,     //弹框
    Top,
}

public class UIBase
{
    protected GameObject m_go;
    public PanelType m_type = PanelType.Dialog;
    public bool m_IsActive = false;
    string m_prefab;
    protected object[] m_jumpdata;
    public UIBase(string path, PanelType type = PanelType.Dialog)
    {
        m_type = type;
        m_prefab = path;
    }
    public void SetJumpData(params object[] data) 
    {
        m_jumpdata = data;
    }
    public void CreatePanel() 
    {
        m_go = GameObject.Instantiate<GameObject>(ResourcesManager.LoadGameObject(/*@"Assets/Res/UIPrefab/" +*/ m_prefab /*+ ".prefab"*/));
        m_go.name = m_prefab;
        m_go.transform.SetParent(GameCore.GetInstance().m_panelRoots[m_type]);
        m_go.transform.localScale = Vector3.one;
        m_go.transform.localPosition = Vector3.zero;
        m_IsActive = true;
        OnCreate(m_jumpdata);
    }
    public virtual void SetActive(bool active) 
    {
        if (m_go && m_go.activeSelf != active)
        {
            m_IsActive = active;
            m_go.SetActive(active);
        }
        if (active)
        {
            OnActive(active);
        }
    }

    public virtual void OnCreate(object[] data) { }
    public virtual void OnActive(bool active) { }
    public virtual void Update() { }
    public virtual void OnDestory()
    {
        m_IsActive = false;
        GameObject.DestroyImmediate(m_go);
    }
}
