using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    bool left;
    Rigidbody2D rb;
    SpriteRenderer spr;
    bool exploded;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = C.c.carSprites[Random.Range(0,C.c.carSprites.Length)];

        if (Random.value > .5f) left = true;
        if (left) {
            transform.position = C.rightSpawn.position;
            rb.velocity = Vector2.left * 3;
            //spr.flipX = true;
            var sca = transform.localScale;
            sca.x *= -1; transform.localScale = sca;
        } else {
            transform.position = C.leftSpawn.position;
            rb.velocity = Vector2.right;
            spr.sortingOrder = 1;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Poop")) {
            if (!exploded) {
                C.c.PlaySound(1);
                exploded = true;
                rb.isKinematic = false;
                rb.velocity = Vector3.up * 8;
                rb.angularVelocity = Random.Range(-10, 10);
                spr.color = Color.HSVToRGB(0,0,.1f);
                var inst = Instantiate(C.c.explosionPrefab,transform.position + Vector3.back,Quaternion.identity);
                inst.GetComponent<Rigidbody2D>().velocity = Vector2.left;
                Destroy(inst, 4f);
                Destroy(collision.gameObject, .2f);
                transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            }
        }
    }

}
