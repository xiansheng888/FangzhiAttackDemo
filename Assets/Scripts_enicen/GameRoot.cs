using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private void Awake()
    {
        
    }
    private void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        GameCore.GetInstance().Init();
    }

    private void Update()
    {
        GameCore.GetInstance().Update();
    }
}
