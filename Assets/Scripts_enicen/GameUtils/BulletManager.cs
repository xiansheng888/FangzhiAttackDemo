using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : Singleton<BulletManager>
{
    //public Dictionary<string, List<BulletControl>> m_bullets = new Dictionary<string, List<BulletControl>>();

    public void PlayBullet(string path,float speed,Vector3[] posPath ,ObjectInfoBase attacker, ObjectInfoBase target, SkillData skilldata, SkillComponent componentdata, Action endCb = null) 
    {
        BulletControl bullet = null;
        bool needCreate = true;
        //if (!m_bullets.ContainsKey(path)) m_bullets[path] = new List<BulletControl>();
        //if (m_bullets[path].Count  > 0 )
        //{
        //    for (int i = 0; i < m_bullets[path].Count; i++)
        //    {
        //        if(!m_bullets[path][i].isActive) 
        //        {
        //            bullet = m_bullets[path][i];
        //            needCreate = false;
        //        }
        //    }
        //}
        if (needCreate)
        {
            GameObject go = GameObject.Instantiate(ResourcesManager.LoadGameObject(path), GameScenesManager.GetInstance().m_effectRoot.transform);
            go.name = path;
            bullet = go.AddComponent<BulletControl>();
            //m_bullets[path].Add(bullet);
            go.SetActive(false);
        }
        bullet.ResetData(attacker, target, skilldata, componentdata);
        bullet.PlayNormal(posPath, speed, endCb);
    }


    public void Release() {
        //foreach (var item in m_bullets)
        //{
        //    for (int i = 0; i < item.Value.Count; i++)
        //    {
        //        GameObject.Destroy(item.Value[i].gameObject);
        //    }
        //}
        //m_bullets.Clear();
    }
    
}
