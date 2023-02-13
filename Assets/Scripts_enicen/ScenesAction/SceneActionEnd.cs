//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SceneActionEnd : ScenesActionBase
//{
//    List<int> m_finishNodeId = new List<int>();
//    public override void Trigger()
//    {
//        base.Trigger();
//        string[] nodeid = m_data.param1.Split('|');
//        for (int i = 0; i < nodeid.Length; i++)
//        {
//            m_finishNodeId.Add(int.Parse(nodeid[i]));
//        }
//    }
//    public override void Checker()
//    {
//        base.Checker();
//        //for (int i = 0; i < m_entity. ; i++)
//        //{
//        //    if ( nodeid)
//        //    {

//        //    }
//        //}
//    }

//    public override void Leave()
//    {
//        //base.Leave();
//        //结束行为直接对接场景管理器
//        m_entity.Release();
//        GameScenesManager.GetInstance().GameLevelFinish();
//        m_data = null;
//        m_entity = null;
//        m_nextIndex = null;
//        m_finishNodeId = null;
//    }
//}
