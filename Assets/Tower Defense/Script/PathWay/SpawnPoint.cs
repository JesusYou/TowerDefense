using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [System.Serializable]
    public class Wave
	{
        public float delayBeforeWave;
        public List<GameObject> enemies;
	}

    public float speedRandomizer = 0.2f;
    public float unitSpawnDelay = 0.8f;
    public List<Wave> waves;

    [HideInInspector] public List<GameObject> randomEnemiesList = new List<GameObject>();

    //敌人前进的路线
    private Pathway path;
    private float counter;
    private List<GameObject> activeEnemies = new List<GameObject>();
    //所有敌人是否生成完毕
    private bool finished = false;

    void Awake()
	{
        path = GetComponentInParent<Pathway>();
	}

    void OnEnable()
	{
        EventManager.StartListening("UnitDie", UnitDie);
        EventManager.StartListening("WaveStart", WaveStart);
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((finished == true) && (activeEnemies.Count <= 0))
		{
            EventManager.TriggerEvent("AllEnemiesAreDead", null, null);
            gameObject.SetActive(false);
		}
    }

    void OnDisable()
	{
        EventManager.StopListening("UnitDie", UnitDie);
        EventManager.StopListening("WaveStart", WaveStart);
    }

    void OnDestroy()
	{
        StopAllCoroutines();
	}

    //单位死亡
    private void UnitDie(GameObject obj, string str)
	{
        if (activeEnemies.Contains(obj) == true)
		{
            activeEnemies.Remove(obj);
		}
	}

    private void WaveStart(GameObject obj, string str)
	{
        int waveIdx;
        int.TryParse(str, out waveIdx);
        StartCoroutine("RunWave", waveIdx);
	}

    private IEnumerator RunWave(int waveIdx)
	{
        if (waves.Count > waveIdx)
		{
            yield return new WaitForSeconds(waves[waveIdx].delayBeforeWave);
            foreach (GameObject enemy in waves[waveIdx].enemies)
			{
                GameObject prefab = null;
                //如果未指定，随机生成
                if (prefab == null && randomEnemiesList.Count > 0)
				{
                    prefab = randomEnemiesList[Random.Range(0, randomEnemiesList.Count)];
				}
                if (prefab == null)
				{
                    Debug.LogError("NO ENEMY PREFAB!");
				}
                //创建敌人
                GameObject newEnemy = Instantiate(prefab, transform.position, transform.rotation);
                //设置路径
                newEnemy.GetComponent<AIStatePatrol>().path = path;
                NavAgent agent = newEnemy.GetComponent<NavAgent>();
                agent.speed = Random.Range(agent.speed * (1f - speedRandomizer), agent.speed * (1f + speedRandomizer));
                //把创建的敌人添加的行动列表中
                activeEnemies.Add(newEnemy);
                yield return new WaitForSeconds(unitSpawnDelay);
            }
            if (waveIdx + 1 == waves.Count)
			{
                finished = true;
			}
		}
	}
}
