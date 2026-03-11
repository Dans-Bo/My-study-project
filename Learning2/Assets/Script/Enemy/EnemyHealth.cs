using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Enemy_HealthSO health;
    //private int currentDefense ;


    public Action<Transform> onTakeDamage; //受伤事件
    public event Action<Transform,float> OnHurt;
    public event Action OnDie;



    void Awake()
    {
        health.currentHealth = health.maxHealth;
        
    }

    public void TakeDamage(Data_AttackSO attack, Transform attackerTransform)
    {
        if (health.isInvulnerable)
        {
            return;
        }
        
        health.currentHealth -= attack.currentAttackPower ;
        health.currentHealth = Mathf.Clamp(health.currentHealth, 0, health.maxHealth);

        StartCoroutine(nameof(InvelnerableCoroutine));//启动无敌时间协程
        OnHurt?.Invoke(attackerTransform ,attack.knockbackForce );
        Debug.Log($"怪物受伤,当前生命值：{health.currentHealth}");
        
        if (health.currentHealth <= 0)
        {
            OnDie?.Invoke();
        }
    }

    protected virtual IEnumerator InvelnerableCoroutine()
    {
        health.isInvulnerable = true;
        //等待无敌时间
        yield return new WaitForSeconds(health.invulnerableDuration);

        health.isInvulnerable = false;
    }
}
