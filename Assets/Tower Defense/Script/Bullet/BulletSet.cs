using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletSet : MonoBehaviour
{
    public int damage = 1;
    //子弹最长存活时间
    public float liftTime = 5f;
    //子弹速度
    public float speed = 3f;
    //子弹加速度
    public float acceleration = 0.5f;
    //销毁距离
    public float hitDistance = 0.2f;
    //弹道偏移
    public float ballisticOffset = 0.5f;
    //飞行中自动是否旋转
    public bool flyRotation = false;
    //是否只有AOE伤害
    public bool onlyAOEDamage = false;
    //子弹发射器
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
            targetPosition = target.position;
        }
        //记录初始距离
        Vector2 targetDistance = targetPosition - firePosition;
        //计算剩余距离
        Vector2 remainingDistance = targetPosition - (Vector2) currentPosition;
        //向目标移动
        currentPosition = Vector2.Lerp(firePosition, targetPosition, counter * speed / targetDistance.magnitude);
        //添加偏移
        transform.position = AddBallisticOffset(targetDistance.magnitude, remainingDistance.magnitude);
        //子弹旋转
        DirectionRotation((Vector2) transform.position - previousPosition);
        previousPosition = transform.position;
        sprite.enabled = true;
        if (remainingDistance.magnitude <= hitDistance)
        {
            if (target != null)
            {
                if (onlyAOEDamage == false)
                {
                    DamageTaker damageTaker = target.GetComponent<DamageTaker>();
                    if (damageTaker != null)
                    {
                        damageTaker.TakeDamage(damage);
                    }
                }
            }
            Destroy(gameObject);
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
        //不清楚飞行方向，所以在第一帧禁用sprite
        sprite.enabled = false;
        firePosition = currentPosition = previousPosition = transform.position;
        this.target = target;
        targetPosition = target.position;
        Destroy(gameObject, liftTime);
    }
    
    //子弹偏移    
    private Vector2 AddBallisticOffset(float taregtDistance, float remainingDistance)
    {
        if (ballisticOffset > 0f)
        {
            float offset = Mathf.Sin(Mathf.PI * ((taregtDistance - remainingDistance) / taregtDistance));
            offset *= remainingDistance;
            return (Vector2) currentPosition + (ballisticOffset * offset * Vector2.up);
        }
        else
        {
            return currentPosition;
        }
    }

    //方向旋转
    private void DirectionRotation(Vector2 direction)
    {
        if (flyRotation == false)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
