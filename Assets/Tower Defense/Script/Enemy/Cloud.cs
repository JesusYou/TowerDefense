using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [HideInInspector] public float duration;
    [HideInInspector] private Collider2D col;
    private float counter;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponentInParent<Collider2D>();
        counter = duration;
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
	{
        if (counter > 0f)
		{
            counter -= Time.fixedDeltaTime;
		}
        else
		{
            col.enabled = true;
            Destroy(gameObject);
		}
	}
}
