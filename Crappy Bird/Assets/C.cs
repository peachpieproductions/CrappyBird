using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C : MonoBehaviour {

    public static C c;
    public GameObject carPrefab;
    public static Transform leftSpawn;
    public static Transform rightSpawn;
    public Sprite[] carSprites;

	// Use this for initialization
	void Start () {
        c = GameObject.Find("C").GetComponent<C>();
        leftSpawn = GameObject.Find("LSpawn").transform;
        rightSpawn = GameObject.Find("RSpawn").transform;

        InvokeRepeating("SpawnCar", 0f, 3f);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void SpawnCar() {
        Instantiate(carPrefab);
    }

}
