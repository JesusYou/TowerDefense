using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStatePatrol : AIState
{
    [Space(10)]
    [HideInInspector] public Pathway pathway;
    public bool loop = false;
    private WayPoint destination;
    NavAgent navAgent;

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
        if (destination != null)
		{
            //如果到达目的地
            if ((Vector2) destination.transform.position == (Vector2) transform.position)
			{
                destination = pathway.GetNextWayPoint(destination, loop);
                if (destination != null)
				{
                    navAgent.destination = destination.transform.position;
				}
			}
		}
	}

    public override void OnStateEnter(AIState previousState, AIState newState)
	{
        if (pathway == null)
		{
            pathway = FindObjectOfType<Pathway>();
            Debug.Assert(pathway, "Have no path");
        }
        if (destination == null)
		{
            //获取下一个节点
            destination = pathway.GetNearestWayPoint(transform.position);
		}
        navAgent.destination = destination.transform.position;
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

    //获取路径上剩下的距离
    public float GetRemainingPath()
	{
        Vector2 distance = destination.transform.position - transform.position;
        return (distance.magnitude + pathway.GetPathDistance(destination));
	}
}
