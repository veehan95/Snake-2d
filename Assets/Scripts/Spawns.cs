using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawns : MonoBehaviour {
    // Prefab
    public GameObject foodPrefab;
    public GameObject meatPrefab;
    public GameObject medPrefab;
    public GameObject virusPrefab;
    public GameObject runePrefab;

    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    // Reset Time
    public float timeLeft_food = 30f;
    public float timeLeft_meat = 70f;
    public float timeLeft_med = 40f;
    public float timeLeft_wall = 10f;
    public float timeLeft_rune = 30f;

    //  Expire Time
    public float timeKill_meat = 10f;
    public float timeKill_med = 10f;
    public float timeKill_rune = 10f;

    //  Expire Time Temp
    public float timeTemp_meat = 0;
    public float timeTemp_med = 0;
    public float timeTemp_rune = 0;

    // Max number of wall
    public int max_wall_num = 20;

    // Percentage of wall to trigger medicine spawn
    public double med_trigger = 0.8;

    // Use this for initialization
    void Start()
    {
        SpawnFood();
        InvokeRepeating("SpawnFood", (float)(timeLeft_food * 0.2), timeLeft_food);
        InvokeRepeating("SpawnMeat", (float)(timeLeft_meat * 0.2), timeLeft_meat);
        InvokeRepeating("SpawnMed", (float)(timeLeft_med * 0.2), timeLeft_med);
        InvokeRepeating("SpawnWall", (float)(timeLeft_wall * 0.2), timeLeft_wall);
        InvokeRepeating("SpawnRune", (float)(timeLeft_rune * 0.2), timeLeft_rune);
    }

    // Update is called once per Frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Medicine"))
            if (timeTemp_med > 0)
                timeTemp_med -= Time.deltaTime;
            else if (GameObject.FindGameObjectWithTag("Medicine"))
                Destroy(GameObject.FindGameObjectWithTag("Medicine"));

        if (GameObject.FindGameObjectWithTag("Meat"))
            if (timeTemp_meat > 0)
                timeTemp_meat -= Time.deltaTime;
            else if (GameObject.FindGameObjectWithTag("Meat"))
                Destroy(GameObject.FindGameObjectWithTag("Meat"));

        if (GameObject.FindGameObjectWithTag("Rune"))
            if (timeTemp_rune > 0)
                timeTemp_rune -= Time.deltaTime;
            else if (GameObject.FindGameObjectWithTag("Rune"))
                Destroy(GameObject.FindGameObjectWithTag("Rune"));
}

    // Spawn one piece of food
    void SpawnFood()
    {
        int[] loc = SpawnLoc();

        if (GameObject.FindGameObjectWithTag("Apple"))
            Destroy(GameObject.FindGameObjectWithTag("Apple"));

        // Instantiate the wall at (loc[0], loc[1])
        Instantiate(foodPrefab,
                    new Vector2(loc[0], loc[1]),
                    Quaternion.identity); // default rotation
    }

    // Spawn one piece of meat
    void SpawnMeat()
    {
        int[] loc = SpawnLoc();

        if (GameObject.FindGameObjectWithTag("Meat"))
            Destroy(GameObject.FindGameObjectWithTag("Meat"));
        // Instantiate the wall at (loc[0], loc[1])
        Instantiate(meatPrefab,
                    new Vector2(loc[0], loc[1]),
                    Quaternion.identity); // default rotation

        timeTemp_meat = timeKill_meat;
    }

    // Spawn one piece of medicine
    void SpawnMed()
    {
        if (GameObject.FindGameObjectsWithTag("Virus").Length > max_wall_num * med_trigger)
            if (!GameObject.FindGameObjectWithTag("Medicine"))
            {
                int[] loc = SpawnLoc();
                // Instantiate the wall at (loc[0], loc[1])
                Instantiate(medPrefab,
                            new Vector2(loc[0], loc[1]),
                            Quaternion.identity); // default rotation
            }
        
        timeTemp_med = timeKill_med;
    }

    // Spawn one piece of wall
    void SpawnWall()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Virus");
        if (walls.Length >= max_wall_num)
            Destroy(walls[Random.Range(0, walls.Length - 1)].gameObject);

        int[] loc = SpawnLoc();

        // Instantiate the wall at (loc[0], loc[1])
        Instantiate(virusPrefab,
                    new Vector2(loc[0], loc[1]),
                    Quaternion.identity); // default rotation
    }

    // Spawn one piece of wall
    void SpawnRune()
    {
        int[] loc = SpawnLoc();

        if (GameObject.FindGameObjectWithTag("Rune"))
            Destroy(GameObject.FindGameObjectWithTag("Rune"));
        // Instantiate the wall at (loc[0], loc[1])
        Instantiate(runePrefab,
                    new Vector2(loc[0], loc[1]),
                    Quaternion.identity); // default rotation

        timeTemp_rune = timeKill_rune;
    }

    int[] SpawnLoc()
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

        int[] loc = { x, y };

        return loc;
    }
}
