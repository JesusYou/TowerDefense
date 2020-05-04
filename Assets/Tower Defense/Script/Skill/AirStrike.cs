using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStrike : MonoBehaviour
{
    public float[] delayDamage = { 0.5f };
    public int damage = 10;
    public float radius = 1f;
    public GameObject effectPrefab;
    public float effectDuration = 2f;
    private enum MyState
	{
        WaitForClick,
        WaitForFX
	}
    private MyState myState = MyState.WaitForClick;

    void OnEable()
	{
        EventManager.StartListening("UserClick", UserClick);
        EventManager.StartListening("UserUIClick", UserUIClick);
	}

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(effectPrefab, "Wrong initial settings");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
	{
        EventManager.StopListening("UserClick", UserClick);
        EventManager.StopListening("UserUIClick", UserUIClick);
	}

    void OnDestroy()
	{
        if (myState == MyState.WaitForClick)
		{
            EventManager.TriggerEvent("ActionCancel", gameObject, null);
		}
        StopAllCoroutines();
	}

    private void UserClick(GameObject obj, string str)
	{
        if (myState == MyState.WaitForClick)
		{
            //点击目标不是塔
            if (obj == null || obj.CompareTag("Tower") == false)
			{
                transform.position = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, effectDuration);
                EventManager.TriggerEvent("ActionStart", gameObject, null);
                StartCoroutine(DamageCoroutine());
                myState = MyState.WaitForFX;
			}
			else
			{
                Destroy(gameObject);
			}
		}
	}

    private void UserUIClick(GameObject obj, string str)
	{
        if (myState == MyState.WaitForClick)
		{
            Destroy(gameObject);
		}
	}

    private IEnumerator DamageCoroutine()
	{
        foreach (float delay in delayDamage)
		{
            yield return new WaitForSeconds(delay);
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D hit in hits)
			{
                if (hit.CompareTag("Enemy") == true)
				{
                    DamageTaker damageTaker = hit.GetComponent<DamageTaker>();
                    if (damageTaker != null)
					{
                        damageTaker.TakeDamage(damage);
					}
				}
			}
		}
        Destroy(gameObject);
	}
}
