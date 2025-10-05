using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseState
{
     void OnEnter();
     void OnUpdate();
     void OnFixedUpdate();
     void OnExit();

     //bool CanTransitionTo(PlayerStateType type);
}
