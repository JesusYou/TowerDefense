using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateAttack : AIState
{
    [Space(10)]
    //攻击离终点最近的对象
    public bool targetPriority = false;
    public AIState aiState;
    private GameObject target;
    private List<GameObject> targets = new List<GameObject>();
    private Attack attackMelee;
    private Attack attackRange;
    private Attack lastAttack;
    private NavAgent navAgent;
    private bool targetState;

    public override void Awake()
	{
        base.Awake();
        attackMelee = GetComponentInChildren<AttackMelee>() as Attack;
        attackRange = GetComponentInChildren<AttackRange>() as Attack;
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
        if ((target == null) && (targets.Count > 0))
		{
            target = GetTopTarget();
            if ((target != null) && (navAgent != null))
			{
                navAgent.LookAtTarget(target.transform);
			}
		}
        if (target == null)
		{
            if (targetState == false)
			{
                targetState = true;
			}
            else
			{
                aiBehavior.ChangeState(aiState);
			}
		}
	}

    public override void OnStateExit(AIState previousState, AIState newState)
	{
        LoseTarget();
	}

    public override bool OnTrigger(AIState.Trigger trigger, Collider2D mycol, Collider2D other)
	{
        if (base.OnTrigger(trigger, mycol, other) == false)
        {
            switch (trigger)
			{
                case AIState.Trigger.TriggerStay:
                    TriggerStay(mycol, other);
                    break;
                case AIState.Trigger.TriggerExit:
                    TriggerExit(mycol, other);
                    break;
			}
		}
        return false;
	}

    private void LoseTarget()
	{
        target = null;
        targetState = false;
        lastAttack = null;
	}

    private void TriggerStay(Collider2D mycol, Collider2D other)
	{
        if (target == null)
        {
            targets.Add(other.gameObject);
		}
        else
		{
            if (target == other.gameObject)
			{
                if (mycol.name == "AttackMelee")
				{
                    if (attackMelee != null)
					{
                        lastAttack = attackMelee as Attack;
                        attackMelee.AttackTarget(other.transform);
					}
				}
                else if (mycol.name == "AttackRange")
				{
                    if (attackRange != null)
					{
                        if ((attackMelee == null) || ((attackMelee != null) && (lastAttack != attackMelee)))
						{
                            lastAttack = attackRange as Attack;
                            attackRange.AttackTarget(other.transform);
						}
					}
				}
			}
		}
	}

    private void TriggerExit(Collider2D mycol, Collider2D other)
	{
        if (target == other.gameObject)
		{
            LoseTarget();
		}
	}

    private GameObject GetTopTarget()
	{
        GameObject res = null;
        if (targetPriority == true)
		{
            float minDistance = float.MaxValue;
            foreach (GameObject obj in targets)
			{
                if (obj != null)
				{
                    AIStatePatrol aiStatePatrol = obj.GetComponent<AIStatePatrol>();
                    float distance = aiStatePatrol.GetRemainingPath();
                    if (distance < minDistance)
					{
                        minDistance = distance;
                        res = obj;
					}
				}
			}
		}
        else
		{
            res = targets[0];
		}
        targets.Clear();
        return res;
	}
}
