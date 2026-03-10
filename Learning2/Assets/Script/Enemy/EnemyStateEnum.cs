using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    /// <summary>
    /// 攻击
    /// </summary>
    Attack,
    /// <summary>
    /// 站立
    /// </summary>
    Idle,
    /// <summary>
    /// 巡逻
    /// </summary>
    Partol,
    /// <summary>
    /// 追击
    /// </summary>
    Chase,
    /// <summary>
    /// 受伤
    /// </summary>
    Hurt,
    /// <summary>
    /// 死亡
    /// </summary>
    Died,
    /// <summary>
    /// 攻击冷却
    /// </summary>
    AttackCooldown,
}
