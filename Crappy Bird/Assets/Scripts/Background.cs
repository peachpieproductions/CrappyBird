using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    BoxCollider2D coll;
    Rigidbody2D rb;
    float width;
    public float depth;

	// Use this for initialization
	void Start () {
        coll = GetComponent<BoxCollider2D>();
        width = coll.bounds.size.x;
        rb = GetComponent<Rigidbody2D>();
        SetSpeed();
        C.UpdateScrollSpeedsTrigger += SetSpeed;
        
	}

    void SetSpeed() {
        rb.velocity = new Vector2(-C.c.scrollSpeed + depth * .4f, 0);
    }

    // Update is called once per frame
    void Update () {
        if (transform.position.x < -width) {
            var pos = transform.position;
            pos.x = width * .95f;
            transform.position = pos;
        }
	}
}
