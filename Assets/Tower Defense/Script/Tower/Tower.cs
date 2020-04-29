using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject buildingTreePrefab;
    public GameObject attackRange;
    private UIManager uiManager;
    private Canvas canvas;
    private Collider2D bodyCollider;
    private BuildingTree activeBuildingTree;

    void OnEnable()
	{
        EventManager.StartListening("GamePaused", GamePaused);
        EventManager.StartListening("UserClick", UserClick);
        EventManager.StartListening("UserUIClick", UserClick);
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDertroy()
	{
        EventManager.StopListening("GamePaused", GamePaused);
        EventManager.StopListening("UserClick", UserClick);
        EventManager.StopListening("UserUIClick", UserClick);
    }

    public void BuildTower(GameObject towerPrefab)
	{
        CloseBuildingTree();
        Price price = towerPrefab.GetComponent<Price>();
        //如果金钱足够
        if (uiManager.SpendGold(price.price) == true)
		{
            //在此坐标创建一个新塔
            GameObject newTower = Instantiate<GameObject>(towerPrefab, transform.parent);
            newTower.transform.position = transform.position;
            newTower.transform.rotation = transform.rotation;
            Destroy(gameObject);
		}
	}

    //打开建造升级的模块
    private void OpenBuildingTree()
	{
        if (buildingTreePrefab != null)
		{
            activeBuildingTree = Instantiate<GameObject>(buildingTreePrefab, canvas.transform).GetComponent<BuildingTree>();
            activeBuildingTree.transform.position = Camera.main.WorldToScreenPoint(transform.position);
            activeBuildingTree.myTower = this;
            bodyCollider.enabled = false;
        }
	}

    //关闭建造升级的模块
    private void CloseBuildingTree()
	{
        if (activeBuildingTree != null)
		{
            Destroy(activeBuildingTree.gameObject);
            bodyCollider.enabled = true;
		}
	}

    private void ShowRange(bool condition)
	{
        if (attackRange != null)
		{
            attackRange.SetActive(condition);
		}
	}

    private void GamePaused(GameObject obj, string str)
	{
        if (str == bool.TrueString)
		{
            CloseBuildingTree();
            bodyCollider.enabled = false;
		}
        else
		{
            bodyCollider.enabled = true;
		}
	}

    private void UserClick(GameObject obj, string str)
	{
        //点击当前塔
        if (obj == gameObject)
		{
            ShowRange(true);
            if (activeBuildingTree == null)
			{
                OpenBuildingTree();
			}
		}
        else
		{
            ShowRange(false);
            CloseBuildingTree();
		}
	}
}
