using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

static public class UIUtils
{
    static public T GetComponent<T>(GameObject parent,string path) 
    {
        return parent.transform.Find(path).GetComponent<T>();
    }
    static public GameObject GetGameObject(GameObject parent, string path) 
    {
        return parent.transform.Find(path).gameObject;
    }
    static public void SetImage(Image image, string icon) 
    {
        image.sprite = ResourcesManager.Load<Sprite>(icon);
    }
    static public void SetClick(GameObject btngo , UnityAction cb) 
    {
        Button btn = btngo.GetComponent<Button>();
        if (btn)
        {
            GameObject.DestroyImmediate(btn);
            btn = null;
        }
        btn = btngo.AddComponent<Button>();
        btn.onClick.AddListener(cb);
    }
}
