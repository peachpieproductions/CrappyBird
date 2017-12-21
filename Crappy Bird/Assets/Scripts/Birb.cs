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
            C.c.PlaySound(5);
        }

        if (Input.GetMouseButtonDown(0)) {
            var inst = Instantiate(C.c.poopPrefab, transform.position + Vector3.down * .5f, Quaternion.identity);
            inst.GetComponent<Rigidbody2D>().velocity = Vector2.right * 2;
            C.c.PlaySound(0);
        }

        transform.eulerAngles = new Vector3(0, 0, rb.velocity.y*5);
	}
}
