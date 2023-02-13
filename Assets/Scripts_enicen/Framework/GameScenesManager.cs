using BitBenderGames;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScenesManager:Singleton<GameScenesManager>
{
    public GameObject m_terrainMesh;
    public GameObject m_entityRoot;
    public Camera m_battleCamera;
    public GameObject m_effectRoot;

    TouchInputController m_camera;
    Action m_loadSceneCB;
    ScenesEntity m_sceneEntity;

    Vector3 m_curCameraPos;
    Vector3 m_cameraPosPreview;
    public Vector3 m_mowang_target;
    public void Init() 
    {
        SceneManager.sceneLoaded += SceneLoadedCallBack;
    }

    private void SceneLoadedCallBack(Scene arg0, LoadSceneMode arg1)
    {
        RefreshSceneRootNode();
        InvokeCallBack();
    }

    private void InvokeCallBack() 
    {
        if (m_loadSceneCB != null)
        {
            m_loadSceneCB();
            m_loadSceneCB = null;
        }
    }

    private void RefreshSceneRootNode()
    {
        m_terrainMesh = GameObject.Find("TerrainMesh");
        m_entityRoot = GameObject.Find("EntityRoot");
        GameObject camerago = GameObject.Find("BattleCamera");
        m_effectRoot = GameObject.Find("TerrainMesh/4_effects");
        
        if (camerago) 
        {
            m_camera = camerago.GetComponent<TouchInputController>();
            m_battleCamera = camerago.GetComponent<Camera>();
            GameCore.GetInstance().m_camerashake.cameras.Clear();
            GameCore.GetInstance().m_camerashake.cameras.Add(m_battleCamera);
            m_camera.enabled = false;
            m_battleCamera.transform.position = m_cameraPosPreview;
            TimerUtils.StartTimer(0.1f, true, () => { m_camera.enabled = true; });
        }
    }

    public void Update()
    {
        if (m_sceneEntity != null)
        {
            m_sceneEntity.Update();
        }
    }

    public void LoadScene(string name, Action cb)
    {
        m_loadSceneCB = cb;
        SceneManager.LoadScene(name);
    }

    public void GameLevelFinish() 
    {
        m_sceneEntity.Release();
        m_sceneEntity = null;
        OpenScene(1);
    }

    public void OpenScene(int id , Action cb = null) 
    {
        SceneData data = GameConfigManager.GetInstance().GetItem<SceneData>("D_GameLevel", id);
        if (data != null)
        {
            m_cameraPosPreview = data.camera_pos_preview;
            m_curCameraPos = data.camera_pos;
            m_mowang_target = data.mowang_target;
            GameUIManager.GetInstance().ShowPanel<UILoading>("test", "test1");
            if (data.scene_type == 0)//非副本即不启动副本行为
            {
                LoadScene(data.scene_name, cb);
                GameUIManager.GetInstance().ShowPanel<UIMainpage>();
            }
            else if (data.scene_type == 1)
            {
                m_sceneEntity = new ScenesEntity(data, GameLevelFinish);
                m_sceneEntity.Enter();
            }
        }
        else
        {
            Debug.LogError("检查 D_Scene 中是否存在ID:" + id);
            //InvokeCallBack();
        }
    }
    public void ShowHeroPland(bool active) 
    {
        m_sceneEntity.ShowHeroPland(active, m_effectRoot,GameCore.GetInstance().m_gameLogic.m_myDemonKingInfo.m_entityId);
    }

    public void CreateHero(int id,Vector3 pos) 
    {
        if (m_sceneEntity == null)
        {
            Debug.LogError("非战斗生成英雄？id：" + id);
            return;
        }

        GameCore.GetInstance().m_gameLogic.CreateObjectHero(id,pos,Vector3.one);
    }

    public void StartBattle() 
    {
        m_camera.enabled = false;
        DOTween.Sequence().Append(m_battleCamera.transform.DOMove(m_curCameraPos, 0.5f)).AppendCallback(()=> {
            m_camera.enabled = true;
        });
        //m_camera.CamOverdragMargin2d = m_camera.ProjectVector3(m_curCameraPos);
        //m_camera.SetTargetPosition(m_curCameraPos);
        m_sceneEntity.StartBattle();
    }
    public ObjectBase GetDemonKingEntity() 
    {
        if (m_sceneEntity != null)
        {
            return m_sceneEntity.GetDemonKingEntity();
        }
        return null;
    }

    public void PlayEntityBuffEffect(int id, string mount, string res,bool isActive, Vector3 pos, Vector3 rotate, Vector3 scale)
    {
        m_sceneEntity.PlayEntityBuffEffect(id, mount, res, isActive,pos,rotate,scale);
    }

}
