using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerUIController : MonoBehaviour
{
    private PlayerController playerCtrl;

    private PlayerInput PlayerInput //延迟初始化
    {
        get
        {
            PlayerInput input = GetComponent<PlayerInput>();

            if(input == null)
            {
                Debug.LogError($"{nameof(PlayerUIController)} 未找到PlayerInput组件"); 
            }
            return input;
        }
    }

    void Start()
    {
        playerCtrl = GetComponent<PlayerController>();
    }

    void Update()
    {
        if(PlayerInput.isGameESC) UIManager.Instance.OpenPanel(ConstUIName.homePanel);
        if( PlayerInput.isUIEsc) UIManager.Instance.CloseCurrentActivePanel();
        CheckUiInput();
    }

    private void CheckUiInput()
    {
        //打开&关闭背包界面
        if(PlayerInput.isOpenBag) UIManager.Instance.OpenPanel(ConstUIName.packagePanel);
        if(PlayerInput.isClosedBag) UIManager.Instance.CloseCurrentActivePanel();

        //按下F键且存在可交互物体
        if(PlayerInput.IsConfirm && playerCtrl.currentInteractable != null)
        {
            playerCtrl.currentInteractable.TriggerAction();
        }

        //打开&关闭装备界面
        if(PlayerInput.isOpenEquipPanel) UIManager.Instance.OpenPanel(ConstUIName.equipmentPanel);
        if(PlayerInput.isCloseEquipPanel) UIManager.Instance.CloseCurrentActivePanel();
    }
    
}
