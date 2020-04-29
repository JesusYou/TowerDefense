using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public string mapUISceneName;
    public int goldAmount = 25;
    public int defeatCounts = 3;
    //存放当前允许生成的敌人
    public List<GameObject> allowedEnemies = new List<GameObject>();
    private UIManager uiManager;
    private int spawnNumbers;
    private int beforeLooseCounter;

    void Awake()
	{
        SceneManager.LoadScene(mapUISceneName, LoadSceneMode.Additive);
	}

    void OnEnable()
	{
        EventManager.StartListening("Captured", Captured);
        EventManager.StartListening("AllEnemiesAreDead", AllEnemiesAreDead);
	}

    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        spawnNumbers = spawnPoints.Length;
        if (spawnNumbers <= 0)
		{
            Debug.LogError("NO SPAWNERS");
		}
        //为每个出生点设置随机敌人列表
        foreach (SpawnPoint spawnPoint in spawnPoints)
		{
            spawnPoint.randomEnemiesList = allowedEnemies;
		}
        uiManager.SetGold(goldAmount);
        beforeLooseCounter = defeatCounts;
        uiManager.SetDefeatCounts(beforeLooseCounter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
	{
        EventManager.StopListening("Captured", Captured);
        EventManager.StopListening("AllEnemiesAreDead", AllEnemiesAreDead);
	}

    //敌人到达终点（游戏失败）
    private void Captured(GameObject obj, string str)
	{
        if (beforeLooseCounter > 0)
		{
            beforeLooseCounter--;
            uiManager.SetDefeatCounts(beforeLooseCounter);
            if (beforeLooseCounter <= 0)
			{
                uiManager.GoToDefeatMenu();
			}
		}
	}

    //所以敌人死亡（游戏获胜）
    private void AllEnemiesAreDead(GameObject obj, string str)
	{
        spawnNumbers--;
        if (spawnNumbers <= 0)
		{
            uiManager.GoToVictoryMenu();
        }
	}
}
