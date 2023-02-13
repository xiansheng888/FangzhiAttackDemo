using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHeroItem
{
    public GameObject m_go;
    Image img_icon;
    Image img_mask;
    Image img_bg;
    GameObject img_click;
    Text text_cost;
    Text text_free;
    Text text_name;

    ObjectData m_data;
    float m_cd = 0;

    public bool m_isDrag = false;
    public bool m_isClick = false;
    public UIHeroItem(GameObject go)
    {
        m_go = go;
        img_bg = UIUtils.GetComponent<Image>(go, "hero/bg");
        img_icon = UIUtils.GetComponent<Image>(go, "hero/icon");
        text_cost = UIUtils.GetComponent<Text>(go, "hero/cost/value");
        img_mask = UIUtils.GetComponent<Image>(go, "hero/mask");
        text_free = UIUtils.GetComponent<Text>(go, "hero/text_free");
        text_name = UIUtils.GetComponent<Text>(go, "hero/text_name");
        img_click = UIUtils.GetGameObject(go, "hero/click");
    }

    public void SetDrag(Action<Vector3, bool, string> dragcb, Action begincb)
    {
        m_isDrag = true;
        UIDragListener m_dragListener = m_go.AddComponent<UIDragListener>();
        m_dragListener.SetData(GameScenesManager.GetInstance().m_battleCamera, (pos, isend, tag) =>
        {
            if (m_cd <= 0 && isend? GameCore.GetInstance().m_gameLogic.GetCanUseEnergy(1, m_data.energy_cost, m_data.id):true)
                dragcb(pos, isend, tag);
        }, () =>
        {
            if (m_cd <= 0 && GameCore.GetInstance().m_gameLogic.GetCanUseEnergy(1, m_data.energy_cost, m_data.id)) begincb();
        });
    }

    public void SetDrag2D(Action<PointerEventData>begin, Action<PointerEventData> drag, Action<PointerEventData,string> end,string tag) 
    {
        m_isDrag = true;
        UIDrag2DListener m_drag = m_go.AddComponent<UIDrag2DListener>();
        m_drag.SetAction(begin, drag, end, tag);
    }

    public void SetClick(Action<ObjectData> clickcb)
    {
        m_isClick = true;
        UIUtils.SetClick(img_click, () => {
            clickcb(m_data);
        });
    }
    public void SetActive(bool active) 
    {
        m_go.SetActive(active);
    }
    public void SetData(ObjectData data,int cnt = 0)
    {
        m_data = data;
        text_cost.text = m_data.energy_cost.ToString();
        text_free.gameObject.SetActive(cnt > 0);
        text_free.text = cnt > 0 ? "Ãâ·Ñ´ÎÊý:" + cnt.ToString() : "";
        text_name.text = data.name;
        UIUtils.SetImage(img_icon, data.icon_ui);
    }

    public void SetCD(float cd) 
    {
        m_cd = cd;
    }

    public void Update() 
    {
        if (m_cd > 0)
        {
            m_cd -= Time.deltaTime;
            if (m_cd <= 0)
            {
                m_cd = 0;
                img_mask.fillAmount = 0;
            }
            else
            {
                img_mask.fillAmount = m_cd / m_data.cd_create;
            }
        }
    }
}
