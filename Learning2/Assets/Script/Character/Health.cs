using System.Collections;
using Microsoft.Win32;
using Unity.Mathematics;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Health : MonoBehaviour
{   
    [Header("血量")]
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;


    [Header("无敌时间")]
    [SerializeField] float invulnerableDuration;
    [SerializeField] bool isInvulnerable;

    //public UnityEvent<Transform> onTakeDamage; //受伤事件
    public event Action OnHurt;
    public event Action OnDie;


    void Awake()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Die()
    {
        
    }

    public void TakeDamage(Attack attack)
    {
        if (isInvulnerable)
        {
            return;
        }
        currentHealth -= attack.currentAttackPower;
        currentHealth = math.max(0, currentHealth);
        StartCoroutine(nameof(InvelnerableCoroutine));//启动无敌时间协程
        // onTakeDamage?.Invoke(attack.transform);
        OnHurt?.Invoke();
        if (currentHealth <= 0)
        {
            OnDie?.Invoke();
        }
    }

    protected virtual IEnumerator InvelnerableCoroutine()
    {
        isInvulnerable = true;
        //等待无敌时间
        yield return new WaitForSeconds(invulnerableDuration);

        isInvulnerable = false;
    }

}

