using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    public int healAmount = 1;
    public float coolDown = 3f;
    public GameObject healVisual;
    public float healVisualDuration = 1f;
    public List<string> tags = new List<string>();
    private float coolDownCounter;

    // Start is called before the first frame update
    void Start()
    {
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
	}

    //获取治疗对象
    void OnTriggerStay2D(Collider2D other)
    {
        if (IsTagAllowed(other.tag) == true)
        {
            DamageTaker target = other.gameObject.GetComponent<DamageTaker>();
            if (target != null)
            {
                //判断对象是否需要治疗
                if (target.currentHitPoint < target.hitPoint)
                {
                    TryToHeal(target);
                }
            }
        }
    }

    //根据Tag确定对象是否可行
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

    //尝试治愈
    private void TryToHeal(DamageTaker target)
	{
        if (coolDownCounter >= coolDown)
		{
            coolDownCounter = 0f;
            target.TakeDamage(-healAmount);
            if (healVisual != null)
			{
                GameObject effect = Instantiate(healVisual, target.transform);
                Destroy(effect, healVisualDuration);
			}
		}
	}
}
