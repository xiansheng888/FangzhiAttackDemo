using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
    //public GameObject playerroot;
    //public GameObject player;
    //public GameObject player1;
    //public GameObject enemy;

    //private Vector3 m_playerpos = new Vector3(17,0,10);
    //private Vector3 m_player1pos = new Vector3(10, 0, 10);
    //private Vector3 m_enemypos = new Vector3(50, 0, 8);
    //private Vector3 m_playerrotate = new Vector3(0,90,0);
    //private Vector3 m_player1rotate = new Vector3(0,90, 0);
    //private Vector3 m_enemyrotate = new Vector3(0,90,0);
    //private GameObject _enemy;
    //private Vector3 m_targetPos;

    //NavMeshAgent nma;
    //NavMeshAgent nma1;
    //private bool ismove = false;

    public NavMeshAgent obj1;
    public NavMeshAgent obj2;
    NavMeshPath tmp;
    private void Start()
    {
        //GameObject ins = GameObject.Instantiate(player, playerroot.transform);
        //ins.transform.position = m_playerpos;
        //ins.transform.Rotate(m_playerrotate);
        //nma = ins.GetComponent<NavMeshAgent>();
        //nma.enabled = true;


        //ins = GameObject.Instantiate(player1, playerroot.transform);
        //ins.transform.position = m_player1pos;
        //ins.transform.Rotate(m_player1rotate);
        //nma1 = ins.GetComponent<NavMeshAgent>();
        //nma1.enabled = true;

        //_enemy = GameObject.Instantiate(enemy, playerroot.transform);
        //_enemy.transform.position = m_enemypos;
        //_enemy.transform.Rotate(m_enemyrotate);

        //m_targetPos = _enemy.transform.position;

        ////nma.autoRepath = false;
        ////nma1.autoRepath = false;//遇到阻挡不重新计算path，当场停下
        //nma.SetDestination(m_targetPos);
        //nma1.SetDestination(m_targetPos);
        //Debug.Log(nma.path.corners.Length);//路径点 v3
        //ismove = true;
        obj1.SetDestination(obj2.transform.position);

        tmp = new NavMeshPath();
        Debug.Log("直线距离" + Vector3.Distance(obj1.transform.position, obj2.transform.position));
        obj1.CalculatePath(obj2.transform.position, tmp);
        //NavMesh.CalculatePath(obj1.transform.position, obj2.transform.position, obj1.areaMask, tmp);
    }
    float dis = -1;
    private void Update()
    {
        if (dis < 0 && tmp.status == NavMeshPathStatus.PathComplete)// dis < 0 && tmp.corners.Length > 0)
        {
            Debug.Log(obj1.remainingDistance);
            dis = 0;
            for (int i = 0; i < tmp.corners.Length - 1; i++)
            {
                dis += Vector3.Distance(tmp.corners[i], tmp.corners[i + 1]);
            }
            Debug.Log("寻路距离" + dis);
            Debug.Log("ai寻路距离" + tmp.GetCornersNonAlloc(tmp.corners));
        }
        //if (dis < 0 && obj1.path.corners.Length > 0)
        //{
        //    dis = 0;
        //    for (int i = 0; i < obj1.path.corners.Length - 1; i++)
        //    {
        //        dis += Vector3.Distance(obj1.path.corners[i], obj1.path.corners[i + 1]);
        //    }
        //    Debug.Log("寻路距离" + dis);
        //}
        //if (Mathf.Abs(m_targetPos.x - _enemy.transform.position.x) > 0.2 ||  Mathf.Abs(m_targetPos.z - _enemy.transform.position.z) > 0.2 )
        //{
        //    m_targetPos = _enemy.transform.position;
        //    nma.SetDestination(m_targetPos);
        //}

        //Debug.Log(nma.path.corners.Length);
        //if (ismove)
        //{
        //    if (!nma.pathPending && nma.remainingDistance < nma.stoppingDistance)
        //    {
        //        // 移动终止
        //        Debug.Log("移动结束了");
        //        ismove = false;
        //    }
        //}

    }
}
