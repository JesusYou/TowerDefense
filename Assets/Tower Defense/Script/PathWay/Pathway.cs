using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Pathway : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //获取到指定节点的最短路径
    public WayPoint GetNearestWayPoint(Vector3 position)
	{
        float minDistance = float.MaxValue;
        WayPoint nearestWayPoint = null;
        foreach (WayPoint wayPoint in GetComponentsInChildren<WayPoint>())
		{
            if (wayPoint.GetHashCode() != GetHashCode())
			{
                Vector3 vec = position - wayPoint.transform.position;
                float distance = vec.magnitude;
                if (distance < minDistance)
				{
                    minDistance = distance;
                    nearestWayPoint = wayPoint;
				}
			}
		}
        return nearestWayPoint;
	}

    //获取下一个节点
    public WayPoint GetNextWayPoint(WayPoint currentWayPoint, bool loop)
	{
        WayPoint res = null;
        int idx = currentWayPoint.transform.GetSiblingIndex();
        if (idx < (transform.childCount - 1))
		{
            idx += 1;
		}
		else
		{
            idx = 0;
		}
        if (!(loop == false && idx == 0))
		{
            res = transform.GetChild(idx).GetComponent<WayPoint>();
		}
        return res;
	}

    //获取到指定节点的剩余距离
    public float GetPathDistance(WayPoint wayPoint)
	{
        WayPoint[] wayPoints = GetComponentsInChildren<WayPoint>();
        bool hitted = false;
        float pathDistance = 0f;
        //计算剩余路径
        for (int idx = 0; idx < wayPoints.Length; ++idx)
		{
            if (hitted == true)
			{
                Vector2 distance = wayPoints[idx].transform.position - wayPoints[idx - 1].transform.position;
                pathDistance += distance.magnitude;
			}
            if (wayPoints[idx] == wayPoint)
			{
                hitted = true;
			}
		}
        return pathDistance;
	}
}
