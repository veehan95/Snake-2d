using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWall : MonoBehaviour {
    // Food Prefab
    public GameObject wallPrefab;

    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    // Max Number of Wall
    public int max_wall_num  = 20;

    // Wall Spawn Speed
    public float wall_speed = 10;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Spawn", (float)(wall_speed * 0.2), wall_speed);
    }

    // Spawn one piece of wall
    void Spawn()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        if (walls.Length >= max_wall_num)
            Destroy(walls[0].gameObject);

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

        // Instantiate the wall at (x, y)
        Instantiate(wallPrefab,
                    new Vector2(x, y),
                    Quaternion.identity); // default rotation
    }
}
