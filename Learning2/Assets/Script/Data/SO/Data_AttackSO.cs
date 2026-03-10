using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Data/Data_Attack" )]
public class Data_AttackSO : ScriptableObject
{
    [Header("攻击力")]
    public float currentAttackPower;

    [Header("攻击段数")]
    public int attackStage = 1;
    [Header("击退力度")]
    public float knockbackForce = 2f ; 

}
