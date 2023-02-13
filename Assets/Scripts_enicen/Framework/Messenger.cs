using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyData
{
    public MessgeType key;
    public NotifyData(MessgeType _key)
    {
        key = _key;
    }
}


public class Messenger : Singleton<Messenger>
{
    Dictionary<MessgeType, List<Action<NotifyData>>> _MessengeDic = new Dictionary<MessgeType, List<Action<NotifyData>>>();

    public void AddMessenge(MessgeType key, Action<NotifyData> action)
    {
        if (!_MessengeDic.ContainsKey(key))
        {
            _MessengeDic.Add(key, new List<Action<NotifyData>>());
        }
        _MessengeDic[key].Add(action);
    }

    public void RemoveMessenge(MessgeType key, Action<NotifyData> action)
    {
        if (_MessengeDic.ContainsKey(key))
        {
            _MessengeDic[key].Remove(action);
        }
    }

    public void Broadcast(NotifyData data)
    {
        if (_MessengeDic.ContainsKey(data.key))
        {
            for (int i = 0; i < _MessengeDic[data.key].Count; i++)
            {
                _MessengeDic[data.key][i](data);
            }
        }
    }
}
