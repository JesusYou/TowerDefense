using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour
{
    public int hitPoint = 1;

    [HideInInspector] public int currentHitPoint;
    //伤害视觉效果持续时间
    public float damageDisplayTime = 0.2f;
    public Transform health;
    public bool isTrigger;
    private SpriteRenderer sprite;
    private bool coroutineInProgress;
    //初始血量值
    private float originHealth;

    void Awake()
    {
        currentHitPoint = hitPoint;
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        originHealth = health.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        EventManager.TriggerEvent("UnitDie", gameObject, null);
        StopAllCoroutines();
    }

    //受伤or治愈
    public void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            if (this.enabled == true)
            {
                if (currentHitPoint > damage)
                {
                    currentHitPoint -= damage;
                    UpdateHealth();
                    if (coroutineInProgress == false)
                    {
                        StartCoroutine(DisplayDamage());
                    }

                    if (isTrigger == true)
                    {
                        SendMessage("OnDamage");
                    }
                }
                else
                {
                    currentHitPoint = 0;
                    UpdateHealth();
                    Die();
                }
            }
        }
        else
        {
            //治愈
            currentHitPoint = Mathf.Min(currentHitPoint - damage, hitPoint);
            UpdateHealth();
        }
    }

    //伤害可视化
    public IEnumerator DisplayDamage()
    {
        coroutineInProgress = true;
        Color originColor = sprite.color;
        for (float counter = 0; counter < damageDisplayTime; ++counter)
        {
            sprite.color = Color.Lerp(originColor, Color.black, Mathf.PingPong(counter, damageDisplayTime));
            yield return new WaitForFixedUpdate();
        }

        sprite.color = originColor;
        coroutineInProgress = false;
    }

    //更新血量
    private void UpdateHealth()
    {
        float _health = originHealth * currentHitPoint / hitPoint;
        health.localScale = new Vector2(_health, health.localScale.y);
    }

    private void Die()
    {
        EventManager.TriggerEvent("UnitKilled", gameObject, null);
        Destroy(gameObject);
    }
}
