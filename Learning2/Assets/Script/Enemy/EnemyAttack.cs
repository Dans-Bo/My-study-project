using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] Enemy_AttackSO attack;
    void Awake()
    {
        attack.currentAttackPower = attack.baseDamge;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Health>()?.TakeDamage(attack,this.transform);
    }
}
