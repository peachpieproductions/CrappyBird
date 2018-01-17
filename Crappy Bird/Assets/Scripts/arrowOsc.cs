using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowOsc : MonoBehaviour {

    Vector3 startPos;
    float osc;
    public float oscOffset;

	// Use this for initialization
	void Start () {
        startPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        osc = Mathf.Sin((Time.time + oscOffset) * 5);
        transform.localPosition = startPos + transform.right * osc * 8;
	}
}
