﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour {
    // Food Prefab
    public GameObject foodPrefab;

    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    // Food Expire Time
    public float timeLeft = 30f;
    private float time;

    // Use this for initialization
    void Start()
    {
        time = timeLeft;
    }

    // Update is called once per Frame
    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("Apple"))
            Spawn();
        else
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                Destroy(GameObject.FindGameObjectWithTag("Apple"));
                Spawn();
                time = timeLeft;
            }

        }
    }

    // Spawn one piece of food
    void Spawn()
    {
        int x, y;
        do
        {
            // x position between left & right border
            x = (int)Random.Range(borderLeft.position.x,
                                      borderRight.position.x);

            // y position between top & bottom border
            y = (int)Random.Range(borderBottom.position.y,
                                      borderTop.position.y);
        } while (Physics.CheckSphere(new Vector2(x, y), 0.2f));

        // Instantiate the food at (x, y)
        Instantiate(foodPrefab,
                    new Vector2(x, y),
                    Quaternion.identity); // default rotation
    }
}
