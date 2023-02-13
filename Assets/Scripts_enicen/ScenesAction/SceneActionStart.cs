using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneActionStart : ScenesActionBase
{
    public string m_scnenName;

    public override void Trigger()
    {
        base.Trigger();
        m_scnenName = m_data.param1;
        m_nextIndex = m_data.action_next;
        GameScenesManager.GetInstance().LoadScene(m_scnenName, () =>
        {
            //GameUIManager.GetInstance().ShowPanel<UIBattle>();
            GameUIManager.GetInstance().ShowPanel<UIHeroFormation>();
            Leave();
        });
    }

    public override void Checker()
    {
        base.Checker();
        if (m_nextIndex != null)
        {
            for (int i = 0; i < m_nextIndex.Count; i++)
            {
                m_entity.TriggerNodeById(m_nextIndex[i]);
            }
        }
        
    }

    public override void Leave()
    {
        Checker();
        base.Leave();
        m_scnenName = null;
        m_nextIndex = null;
        m_data = null;
    }
}
