using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamage : MonoBehaviour
{
    //AOE伤害比
    public float aoeDamageRate = 1f;
    //AOE半径
    public float radius = 0.5f;
    public GameObject explosion;
    //爆炸效果持续时间
    public float explosionDuration = 1f;
    private Bullet bullet;
    //当场景已关闭，禁止在销毁时创建新对象
    private bool isQuit;

    void Awake()
    {
        bullet = GetComponent<Bullet>();
    }

    void OnEnable()
    {
        EventManager.StartListening("SceneQuit", SceneQuit);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnDisable()
    {
        EventManager.StopListening("SceneQuit", SceneQuit);
    }

    void OnApplicaitonQuit()
    {
        isQuit = true;
    }

    void OnDestroy()
    {
        if (isQuit == false)
        {
            //查找半径内全部的碰撞检测器
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D col in cols)
            {
                //当前目标可以受到伤害
                DamageTaker damageTaker = col.gameObject.GetComponent<DamageTaker>();
                if (damageTaker != null)
                {
                    //造成伤害
                    damageTaker.TakeDamage((int)(Mathf.Ceil(aoeDamageRate * (float)bullet.GetDamage())));
                }
            }

            //创造爆炸效果
            if (explosion != null)
            {
                Destroy(Instantiate<GameObject>(explosion, transform.position, transform.rotation), explosionDuration);
            }
        }
    }

    private void SceneQuit(GameObject obj, string str)
    {
        isQuit = true;
    }
}
