﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFX : MonoBehaviour
{
    public AudioSource Eat;
    public AudioSource Blip;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag.Equals("Apple"))
        {
            Eat.Play();
        }
        else if (coll.gameObject.tag.Equals("Meat"))
        {
            Eat.Play();
        }
        else if (coll.gameObject.tag.Equals("Medicine"))
        {
            Eat.Play();
        }
        else if (coll.gameObject.tag.Equals("Potion"))
        {
            Eat.Play();
        }
        else if (coll.gameObject.tag.Equals("Virus"))
        {
            Blip.Play();
        }

    }
}
