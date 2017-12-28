using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour {

    bool left;
    bool moving = true;
    float movingTimer;
    Rigidbody2D rb;
    float speed;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        movingTimer = Random.Range(5, 20);
	}
	
	// Update is called once per frame
	void Update () {
        
        if (moving) {
            speed = Mathf.Lerp(speed, 1, .005f);
        } else {
            speed = Mathf.Lerp(speed, 0, .01f);
        }

        transform.eulerAngles = new Vector3(0, 0, 15 * -speed);

        rb.velocity = Vector3.right * speed;

        if (movingTimer > 0) movingTimer -= Time.deltaTime;
        else {
            if (Random.value < .01) {
                moving = !moving;
                movingTimer = Random.Range(5, 12);
            }
        }
	}
}
