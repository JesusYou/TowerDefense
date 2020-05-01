using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public string exitSceneName;
	public GameObject startUI;
	public GameObject pauseUI;
	public GameObject defeatUI;
	public GameObject victoryUI;
	public GameObject fightUI;
	public Text goldAmount;
	public Text defeatCounts;
	public float displayDelay = 1f;
	private bool pauseStatus;
	private bool cameraStatus;
	//拖动相机的起始位置
	private Vector3 cameraPosition = Vector3.zero;
	private CameraControl cameraControl;

	void Awake()
	{
		cameraControl = FindObjectOfType<CameraControl>();
	}
	void OnEnable()
	{
		EventManager.StartListening("UnitKilled", UnitKilled);
		EventManager.StartListening("ButtonPressed", ButtonPressed);
	}

	// Start is called before the first frame update
    void Start()
    {
	    PauseGame(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseStatus == false)
		{
            if (Input.GetMouseButtonDown(0) == true)
			{
                //检查是否指在UI上
				GameObject hittedObj = null;
				PointerEventData pointerData = new PointerEventData(EventSystem.current);
				pointerData.position = Input.mousePosition;
				List<RaycastResult> results = new List<RaycastResult>();
				EventSystem.current.RaycastAll(pointerData, results);
                if (results.Count > 0)
				{
                    foreach (RaycastResult res in results)
					{
						if (res.gameObject.CompareTag("SkillIcon"))
						{
							hittedObj = res.gameObject;
							break;
						}
					}
					EventManager.TriggerEvent("UserUIClick", hittedObj, null);
				}
				else
				{
					RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
					foreach (RaycastHit2D hit in hits)
					{
						if (hit.collider.gameObject.CompareTag("Tower") || hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Soldier"))
					    {
							hittedObj = hit.collider.gameObject;
							break;
						}
					}
					EventManager.TriggerEvent("UserClick", hittedObj, null);
				}
                if (hittedObj == null)
				{
					cameraStatus = true;
					cameraPosition = Input.mousePosition;
				}
			}
            if (Input.GetMouseButtonUp(0) == true)
			{
				cameraStatus = false;
			}
            if (cameraStatus == true)
			{
				Vector3 position = Camera.main.ScreenToViewportPoint(Input.mousePosition - cameraPosition);
				cameraControl.MoveX(-position.x);
				cameraControl.MoveY(-position.y);
			}
		}
    }
    
    void OnDisable()
    {
	    EventManager.StopListening("UnitKilled", UnitKilled);
	    EventManager.StopListening("ButtonPressed", ButtonPressed);
    }

    void OnDestroy()
	{
		StopAllCoroutines();
	}

    public void GoToDefeatMenu()
	{
		StartCoroutine("DefeatCoroutine");
	}

    public void GoToVictoryMenu()
	{
		StartCoroutine("VictoryCoroutine");
	}

    //设置允许通关的敌人数量的text
    public void SetDefeatCounts(int count)
	{
        defeatCounts.text = count.ToString();
	}

    //花费金币
	public bool SpendGold(int cost)
	{
		bool res = false;
		int currentGold = GetGold();
		if (currentGold >= cost)
		{
			SetGold(currentGold - cost);
			res = true;
		}
		return res;
	}

    //设置金币的text
	public void SetGold(int gold)
	{
		goldAmount.text = gold.ToString();
	}

    //获取当前金币数量
    private int GetGold()
	{
		int gold;
		int.TryParse(goldAmount.text, out gold);
		return gold;
	}

    //添加金币数量
    private void AddGold(int gold)
	{
		SetGold(GetGold() + gold);
	}

    //重新游戏
    private void RestartMap()
	{
		LoadScene(SceneManager.GetActiveScene().name);
	}

    private void GoToPauseMenu()
	{
		PauseGame(true);
		CloseAllUI();
		pauseUI.SetActive(true);
	}

    private void GoToMap()
	{
		CloseAllUI();
		fightUI.SetActive(true);
		PauseGame(false);
	}

    //结束当前场景并加载新场景
    private void LoadScene(string sceneName)
	{
		EventManager.TriggerEvent("SceneQuit", null, null);
		SceneManager.LoadScene(sceneName);
	}

    //退出地图返回选择界面
    private void ExitFormMap()
	{
		LoadScene(exitSceneName);
	}

    //恢复游戏
    private void ResumeGame()
	{
		GoToMap();
		PauseGame(false);
	}

    //关闭所以UI
    private void CloseAllUI()
	{
		startUI.SetActive(false);
		pauseUI.SetActive(false);
		defeatUI.SetActive(false);
		victoryUI.SetActive(false);
	}

    //暂停游戏
    private void PauseGame(bool paused)
    {
	    pauseStatus = paused;
	    Time.timeScale = paused ? 0f : 1f;
	    EventManager.TriggerEvent("GamePuased", null, paused.ToString());
    }


    private void UnitKilled(GameObject obj, string str)
    {
	    if (obj.CompareTag("Enemy"))
		{
			Price price = obj.GetComponent<Price>();
            if (price != null)
			{
				AddGold(price.price);
			}
		}
    }

    private void ButtonPressed(GameObject obj, string str)
    {
	    switch (str)
		{
			case "Pause":
				GoToPauseMenu();
				break;
			case "Play":
				GoToMap();
				break;
			case "Back":
				ExitFormMap();
				break;
			case "Restart":
				RestartMap();
				break;
		}
    }

    //延迟后显示失败界面
    private IEnumerator DefeatCoroutine()
	{
		yield return new WaitForSeconds(displayDelay);
		PauseGame(true);
		defeatUI.SetActive(true);
	}

	//延迟后显示胜利界面
	private IEnumerator VictoryCoroutine()
	{
		yield return new WaitForSeconds(displayDelay);
		PauseGame(true);
		CloseAllUI();
        //更新已通关的地图
		DataManager.instance.gameProgressData.lastMap = SceneManager.GetActiveScene().name;
		bool hit = false;
		foreach (string map in DataManager.instance.gameProgressData.openedMaps)
		{
			if (map == SceneManager.GetActiveScene().name)
			{
				hit = true;
				break;
			}
		}
		if (hit == false)
		{
			DataManager.instance.gameProgressData.openedMaps.Add(SceneManager.GetActiveScene().name);
		}
        //保存游戏进度
		DataManager.instance.SaveGameProgressData();
		victoryUI.SetActive(true);
	}
}
