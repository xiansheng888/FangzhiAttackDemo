using BitBenderGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDragCheck : MonoBehaviour
{
    private TouchInputController m_cameraInput;
    private void Awake()
    {
        m_cameraInput = gameObject.GetComponent<TouchInputController>();
    }
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            m_cameraInput.enabled = !EventSystem.current.IsPointerOverGameObject();
        }
    }
}
