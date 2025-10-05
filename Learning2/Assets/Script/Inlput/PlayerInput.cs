using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerInputController playerInputActions;

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
    public bool IsStopAttack => playerInputActions.Game.Attack.WasReleasedThisFrame();


    void Awake()
    {
        playerInputActions = new PlayerInputController();
        waitJumpInputBufferTime = new WaitForSeconds(jumpInputBufferTime);
    }

    void OnEnable()
    {
        playerInputActions.Game.Jump.canceled += delegate
        {
            HasJumpInputBuffer = false;
        };
    }

   /*   void OnGUI()
    {
        Rect rect = new Rect(200, 200, 200, 200);
        string message = "Has Jump Input Buffer:" +jumpInputBufferTime ;
        GUIStyle style = new GUIStyle();

        style.fontSize = 20;
        style.fontStyle = FontStyle.Bold;
        GUI.Label(rect, message, style);
    }  */ 

    /// <summary>
    /// 启用控制表
    /// </summary>
    public void EnableGameplayerInput()
    {
        playerInputActions.Game.Enable();
        //Cursor.lockState = CursorLockMode.Locked; //将鼠标光标设置为锁定模式
    }
    public void DisableGamePlayerInput()
    {
        playerInputActions.Game.Disable();
    }
    /// <summary>
    /// 启用预输入跳跃协程
    /// </summary>
    public void SetJumpInputBufferTime()
    {
        StopCoroutine(nameof(JumpInputBufferCoroutine));
        StartCoroutine(nameof(JumpInputBufferCoroutine));
    }

    IEnumerator JumpInputBufferCoroutine()
    {
        HasJumpInputBuffer = true;

        yield return waitJumpInputBufferTime;

        HasJumpInputBuffer = false;
    }
}
