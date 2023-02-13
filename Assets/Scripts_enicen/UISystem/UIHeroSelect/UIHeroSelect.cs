using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISelectHeroItem 
{
    GameObject m_go;
    GameObject m_select;
    UIHeroItem m_uiitem;
    public ObjectData m_data;

    public UISelectHeroItem(GameObject go, Action<PointerEventData> begin, Action<PointerEventData> drag, Action<PointerEventData, string> end, string tag) 
    {
        m_go = go;
        m_uiitem = new UIHeroItem(UIUtils.GetGameObject(m_go, "cell_hero"));
        m_uiitem.SetDrag2D(begin,drag,end,tag);
        m_select = UIUtils.GetGameObject(go, "select");
        m_select.SetActive(false);
    }

    public void SetActive(bool active) 
    {
        m_go.SetActive(active);
    }
    public void SetData(ObjectData data, int cnt = 0) 
    {
        m_uiitem.SetData(data,cnt);
    }
    public void SetSelect(bool active) 
    {
        m_select.SetActive(active);
    }
}

public class UIHeroSelect : UIBase
{
    GameObject cell_hero;
    UIHeroItem m_drag;
    List<UISelectHeroItem> m_heroList = new List<UISelectHeroItem>();
    List<UIHeroItem> m_formationList = new List<UIHeroItem>();
    RectTransform m_DraggingPlane;
    RectTransform m_DragItemRt;

    public UIHeroSelect() : base("uiheroselect", PanelType.Window){}
    public override void OnCreate(object[] data)
    {
        base.OnCreate(data);
        m_DraggingPlane = m_go.GetComponent<RectTransform>();
        cell_hero = UIUtils.GetGameObject(m_go, "panel_hero/scroll_hero/viewport/content/heroitem");
        cell_hero.SetActive(false);
        GameObject formationgo = UIUtils.GetGameObject(m_go, "panel_hero/slot_hero");
        for (int i = 0; i < formationgo.transform.childCount; i++)
        {
            m_formationList.Add(new UIHeroItem(formationgo.transform.GetChild(i).gameObject));
        }
        m_drag = new UIHeroItem(UIUtils.GetGameObject(m_go, "panel_hero/drag"));
        m_drag.SetActive(false);
        m_DragItemRt = m_drag.m_go.GetComponent<RectTransform>();

        UIUtils.SetClick(UIUtils.GetGameObject(m_go, "btn_return"), () => {
            GameUIManager.GetInstance().ShowPanel<UIHeroFormation>();
        });
    }

    public override void OnActive(bool active)
    {
        base.OnActive(active);
        if (active)
        {
            RefreshHeroList();
            RefreshFormationList();
        }
    }
    public override void OnDestory()
    {
        base.OnDestory();
        m_heroList.Clear();
    }
    void RefreshFormationList() 
    {
        for (int i = 0; i < PlayerData.GetInstance().m_formationHero.Count; i++)
        {
            ObjectData data = PlayerData.GetInstance().m_formationHero[i];
            if (m_formationList.Count > i)
            {
                m_formationList[i].SetData(data);
            }
            m_formationList[i].SetActive(m_formationList.Count > i);
        }
    }

    void RefreshHeroList() 
    {
        HeroFormationNode cfg = GameConfigManager.GetInstance().GetConfig<HeroFormationNode>("D_HeroFormation");
        int index = 0;
        Vector3 globalMousePos;
        foreach (var item in cfg.m_map)
        {
            HeroFormationData data = (HeroFormationData)item.Value;
            ObjectData objdata = GameConfigManager.GetInstance().GetItem<ObjectData>("D_ObjectBase", data.hero_id);
            UISelectHeroItem uiitem = null;
            if (m_heroList.Count > index)
            {
                uiitem = m_heroList[index];
            }
            if (uiitem == null)
            {
                uiitem = new UISelectHeroItem(GameObject.Instantiate(cell_hero,cell_hero.transform.parent),(eventdata) =>
                {
                    if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, eventdata.position, eventdata.pressEventCamera, out globalMousePos))
                    {
                        m_DragItemRt.position = globalMousePos;
                        m_DragItemRt.rotation = m_DraggingPlane.rotation;
                    }
                    m_drag.SetData(objdata);
                    m_drag.SetActive(true);
                }, (eventdata) => {
                    if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, eventdata.position, eventdata.pressEventCamera, out globalMousePos))
                    {
                        m_DragItemRt.position = globalMousePos;
                        m_DragItemRt.rotation = m_DraggingPlane.rotation;
                    }
                }, (eventdata, pointStr) => {
                    m_drag.SetActive(false);
                    if (! string.IsNullOrEmpty(pointStr))
                    {
                        int slot = int.Parse(pointStr);
                        PlayerData.GetInstance().RefreshHeroBySlot(objdata, slot);
                        RefreshHeroList();
                        RefreshFormationList();
                    }
                }, "UIHeroSelectSlot");
                m_heroList.Add(uiitem);
            }
            uiitem.SetActive(true);
            uiitem.SetData(objdata,data.default_num);
            uiitem.SetSelect(PlayerData.GetInstance().GetHeroFormationState(objdata.id));
            index++;
        }
    }

}
