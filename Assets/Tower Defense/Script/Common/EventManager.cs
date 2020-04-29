using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyEvent : UnityEvent<GameObject, string>
{
    
}

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    //监听事件列表
    private Dictionary<string, MyEvent> eventDictionary = new Dictionary<string,MyEvent>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        instance = null;
    }

    //开始监听指定事件
    public static void StartListening(string eventName, UnityAction<GameObject, string> listener)
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(EventManager)) as EventManager;
            if (instance == null)
            {
                Debug.Log("NO EVENTMANAGER!");
                return;
            }
        }

        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new MyEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }
    
    //停止监听指定事件
    public static void StopListening(string eventName, UnityAction<GameObject, string> listener)
    {
        if (instance == null)
        {
            return;
        }

        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    
    //触发指定事件
    public static void TriggerEvent(string eventName, GameObject obj, string str)
    {
        if (instance == null)
        {
            return;
        }

        MyEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(obj, str);
        }
    }
}
