using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ResourcesManager
{
    static public GameObject LoadGameObject(string path)
    {
        return Resources.Load<GameObject>(path);
    }

    static public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
    static public string LoadConfig(string path) 
    {
        string str = "";
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                str = sr.ReadToEnd();
                fs.Close();
                sr.Close();
            }
        }
        return str;
    }
}
