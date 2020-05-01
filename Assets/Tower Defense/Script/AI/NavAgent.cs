using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour
{
    public float speed = 1f;
    [HideInInspector] public bool move = true;
    [HideInInspector] public bool turn = true;
    //目的地
    [HideInInspector] public Vector2 destination;
    //速度矢量
    [HideInInspector] public Vector2 velocity;
    private Vector2 previousPosition;

    void OnEnable()
	{
        previousPosition = transform.position;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

        if (move == true)
        {
            //向目标移动
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);
        }
        //计算速度
        Vector2 velocity = (Vector2)transform.position - previousPosition;
        velocity /= Time.fixedDeltaTime;
        if (turn == true)
		{
            SetSpriteDirection(destination - (Vector2)transform.position);
		}
        previousPosition = transform.position;
	}

    public void LookAtTarget(Transform target)
	{
        SetSpriteDirection(target.position - transform.position);
	}

    public void LookAtTarget(Vector2 direction)
	{
        SetSpriteDirection(direction);
	}

    //设置x轴上sprite的方向
    private void SetSpriteDirection(Vector2 direction)
	{
        //向右
        if (direction.x > 0f && transform.localScale.x < 0f)
		{
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
        //向左
        else if (direction.x < 0f && transform.localScale.x > 0f)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
