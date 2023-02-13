using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainpage : UIBase
{
    GameObject cell;
    public UIMainpage() : base("uimainpage", PanelType.Window) {}
    public override void OnCreate(object[] data)
    {
        base.OnCreate(data);

        cell = UIUtils.GetGameObject(m_go, "scroll/viewport/content/btn_cell");
        cell.SetActive(false);
        //m_battle = UIUtils.GetComponent<Button>(m_go, "btn_battle");
        //m_battle.onClick.AddListener(() => {
        //    Debug.Log("进入战斗");
        //    GameScenesManager.GetInstance().OpenScene(10001);
        //});
    }

    public override void OnActive(bool active)
    {
        base.OnActive(active);
        if (active)
        {
            RefreshGameLevel();
        }
    }

    void RefreshGameLevel() 
    {
        SceneDataNode cfg = GameConfigManager.GetInstance().GetConfig<SceneDataNode>("D_GameLevel");
        foreach (var item in cfg.m_map)
        {
            SceneData data = (SceneData)item.Value;
            if (data.scene_type == 1)
            {
                GameObject btngo = GameObject.Instantiate(cell, cell.transform.parent);
                Text nam = UIUtils.GetComponent<Text>(btngo, "Text");
                nam.text = data.scene_name;
                btngo.SetActive(true);
                UIUtils.SetClick(btngo, () => {
                    GameScenesManager.GetInstance().OpenScene(data.id);
                });
            }
        }
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }


}
