using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Data/Data_Attack" )]
public class Data_Attack : ScriptableObject
{
    [Header("攻击力")]
    public float baseDamge;
    public float currentAttackPower;

}
