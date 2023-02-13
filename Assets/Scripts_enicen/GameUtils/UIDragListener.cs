using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragListener : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerClickHandler
{
    Action<Vector3, bool,string> m_cb;
    Action m_begincb;
    Camera m_camera;

    bool isPickUp = false;
    public void SetData(Camera sceneCamera,Action<Vector3,bool,string> cb,Action begincb) 
    {
        m_camera = sceneCamera;
        m_cb = cb;
        m_begincb = begincb;
    }
    Ray ray;
    RaycastHit rayHit;
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        ray = m_camera.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out rayHit))
        {
            if (m_cb != null)
            {
                m_cb.Invoke(rayHit.point, false, string.Empty);
            }

        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        ray = m_camera.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out rayHit))
        {
            if (m_cb != null)
            {
                m_cb.Invoke(rayHit.point, true, rayHit.collider.gameObject.tag);
            }
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (m_begincb!= null)
        {
            m_begincb();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_begincb != null)
        {
            m_begincb();
            isPickUp = true;
        }
    }

    private void Update()
    {

        if (isPickUp)
        {
            if (Input.GetMouseButton(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    ray = m_camera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out rayHit))
                    {
                        if (m_cb != null)
                        {
                            m_cb.Invoke(rayHit.point, true, rayHit.collider.gameObject.tag);
                        }
                    }
                }
                else
                {
                    m_cb.Invoke(Vector3.one * -10000, true, "");
                }
                isPickUp = false;
            }

            ray = m_camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayHit))
            {
                if (m_cb != null)
                {
                    m_cb.Invoke(rayHit.point, false, string.Empty);
                }
            }
        }
    }
}
