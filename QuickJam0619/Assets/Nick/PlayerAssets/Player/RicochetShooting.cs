﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetShooting : MonoBehaviour
{

    public GameObject shot;

    public Vector3 offset;

    private Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(shot, playerPos.position, Quaternion.identity);
        }
    }
}