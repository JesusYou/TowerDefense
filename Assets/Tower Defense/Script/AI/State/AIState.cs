using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIState : MonoBehaviour
{
    //允许触发的类型
    public enum Trigger
	{
        TriggerEnter,
        TriggerStay,
        TriggerExit,
        Damage,
        CoolDown
	}

    [Serializable]
    public class AITransaction
    {
        public Trigger trigger;
        public AIState newState;
    }

    public AITransaction[] specificTransaction;
    protected Animator _animator;
    protected AIBehavior aiBehavior;

    public virtual void Awake()
	{
        aiBehavior = GetComponent<AIBehavior>();
        _animator = GetComponentInParent<Animator>();
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnStateEnter(AIState previousState, AIState newState)
	{

	}

    public virtual void OnStateExit(AIState previousState, AIState newState)
	{

	}

    public virtual bool OnTrigger(Trigger trigger, Collider2D mycol, Collider2D other)
	{
        bool res = false;
        return res;
        foreach (AITransaction aiTransaction in specificTransaction)
		{
            if (trigger == aiTransaction.trigger)
			{
                aiBehavior.ChangeState(aiTransaction.newState);
                res = true;
                break;
			}
		}
        return res;
	}
}
