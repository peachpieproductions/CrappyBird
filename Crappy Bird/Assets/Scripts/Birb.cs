using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Birb : MonoBehaviour {

    Rigidbody2D rb;
    SpriteRenderer spr;
    bool hit;
    Collider2D coll;
    internal int lives = 4;
    bool dead;
    float carbounceTimer;
    float flapTimer;
    float poopTimer;
    public int skin;
    public int hat;
    public int player;
    GamePadState gamepad;

    //sprites
    public Sprite sprDown;
    public Sprite sprUp;
    public Sprite sprGlide;

    //Inputs
    KeyCode inputFlap;
    KeyCode inputCrap;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();

        skin = C.skinsPicked[player];
        sprDown = C.c.birdSprites[skin * 3];
        sprUp = C.c.birdSprites[skin * 3 + 1];
        sprGlide = C.c.birdSprites[skin * 3 + 2];

        SetHat();

        C.c.UpdateScore(player);
        C.c.hearts[player].SetActive(true);

        //get inputs
        inputFlap = C.c.inputFlap[player];
        inputCrap = C.c.inputCrap[player];

    }

    // Update is called once per frame
    void Update () {

        GamePadState prevGamepadState = gamepad;
        gamepad = GamePad.GetState((PlayerIndex)player);

        var pos = transform.position;
        pos.x = player * -1.5f;
        transform.position = pos;

        if (dead) {
            rb.velocity = Vector2.zero;
            return;
        }

        if (poopTimer > 0) {
            poopTimer -= Time.deltaTime;
        }

            //sprite
            if (flapTimer > 0) {
            flapTimer -= Time.deltaTime;
            spr.sprite = sprUp;
        } else spr.sprite = sprDown;
        if (Mathf.Abs(rb.velocity.y) > 6) spr.sprite = sprGlide;

        if (carbounceTimer > 0) carbounceTimer -= Time.deltaTime;

		if (Input.GetKeyDown(inputFlap) || (gamepad.Buttons.A == ButtonState.Pressed && prevGamepadState.Buttons.A == ButtonState.Released)) {
            rb.velocity = Vector3.zero;
            rb.velocity = new Vector2(0, 6);
            C.c.PlaySound(5);
            flapTimer = .25f;
        }

        if (Input.GetKeyDown(inputCrap) || (prevGamepadState.Buttons.X == ButtonState.Released && gamepad.Buttons.X == ButtonState.Pressed) || (gamepad.Triggers.Right > .6f && prevGamepadState.Triggers.Right < .6f)) {
            if (poopTimer <= 0) {
                var inst = Instantiate(C.c.poopPrefab, transform.position + Vector3.down * .5f, Quaternion.identity);
                inst.GetComponent<Rigidbody2D>().velocity = Vector2.right * 2 + Vector2.up * rb.velocity.y;
                inst.GetComponent<Poop>().owner = gameObject;
                C.c.PlaySound(0);
                poopTimer = .25f;
            }
        }

        transform.eulerAngles = new Vector3(0, 0, rb.velocity.y*5);
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!hit) {
            if (collision.transform.CompareTag("Obstacle") || collision.transform.CompareTag("Scenery")) {
                StartCoroutine(Death());
            }
        }
        if (collision.transform.CompareTag("Car")) {
            if (carbounceTimer <= 0 && !collision.transform.GetComponent<Car>().exploded) {
                rb.velocity = new Vector2(0, collision.relativeVelocity.magnitude);
                C.c.PlaySound(6);
                carbounceTimer = .5f;
                C.c.SpawnCoins(transform.position, 1, transform);
                C.c.score[player] += 25 * C.c.combo[player];
                C.c.UpdateScore(player);
            }
        }
    }

    public void SetHat() {
        if (hat == -1) {
            transform.GetChild(1).gameObject.SetActive(false);
            return;
        }
        transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = C.c.hatSprites[hat];
        transform.GetChild(1).localEulerAngles = new Vector3(0, 0, C.c.hatRotations[hat]);
        transform.GetChild(1).localPosition = C.c.hatOffsets[hat];
    }

    public IEnumerator Death() {
        C.c.PlaySound(Random.Range(7, 10));
        C.c.combo[player] = 0;
        C.c.comboTimer[player] = 0;
        C.c.comboText[player].text = "";
        spr.enabled = false;
        rb.isKinematic = true;
        hit = true;
        coll.enabled = false;
        dead = true;
        transform.GetChild(1).gameObject.SetActive(false); //hide hat
        if (lives > 1) C.c.hearts[player].transform.GetChild(lives - 2).gameObject.SetActive(false);
        var inst = Instantiate(C.c.birdDeathPoofPrefab, transform.position, Quaternion.identity);
        Destroy(inst, .65f);
        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();

        lives--;
        if (lives == 0) {
            C.c.GameOver();
            yield break;
        }
        
        yield return new WaitForSeconds(1f);

        if (hat != -1) transform.GetChild(1).gameObject.SetActive(true); //show hat
        coll.enabled = true;
        var pos = transform.position; pos.y = 3; transform.position = pos;
        rb.velocity = Vector3.zero;
        spr.enabled = true;
        rb.isKinematic = false;
        dead = false;
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(2f);

        hit = false;
    }
}
