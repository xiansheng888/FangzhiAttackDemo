using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleResult : UIBase
{

    Text text_result;
    public UIBattleResult() : base("uibattleresult",  PanelType.Window){}

    public override void OnCreate(object[] data)
    {
        base.OnCreate(data);
        text_result = UIUtils.GetComponent<Text>(m_go,"text_result");

        bool isWin = GameCore.GetInstance().m_gameLogic.m_enemyDemonKingInfo.m_curType == FSMStateType.Die ? true : false;
        text_result.text = isWin ? "Ê¤Àû" : "Ê§°Ü";

        Button btn = UIUtils.GetComponent<Button>(m_go, "btn_close");
        btn.onClick.AddListener(() =>
        {
            Action cb = (Action)data[0];
            cb();
        });
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }
}
