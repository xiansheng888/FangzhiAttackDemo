using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBuffTips : UIBase
{
    Text m_content;

    public UIBuffTips() : base("uibufftips", PanelType.Dialog){}
    public override void OnCreate(object[] data)
    {
        base.OnCreate(data);

        Button close = UIUtils.GetComponent<Button>(m_go, "close");
        close.onClick.AddListener(()=> {
            //GameUIManager.GetInstance().DestoryPanel<UIBuffTips>();
            SetActive(false);
        });
        m_content = UIUtils.GetComponent<Text>(m_go, "content");
    }
    public override void OnActive(bool active)
    {
        base.OnActive(active);
        int id = (int)m_jumpdata[0];
        m_content.text = GameConfigManager.GetInstance().GetItem<BuffData>("D_Buff", id).desc;
    }
    public override void OnDestory()
    {
        base.OnDestory();
    }
}
