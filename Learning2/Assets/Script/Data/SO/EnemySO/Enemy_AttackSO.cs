using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Data/Enemy/Enemy_Attack" )]
public class Enemy_AttackSO: ScriptableObject
{
    [Header("攻击力")]
    public float baseDamge;
    public float currentAttackPower;

    [Header("击退力度")]
    public float knockbackForce = 2f ; 
}
