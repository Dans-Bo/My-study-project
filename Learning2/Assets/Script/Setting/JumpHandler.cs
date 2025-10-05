using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 跳跃处理
/// </summary>
[System.Serializable]
public class JumpHandler:MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float fallGravityScale = 5f;
    [SerializeField] private float coyouteTime = 0.1f;

    private float coyouteTimeCounter;
    private Rigidbody2D rigid2D;
    private PhysicsCheck physicsCheck;

    public float JumpForce => jumpForce;
    public float CoyoteTime => coyouteTime;
    public float CoyouteTimeCounter => coyouteTimeCounter;

    public void Initialize(Rigidbody2D rb , PhysicsCheck physics)
    {
        rigid2D = rb;
        physicsCheck = physics;
    }


    public void Update()
    {
        if (physicsCheck.isGround)
        {
            coyouteTimeCounter = coyouteTime;
        }
        else
        {
            coyouteTimeCounter -= Time.deltaTime;
        }

        if (rigid2D.velocity.y > 0)
        {
            rigid2D.gravityScale = gravityScale;
        }
        else if (rigid2D.velocity.y < 0)
        {
            rigid2D.gravityScale = fallGravityScale;
        }
    }


    public bool CanJump()
    {
        return coyouteTimeCounter > 0 || physicsCheck.isGround;
    }

    public void ExecuteJump()
    {
        rigid2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        coyouteTimeCounter = 0;
    }
}
