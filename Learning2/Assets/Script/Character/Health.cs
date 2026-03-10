using System.Collections;
using Microsoft.Win32;
using Unity.Mathematics;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerAttributeManager))]
public class Health : MonoBehaviour
{   
    //TODO 与角色属性关联
    [SerializeField] Data_HealthSO healthSO;
    [SerializeField] PlayerAttributeManager attributeManager;
    private int currentDefense ;


    public Action<Transform> onTakeDamage; //受伤事件
    public event Action<Transform,float> OnHurt;
    public event Action OnOnlyHurt;
    public event Action OnDie;
    private bool isDead = false;



    void Awake()
    {
        attributeManager = GetComponent<PlayerAttributeManager>();
        
    }

    void Start()
    {
        currentDefense = attributeManager.GetAttribute(PlayerAttribute.Defense);
        healthSO.isInvulnerable = false;
    }

    void OnEnable()
    {
        if(attributeManager != null)
        {
            attributeManager.OnAttributeChange += OnAttributeChange;
        }

        
    }

    void OnDisable()
    {
        if(attributeManager != null)
        {
            attributeManager.OnAttributeChange -= OnAttributeChange;
        }
    }

    private void OnAttributeChange(PlayerAttribute attribute, int newValue)
    {
        if(attribute == PlayerAttribute.Defense)
        {
            currentDefense = attributeManager.GetAttribute(PlayerAttribute.Defense);
        }
        else if(attribute == PlayerAttribute.HP && newValue <= 0 && !isDead)
        {
            isDead = true;
            Debug.Log($"死亡");
            OnDie?.Invoke();
        }
    }

    public void TakeDamage(Enemy_AttackSO attack,Transform position)
    {
        if (healthSO.isInvulnerable)
        {
            return;
        }
        
        Debug.Log($"触发受伤,敌人攻击力为{attack.currentAttackPower}");

        int damage = (int)Mathf.Max(attack.currentAttackPower - currentDefense, 0);    
        
        //if( damage <= 0) return; 

        TakePassiveDamage(damage);
        
        

        StartCoroutine(nameof(InvelnerableCoroutine));//启动无敌时间协程
        
        OnHurt?.Invoke(position,attack.knockbackForce);
        
    }

    /* public void TakeDamage(Enemy_AttackSO attack)
    {
        if (healthSO.isInvulnerable)
        {
            return;
        }
        
        int damage = (int)Mathf.Max(attack.currentAttackPower - currentDefense, 0);    
        //if( damage <= 0) return;

        TakePassiveDamage(damage);
        

        StartCoroutine(nameof(InvelnerableCoroutine));//启动无敌时间协程
        
        OnOnlyHurt?.Invoke();
        
    } */

    protected virtual IEnumerator InvelnerableCoroutine()
    {
        healthSO.isInvulnerable = true;
        //等待无敌时间
        yield return new WaitForSeconds(healthSO.invulnerableDuration);

        healthSO.isInvulnerable = false;
    }

    /// <summary>
    /// 恢复HP
    /// </summary>
    /// <param name="value"></param>
    public void RestoreHealth(int value)
    {
        attributeManager.ModifyAttribute(PlayerAttribute.HP, value);
    }
    /// <summary>
    /// 扣血
    /// </summary>
    /// <param name="damage"></param>
    public void TakePassiveDamage(int damage)
    {
        if (isDead) return;
        
        attributeManager.ModifyAttribute(PlayerAttribute.HP, -damage);
    }
/// <summary>
/// 获取当前生命值
/// </summary>
/// <returns></returns>
    private int GetCurrentHp()
    {
        return attributeManager.GetAttribute(PlayerAttribute.HP);
    }

/// <summary>
/// 获取最大生命值
/// </summary>
/// <returns></returns>
    private int GetMaxHp()
    {
        return attributeManager.GetAttribute(PlayerAttribute.MaxHP);
    }

}

