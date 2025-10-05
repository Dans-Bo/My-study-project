using System.Net.Http.Headers;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Collections;


public class Enemy : MonoBehaviour
{

    //追踪玩家
    /*    public Transform playerTransform;
             public float enemyMoveSpeed;
             public float followDistance; */

    protected Rigidbody2D rb;
    protected Animator animator;

    //受伤条件
    [HideInInspector] public Transform attacker;
    public bool isHurt;
    public bool isDead;
    public float hurtForce; // 受伤击退力量


    [Header("移动")]

    public float normalSpeed;
    public float currentSpeed;
    private Vector3 faceDirection;

    void Start()
    {
        //playerTransform = GameObject.FindWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentSpeed = normalSpeed;
    }

    void FixedUpdate()
    {
        Move();
    }

    void Update()
    {
        // FollowPlayer();

        faceDirection = new Vector3(transform.localScale.x, 0, 0);

        
    }

    /*  void FollowPlayer()
     {
         if (Mathf.Abs(transform.position.x - playerTransform.position.x) < followDistance)
         {
             transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, enemyMoveSpeed * Time.deltaTime);
             if (transform.position.x - playerTransform.position.x < 0) transform.eulerAngles = new Vector3(0, 0, 0);
             if (transform.position.x - playerTransform.position.x > 0) transform.eulerAngles = new Vector3(0, 180, 0);
         }
     } */

    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDirection.x, rb.velocity.y);
    }


    //受伤朝攻击方向转身
    public void OnTakeDamage(Transform attackTransform)
    {
        attacker = attackTransform;
        Debug.Log( attackTransform.position.x - transform.position.x);
        if (attackTransform.position.x - transform.position.x > 0.01)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform .localScale.y ,transform.localScale.z);
        }

        if (attackTransform.position.x - transform.position.x < 0.01)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform .localScale.y ,transform.localScale.z);
        }


        isHurt = true;
        //animator.SetTrigger("hurt");
        //受伤击退方向
        Vector2 dir = new Vector2(transform.position.x - attackTransform.position.x, 0).normalized;

        StartCoroutine(OnHurt(dir));
    }

    private IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.35f);
        isHurt = false;
    }
}
