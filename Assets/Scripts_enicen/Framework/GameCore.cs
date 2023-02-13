using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore:Singleton<GameCore>
{
    public Dictionary<PanelType, Transform> m_panelRoots = new Dictionary<PanelType, Transform>();
    public GameObject m_uiCanvas;
    public GameObject m_uiHeadRoot;
    public Camera m_uiCamera;
    public GameObject m_timerRoot;
    public GameLogic m_gameLogic;
    public GameObject m_audioroot;
    public CameraShake m_camerashake;

    public void Init() 
    {
        SetDefaultElement();
        GameConfigManager.GetInstance().InitConfigData();
        GameUIManager.GetInstance().Init();
        GameScenesManager.GetInstance().Init();
        PlayerData.GetInstance();
        GameScenesManager.GetInstance().OpenScene(1, () => {
            GameUIManager.GetInstance().ShowPanel<UIGM>();
            GameAudioManager.GetInstance().Play("bgm_1", true, 1f);
        });
    }

    public void Update()
    {
        GameScenesManager.GetInstance().Update();
        GameUIManager.GetInstance().Update();
    }

    void SetDefaultElement()
    {
        m_uiCanvas = GameObject.Find("Canvas");
        m_uiHeadRoot = m_uiCanvas.transform.Find("head_root").gameObject;
        GameObject.DontDestroyOnLoad(m_uiCanvas);
        m_panelRoots.Add(PanelType.Window, m_uiCanvas.transform.Find("win_root"));
        m_panelRoots.Add(PanelType.Dialog, m_uiCanvas.transform.Find("dialog_root"));
        m_panelRoots.Add(PanelType.Top, m_uiCanvas.transform.Find("top_root"));

        m_uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        GameObject.DontDestroyOnLoad(m_uiCamera.gameObject);

        m_timerRoot = new GameObject("TimerUtils");
        GameObject.DontDestroyOnLoad(m_timerRoot);

        GameObject _eventSystem = GameObject.Find("EventSystem");
        GameObject.DontDestroyOnLoad(_eventSystem);

        GameObject logic = new GameObject("GameLogic");
        GameObject.DontDestroyOnLoad(logic);
        m_gameLogic = logic.AddComponent<GameLogic>();

        m_audioroot = new GameObject("AudioMain");
        GameObject.DontDestroyOnLoad(m_audioroot);

        GameObject camerashake = new GameObject("camerashake");
        m_camerashake = camerashake.AddComponent<CameraShake>();
        GameObject.DontDestroyOnLoad(camerashake);
    }
}
