using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour {

    bool left;
    bool moving = true;
    float movingTimer;
    Rigidbody2D rb;
    float speed;
    SpriteRenderer spr;
    bool exploded;
    bool fast;
    float carHitSndTimer;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        movingTimer = Random.Range(5, 20);
        spr = GetComponent<SpriteRenderer>();
        if (Random.value < .25f) {
            fast = true;
            GetComponent<Animator>().SetBool("Red",true);
        }
        InvokeRepeating("CheckOutOfBounds", 4f, 1f);
    }
	
	// Update is called once per frame
	void Update () {

        if (exploded) return;

        if (moving) {
            var maxSpeed = 1;
            if (fast) maxSpeed = 2;
            speed = Mathf.Lerp(speed, maxSpeed, .005f);
        } else {
            speed = Mathf.Lerp(speed, 0, .01f);
        }

        transform.eulerAngles = new Vector3(0, 0, 15 * -speed);

        rb.velocity = Vector3.right * speed;

        if (carHitSndTimer > 0) carHitSndTimer -= Time.deltaTime;

        if (movingTimer > 0) movingTimer -= Time.deltaTime;
        else {
            if (Random.value < .01 && !fast) {
                moving = !moving;
                movingTimer = Random.Range(5, 12);
            }
        }
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.CompareTag("Poop")) {
            if (!exploded) {
                var owner = collision.transform.GetComponent<Poop>().owner;
                var player = owner.GetComponent<Birb>().player;
                C.c.SpawnCoins(transform.position, 2 + Random.Range(0, 2), owner.transform);
                C.c.score[player] += 100 * C.c.combo[player];
                C.c.UpdateScore(player);
                C.c.PlaySound(1);
                exploded = true;
                gameObject.layer = 11;
                rb.isKinematic = false;
                rb.velocity = Vector3.up * Random.Range(0,8);
                rb.angularVelocity = Random.Range(-500, 500);
                spr.color = Color.HSVToRGB(0, 0, .1f);
                var inst = Instantiate(C.c.explosionPrefab, transform.position + Vector3.back, Quaternion.identity);
                inst.GetComponent<Rigidbody2D>().velocity = Vector2.left;
                Destroy(inst, 4f);
                GetComponent<Animator>().speed = 0;
                var partSys = transform.GetChild(0).GetComponent<ParticleSystem>();
                partSys.Play();
                var vol = partSys.velocityOverLifetime;
                vol.x = -C.c.scrollSpeed;
            }
        } else {
            if (carHitSndTimer <= 0) {
                carHitSndTimer = .1f;
                C.c.PlaySound(6, Mathf.Min(.5f, collision.relativeVelocity.magnitude * .2f));
            }
        }
    }

    void CheckOutOfBounds() {
        if (!C.c.InBounds(transform.position)) {
            Destroy(gameObject);
        }
    }
}
