using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour, Attack
{
    public int damage = 1;
    public float coolDown = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;
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

    //冷却时间结束攻击目标
    public void AttackTarget(Transform target)
	{
        if (coolDownCounter >= coolDown)
		{
            coolDownCounter = 0f;
            Fire(target);
		}
	}

    //攻击范围内目标
    private void Fire(Transform target)
	{
        if (target != null)
		{
            //创建子弹
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet mybullet = bullet.GetComponent<Bullet>();
            mybullet.SetDamage(damage);
            mybullet.Fire(target);
            if (_animator != null)
			{
                _animator.SetTrigger("attackRange");
			}
		}
	}
}
