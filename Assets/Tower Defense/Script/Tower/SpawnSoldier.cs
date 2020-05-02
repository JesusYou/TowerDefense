using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSoldier : MonoBehaviour
{
    public float coolDown = 10f;
    public int maxNum = 3;
    public GameObject soldierPrefab;
    public Transform spwanPoint;
    private DefendPoint defendPoint;
    private float coolDownCounter;
    private Dictionary<GameObject, Transform> soldiersList = new Dictionary<GameObject, Transform>();

    void OnEnable()
	{
        EventManager.StartListening("UnitDie", UnitDie);
	}

    // Start is called before the first frame update
    void Start()
    {
        BuildingPlace buildingPlace = GetComponentInParent<BuildingPlace>();
        defendPoint = buildingPlace.GetComponentInChildren<DefendPoint>();
        coolDownCounter = coolDown;
        //升级当前已生成的士兵
        foreach (Transform point in defendPoint.GetDefendPoint())
        {
            //如果防守点已经有士兵
            AIBehavior soldier = point.GetComponentInChildren<AIBehavior>();
            if (soldier != null)
            {
                //在相同的坐标上生成新的
                Spawn(soldier.transform, point);
                //摧毁之前的士兵
                Destroy(soldier.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
	{
        coolDownCounter += Time.fixedDeltaTime;
        if (TryToSpawn() == true)
		{
            coolDownCounter = 0f;
		}
        else
		{
            coolDownCounter = coolDown;
		}
	}

    void OnDisable()
	{
        EventManager.StopListening("UnitDie", UnitDie);
	}

    //获取空闲的防守点
    private Transform GetFreeDefendPosition()
	{
        Transform res = null;
        List<Transform> points = defendPoint.GetDefendPoint();
        foreach (Transform point in points)
		{
            if (soldiersList.ContainsValue(point) == false)
			{
                res = point;
                break;
			}
		}
        return res;
	}

    //尝试生成
    private bool TryToSpawn()
	{
        bool res = false;
        if ((soldierPrefab != null) && (soldiersList.Count < maxNum))
		{
            Transform destination = GetFreeDefendPosition();
            if (destination != null)
			{
                Spawn(spwanPoint, destination);
                res = true;
			}
		}
        return res;
	}

    //在指定的地点生成
    private void Spawn(Transform position, Transform destination)
	{
        GameObject obj = Instantiate<GameObject>(soldierPrefab, position.position, position.rotation);
        obj.transform.SetParent(destination);
        obj.GetComponent<AIStateMove>().destination = destination;
        soldiersList.Add(obj, destination);
	}


    private void UnitDie(GameObject obj, string str)
	{
        if (soldiersList.ContainsKey(obj) == true)
		{
            soldiersList.Remove(obj);
		}
	}
}
