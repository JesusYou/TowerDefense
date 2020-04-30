using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMelee : MonoBehaviour, Attack
{
    public int damage = 1;
    public float coolDown = 1f;
    private Animator _animator;
    private float coolDownCounter;

    void Awake()
	{
        _animator = GetComponentInParent<Animator>();
        coolDownCounter = coolDown;
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
        if (coolDownCounter < coolDown)
		{
            coolDownCounter += Time.fixedDeltaTime;
		}
	}

    public void AttackTarget(Transform target)
	{
        if (coolDownCounter >= coolDown)
		{
            coolDownCounter = 0f;
            Hack(target);
		}
	}

    private void Hack(Transform target)
	{
        if (target != null)
		{
            DamageTaker damageTaker = target.GetComponent<DamageTaker>();
            if (damageTaker != null)
			{
                damageTaker.TakeDamage(damage);
			}
            if (_animator != null)
			{
                _animator.SetTrigger("attackMelee");
			}
		}
	}
}
