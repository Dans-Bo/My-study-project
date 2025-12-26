using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerUIController : MonoBehaviour
{
    //private bool isUIPanelOpen = false;
    //private bool isInputProcessing = false;

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

    void Update()
    {
        if(PlayerInput.isGameESC) UIManager.Instance.OpenPanel(ConstUIName.homePanel);
        if( PlayerInput.isUIEsc) UIManager.Instance.CloseCurrentActivePanel();
        
    }

   /*  private void HandleGameESC()
    {
        if(PlayerInput.isGameESC && !isUIPanelOpen)
        {
            StartCoroutine(ProcessGameEsc());
        }
    }

   

    private IEnumerator ProcessGameEsc()
    {
        isInputProcessing = true;

        playerInput.DisableGamePlayerInput();
        playerInput.EnableUIActionMap();

        if(UIManager.Instance != null)
        {
            UIManager.Instance.OpenPanel(ConstUIName.homePanel);
            isUIPanelOpen = true;
        }
        else
        {
            Debug.LogError($"[{nameof(PlayerUIController)}] UIManager为空",this);
            playerInput.EnableGameplayerInput(); //回滚输入映射
        }

        //等待一帧，防止重复触发
        yield return null ;
        isInputProcessing = false;
    }

    private void HandleUIESC()
    {
        if(playerInput.isUIEsc && !isInputProcessing )
        {
            StartCoroutine(ProcessUIESC());
        }
    }

    private IEnumerator ProcessUIESC()
    {
        isInputProcessing = true;

        bool isClosed = UIManager.Instance.CloseCurrentActivePanel();
        if(isClosed)
        {
            playerInput.DisableUIAcitionMap();
            playerInput.EnableGameplayerInput();
        }

        yield return null;
        isInputProcessing = false;
    } */

    
}
