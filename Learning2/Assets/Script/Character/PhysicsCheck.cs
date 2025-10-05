using System.Runtime.CompilerServices;
using System.Security;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private Rigidbody2D rb;
    //private Player player;
    [SerializeField] float angle; 
    [SerializeField] LayerMask ground;
    [SerializeField] LayerMask wall;
    
    
    [Header("地面检测")]
    [SerializeField] Vector2 groundSize;
    [SerializeField] Vector2 groundOffset;
    [SerializeField] float groundDistance;
    [Header("墙壁检测")]
    [SerializeField] Vector2 wallSize;
    [SerializeField] Vector2 leftWallOffset;
    [SerializeField] Vector2 rightWallOffset;
    [SerializeField] float wallDistance;
    [SerializeField] float rayOffset;
    [SerializeField] float rayHight;

    public bool isGround;
    public bool IsFall => rb.velocity.y < 0f && !isGround;
    public bool isWall;
    //private bool IsLeft => transform.parent.localScale.x < 0.1f;
    //private bool IsRight => transform.parent.localScale.x > 0.1f;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
       // player = GetComponent<Player>();
    }

    void FixedUpdate()
    {
        PlayerPhysicsCheck();
    }

    void PlayerPhysicsCheck()
    {
         RaycastHit2D groundCheck = Raycast(groundOffset, groundSize,angle, Vector2.down, groundDistance, ground);

        if (groundCheck)
        {
            isGround = true;
        }
        else isGround = false;

        float direction = transform.parent.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0f);

        RaycastHit2D leftWallCheck = RaycastWll(new Vector2(rayOffset * direction,rayHight), grabDir,  wallDistance, wall);
        //RaycastHit2D rightWallCheck = RaycastWll(rightWallOffset, Vector2.right, wallDistance, wall);
        
        if (leftWallCheck )
        {
            isWall = true;
        }
        else isWall = false; 


        
    }

    private RaycastHit2D Raycast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance,LayerMask ground)
    {

        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.BoxCast(pos + origin, size, angle, direction, distance,ground);  

        //Debug.DrawRay(pos + origin, direction * distance, hit ? Color.red : Color.green);
        return hit;
    }

    private RaycastHit2D RaycastWll(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;//获得游戏角色位置

        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer); //图层判断（角色当前位置加上offset，方向，距离，图层）

        Color color = hit ? Color.red : Color.green;

        Debug.DrawRay(pos + offset, rayDiraction * length, color);

        return hit;
    }


      void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + (Vector3)groundOffset , Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, groundSize);

    }  
}
