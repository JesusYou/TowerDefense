using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEHealer : MonoBehaviour
{
    public int healAmount = 1;
    public float healRadius = 1.5f;
    public float coolDown = 3f;
    public GameObject healVisual;
    public float healVisualDuration = 1f;
    public List<string> tags = new List<string>();
    private float coolDownCounter;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
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
            if (AOEHeal() == true)
			{
                if (_animator != null)
				{
                    _animator.SetTrigger("heal");
				}
			}
		}
	}

    //根据Tag确定对象是否可行
    private bool IsTagAllowed(string tag)
	{
        bool res = false;
        if (tags.Count >= 0)
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

    //治愈半径内全部目标
    private bool AOEHeal()
    {
        bool res = false;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, healRadius);
        foreach (Collider2D col in cols)
		{
            if (IsTagAllowed(col.tag) == true)
			{
                //判断对象是否需要治疗
                DamageTaker target = col.gameObject.GetComponent<DamageTaker>();
                if (target.currentHitPoint < target.hitPoint)
				{
                    res = true;
                    //进行治愈
                    target.TakeDamage(-healAmount);
                    ShowHealVisual(target);
				}
			}
		}
        return res;
    }

    //显示治疗效果
    private void ShowHealVisual(DamageTaker target)
	{
        if (healVisual != null)
		{
            GameObject effect = Instantiate(healVisual, target.transform);
            Destroy(effect, healVisualDuration);
		}
	}
}
