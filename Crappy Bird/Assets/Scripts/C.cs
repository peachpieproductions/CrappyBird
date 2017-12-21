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
    public GameObject heliPrefab;
    public GameObject poopPrefab;
    public GameObject poopSplatPrefab;
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

        InvokeRepeating("SpawnCar", 0f, 2f);
    }

    // Update is called once per frame
    void Update () {
		
	}

    void SpawnCar() {
        Instantiate(carPrefab);
        if (Random.value < .1f) Instantiate(heliPrefab,new Vector3(-12,Random.Range(-2,5),0),Quaternion.identity);
    }

    public void PlaySound(int index, float vol = 1) {
        if (snds[index].pitchChange) {
            AS.pitch = Random.Range(.8f, 1.2f);
        }
        else AS.pitch = 1;
        AS.PlayOneShot(snds[index].ac,vol);
    }

}

