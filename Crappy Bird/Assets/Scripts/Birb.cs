using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birb : MonoBehaviour {

    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
            rb.velocity = Vector3.zero;
            rb.velocity = new Vector2(0, 6);
        }

        transform.eulerAngles = new Vector3(0, 0, rb.velocity.y*5);
	}
}
