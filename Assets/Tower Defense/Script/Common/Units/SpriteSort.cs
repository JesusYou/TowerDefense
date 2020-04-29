using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSort : MonoBehaviour
{ 
    public bool isStatic;
    public float rangeFactor = 100f;
    private Dictionary<SpriteRenderer, int> sprites = new Dictionary<SpriteRenderer, int>();

    void Awake()
	{
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
		{
            sprites.Add(sprite, sprite.sortingOrder);
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        UpdateSortingOrder();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStatic == false)
		{
            UpdateSortingOrder();
		}
    }

    //更新sprite分类表
    private void UpdateSortingOrder()
	{
        foreach (KeyValuePair<SpriteRenderer, int> sprite in sprites)
		{
            sprite.Key.sortingOrder = sprite.Value - (int)(transform.position.y * rangeFactor);
		}
	}
}

