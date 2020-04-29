using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTree : MonoBehaviour
{
    [HideInInspector] public Tower myTower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Build(GameObject prefab)
	{
        myTower.BuildTower(prefab);
	}
}
