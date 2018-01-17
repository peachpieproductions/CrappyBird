using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {

    Rigidbody2D rb;
    SpriteRenderer spr;
    PolygonCollider2D coll;
    bool exploded;
    bool collGenerated;
    public Sprite[] sprs;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<PolygonCollider2D>();
        SetSpeed();
        C.UpdateScrollSpeedsTrigger += SetSpeed;
        spr = GetComponent<SpriteRenderer>();
        if (Random.value < .5f) {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        InvokeRepeating("CheckOutOfBounds", 4f, 1f);
        spr.sprite = sprs[C.c.currentStage - 1];
        if (C.c.currentStage == 3) Invoke("GenerateNewCollision", .25f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy() {
        C.UpdateScrollSpeedsTrigger -= SetSpeed;
    }

    void SetSpeed() {
        if (!exploded) {
            rb.velocity = Vector2.left * C.c.scrollSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Poop")) {
            if (!exploded) {
                var owner = collision.transform.GetComponent<Poop>().owner;
                var player = owner.GetComponent<Birb>().player;
                C.c.SpawnCoins(transform.position, 3 + Random.Range(0, 2), owner.transform);
                C.c.score[player] += 100 * C.c.combo[player];
                C.c.UpdateScore(player);
                C.c.PlaySound(1);
                exploded = true;
                gameObject.layer = 11;
                rb.isKinematic = false;
                rb.velocity = Vector3.up * 8;
                rb.angularVelocity = Random.Range(-150, 150);
                spr.color = Color.HSVToRGB(0, 0, .1f);
                var inst = Instantiate(C.c.explosionPrefab, transform.position + Vector3.back, Quaternion.identity);
                inst.GetComponent<Rigidbody2D>().velocity = Vector2.left;
                Destroy(inst, 4f);
            }
        } 
    }

    void GenerateNewCollision() {
        Destroy(coll);
        coll = gameObject.AddComponent<PolygonCollider2D>();
    }

    void CheckOutOfBounds() {
        if (transform.position.x < -12) {
            Destroy(gameObject);
        }
    }
}
