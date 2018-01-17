using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMeter : MonoBehaviour {

    public int player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
        var scale = transform.localScale;
        scale.x = C.c.comboTimer[player] * .25f;
        transform.localScale = scale;
        
	}
}
