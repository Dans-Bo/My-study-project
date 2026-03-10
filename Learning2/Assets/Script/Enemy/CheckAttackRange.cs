using System;

using UnityEngine;

public class CheckAttackRange : MonoBehaviour
{
    bool canAttack;
    public event Action<bool> IsCanAttack;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            canAttack = true;
            IsCanAttack?.Invoke(canAttack);
            //Debug.Log($"进入攻击距离");
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            IsCanAttack?.Invoke(canAttack);
        }
    }

    void OnTriggerExit2D(Collider2D collision) 
    {
        if(collision.CompareTag("Player"))
        {
            canAttack = false;
            IsCanAttack?.Invoke(canAttack);
            //Debug.Log($"超出攻击距离");
        } 
    }
    
      
}
