using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour {
    // Current Movement Direction
    // (by default it moves to the right)
    Vector2 dir = Vector2.right;
    // Keep Track of Tail
    List<Transform> tail = new List<Transform>();
    // Did the snake eat something?
    bool ate = false;
    // Tail Prefab
    public GameObject tailPrefab;
    // Snake Speed
    public float speed = 0.3f;
    // reference to the GameControl Script
    public GameControl gc;
    // original starting position for the spaceship
    Vector3 originalPos = new Vector3(0f, 0f, 0f);

    // Use this for initialization
    void Start()
    {
        // Move the Snake every 300ms
        InvokeRepeating("Move", speed, speed);
    }

    void Move()
    {
        // Save current position (gap will be here)
        Vector2 v = transform.position;

        // Move head into new direction (now there is a gap)
        transform.Translate(dir);

        // Ate something? Then insert new Element into gap
        if (ate)
        {
            // Load Prefab into the world
            GameObject g = (GameObject)Instantiate(tailPrefab,
                                                  v,
                                                  Quaternion.identity);

            // Keep track of it in our tail list
            tail.Insert(0, g.transform);

            // Reset the flag
            ate = false;
        }
        // Do we have a Tail?
        else if (tail.Count > 0)
        {
            // Move last Tail Element to where the Head was
            tail.Last().position = v;

            // Add to front of list, remove from the back
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count - 1);
        }
    }

    // Update is called once per Frame
    void Update()
    {
        // Move in a new Direction?
        if (Input.GetKey(KeyCode.RightArrow))
            dir = Vector2.right;
        else if (Input.GetKey(KeyCode.DownArrow))
            dir = -Vector2.up;    // '-up' means 'down'
        else if (Input.GetKey(KeyCode.LeftArrow))
            dir = -Vector2.right; // '-right' means 'left'
        else if (Input.GetKey(KeyCode.UpArrow))
            dir = Vector2.up;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // Food?
        if (coll.gameObject.name.Equals("ApplePrefab(Clone)"))
        {
            // Get longer in next Move call
            ate = true;
            // Remove the Food
            Destroy(coll.gameObject);
            gc.AddScore(1);
        }
        // Collided with Tail or Border
        else if(coll.gameObject.name.Equals("virus(Clone)"))
        {
            // Destroy Snake? 
            // Destroy(this.gameObject); 
            Destroy(coll.gameObject);
            transform.position = originalPos;
            gc.PlayerDie();
            // ToDo 'You lose' screen
        }
        else if (coll.gameObject.name.Equals("border_horizontal_top") ||
            coll.gameObject.name.Equals("border_horizontal_bottom") ||
            coll.gameObject.name.Equals("border_horizontal_left") ||
            coll.gameObject.name.Equals("border_horizontal_right"))
        {
            // Destroy Snake? 
            // Destroy(this.gameObject); 
            transform.position = originalPos;
            gc.PlayerDie();
        }
    }
}
