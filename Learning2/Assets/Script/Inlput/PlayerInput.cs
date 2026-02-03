using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputController playerInputActions;

    //移动属性
    Vector2 Axes => playerInputActions.Game.Move.ReadValue<Vector2>();
    public float AxisX => Axes.x;
    public bool IsMove => AxisX != 0f;

    //跳跃属性
    public bool IsJump => playerInputActions.Game.Jump.WasPressedThisFrame();
    public bool IsStopJump => playerInputActions.Game.Jump.WasReleasedThisFrame();
    public bool HasJumpInputBuffer { get; set; } //是否处于预输入跳跃输入状态
    private float jumpInputBufferTime = 0.2f; //预输入跳跃缓冲时间
    WaitForSeconds waitJumpInputBufferTime;

    //攻击属性
    public bool IsAttack => playerInputActions.Game.Attack.WasPressedThisFrame();
    //public bool IsStopAttack => playerInputActions.Game.Attack.WasReleasedThisFrame();

    #region UI交互
    public bool IsConfirm => playerInputActions.Game.Confirm.WasPressedThisFrame(); //f键交互
    public bool isGameESC => playerInputActions.Game.ESC.WasPressedThisFrame(); //正常状态下ESC键
    public bool isUIEsc => playerInputActions.UI.ESC.WasPressedThisFrame(); //UI状态下ESC
    //背包操作
    public bool isOpenBag => playerInputActions.Game.Bag.WasPressedThisFrame(); //打开背包
    public bool isClosedBag => playerInputActions.UI.ClosedBag.WasPressedThisFrame(); //关闭背包

    //装备&属性界面
    public bool isOpenEquipPanel => playerInputActions.Game.EquipPanel.WasPressedThisFrame();
    public bool isCloseEquipPanel => playerInputActions.UI.CloseEquipPanel.WasPressedThisFrame();
    #endregion


    void Awake()
    {
        if (playerInputActions == null)
        {
            playerInputActions = new PlayerInputController();
        }

        waitJumpInputBufferTime = new WaitForSeconds(jumpInputBufferTime);
    }

    void OnEnable()
    {
        if (playerInputActions != null)
        {
            playerInputActions.Game.Enable(); 
        
            playerInputActions.Game.Jump.canceled += delegate
            {
                HasJumpInputBuffer = false;
            };
        }else Debug.Log("OnEnable 时，playerInputAction为空");
    }

    void OnDisable()
    {
        playerInputActions?.Game.Disable();
    }

    /// <summary>
    /// 启用玩家控制表
    /// </summary>
    public void EnableGameplayerInput()
    {
        if(playerInputActions == null)
        {
            Debug.LogError("EnableGameplayerInput：playerInputActions为null，已自动重新初始化");
            playerInputActions = new PlayerInputController(); 
        }

        playerInputActions.Game.Enable();
        playerInputActions.UI.Disable();
        Cursor.lockState = CursorLockMode.Locked; //将鼠标光标设置为锁定模式
        Cursor.visible = false; //隐藏鼠标光标
    }
    public void DisableGamePlayerInput()
    {
        playerInputActions.Game.Disable();
    }
/// <summary>
/// 启用UI控制
/// </summary>
    public void EnableUIActionMap()
    {
        if (playerInputActions != null) 
        {
            playerInputActions.Game.Disable();
            playerInputActions.UI.Enable();
            Cursor.lockState = CursorLockMode.None; //解锁光标
            Cursor.visible = true; //显示光标
        }
    }
    /// <summary>
    /// 禁用ui控制，并切换回game控制
    /// </summary>
    public void DisableUIAcitionMap()
    {
        EnableGameplayerInput();

    }
    /// <summary>
    /// 启用预输入跳跃协程
    /// </summary>
    public void SetJumpInputBufferTime()
    {
        StopCoroutine(nameof(JumpInputBufferCoroutine));
        StartCoroutine(nameof(JumpInputBufferCoroutine));
    }

    private IEnumerator JumpInputBufferCoroutine()
    {
        HasJumpInputBuffer = true;

        yield return waitJumpInputBufferTime;

        HasJumpInputBuffer = false;
    }
}
