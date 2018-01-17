using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEnemy : MonoBehaviour {

    float osc;
    float oscOffset;
    public Sprite[] sprs;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(12 + Random.Range(0,8), 1.5f);
        GetComponent<Rigidbody2D>().velocity = new Vector2(-C.c.scrollSpeed, 0);
        oscOffset = Random.Range(0, 100f);
        InvokeRepeating("CheckOutOfBounds", 4f, 1f);
        GetComponent<SpriteRenderer>().sprite = sprs[C.c.currentStage-1];
    }
	
	// Update is called once per frame
	void Update () {
        osc = Mathf.Sin(Time.time + oscOffset);
        var pos = transform.position;
        pos = new Vector3(pos.x, 1.5f + osc * 3);
        transform.position = pos;
        if (C.c.currentStage == 3) transform.eulerAngles += new Vector3(0, 0, 1);
    }

    void CheckOutOfBounds() {
        if (!C.c.InBounds(transform.position)) {
            Destroy(gameObject);
        }
    }

}
