using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    bool left;
    Rigidbody2D rb;
    SpriteRenderer spr;
    internal bool exploded;
    bool hasCollider = true;
    float carHitSndTimer;
    BoxCollider2D coll;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        spr.sprite = C.c.carSprites[Random.Range(0,C.c.carSprites.Length)];
        coll = GetComponent<BoxCollider2D>();
        C.UpdateScrollSpeedsTrigger += SetSpeed;

        if (Random.value > .5f) left = true;
        if (left) {
            transform.position = C.rightSpawn.position;
            SetSpeed();
            var sca = transform.localScale;
            sca.x *= -1; transform.localScale = sca;
        } else {
            transform.position = C.leftSpawn.position;
            SetSpeed();
            spr.sortingOrder = 2;
        }
        InvokeRepeating("CheckOutOfBounds", 4f, 1f);
    }
	
	// Update is called once per frame
	void Update () {
        if (exploded) {
            if (transform.position.y > -2.2f) {
                if (hasCollider) {
                    hasCollider = false;
                    if (Random.value > .25f) {
                        Destroy(GetComponent<Collider2D>());
                        spr.sortingOrder = 2;
                    }
                }
            }
        }
        if (carHitSndTimer > 0) carHitSndTimer -= Time.deltaTime;
	}

    void SetSpeed() {
        if (!exploded && rb != null) {
            if (left) {
                rb.velocity = Vector2.left * (C.c.scrollSpeed + 2);
            } else {
                rb.velocity = Vector2.right * (C.c.scrollSpeed - 1);
            }
        }
    }

    private void OnDestroy() {
        C.UpdateScrollSpeedsTrigger -= SetSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Poop")) {
            if (!exploded) {
                var owner = collision.transform.GetComponent<Poop>().owner;
                var player = owner.GetComponent<Birb>().player;
                C.c.SpawnCoins(transform.position, 2 + Random.Range(0, 2), owner.transform);
                C.c.score[player] += 100 * C.c.combo[player];
                C.c.UpdateScore(player);
                spr.sortingOrder = 0;
                C.c.PlaySound(1);
                exploded = true;
                Invoke("SetLayerToDestroyed", .5f); //delayed so that poop flips car correctly
                rb.isKinematic = false;
                rb.velocity = Vector3.up * 8;
                rb.angularVelocity = Random.Range(-10, 10);
                spr.color = Color.HSVToRGB(0,0,.1f);
                var inst = Instantiate(C.c.explosionPrefab,transform.position + Vector3.back,Quaternion.identity);
                inst.GetComponent<Rigidbody2D>().velocity = Vector2.left;
                Destroy(inst, 4f);
                var partSys = transform.GetChild(0).GetComponent<ParticleSystem>();
                partSys.Play();
                var vol = partSys.velocityOverLifetime;
                vol.x = -C.c.scrollSpeed;
                var size = coll.size; size.y *= 1.3f; coll.size = size;

            }
        } else {
            if (carHitSndTimer <= 0) {
                carHitSndTimer = .25f;
                C.c.PlaySound(6, Mathf.Min(.5f, collision.relativeVelocity.magnitude * .1f));
            }
        }
    }

    void SetLayerToDestroyed() { //delayed function so that poop flips car correctly
        gameObject.layer = 11;
    }

    void CheckOutOfBounds() {
        if (!C.c.InBounds(transform.position)) {
            Destroy(transform.parent.gameObject);
        }
    }

}
