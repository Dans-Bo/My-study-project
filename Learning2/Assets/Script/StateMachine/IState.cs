using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 状态机接口
/// </summary>
public interface IState
{
    void Enter();
    void Update();
    void FixedUpadte();
    void Exit();

} 

