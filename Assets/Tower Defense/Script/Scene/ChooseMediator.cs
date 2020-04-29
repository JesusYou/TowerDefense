using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChooseMediator : MonoBehaviour
{
    public string exitSceneName;
    public Toggle activeTogglePre;
    public Toggle inactiveTogglePre;
    public Button rightButton;
    public Button leftButton;
    public GameObject currentMap;
    public List<GameObject> allMaps = new List<GameObject>();
    public Transform toggleView;
    private int maxMapIdx;
    private int currentMapIdx;
    private List<Toggle> activeToggles = new List<Toggle>();

    // Start is called before the first frame update
    void Start()
    {
        int hitIdx = -1;
        //获取已解锁地图的数量
        int mapsCount = DataManager.instance.gameProgressData.openedMaps.Count;
        if (mapsCount > 0)
        {
            //获取最后一个解锁的地图
            string openedMapsName = DataManager.instance.gameProgressData.openedMaps[mapsCount - 1];
            for (int count = 0; count < allMaps.Count; ++count)
            {
                //找到最后解锁的地图
                if (allMaps[count].name == openedMapsName)
                {
                    hitIdx = count;
                    break;
                }
            }
        }

        //找到地图
        if (hitIdx >= 0)
        {
            if (allMaps.Count > hitIdx + 1)
            {
                maxMapIdx = hitIdx + 1;
            }
            else
            {
                maxMapIdx = hitIdx;
            }
        }
        //没找到地图
        else
        {
            if (allMaps.Count > 0)
            {
                maxMapIdx = 0;
            }
            else
            {
                Debug.LogError("NO FOUND!");
            }
        }

        if (maxMapIdx >= 0)
        {
            DisplayToggles();
            DisplayMaps(maxMapIdx);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //显示关卡数量
    private void DisplayToggles()
    {
        foreach (Toggle toggle in toggleView.GetComponentsInChildren<Toggle>())
        {
            Destroy(toggle.gameObject);
        }
        
        for (int count = 0; count < maxMapIdx + 1; ++count)
        {
            GameObject toggle = Instantiate(activeTogglePre.gameObject, toggleView);
            activeToggles.Add(toggle.GetComponent<Toggle>());
        }

        if (maxMapIdx < allMaps.Count - 1)
        {
            Instantiate(inactiveTogglePre.gameObject, toggleView);
        }
    }

    //显示选择的地图
    private void DisplayMaps(int mapIdx)
    {
        Transform parentOfMap = currentMap.transform.parent;
        Vector3 mapPosition = currentMap.transform.position;
        Quaternion mapRotation = currentMap.transform.rotation;
        Destroy(currentMap);
        currentMap = Instantiate(allMaps[mapIdx], parentOfMap);
        currentMap.name = allMaps[mapIdx].name;
        currentMap.transform.position = mapPosition;
        currentMap.transform.rotation = mapRotation;
        currentMapIdx = mapIdx;
        foreach (Toggle toggle in activeToggles)
        {
            toggle.isOn = false;
        }
        activeToggles[mapIdx].isOn = true;
        UpdateButtonsStatus(mapIdx);
    }

    //更新按钮状态
    private void UpdateButtonsStatus(int mapIdx)
    {
        leftButton.interactable = mapIdx > 0 ? true : false;
        rightButton.interactable = mapIdx < maxMapIdx ? true : false;
    }

    public void LeftButton()
    {
        if (currentMapIdx > 0)
        {
            DisplayMaps(currentMapIdx - 1);
        }
    }

    public void RightButton()
    {
        if (currentMapIdx < maxMapIdx)
        {
            DisplayMaps(currentMapIdx + 1);
        }
    }
    
    public void PlayButton()
    {
        SceneManager.LoadScene(currentMap.name);
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(exitSceneName);
    }
}
