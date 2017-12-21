using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject,.025f);
        C.c.PlaySound(Random.Range(2, 5));
        if (collision.transform.CompareTag("Scenery")) {
            var inst = Instantiate(C.c.poopSplatPrefab, transform.position, Quaternion.identity);
            Destroy(inst, 10f);
        }
    } 
}
