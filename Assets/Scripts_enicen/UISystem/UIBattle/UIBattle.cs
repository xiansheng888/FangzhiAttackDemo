using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UIBattleHeroItem 
{
    GameObject m_go;
    UIHeroItem m_hero;
    public UIBattleHeroItem(GameObject go) 
    {
        m_go = go;
        m_hero = new UIHeroItem(UIUtils.GetGameObject(m_go, "cell_hero"));
    }

    public void SetData(ObjectData data, int cnt = 0) 
    {
        m_hero.SetData(data,cnt);
    }

    public void SetDrag(Action<Vector3, bool, string> dragcb, Action begincb) 
    {
        m_hero.SetDrag(dragcb, begincb);
    }
    public void SetCD(float cd) 
    {
        m_hero.SetCD(cd);
    }
    public void Update() 
    {
        if (m_hero != null) m_hero.Update();
    }

}


public class UIBattle : UIBase
{
    GameObject go_endtime = null;
    Text text_endtime = null;
    Image slider_mp = null;
    Text text_mp = null;
    GameObject go_herolayout = null;
    GameObject skillCell = null;
    Timer m_timer;
    Dictionary<int, GameObject> m_models = new Dictionary<int, GameObject>();
    Dictionary<string, GameObject> m_skillAim = new Dictionary<string, GameObject>();
    Dictionary<int,UISkillItem> m_uiSkillCells = new Dictionary<int,UISkillItem>();
    List<UIBattleHeroItem> m_heroitems = new List<UIBattleHeroItem>();

    public UIBattle() : base("uibattle", PanelType.Window) { }

    public override void OnCreate(object[] data)
    {
        base.OnCreate(data);

        go_endtime = UIUtils.GetGameObject(m_go, "title_time");
        text_endtime = UIUtils.GetComponent<Text>(m_go, "title_time/text_endtime");
        go_herolayout = UIUtils.GetGameObject(m_go, "layout_hero");
        skillCell = UIUtils.GetGameObject(m_go, "layout_skill/cell_skill");
        skillCell.SetActive(false);
        slider_mp = UIUtils.GetComponent<Image>(m_go, "slider_mp/slider");
        text_mp = UIUtils.GetComponent<Text>(m_go, "slider_mp/value");
        Messenger.GetInstance().AddMessenge(MessgeType.SceneTime, StartTime);
        Messenger.GetInstance().AddMessenge(MessgeType.Battle_Create, RefreshMP);
        UIBattleData.GetInstance().InitPlayerSkill();
        InitHeroUI();
        InitSkillUI();
        RefreshMP(null);
        go_endtime.SetActive(false);
        text_endtime.text = string.Empty;
    }

    private void RefreshMP(NotifyData obj)

    {
        slider_mp.fillAmount = PlayerData.GetInstance().m_energy / PlayerData.GetInstance().m_energyMax;
        text_mp.text = PlayerData.GetInstance().m_energy.ToString();
    }
    Dictionary<int, UISkillItem>.Enumerator enumerator1;
    public override void Update()
    {
        base.Update();
        enumerator1 = m_uiSkillCells.GetEnumerator();
        while (enumerator1.MoveNext())
        {
            enumerator1.Current.Value.Update();
        }
        for (int i = 0; i < m_heroitems.Count; i++)
        {
            m_heroitems[i].Update();
        }
    }

    public override void OnDestory()
    {
        if (m_timer != null)
        {
            m_timer.Stop();
        }
        m_timer = null;
        Messenger.GetInstance().RemoveMessenge(MessgeType.SceneTime, StartTime);
        Messenger.GetInstance().RemoveMessenge(MessgeType.Battle_Create, RefreshMP);
        UIBattleData.GetInstance().ClearCache();
        foreach (var item in m_models)
        {
            GameObject.Destroy(item.Value);
        }
        m_models.Clear();
        m_models = null;
        foreach (var item in m_skillAim)
        {
            GameObject.Destroy(item.Value);
        }
        m_skillAim.Clear();
        m_skillAim = null;
        m_uiSkillCells.Clear();
        m_heroitems.Clear();
        base.OnDestory();

    }
    void InitSkillUI()
    {
        foreach (var item in PlayerData.GetInstance().m_playerskill)
        {
            string aim = item.Value.m_cfgData.skill_aim;
            if (!m_skillAim.ContainsKey(aim))
            {
                m_skillAim[aim] = GameObject.Instantiate(ResourcesManager.LoadGameObject(aim), GameScenesManager.GetInstance().m_entityRoot.transform);
                m_skillAim[aim].SetActive(false);
            }
            UISkillItem uicell;
            if (!m_uiSkillCells.ContainsKey(item.Value.m_cfgData.id))
            {
                GameObject obj = GameObject.Instantiate(skillCell, skillCell.transform.parent);
                uicell = new UISkillItem(obj);
                m_uiSkillCells.Add(item.Value.m_cfgData.id, uicell);
            }
            uicell = m_uiSkillCells[item.Value.m_cfgData.id];
            uicell.SetData(item.Value.m_cfgData, UIBattleData.GetInstance().GetCostValue(item.Value.m_cfgData.id), PlayerData.GetInstance().m_playerSkillCount[item.Value.m_cfgData.id]);
            uicell.SetDrag((pos, isend, str) =>
            {
                if (isend)
                {
                    GameCore.GetInstance().m_gameLogic.UsePlayerSKill(item.Value.m_cfgData);
                    InitSkillUI();
                    m_skillAim[aim].SetActive(false);
                    item.Value.PlaySkill( 1, pos);
                    if (!GameUtils.playerskill_free)
                        uicell.cd = item.Value.m_cfgData.cd;
                    RefreshMP(null);
                }
                else
                {
                    m_skillAim[aim].transform.position = pos + (Vector3.up * 0.1f);
                }
            }, () =>
            {
                m_skillAim[aim].SetActive(true);
            });
        }
    }

    void InitHeroUI()
    {
        for (int i = 0; i < PlayerData.GetInstance().m_formationHero.Count; i++)
        {
            ObjectData herodata = PlayerData.GetInstance().m_formationHero[i];
            UIBattleHeroItem item = null;
            if (m_heroitems.Count < i) item = m_heroitems[i];
            if (item == null)
            {
                CheckModel(herodata.id);
                item = new UIBattleHeroItem(UIUtils.GetGameObject(go_herolayout,"cell"+i));
                m_heroitems.Add(item);
            }
            item.SetData(herodata, PlayerData.GetInstance().m_heroCreateCount[herodata.id]);
            item.SetDrag((pos, isend, str) =>
            {
                if (isend) EndDrag(herodata.id, str, pos);
                else m_models[herodata.id].transform.position = pos;
            }, () =>
            {
                StartDrag(herodata.id);
            });
        }
        
        
    }
    void StartTime(NotifyData _data)
    {
        SceneTime data = (SceneTime)_data;
        float time = 0;
        go_endtime.SetActive(true);
        m_timer = TimerUtils.StartTimer(1, false, () =>
        {
            text_endtime.text = "结束倒计时:" + ToTimeFormat(data.m_time - time);
            if (data.m_time <= time)
            {
                go_endtime.SetActive(false);
                m_timer.Stop();
                m_timer = null;
            }
            time++;
        }, 0);
    }
    public string ToTimeFormat(float seconds)
    {
        int minute = Mathf.FloorToInt(seconds / 60);
        int second = Mathf.FloorToInt(seconds % 60);
        return string.Format("{0:D2}:{1:D2}", minute, second);
    }

    void CheckModel(int id)
    {
        if (!m_models.ContainsKey(id))
        {
            ObjectData data = GameConfigManager.GetInstance().GetItem<ObjectData>("D_ObjectBase", id);
            m_models[id] = GameObject.Instantiate(ResourcesManager.LoadGameObject(data.res_name), GameScenesManager.GetInstance().m_entityRoot.transform);
            NavMeshAgent tmpna = m_models[id].GetComponent<NavMeshAgent>();
            if(tmpna) tmpna.enabled = false;
            m_models[id].transform.Rotate(data.drag_rotate, Space.World);
            m_models[id].SetActive(false);
        }
    }
    void StartDrag(int id)
    {
        GameScenesManager.GetInstance().ShowHeroPland(true);
        m_models[id].SetActive(true);
    }
    void EndDrag(int id, string str, Vector3 pos)
    {
        GameScenesManager.GetInstance().ShowHeroPland(false);
        m_models[id].SetActive(false);
        if (str == "HeroPland")
        {
            GameScenesManager.GetInstance().CreateHero(id, pos);
            InitHeroUI();
            for (int i = 0; i < PlayerData.GetInstance().m_formationHero.Count; i++)
            {
                if (PlayerData.GetInstance().m_formationHero[i].id == id)
                {
                    m_heroitems[i].SetCD(PlayerData.GetInstance().m_formationHero[i].cd_create);
                }
            }
        }
    }
}
