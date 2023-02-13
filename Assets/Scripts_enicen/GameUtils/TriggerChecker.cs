using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    Action m_callBack;
    Vector3 m_target;
    float m_radius;
    public void SetTriggerCallBack(Action cb)
    {
        m_callBack = cb;
    }

    public void SetData(Vector3 target, float radius)
    {
        m_target = target;
        m_radius = radius;
    }
    private void Update()
    {
        if (CircleAttack(this.transform.position, m_target, m_radius))
        {
            if (m_callBack != null)
            {
                m_callBack();
            }
        }
    }

    bool CircleAttack(Vector3 attacked, Vector3 target, float radius)
    {
        if (Vector3.Distance(attacked, target) < radius)
        {
            return true;
        }
        return false;
    }

}
