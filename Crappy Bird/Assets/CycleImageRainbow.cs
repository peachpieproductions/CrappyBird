using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleImageRainbow : MonoBehaviour {

    Image img;
    float hue;
    float pressStartAlpha = -.5f;
    RectTransform titlePanel;
    public RectTransform pressStartText;
    public RectTransform pressStartToBeginText;

    // Use this for initialization
    void Start () {
        img = GetComponent<Image>();
        titlePanel = transform.GetChild(0).GetComponent<RectTransform>();
        if (C.c.options.musicOn) GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update () {
        var col = img.color;
        col = Color.HSVToRGB(hue, .75f, 1f);
        col.a = .6f;
        img.color = col;
        hue+=.001f;
        if (hue > 1f) hue = 0f;

        if (C.pressedStart) {
            pressStartText.gameObject.SetActive(false);
        }

        //scale title
        var scale = titlePanel.localScale;
        scale.x = 1 + C.c.osc2 * .2f;
        scale.y = 1 + C.c.osc2 * .2f;
        titlePanel.localScale = scale;

        //Press start
        if (pressStartAlpha < 1) {
            pressStartAlpha += Time.deltaTime * .3f;
            col = pressStartText.GetComponent<Text>().color;
            col.a = pressStartAlpha;
            pressStartText.GetComponent<Text>().color = col;
        }
        var pos = pressStartText.position;
        pos.y = 125 + Mathf.Abs(C.c.osc2) * 50f;
        pressStartText.position = pos;

        if (C.pressStartToBegin) {
            pressStartToBeginText.gameObject.SetActive(true);
            pos = pressStartToBeginText.position;
            pos.y = Mathf.Abs(C.c.osc1) * 50f;
            pressStartToBeginText.position = pos;
        }


    }
}
