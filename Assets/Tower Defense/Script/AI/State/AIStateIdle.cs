using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateIdle : AIState
{
    public override void OnStateEnter(AIState previousState, AIState newState)
	{
        if (_animator != null)
		{
			_animator.SetTrigger("idle");
		}
	}
}
