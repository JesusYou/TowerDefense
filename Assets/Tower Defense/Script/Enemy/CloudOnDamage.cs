using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudOnDamage : MonoBehaviour
{
    public float duration = 3f;
    public float coolDown = 3f;
    public float radius = 1f;
    public Cloud cloudPrefab;
    public GameObject exhaustFX;
    public List<string> tags = new List<string>();
    private enum MyState
	{
        WaitForDamage,
        Cloud,
        CoolDown
	}
    private MyState myState = MyState.WaitForDamage;
    private float counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (myState)
		{
            case MyState.CoolDown:
                //等待冷却结束
                if (counter > 0f)
				{
                    counter -= Time.deltaTime;
				}
                else
				{
                    counter = 0f;
                    myState = MyState.WaitForDamage;
				}
                break;
            case MyState.Cloud:
                if (counter > 0f)
				{
                    counter -= Time.deltaTime;
				}
                else
				{
                    counter = coolDown;
                    myState = MyState.CoolDown;
				}
                break;
		}
    }

    //触发伤害事件
    public void OnDamage()
	{
        if (myState == MyState.WaitForDamage)
		{
            myState = MyState.Cloud;
            counter = duration;
            CloudNow();
            //效果可视化
            GameObject obj = Instantiate(exhaustFX, transform);
            Destroy(obj, duration);
		}
	}

    private void CloudNow()
	{
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D col in cols)
		{
            if (IsTagAllowed(col.tag) == true)
			{
                //添加云雾到单位
                Cloud cloud = Instantiate(cloudPrefab, col.gameObject.transform);
                cloud.duration = duration;
			}
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
