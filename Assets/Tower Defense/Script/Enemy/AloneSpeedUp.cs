using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AloneSpeedUp : MonoBehaviour
{
    public float radius = 1f;
    public float speedUpAmount = 1f;
    public float coolDown = 1f;
    public List<string> tags = new List<string>();
    private float coolDownCounter;
    private NavAgent navAgent;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavAgent>();
        coolDownCounter = coolDown;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
	{
        if (coolDownCounter < coolDown)
		{
            coolDownCounter += Time.fixedDeltaTime;
		}
        else
		{
            coolDownCounter = 0f;
            if (IsAlone() == true)
			{
                StartCoroutine(SpeedUpCoroutine());
			}
		}
	}

    void OnDestroy()
	{
        StopAllCoroutines();
	}

    private bool IsAlone()
	{
        bool alone = true;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D col in cols)
		{
            if (IsTagAllowed(col.tag) == true && col.gameObject != gameObject)
			{
                alone = false;
                break;
			}
		}
        return alone;
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

    private IEnumerator SpeedUpCoroutine()
	{
        navAgent.speed += speedUpAmount;
        yield return new WaitForSeconds(coolDown);
        navAgent.speed -= speedUpAmount;
	}
}
