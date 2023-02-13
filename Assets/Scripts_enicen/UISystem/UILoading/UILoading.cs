using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UIBase
{
    Slider m_slider;
    Timer m_timer;
    public UILoading() : base("uiloading") {}

    public override void OnCreate(object[] data)
    {
        base.OnCreate(data);

        string test = data[0] as string;
        string test1 = data[1] as string;

        m_slider = UIUtils.GetComponent<Slider>(m_go, "slider");
        m_slider.value = 0.1f;
        m_timer = TimerUtils.StartTimer(0.5f, false, () => {
            m_slider.value = m_slider.value + 0.2f;
            if (m_slider.value >= 0.9f)
            {
                m_timer.Stop();
                GameUIManager.GetInstance().DestoryPanel<UILoading>();
            }
        });
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }
}
