using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMove : AIState
{
    [Space(10)] public Transform destination;
    public AIState aiState;
    private NavAgent navAgent;

    public override void Awake()
	{
        base.Awake();
        navAgent = GetComponent<NavAgent>();
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
	{
        if ((Vector2) transform.position == (Vector2)destination.transform.position)
		{
            navAgent.LookAtTarget(destination.right);
            aiBehavior.ChangeState(aiState);
		}
	}

    public override void OnStateEnter(AIState previousState, AIState newState)
	{
        navAgent.destination = destination.position;
        navAgent.move = true;
        navAgent.turn = true;
        if (_animator != null)
		{
            _animator.SetTrigger("move");
		}
	}

    public override void OnStateExit(AIState previousState, AIState newState)
	{
        navAgent.move = false;
        navAgent.turn = false;
	}
}
