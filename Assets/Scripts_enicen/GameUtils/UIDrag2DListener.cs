using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIDrag2DListener : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    string targetTag = "";
    Action<PointerEventData> m_begin;
    Action<PointerEventData> m_drag;
    Action<PointerEventData,string> m_end;
    public void SetAction(Action<PointerEventData> begin, Action<PointerEventData> drag, Action<PointerEventData,string> end, string tag)
    {
        targetTag = tag;
        m_begin = begin;
        m_drag = drag;
        m_end = end;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_begin(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_drag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool isPoint = eventData.pointerCurrentRaycast.gameObject.tag == targetTag;
        if (m_end != null)
        {
            m_end.Invoke(eventData, isPoint ? eventData.pointerCurrentRaycast.gameObject.name : "");
        }
    }
}