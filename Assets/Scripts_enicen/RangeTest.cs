using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTest : MonoBehaviour
{
    public GameObject target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Debug.LogWarning(string.Format("{0} æ‡¿Î {1} -- {2}", this.gameObject.name, target.gameObject.name, Vector3.Distance(this.gameObject.transform.position, target.transform.position)));
        }
    }
}
