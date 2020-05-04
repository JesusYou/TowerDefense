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

    #if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        WayPoint[] WayPoints = GetComponentsInChildren<WayPoint>();
        if (WayPoints.Length > 1) {
            int idx;
            for (idx = 1; idx < WayPoints.Length; idx++)
            {
                Debug.DrawLine(WayPoints[idx - 1].transform.position, WayPoints[idx].transform.position, new Color(1f, 0f, 0f));
            }
        }
    }
    #endif

    //获取到指定节点的最短路径
    public WayPoint GetNearestWayPoint(Vector3 point)
	{
        float minDistance = float.MaxValue;
        WayPoint nearestWayPoint = null;
        foreach (WayPoint WayPoint in GetComponentsInChildren<WayPoint>())
		{
            if (WayPoint.GetHashCode() != GetHashCode())
			{
                Vector3 vec = point - WayPoint.transform.position;
                float distance = vec.magnitude;
                if (distance < minDistance)
				{
                    minDistance = distance;
                    nearestWayPoint = WayPoint;
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
    public float GetPathDistance(WayPoint WayPoint)
	{
        WayPoint[] WayPoints = GetComponentsInChildren<WayPoint>();
        bool hitted = false;
        float pathDistance = 0f;
        int idx;
        //计算剩余路径
        for (idx = 0; idx < WayPoints.Length; ++idx)
		{
            if (hitted == true)
			{
                Vector2 distance = WayPoints[idx].transform.position - WayPoints[idx - 1].transform.position;
                pathDistance += distance.magnitude;
			}
            if (WayPoints[idx] == WayPoint)
			{
                hitted = true;
			}
		}
        return pathDistance;
	}
}
