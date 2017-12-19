using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    bool left;
    Rigidbody2D rb;
    SpriteRenderer spr;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = C.c.carSprites[Random.Range(0,C.c.carSprites.Length)];

        if (Random.value > .5f) left = true;
        if (left) {
            transform.position = C.rightSpawn.position;
            rb.velocity = Vector2.left * 3;
            spr.flipX = true;
        } else {
            transform.position = C.leftSpawn.position;
            rb.velocity = Vector2.right;
            spr.sortingOrder = 1;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
