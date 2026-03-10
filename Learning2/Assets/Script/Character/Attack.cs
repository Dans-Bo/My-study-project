using System;
using UnityEngine;

public class Attack : MonoBehaviour
{

    [SerializeField] Data_AttackSO attack;
    [SerializeField] PlayerAttributeManager attributeManager;
    private int AttackStage => attack.attackStage;

    void Awake()
    {
        attributeManager = GetComponentInParent<PlayerAttributeManager>();
        attack.currentAttackPower = attributeManager.GetAttribute(PlayerAttribute.Attack);

        Debug.Log($"当前攻击力为{attack.currentAttackPower}");
        UpdateAttackPower();
        Debug.Log($"当前攻击力为{attack.currentAttackPower}");
    }

    void OnEnable()
    {
        attributeManager.OnAttributeChange += OnAttackChange;
    }

    void OnDisable()
    {
        attributeManager.OnAttributeChange -= OnAttackChange;
    }

    private void UpdateAttackPower()
    {
        float baseAttack = attributeManager.GetAttribute(PlayerAttribute.Attack);
        
        //根据段数设置不同倍率
        float multiplier = AttackStage switch
        {
            1 => 1.0f,
            2 => 1.2f,
            3 => 1.5f,
            _ => 1.0f
        };

        attack.currentAttackPower = baseAttack * multiplier;
    }

    private void OnAttackChange(PlayerAttribute attribute, int newValue)
    {
        if(attribute == PlayerAttribute.Attack)
        {
            UpdateAttackPower();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<EnemyHealth>()?.TakeDamage(attack ,this.transform);
    }

    private int GetCurrentAttackPower()
    {
        return attributeManager.GetAttribute(PlayerAttribute.Attack);
    }
}
