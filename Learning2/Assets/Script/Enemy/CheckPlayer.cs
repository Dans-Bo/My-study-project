using System;
using System.Collections;
using System.Security.Permissions;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))]

/// <summary>
/// 玩家检测
/// </summary>
public class CheckPlayer : MonoBehaviour
{
    [Header("检测参数")]
    [Tooltip("检测图层")]
    [SerializeField] LayerMask playerLayer;
    [Tooltip("检测半径")]
    [SerializeField] float checkRadius = 10f;
    [Tooltip("检测中点")]
    [SerializeField] Vector2 checkPoint;

    [Tooltip("射线检测时间")]
    [SerializeField] float checkTime = 0.2f;
    [Tooltip("玩家离开检测范围时，多长时间后才停止射线检测")]
    [SerializeField] float stopCheckTime = 1.5f;
    [Tooltip("粗检测，玩家是否进入视线范围")]
    [SerializeField] bool isPlayerInSight = false; 
    [Tooltip("射线检测，是否检测到玩家")]
    [SerializeField]bool canSeePlayer = false;

    //private bool firstSeePlayer = false;
    Collider2D [] collider2Ds = new Collider2D [1];

    public  Action<bool> IsSeePlayer;//是否发现角色
    public  Action<Vector2>  SetPlayerPosition; //记录玩家位置

    private float lastSeePlayerTime ; //上一次发现玩家的时间
    //Transform playerTransform;

    private Coroutine checkRoutine;
    private Coroutine stopCheckRoutine;

    void Awake()
    {
        IsSeePlayer?.Invoke(false);
        SetPlayerPosition?.Invoke(Vector2.negativeInfinity);
    }
    //事件
    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //firstSeePlayer = false;
           isPlayerInSight = true ;
           Debug.Log("发现玩家");

           if(checkRoutine == null)
            {
                checkRoutine = StartCoroutine(CheckPlayerRoutine());
            }
        }

    }


    /// <summary>
    /// 射线检测玩家位置
    /// </summary>
    private IEnumerator CheckPlayerRoutine()
    {

        while(isPlayerInSight) //进入粗检测碰撞盒后触发射线检测
        {
            Vector2 offset = transform.TransformDirection(checkPoint);  //跟随怪物朝向（本地坐标转换世界坐标）
            Vector2 point = (Vector2)transform.position + offset ;

            Array.Clear(collider2Ds, 0, collider2Ds.Length);  //清空上一次的数据

            int hit = Physics2D.OverlapCircleNonAlloc(point ,checkRadius,collider2Ds,playerLayer);

            if(hit > 0)
            {
                if (!canSeePlayer)
                {
                    canSeePlayer = true;
                    IsSeePlayer?.Invoke(true);
                    
                }

                lastSeePlayerTime = Time.time;
                //获得player的位置
                Collider2D playerCollider = collider2Ds[0];
                Vector2 playerPosition = playerCollider.transform.position;
                IsSeePlayer?.Invoke(true);
                Debug.Log("发现玩家");
                
                if(playerPosition.y > transform.position.y) //如果玩家在怪物上方，投影至和怪物一样的Y轴，防止怪物蹦
                {
                    playerPosition.y = transform.position.y;
                }

                SetPlayerPosition?.Invoke(playerPosition);

                /* if(!firstSeePlayer)  //第一次检测到玩家，事件通知
                {
                    IsSeePlayer?.Invoke(true);
                    firstSeePlayer = true;
                } */

                if(stopCheckRoutine != null) //如果有延迟停止检测，立即取消
                {
                    StopCoroutine(stopCheckRoutine);
                    stopCheckRoutine = null;
                }      
            }
            else
            {
                if(stopCheckRoutine == null) //启动延迟暂停
                {
                    stopCheckRoutine = StartCoroutine(StopCheck());
                    
                    
                } 
            }
            yield return new WaitForSeconds(checkTime); //避免每帧都进行检测

        }

    }
/// <summary>
/// 暂停检测，规定时间内没有检测到玩家才最终停止检测
/// </summary>
/// <returns></returns>
    private IEnumerator StopCheck()
    {
        while(Time.time - lastSeePlayerTime < stopCheckTime) 
        {
            yield return null; //只要在延迟停止检测的时间内，就继续等待
        }

        if(checkRoutine != null)
        {
            StopCoroutine(checkRoutine);
            checkRoutine = null;
        }
        
        canSeePlayer = false;
        isPlayerInSight = false;
        //firstSeePlayer = false;
        IsSeePlayer?.Invoke(canSeePlayer);
        SetPlayerPosition?.Invoke(Vector2.negativeInfinity); // 清空玩家位置
        Debug.Log($"停止检测，当前是否能看见玩家：{IsSeePlayer}");
        
    }
    /// <summary>
    /// 外部调用停止检测（当怪物消失，死亡时调用）
    /// </summary>
    public void ForceStopCheck()
    {
        if(checkRoutine != null)
        {
            StopCoroutine(checkRoutine);
            checkRoutine = null ;
        }
        if(stopCheckRoutine != null)
        {
            StopCoroutine(stopCheckRoutine);
            stopCheckRoutine = null;
        }

        isPlayerInSight = false;
        canSeePlayer = false;

        //firstSeePlayer = false;
        //Debug.Log($"停止检测");
    }

    private void OnDestroy()  //对象销毁后停止协程，防止残留
    {
        ForceStopCheck();
    }

    private void OnDrawGizmosSelected()
    {
        // 只在选中该物体时绘制
        if (!enabled) return;

        //计算检测中心点
        Vector2 offset = transform.TransformDirection(checkPoint);
        Vector3 detectCenter = (Vector2)transform.position + offset;

        //绘制检测圆
        Gizmos.color = canSeePlayer ? Color.red : Color.green; // 看到玩家变红
        Gizmos.DrawWireSphere(detectCenter, checkRadius); // 画检测半径

        //绘制检测中心点
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(detectCenter, 0.1f); // 检测中心点标记

        //绘制从怪物到检测中心点的偏移线
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, detectCenter); 
    }
}
