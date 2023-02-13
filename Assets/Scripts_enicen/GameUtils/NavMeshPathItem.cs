using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshPathItem : MonoBehaviour
{
    NavMeshPath path = null;
    ObjectInfoBase m_target;
    Action<ObjectInfoBase,float> m_end;

    public int pointid;
    public int targetid;
    public void StartPath(ObjectInfoBase pos,ObjectInfoBase target,int areaMask,Action<ObjectInfoBase,float> end)
    {
        pointid = pos.m_entityId;
        targetid = target.m_entityId;
        m_end = end;
        path = new NavMeshPath();
        m_target = target;
        NavMesh.CalculatePath(pos.m_pos,target.m_pos, areaMask, path);
    }

    float totalTime = 0f;
    void Update()
    {
        totalTime += Time.deltaTime;
        if (path != null && path.status == NavMeshPathStatus.PathComplete)
        {
            float m_length = 0f;
            for (int i = 0; i < path.corners.Length-1; i++)
            {
                m_length += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            pointid = 0;
            targetid = 0;
            m_end(m_target, m_length);
            path.ClearCorners();
            path = null;
        }
        if (totalTime >= 2f && path != null && path.status != NavMeshPathStatus.PathComplete)
        {
            Debug.LogWarning("2秒了，还没有计算出来路径" + pointid + " --> " + targetid);
            m_end(m_target, 9999);
            pointid = 0;
            targetid = 0; 
            path.ClearCorners();
            path = null;
        }
    }
}
