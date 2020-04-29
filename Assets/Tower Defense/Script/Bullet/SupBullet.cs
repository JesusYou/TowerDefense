using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupBullet : MonoBehaviour
{
    public int damage = 2;
    //子弹最大存活时间
    public float lifeTime = 5f;
    public float speed = 6f;
    public float acceleration = 0.5f;
    //销毁距离
    public float hitDistance = 0.2f;
    //子弹偏移
    public float ballisticOffset = 0.1f;
    public float penetrationRatio = 0.3f;
    private Vector2 firePosition;
    private Transform target;
    //目标位置
    private Vector2 targetPosition;
    //当前位置
    private Vector2 currentPosition;
    //上一帧位置
    private Vector2 previousPosition;
    //加速计算器
    private float counter;
    private SpriteRenderer sprite;

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
        counter += Time.fixedDeltaTime;
        speed += Time.fixedDeltaTime * acceleration;
        if (target != null)
		{
            targetPosition = GetPenetrationPoint(target.position);
		}
        //记录初始距离
        Vector2 targetDistance = targetPosition - firePosition;
        //计算剩余距离
        Vector2 remainingDistance = targetPosition - (Vector2) currentPosition;
        //向目标移动
        currentPosition = Vector2.Lerp(firePosition, targetPosition, counter * speed / targetDistance.magnitude);
        //添加偏移
        transform.position = AddBallisticOffset(targetDistance.magnitude, remainingDistance.magnitude);
        DirectionRotation((Vector2) transform.position - previousPosition);
        previousPosition = transform.position;
        sprite.enabled = true;
        if (remainingDistance.magnitude <= hitDistance)
		{
            Destroy(gameObject);
		}
	}

    //穿过物体造成伤害
    void OnTriggerEnter2D(Collider2D other)
	{
        DamageTaker damageTaker = other.GetComponent<DamageTaker>();
        if (damageTaker != null)
		{
            damageTaker.TakeDamage(damage);
		}
	}

    public void SetDamage(int damage)
	{
        this.damage = damage;
	}

    public int GetDamage()
	{
        return damage;
	}

    //发射子弹
    public void Fire(Transform target)
	{
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        firePosition = currentPosition = previousPosition = transform.position;
        this.target = target;
        targetPosition = GetPenetrationPoint(target.position);
        Destroy(gameObject, lifeTime);
	}

    //穿透目标
    private Vector2 GetPenetrationPoint(Vector2 _targetPosition)
	{
        Vector2 penetrationVector = _targetPosition - targetPosition;
        return targetPosition + penetrationVector * (1f + penetrationRatio);
	}

    //子弹偏移
    private Vector2 AddBallisticOffset(float targetDistance, float remainingDistance)
	{
        if (ballisticOffset > 0f)
		{
            float offset = Mathf.Sin(Mathf.PI * ((targetDistance - remainingDistance) / targetDistance));
            offset *= targetDistance;
            return (Vector2)currentPosition + (ballisticOffset * offset * Vector2.up);
		}
        else
		{
            return currentPosition;
		}
	}

    //方向旋转
    private void DirectionRotation(Vector2 direction)
	{
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
