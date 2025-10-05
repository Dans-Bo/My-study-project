using UnityEngine;

public class Attack : MonoBehaviour
{

    [Header("攻击力")]
    public float baseDamge;
    public float currentAttackPower;

    void Awake()
    {
        currentAttackPower = baseDamge;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Health>()?.TakeDamage(this);
    }


}
