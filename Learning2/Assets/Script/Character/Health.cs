using System.Collections;
using Microsoft.Win32;
using Unity.Mathematics;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Health : MonoBehaviour
{   
    /* [Header("血量")]
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;


    [Header("无敌时间")]
    [SerializeField] float invulnerableDuration;
    [SerializeField] bool isInvulnerable; */

    [SerializeField] Data_HealthSO health;

    //public UnityEvent<Transform> onTakeDamage; //受伤事件
    public event Action OnHurt;
    public event Action OnDie;


    void Awake()
    {
       // playerHealth = GetComponent<Data_Health> ();
        health.currentHealth = health.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void TakeDamage(Data_AttackSO attack)
    {
        if (health.isInvulnerable)
        {
            return;
        }
        
        health.currentHealth -= attack.currentAttackPower;
        //health.currentHealth = math.max(0, playerHealth.currentHealth);
        StartCoroutine(nameof(InvelnerableCoroutine));//启动无敌时间协程
        // onTakeDamage?.Invoke(attack.transform);
        OnHurt?.Invoke();
        
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

