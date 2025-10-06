using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Health", menuName = "Data/Data_Health" )]
public class Data_Health : ScriptableObject 
{
    [Header("血量")]
    public float maxHealth;
    public float currentHealth;


    [Header("无敌时间")]
    public float invulnerableDuration;
    public bool isInvulnerable;
}

