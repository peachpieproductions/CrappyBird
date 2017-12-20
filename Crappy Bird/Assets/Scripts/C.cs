using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct sound {
    public AudioClip ac;
    public bool pitchChange;
}

public class C : MonoBehaviour {

    public static C c;
    public GameObject carPrefab;
    public GameObject poopPrefab;
    public GameObject explosionPrefab;
    public static Transform leftSpawn;
    public static Transform rightSpawn;
    public Sprite[] carSprites;
    public sound[] snds;
    AudioSource AS;

	// Use this for initialization
	void Start () {
        c = GameObject.Find("C").GetComponent<C>();
        leftSpawn = GameObject.Find("LSpawn").transform;
        rightSpawn = GameObject.Find("RSpawn").transform;

        AS = gameObject.AddComponent<AudioSource>();

        InvokeRepeating("SpawnCar", 0f, 5f);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void SpawnCar() {
        Instantiate(carPrefab);
    }

    public void PlaySound(int index) {
        if (snds[index].pitchChange) {
            AS.pitch = Random.Range(.8f, 1.2f);
        }
        else AS.pitch = 1;
        AS.PlayOneShot(snds[index].ac);
    }

}

