using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test1 : MonoBehaviour
{
    public GameObject target;
    NavMeshAgent agent;
    NavMeshPath path;
    float dis = -1;
    void Start()
    {
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);

        path = new NavMeshPath();
        NavMesh.CalculatePath(this.transform.position, target.transform.position, agent.areaMask, path);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent) { Debug.Log(this.gameObject.name + "  :" + agent.areaMask); }

        if (dis < 0 &&  path.status == NavMeshPathStatus.PathComplete)
        {
            dis = 0;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                dis += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }
            Debug.Log("Â·¾¶³¤¶È" + dis);
        }
    }
}
