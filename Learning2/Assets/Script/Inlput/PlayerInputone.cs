using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.InputSystem;



[CreateAssetMenu(fileName = "PlayerInputone")]
public class PlayerInputone : ScriptableObject,PlayerInputController.IGameActions

{
    public event UnityAction<Vector2> Move;
    //public event UnityAction stopMove;
    public event UnityAction Attack;
    public event UnityAction Jump;

    PlayerInputController playerInputController;

    void OnEnable()
    {
        playerInputController = new PlayerInputController();

        //playerinput脚本继承了这个接口，所以传入this，将其注册为回调函数的接收者
        playerInputController.Game.SetCallbacks(this);
    }

    void OnDisable()
    {
        playerInputController.Disable();
    }

    public void EnableGameInput()
    {
        SwichActionMap(playerInputController.Game); //启用Game动作表
    }


    //切换动作表，可实现多种不同状态下的操作
    void SwichActionMap(InputActionMap actiionMap)
    {
        playerInputController.Disable();
        actiionMap.Enable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            Attack?.Invoke();
        }

        if (context.performed)
        {
            Attack?.Invoke();
        }
        /* switch (context.phase)
        {
            case InputActionPhase.Started: //按键按下的那一刻
                break;
            case InputActionPhase.Performed: //持续按下按键
                break;
            case InputActionPhase.Canceled: //按键松开的那一刻
                break;
        } */
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Jump?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            Move?.Invoke(context.ReadValue<Vector2>());
        }
    }

    
}
