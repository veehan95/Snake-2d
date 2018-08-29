using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Snake : MonoBehaviour {
    // Current Movement Direction
    // (by default it moves to the right)
    Vector2 dir = Vector2.right;
    
    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    // Keep Track of Tail
    List<Transform> tail = new List<Transform>();
    
    // Did the snake eat something?
    bool ate = false;
    
    // Tail Prefab
    public GameObject tailPrefab;
    
    // Snake Speed
    public float speed = 0.1f;
    
    // reference to the GameControl Script
    public GameControl gc;
    
    // original starting position for the spaceship
    Vector3 originalPos = new Vector3(0f, 0f, 0f);

    // Scores
    public int score_rune = 3;
    public int score_meat = 2;
    public int score_apple = 1;

    // Rune effect
    public float rune_duration = 10f;
    public float rune_duration_temp;
    public int double_rune_mult = 2;
    public int triple_rune_mult = 3;
    private int mult_score = 1;
    private int mult_speed = 1;

    // Use this for initialization
    void Start()
    {
        // Move the Snake every speed*mult_speed
        InvokeRepeating("Move", speed * mult_speed, speed * mult_speed);
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
        if (rune_duration_temp > 0)
        {
            rune_duration_temp -= Time.deltaTime;
            if (rune_duration_temp <= 0)
            {
                mult_score = 1;
                mult_speed = 1;
                gc.ShowRuneEffect("");
            }
        }
        // Move in a new Direction?
        if (Input.GetKey(KeyCode.RightArrow))
            dir = Vector2.right;
        else if (Input.GetKey(KeyCode.DownArrow))
            dir = -Vector2.up;    // '-up' means 'down'
        else if (Input.GetKey(KeyCode.LeftArrow))
            dir = -Vector2.right; // '-right' means 'left'
        else if (Input.GetKey(KeyCode.UpArrow))
            dir = Vector2.up;

        //Allow the snake come out from another site of the map
        if (transform.position.x < borderLeft.position.x + 1f)
            transform.position = new Vector3(borderRight.position.x - 1f, transform.position.y, -1f);
        else if (transform.position.x > borderRight.position.x - 1f)
            transform.position = new Vector3(borderLeft.position.x + 1f, transform.position.y, -1f);
        else if (transform.position.y < borderBottom.position.y + 1.5f)
            transform.position = new Vector3(transform.position.x, borderTop.position.y - 1.5f, -1f);
        else if (transform.position.y > borderTop.position.y - 1.5f)
            transform.position = new Vector3(transform.position.x, borderBottom.position.y + 1.5f, -1f);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        // Food?
        if (coll.gameObject.tag.Equals("Apple"))
        {
            // Get longer in next Move call
            ate = true;
            // Remove the Food
            Destroy(coll.gameObject);
            gc.AddScore(score_apple * mult_score);
        }
        // Collided with meat
        else if (coll.gameObject.tag.Equals("Meat"))
        {
            // Remove the Food
            Destroy(coll.gameObject);
            gc.AddScore(score_meat * mult_score);
        }
        // Collided with medicine
        else if (coll.gameObject.tag.Equals("Medicine"))
        {
            // Remove the Food
            Destroy(coll.gameObject);
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Virus"))
                Destroy(go);
        }
        // Collided with Tail or Border
        else if (coll.gameObject.tag.Equals("Virus") || coll.gameObject.tag.Equals("Tail"))
        {
            // Destroy Snake? 
            // Destroy(this.gameObject);
            if (!gc.PlayerDie())
                transform.position = originalPos;
            // ToDo 'You lose' screen
        }
        // Collided with rune
        else if (coll.gameObject.tag.Equals("Rune"))
        {
            // Remove the rune
            Destroy(coll.gameObject);
            rune_duration_temp = rune_duration;
            int temp;
            if (Random.Range(1, 100) % 2 == 0)
                temp = double_rune_mult;
            else
                temp = triple_rune_mult;

            string runeEffect;
            if (Random.Range(1, 100) % 2 == 0)
            {
                runeEffect = "Speed X" + temp;
                mult_speed = temp;
            }
            else
            {
                runeEffect = "Score X" + temp;
                mult_score = temp;
            }

            gc.ShowRuneEffect(runeEffect);
            gc.AddScore(score_rune * mult_score);
        }
        // Collided with WormHole
        else if (coll.gameObject.tag.Equals("WormHole"))
        {
            float loc_y = transform.position.y, loc_x = transform.position.x;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("WormHole"))
                if (go != coll.gameObject)
                {
                    loc_x = go.gameObject.transform.position.x;
                    loc_y = go.gameObject.transform.position.y;
                }
            transform.position = new Vector3((loc_x + 2 * dir[0]), (loc_y + 2 * dir[1]), -1f);
        }
    }
}
