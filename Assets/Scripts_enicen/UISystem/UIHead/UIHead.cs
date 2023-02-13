using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//头顶字特殊处理
public class UIHead
{
    GameObject m_go;
    Transform m_target;

    Text m_value;
    Slider slider_hp;
    Slider slider_mp;
    Image m_sliderImg;
    List<Text> m_flsnows = new List<Text>();
    List<Image> m_buffs = new List<Image>();
    Dictionary<int, int> m_buffIndexToId = new Dictionary<int, int>();

    ObjectInfoBase m_data;
    float m_lastHp = -1;
    bool mpenable = false;
    float m_hindTime = 0;
    int index = 0;
    public UIHead(Transform target,ObjectInfoBase data) 
    {
        m_target = target;
        m_data = data;
        Create();
        mpenable = false;
        foreach (var item in data.m_skillList)
        {
            if (item.Value.m_cfgData.mp_cost>0)
            {
                SetMPEnable(true);
                break;
            }
        }
        RefreshHP();
        RefreshMP();
    }
    private void Create()
    {
        m_go = GameObject.Instantiate(ResourcesManager.LoadGameObject("uihead"));
        m_go.transform.SetParent(GameCore.GetInstance().m_uiHeadRoot.transform);
        m_go.transform.localScale = Vector3.one;
        m_go.transform.localPosition = Vector3.zero;
        m_go.SetActive(false);
        m_value = UIUtils.GetComponent<Text>(m_go, "hptext");
        slider_hp = UIUtils.GetComponent<Slider>(m_go, "hp_slider");
        slider_mp = UIUtils.GetComponent<Slider>(m_go, "mp_slider");

        slider_hp.gameObject.SetActive(true);
        slider_mp.gameObject.SetActive(false);

        m_sliderImg = UIUtils.GetComponent<Image>(slider_hp.gameObject, "Fill");
        for (int i = 0; i < 10; i++)
        {
            m_flsnows.Add(UIUtils.GetComponent<Text>(m_go, "flsnow_root/cell" + i));
            m_flsnows[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < 10; i++)
        {
            int index = i;
            m_buffs.Add(UIUtils.GetComponent<Image>(m_go, "buff_layout/cell" + i));
            Button btn = m_buffs[i].GetComponent<Button>();
            m_buffIndexToId[i] = 0;
            btn.onClick.AddListener(() => {
                GameUIManager.GetInstance().ShowPanel<UIBuffTips>(m_data.m_buffList[m_buffIndexToId[index]].m_data.id);
            });
            m_buffs[i].gameObject.SetActive(false);
        }
        RefreshBuff();

    }

    public void InfoChange(PropertyType type)
    {
        if (m_data != null )
        {
            if (type == PropertyType.HP)
            {
                RefreshHP();
            }
            else if (type == PropertyType.MP)
            {
                RefreshMP();
            }
        }
    }


    private Vector3 WorldPointToUILocalPoint(Vector3 point)
    {
        Vector3 screenPoint = GameScenesManager.GetInstance().m_battleCamera.WorldToScreenPoint(point);
        Vector2 uiPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_go.transform.parent.GetComponent<RectTransform>(), screenPoint, GameCore.GetInstance().m_uiCamera, out uiPosition);
        return uiPosition;
    }

    private void ShowHpTips(float value) 
    {
        m_flsnows[index].text = value.ToString("f2");
        m_flsnows[index].color = value > 0 ? Color.green : Color.red;
        m_flsnows[index].gameObject.SetActive(false);
        m_flsnows[index].gameObject.SetActive(true);

        index++;
        if (index == 10)
        {
            index = 0;
        }
    }
    private void RefreshMP()
    {
        if (mpenable)
        {
            slider_mp.value = m_data.m_mp / m_data.m_mp_max;
            slider_mp.gameObject.SetActive(m_data.m_mp >= m_data.m_mp_max);
        }
    }
    private void RefreshHP()
    {
        if (m_lastHp > 0)
        {
            ShowHpTips(m_data.m_hp - m_lastHp);
        }
        //m_value.text = string.Format("<color={0}>{1}/{2}</color>", m_data.m_cfgData.camp == 1 ? "#3BF900" : "#F93100", m_data.m_hp.ToString("f2"), m_data.m_hp_max);
        m_sliderImg.color = m_data.m_cfgData.camp == 1 ? Color.green : Color.red;
        slider_hp.value = m_data.m_hp / m_data.m_hp_max;
        m_lastHp = m_data.m_hp;
        m_hindTime = m_data.m_hp < m_data.m_hp_max ? 1 : m_hindTime;
    }
    public void RefreshBuff() 
    {
        int index = 0;
        foreach (var item in m_data.m_buffList)
        {
            BuffEffectOnce data = item.Value;
            bool isShow = data != null && data.m_data.icon_show == 1;
            m_buffs[index].gameObject.SetActive(isShow);
            if (isShow)
            {
                UIUtils.SetImage(m_buffs[index], data.m_data.icon);
            }
            m_buffIndexToId[index] = item.Key;
            index++;
        }
        if (index <= m_buffs.Count)
        {
            for (int i = index; i < m_buffs.Count; i++)
            {
                m_buffs[i].gameObject.SetActive(false);
            }
        }
    }
    public void SetMPEnable(bool enable) 
    {
        mpenable = enable;
        slider_mp.gameObject.SetActive(mpenable);
    }

    public void Release()
    {
        GameObject.Destroy(m_go);
        m_go = null;
        m_target = null;
        m_lastHp = -1;
        m_data = null;

        m_value = null;
        slider_hp = null;
        m_flsnows = null;
    }
    public void Update()
    {
        if (m_go && m_target)
        {
            m_go.transform.localPosition = WorldPointToUILocalPoint(m_target.position);
            if (m_hindTime > 0)
            {
                m_hindTime -= Time.deltaTime;
                if (m_hindTime < 0) m_hindTime = 0;

                if (!m_go.activeSelf && m_hindTime > 0)
                {
                    m_go.SetActive(true);
                }
                else if (m_go.activeSelf && m_hindTime == 0)
                {
                    m_go.SetActive(false);
                }
            }
        }
        
    }
}
