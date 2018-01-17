using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hawk : MonoBehaviour {

    float starty;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(12, Random.Range(-1, 3.5f));
        starty = transform.position.y;
        GetComponent<Rigidbody2D>().velocity = new Vector3(-C.c.scrollSpeed - 2, 0);
        InvokeRepeating("CheckOutOfBounds", 4f, 1f);
    }
	
	// Update is called once per frame
	void Update () {
        var pos = transform.position;
        pos = new Vector3(pos.x, 1.5f + C.c.osc1 * 2);
        transform.position = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Poop")) {
            GetComponent<Rigidbody2D>().velocity *= -1;
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void CheckOutOfBounds() {
        if (!C.c.InBounds(transform.position)) {
            Destroy(gameObject);
        }
    }
}
