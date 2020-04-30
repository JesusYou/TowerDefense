using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior : MonoBehaviour
{
    public AIState defaultState;
    private List<AIState> aiStates = new List<AIState>();
    private AIState previousState;
    private AIState currentState;

    // Start is called before the first frame update
    void Start()
    {
        AIState[] states = GetComponents<AIState>();
        if (states.Length > 0)
		{
            foreach (AIState state in states)
			{
                aiStates.Add(state);
			}
            if (defaultState != null)
			{
                previousState = currentState = defaultState;
                if (currentState != null)
				{
                    ChangeState(currentState);
				}
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeState(AIState aiState)
	{
        if (aiState != null)
		{
            foreach (AIState state in aiStates)
			{
                if (state == aiState)
				{
                    previousState = currentState;
                    currentState = aiState;
                    NotifyOnStateExit();
                    DisableAllStates();
                    EnableNewState();
                    NotifyOnStateEnter();
                    return;
                }
			}
            DefaultState();
		}
	}

    public void OnTrigger(AIState.Trigger trigger, Collider2D mycol, Collider2D other)
	{
        currentState.OnTrigger(trigger, mycol, other);
	}

    //设置为默认状态
    public void DefaultState()
	{
        previousState = currentState;
        currentState = defaultState;
        NotifyOnStateExit();
        DisableAllStates();
        EnableNewState();
        NotifyOnStateEnter();
	}

    private void NotifyOnStateEnter()
	{
        currentState.OnStateEnter(previousState, currentState);
	}

    private void NotifyOnStateExit()
	{
        previousState.OnStateExit(previousState, currentState);
	}

    private void DisableAllStates()
	{
        foreach (AIState state in aiStates)
		{
            state.enabled = false;
		}
	}

    private void EnableNewState()
	{
        currentState.enabled = true;
	}
}
