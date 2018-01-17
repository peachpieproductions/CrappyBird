using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    Rigidbody2D rb;
    public Transform target;
    float angle;

	// Use this for initialization
	void Start () {
        angle = Random.Range(0, 360);
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (target != null) {
            var dir = target.position - transform.position;
            float targetAng = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle = Mathf.LerpAngle(angle, targetAng, .075f);
            rb.velocity = (Quaternion.Euler(0, 0, angle) * Vector2.right) * 5;
        } else {

        }
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.CompareTag("Player")) {
            var who = collision.GetComponent<Birb>().player;
            Destroy(gameObject);
            C.c.gold[who]++;
            C.c.UpdateGold(who);
            C.c.PlaySound(Random.Range(11,14));
        }
    }
}
