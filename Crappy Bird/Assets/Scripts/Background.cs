using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    BoxCollider2D coll;
    Rigidbody2D rb;
    float width;
    public float scrollSpeed;

	// Use this for initialization
	void Start () {
        coll = GetComponent<BoxCollider2D>();
        width = coll.bounds.size.x;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-scrollSpeed, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x < -width) transform.position = new Vector3(width-.3f, 0, 0);
	}
}
