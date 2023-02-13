using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillItem
{
    GameObject m_go;
    SkillData m_data;

    Image m_icon;
    Image m_mask;
    Text m_free;
    GameObject m_costgo;
    Text m_cost;

    public float cd;

    public UISkillItem(GameObject go)
    {
        m_go = go;
        m_go.SetActive(true);
        m_icon = UIUtils.GetComponent<Image>(m_go, "icon");
        m_mask = UIUtils.GetComponent<Image>(m_go, "mask_cd");
        m_free = UIUtils.GetComponent<Text>(m_go, "text_free");
        m_costgo = UIUtils.GetGameObject(m_go, "cost");
        m_cost = UIUtils.GetComponent<Text>(m_costgo, "text_cost");
        ResetCD();
    }
    public void SetActive(bool active) 
    {
        m_go.SetActive(active);
    }
    public void SetData(SkillData data,float cost,int free_cnt = 0)
    {
        m_data = data;
        m_free.text = free_cnt > 0 ? "Ãâ·Ñ´ÎÊý:" + free_cnt:"";
        m_free.gameObject.SetActive(free_cnt > 0);
        m_cost.text = cost > 0 ? cost.ToString() : "";
        m_costgo.SetActive(cost > 0);
        UIUtils.SetImage(m_icon, m_data.show_icon);
    }
    public void SetDrag(Action<Vector3, bool, string> cb, Action begin)
    {
        bool ishave = true;
        UIDragListener drag = m_icon.gameObject.AddComponent<UIDragListener>();
        drag.SetData(GameScenesManager.GetInstance().m_battleCamera, (pos, isend, tag) =>
        {
            ishave = isend ? GameCore.GetInstance().m_gameLogic.GetCanUseEnergy(2, UIBattleData.GetInstance().GetCostValue(m_data.id), m_data.id) : true;
            if (cd <= 0 && ishave) cb(pos, isend, tag);
        }, () =>
        {
            if (cd <= 0 && GameCore.GetInstance().m_gameLogic.GetCanUseEnergy(2, UIBattleData.GetInstance().GetCostValue(m_data.id), m_data.id)) begin();
        });
    }
    private void ResetCD()
    {
        cd = 0;
        m_mask.fillAmount = 0;
    }
    public void Update()
    {
        if (cd > 0)
        {
            cd -= Time.deltaTime;
            if (cd <= 0)
            {
                ResetCD();
            }
            else
            {
                m_mask.fillAmount = cd / m_data.cd;
            }
        }
    }
}
