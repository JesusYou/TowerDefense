using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingIcon : MonoBehaviour
{
    public GameObject towerPrefab;
    private Text price;
    private BuildingTree myTree;

    void Awake()
	{
        myTree = transform.GetComponentInParent<BuildingTree>();
        price = GetComponentInChildren<Text>();
        if (towerPrefab == null)
		{
            gameObject.SetActive(false);
		}
		else
		{
            price.text = towerPrefab.GetComponent<Price>().price.ToString();
		}
	}

    void OnEnable()
	{
        EventManager.StartListening("UserUIClick", UserUIClick);
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
        EventManager.StopListening("UserUIClick", UserUIClick);
	}

    private void UserUIClick(GameObject obj, string str)
	{
        if (obj == gameObject)
		{
            myTree.Build(towerPrefab);
		}
	}
}
