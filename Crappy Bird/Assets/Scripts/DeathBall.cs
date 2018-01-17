using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBall : MonoBehaviour {

    Rigidbody2D rb;
    bool type2;
    float spinDir = 1;
    public Sprite[] sprs;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-C.c.scrollSpeed,0);
        if (Random.value < .4f) type2 = true;
        if (Random.value < .5f) spinDir = -1;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprs[C.c.currentStage - 1];
        if (C.c.currentStage == 3) transform.localScale *= 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
        if (type2) {
            transform.eulerAngles = new Vector3(0,0,C.c.osc2 * 90);
        }
        else transform.Rotate(0, 0, .5f * spinDir);
        if (transform.position.x < -12) {
            Destroy(gameObject);
        }
	}
}
