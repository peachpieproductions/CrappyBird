using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModel : MonoBehaviour {

    RectTransform rect;
    RectTransform hatRect;
    Image hatImage;
    Image spr;
    float yspeed;
    float flapTimer;
    public int slot;
    public int skin;

	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();
        spr = transform.GetChild(1).GetComponent<Image>();
        hatRect = transform.GetChild(0).GetComponent<RectTransform>();
        hatImage = transform.GetChild(0).GetComponent<Image>();
        spr.sprite = C.c.birdSprites[skin * 3];
        C.hatsPicked[slot] = -1;
    }

    // Update is called once per frame
    void Update() {
        yspeed -= .025f;
        var pos = rect.position;
        pos.y += yspeed;
        rect.position = pos;

        //hat
        if (C.hatsPicked[slot] != -1) {
            hatImage.gameObject.SetActive(true);
            hatImage.sprite = C.c.hatSprites[C.hatsPicked[slot]];
            hatRect.localPosition = C.c.hatOffsets[C.hatsPicked[slot]] * 300;
            hatRect.localEulerAngles = new Vector3(0, 0, C.c.hatRotations[C.hatsPicked[slot]]);
            hatImage.SetNativeSize();
            var piv = hatRect.pivot;
            piv.x = hatImage.sprite.pivot.x / (hatImage.sprite.bounds.size.x * 100);
            piv.y = hatImage.sprite.pivot.y / (hatImage.sprite.bounds.size.y * 100);
            hatRect.pivot = piv;
        } else {
            hatImage.gameObject.SetActive(false);
        }

        //update skin
        var oldSkin = skin;
        skin = C.skinsPicked[slot];
        if (oldSkin != skin) {
            if (flapTimer > 0) {
                spr.sprite = C.c.birdSprites[skin * 3 + 1];
            } else {
                spr.sprite = C.c.birdSprites[skin * 3];
            }
        }

        if (rect.localPosition.y < -70 && yspeed < 0) {
            yspeed = 2.5f;
            flapTimer = .5f;
            spr.sprite = C.c.birdSprites[skin * 3 + 1];
        } if (flapTimer > 0) {
            flapTimer -= Time.deltaTime;
            if (flapTimer <= 0) spr.sprite = C.c.birdSprites[skin * 3];
        }

        transform.eulerAngles = new Vector3(0, 0, yspeed * 4);
    }
}
