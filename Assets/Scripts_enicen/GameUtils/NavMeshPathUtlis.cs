using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MyNavMeshPath 
{
    public NavMeshPath m_nmpath;
    public ObjectInfoBase m_target;
    public float m_length = -1f;
}
public class NavMeshPathUtlis:MonoBehaviour
{
    Action<ObjectInfoBase> PathCB;
    int endCnt = 0;
    public int pointid;
    public void SetPoint(ObjectInfoBase point , int areaMask, Dictionary<int, ObjectInfoBase> list, Action<ObjectInfoBase> end)
    {
        pointid = point.m_entityId;
        endCnt = list.Count;
        List<ObjectInfoBase> m_list = new List<ObjectInfoBase>(list.Values);
        for (int i = 0; i < m_list.Count; i++)
        {
            this.gameObject.AddComponent<NavMeshPathItem>().StartPath(point,m_list[i],areaMask, CalculateCallBack);
        }
        PathCB = end;
    }
    int calculateCnt = 0;
    float shortestDis = -1;
    ObjectInfoBase endInfo;
    void CalculateCallBack(ObjectInfoBase info,float dis) 
    {
        calculateCnt++;
        if (shortestDis == -1 || dis<shortestDis)
        {
            endInfo = info;
            shortestDis = dis;
        }
        if (calculateCnt == endCnt)
        {
            pointid = 0;
            PathCB(endInfo);
            GameObject.DestroyImmediate(this.gameObject);
        }
    }
}
