using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendPoint : MonoBehaviour
{
    public GameObject defendPointPrefab;
    private List<Transform> defendPlaces = new List<Transform>();

    void Awake()
	{
        foreach (Transform defendPlace in defendPointPrefab.transform)
		{
            Instantiate(defendPlace.gameObject, transform);
		}
        foreach (Transform child in transform)
		{
            defendPlaces.Add(child);
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Transform> GetDefendPoint()
	{
        return defendPlaces;
	}
}
