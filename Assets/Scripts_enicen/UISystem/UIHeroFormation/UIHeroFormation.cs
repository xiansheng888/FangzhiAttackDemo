using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroFormation : UIBase
{
    GameObject cell_hero;
    GameObject cell_skill;
    List<UIHeroItem> m_heroList = new List<UIHeroItem>();
    List<UISkillItem> m_skillList = new List<UISkillItem>();

    bool ShowHeroPanel = true;
    Vector3 ShowPos = new Vector3(0, Screen.height / 2 * -1, 0);
    Vector3 HidePos = new Vector3(0, (Screen.height / 2 * -1) - 315, 0);
    public UIHeroFormation() : base("uiheroformation",  PanelType.Window){}

    public override void OnCreate(object[] data)
    {
        base.OnCreate(data);

        cell_hero = UIUtils.GetGameObject(m_go, "panel_hero/scroll_hero/viewport/layout_hero/cell_hero");
        cell_skill = UIUtils.GetGameObject(m_go, "panel_hero/layout_skill/cell_skill");
        GameObject btn_start = UIUtils.GetGameObject(m_go, "panel_hero/btn_start");
        UIUtils.SetClick(btn_start, () =>
        {
            GameCore.GetInstance().m_gameLogic.StartBattle();
            GameScenesManager.GetInstance().StartBattle();
        });
        RectTransform heroPanelRt = UIUtils.GetComponent<RectTransform>(m_go, "panel_hero");
        GameObject btn_down = UIUtils.GetGameObject(m_go, "panel_hero/btn_down");
        Transform btn_img = UIUtils.GetGameObject(btn_down.gameObject, "Image").transform;
        UIUtils.SetClick(btn_down, () => {
            ShowHeroPanel = !ShowHeroPanel;
            heroPanelRt.transform.localPosition = ShowHeroPanel ? HidePos : ShowPos;
            btn_img.localRotation = Quaternion.Euler(0, 0, ShowHeroPanel ? 180 : 0);
            btn_img.DOLocalRotate(new Vector3(0, 0, ShowHeroPanel ? 0 : 180), 0.3f);
            heroPanelRt.transform.DOLocalMove(ShowHeroPanel ? ShowPos : HidePos, 0.3f);
        });

        GameObject btn_return = UIUtils.GetGameObject(m_go, "btn_return");
        UIUtils.SetClick(btn_return, () =>
        {
            GameScenesManager.GetInstance().GameLevelFinish();
        });

	}

    public override void OnDestory()
    {
        base.OnDestory();
    }
    public override void OnActive(bool active)
    {
        base.OnActive(active);
        if (active)
        {
            RefreshHero();
            RefreshSkill();
        }
    }

    private void RefreshSkill()
    {
        UISkillItem item = null;
        List<SkillData> data = UIHeroFormationData.GetInstance().GetSkills();
        for (int i = 0; i < data.Count; i++)
        {
            item = null;
            if (m_skillList.Count < i)
            {
                item = m_skillList[i];
            }
            if (item == null)
            {
                item = new UISkillItem(GameObject.Instantiate(cell_skill,cell_skill.transform.parent));
                m_skillList.Add(item);
            }
            item.SetData(data[i],0);
            item.SetActive(true);
        }
    }

    private void RefreshHero()
    {
        UIHeroItem item = null;
        List<ObjectData> data = UIHeroFormationData.GetInstance().GetFormations();
        for (int i = 0; i < data.Count; i++)
        {
            item = null;
            if (m_heroList.Count < i)
            {
                item = m_heroList[i];
            }
            if (item == null)
            {
                item = new UIHeroItem(GameObject.Instantiate(cell_hero, cell_hero.transform.parent));
                m_heroList.Add(item);
            }
            item.SetData(data[i]);
            item.SetClick(HeroClick);
            item.SetActive(true);
        }
    }
    private void HeroClick(ObjectData obj)
    {
        GameUIManager.GetInstance().ShowPanel<UIHeroSelect>();
    }
}
