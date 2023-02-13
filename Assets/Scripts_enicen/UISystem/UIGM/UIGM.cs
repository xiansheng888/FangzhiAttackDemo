using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIGM :UIBase
{
    GameObject go_panelgm;
    bool isShowGm = false;
    public UIGM() : base("UIGM", PanelType.Top ){}

    public override void OnCreate(object[] data)
    {
        base.OnCreate(data);
        go_panelgm = UIUtils.GetGameObject(m_go, "panel_gm");

        UnityAction cb = () =>
        {
            isShowGm = !isShowGm;
            RefreshPanelGM();
        };

        Button btn = UIUtils.GetComponent<Button>(m_go, "btn_open");
        btn.onClick.AddListener(cb);
        btn = UIUtils.GetComponent<Button>(m_go, "panel_gm");
        btn.onClick.AddListener(cb);
        RefreshPanelGM();

        Toggle toggle = UIUtils.GetComponent<Toggle>(m_go, "panel_gm/layout/toggle_allspeed");
        toggle.onValueChanged.AddListener((value) =>
        {
            GameUtils.AtkSpeed_basics += value ? 1 : -1;
            GameUtils.Speed_Basics_extra += value ? 1 : -1;
        });
        toggle = UIUtils.GetComponent<Toggle>(m_go, "panel_gm/layout/toggle_otheratk");
        toggle.onValueChanged.AddListener((value) =>
        {
            GameUtils.other_atk = value ? 1000 : 0;
        });
        toggle = UIUtils.GetComponent<Toggle>(m_go, "panel_gm/layout/toggle_selfatk");
        toggle.onValueChanged.AddListener((value) =>
        {
            GameUtils.self_atk = value ? 1000 : 0;
        });
        toggle = UIUtils.GetComponent<Toggle>(m_go, "panel_gm/layout/toggle_othermp");
        toggle.onValueChanged.AddListener((value) =>
        {
            if (value) GameCore.GetInstance().m_gameLogic.ResetEntityMP(1);
            GameUtils.other_mp_free = value;
        });
        toggle = UIUtils.GetComponent<Toggle>(m_go, "panel_gm/layout/toggle_selfmp");
        toggle.onValueChanged.AddListener((value) =>
        {
            if (value) GameCore.GetInstance().m_gameLogic.ResetEntityMP(1);
            GameUtils.self_mp_free = value;
        });
        toggle = UIUtils.GetComponent<Toggle>(m_go, "panel_gm/layout/toggle_mp");
        toggle.onValueChanged.AddListener((value) =>
        {
            if (value) PlayerData.GetInstance().m_energy = GameUtils.energy_max_basics;
            GameUtils.createhero_free = value;
        });
        toggle = UIUtils.GetComponent<Toggle>(m_go, "panel_gm/layout/toggle_playerskillcd");
        toggle.onValueChanged.AddListener((value) =>
        {
            GameUtils.playerskill_free = value;
        });
        toggle = UIUtils.GetComponent<Toggle>(m_go, "panel_gm/layout/toggle_selfinvincible");
        toggle.onValueChanged.AddListener((value) =>
        {
            GameUtils.self_invincible = value;
        });
        toggle = UIUtils.GetComponent<Toggle>(m_go, "panel_gm/layout/toggle_otherinvincible");
        toggle.onValueChanged.AddListener((value) =>
        {
            GameUtils.other_invincible = value;
        });
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }

    public override void Update()
    {
        base.Update();
    }

    void RefreshPanelGM() 
    {
        go_panelgm.SetActive(isShowGm);
    }
}
