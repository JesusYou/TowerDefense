using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIColliderTrigger : MonoBehaviour
{
    public List<string> tags = new List<string>();
    private Collider2D col;
    private AIBehavior aiBehavior;

    void Awake()
	{
        col = GetComponent<Collider2D>();
        aiBehavior = GetComponent<AIBehavior>();
        col.enabled = false;
	}

    // Start is called before the first frame update
    void Start()
    {
        col.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
	{
        if (IsTagAllowed(other.tag) == true)
		{
            aiBehavior.OnTrigger(AIState.Trigger.TriggerEnter, col, other);
		}
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
            aiBehavior.OnTrigger(AIState.Trigger.TriggerStay, col, other);
        }
    }

    void OnTriggerExitr2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
            aiBehavior.OnTrigger(AIState.Trigger.TriggerExit, col, other);
        }
    }

    private bool IsTagAllowed(string tag)
	{
        bool res = false;
        if (tags.Count > 0)
		{
            foreach (string str in tags)
			{
                if (str == tag)
				{
                    res = true;
                    break;
				}
			}
		}
        else
		{
            res = true;
		}
        return res;
	}
}
