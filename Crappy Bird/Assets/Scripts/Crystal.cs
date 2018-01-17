using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour {

    SpriteRenderer spr;
    float starty;
    float osc;

	// Use this for initialization
	void Start () {
        spr = GetComponent<SpriteRenderer>();
        GetComponent<Animator>().SetInteger("Color", Random.Range(0, 5));
        transform.position = new Vector3(12, Random.Range(-2, 5));
        starty = transform.position.y;
        SetSpeed();
        C.UpdateScrollSpeedsTrigger += SetSpeed;
        InvokeRepeating("CheckOutOfBounds", 4f, 1f);
    }
	
	// Update is called once per frame
	void Update () {
        var pos = transform.position;
        pos.y = starty + C.c.osc2 * .5f;
        transform.position = pos;
	}

    private void OnDestroy() {
        C.UpdateScrollSpeedsTrigger -= SetSpeed;
    }

    void SetSpeed() {
        GetComponent<Rigidbody2D>().velocity = new Vector2(-C.c.scrollSpeed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.CompareTag("Player")) {
            var p = collision.GetComponent<Birb>().player;
            Destroy(gameObject);
            C.c.PlaySound(10);
            C.c.score[p] += 50 * C.c.combo[p];
            C.c.UpdateScore(p);
            C.c.combo[p]++;
            C.c.comboTimer[p] = 4f;
            var inst = Instantiate(C.c.crystalPoofPrefab, transform.position, Quaternion.identity);
            inst.GetComponent<Rigidbody2D>().velocity = new Vector2(-C.c.scrollSpeed, 0);
            Destroy(inst, .4f);
        }
    }

    void CheckOutOfBounds() {
        if (transform.position.x < -12) {
            Destroy(gameObject);
        }
    }

}
