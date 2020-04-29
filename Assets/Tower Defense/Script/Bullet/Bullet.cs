﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Bullet
{
    void SetDamage(int damage);
    int GetDamage();
    void Fire(Transform target);
}

/*
public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/