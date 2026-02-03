using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{

    public PlayerAttributeManager attributeManager;

    [Header("获得经验值")]
    public Button getExp;
    public int expValue;
    [Header("恢复血量")]
    public Button recoverHP;
    public int recoverValue;
    [Header("扣除血量")]
    public Button loseHP;
    public int loseValue;
        [Header("恢复蓝量")]
    public Button recoverMP;
    public int recoverMpValue;
    [Header("扣除蓝量")]
    public Button loseMP;
    public int loseMpValue;

    void Awake()
    {
        InitUIClick();

    }

    void InitUIClick()
    {
        getExp.onClick.AddListener(OnGetExp);
        recoverHP.onClick.AddListener(OnRecoverHp);
        loseHP.onClick.AddListener(OnLoseHp);
        recoverMP.onClick.AddListener(OnRecoverMp);
        loseMP.onClick.AddListener(OnLoseMp);
    }

    private void OnLoseMp()
    {
         attributeManager.ModifyAttribute(PlayerAttribute.MP, -loseMpValue);
    }

    private void OnRecoverHp()
    {
         attributeManager.ModifyAttribute(PlayerAttribute.HP,recoverValue);
    }

    private void OnRecoverMp()
    {
        attributeManager.ModifyAttribute(PlayerAttribute.MP,recoverMpValue);
    }

    private void OnLoseHp()
    {
         attributeManager.ModifyAttribute(PlayerAttribute.HP,-loseValue);
    }

    private void OnGetExp()
    {
        attributeManager.ModifyAttribute(PlayerAttribute.Exp,expValue);
    }
}
