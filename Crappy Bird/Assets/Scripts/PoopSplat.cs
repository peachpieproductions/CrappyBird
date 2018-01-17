using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoopSplat : MonoBehaviour {

    public Sprite[] imgs;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().sprite = imgs[Random.Range(0, imgs.Length)];
        SetSpeed();
        C.UpdateScrollSpeedsTrigger += SetSpeed;
	}

    private void OnDestroy() {
        C.UpdateScrollSpeedsTrigger -= SetSpeed;
    }

    void SetSpeed() {
        GetComponent<Rigidbody2D>().velocity = Vector2.left * C.c.scrollSpeed;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
