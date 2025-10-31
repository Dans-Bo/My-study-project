using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider2D))]

/// <summary>
/// 玩家检测
/// </summary>
public class CheckPlayer : MonoBehaviour
{
     
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] BoxCollider2D checkBox;

    [Header("检测")]
    [SerializeField] float checkTime = 0.2f;
    Transform playerTransform;
    bool isPlayerInTrigger;
    bool canSeePlayer;
    Coroutine checkRoutine;

    public event Action<bool> IsSeePlayer;//是否发现角色
    public event Action<Vector2>  SetPlayerPosition; //记录玩家位置
    

    void Awake()
    {
        checkBox = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            isPlayerInTrigger = true;

            checkRoutine ??= StartCoroutine(CheckRoutine());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            canSeePlayer = false;
            playerTransform = null;

            IsSeePlayer?.Invoke(canSeePlayer);
            if(checkRoutine != null)
            {
                StopCoroutine(CheckRoutine());
                checkRoutine = null;
            }
            
        }

    }

    IEnumerator CheckRoutine()
    {
        while(isPlayerInTrigger)
        {
            Check();
            yield return new WaitForSeconds(checkTime);
        }
        checkRoutine = null;
    }

    void Check()
    {
        if (playerTransform == null) return;

        Vector2 rayOrigin = transform.position; 
        Vector2 directionToPlayer = ((Vector2)playerTransform.position - rayOrigin).normalized;
        float distanceToPlayer = Vector2.Distance(rayOrigin, playerTransform.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);
        Debug.DrawRay(transform.position, directionToPlayer * distanceToPlayer, hit.collider == null ? Color.green : Color.red, checkTime);

        bool seePlayer = hit.collider == null;

        if(seePlayer != canSeePlayer)
        {
            canSeePlayer = seePlayer;
            IsSeePlayer?.Invoke(canSeePlayer);   
        }
        if(canSeePlayer)
        {
            SetPlayerPosition?.Invoke(playerTransform.position);
        }
    } 
}
