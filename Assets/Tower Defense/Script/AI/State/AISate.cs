using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AISate : MonoBehaviour
{
    //允许触发的类型
    public enum Trigger
	{
        TriggerEnter,
        TriggerStay,
        TriggerExit,
        Damage,
        CoolDown
	}

    [System.Serializable]
    public class AITransaction
    {
        public Trigger trigger;
        public AISate newState;
    }

    public AITransaction[] specificTransaction;
    protected Animator anim;
    protected AIBehavior aiBehavior;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
