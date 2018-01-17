using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

[System.Serializable]
public struct sound {
    public AudioClip ac;
    public bool pitchChange;
}

[System.Serializable]
public struct optionsMenu {
    public bool musicOn;
    public bool soundEffectsOn;
    public bool debugSkipMenu;
}

public class C : MonoBehaviour {

    public static C c;
    public bool TestingFreeze;
    public float scrollSpeed;
    public GameObject carPrefab;
    public GameObject heliPrefab;
    public GameObject hawkPrefab;
    public GameObject poopPrefab;
    public GameObject poopSplatPrefab;
    public GameObject explosionPrefab;
    public GameObject birdDeathPoofPrefab;
    public GameObject crystalPoofPrefab;
    public GameObject sceneryObjectPrefab;
    public GameObject housePrefab;
    public GameObject boatPrefab;
    public GameObject deathBallPrefab;
    public GameObject blockEnemyPrefab;
    public GameObject crystalPrefab;
    public GameObject coinPrefab;
    public GameObject gameOverImage;
    public static Transform leftSpawn;
    public static Transform rightSpawn;
    public Sprite[] carSprites;
    public Sprite[] birdSprites;
    public Sprite[] hatSprites;
    public float[] hatRotations;
    public Vector2[] hatOffsets;
    public sound[] snds;
    public AudioClip[] songs;
    AudioSource AS;
    AudioSource music;
    internal float osc1;
    internal float osc2;
    internal float distanceThisRun;
    float distanceToSpeedUp;
    float distanceTrigger = 100;
    public bool gameOver;
    public static bool pressedStart;
    public static bool pressStartToBegin;
    public static bool gameStarted;
    public static int players = 1;
    public GameObject[] playerGo;
    public static bool[] playerJoined = new bool[4];
    public static int[] hatsPicked = new int[4];
    public static int[] skinsPicked = new int[4];
    float[] analogCooldown = new float[4];
    AudioClip currentSong;
    public optionsMenu options;
    internal int[] score = new int[4];
    internal int[] gold = new int[4];
    internal int[] combo = new int[4];
    internal float[] comboTimer = new float[4];
    public int currentStage = 1;

    //Inputs
    internal KeyCode[] inputFlap = new KeyCode[4];
    internal KeyCode[] inputCrap = new KeyCode[4];

    public delegate void UpdateScrollSpeeds();
    public static event UpdateScrollSpeeds UpdateScrollSpeedsTrigger;

    //hud
    [Header("HUD")]
    public Text distanceText;
    public GameObject playerSelectRoot;
    public Text[] scoreText;
    public Text[] comboText;
    public Text[] goldText;
    public GameObject[] goldPanel;
    public GameObject[] hearts;
    internal float[] goldShowTimer = new float[4];
    public GameObject FadePanel;

    // Use this for initialization
    void Start () {
        c = GameObject.Find("C").GetComponent<C>();
        leftSpawn = GameObject.Find("LSpawn").transform;
        rightSpawn = GameObject.Find("RSpawn").transform;

        AS = gameObject.AddComponent<AudioSource>();
        music = transform.GetChild(0).GetComponent<AudioSource>();

        //Set and Duplicate Tiling Backgrounds
        GameObject.Find("SceneryRoot").transform.GetChild(currentStage - 1).gameObject.SetActive(true);
        if (currentStage > 1) GameObject.Find("SceneryRoot").transform.GetChild(currentStage - 2).gameObject.SetActive(false);
        DuplicateBackgroundLayers();

        if (TestingFreeze) return;
        InvokeRepeating("UpdateScrollSpeed", 5f, 1f);
        StartCoroutine("SpawnCar");
        StartCoroutine("SpawnBoat");
        StartCoroutine("SpawnHelicopter");
        StartCoroutine("SpawnHouse");
        StartCoroutine("SpawnDeathBall");
        StartCoroutine("SpawnBlockEnemy");
        StartCoroutine("SpawnHawkEnemy");
        StartCoroutine("SpawnCrystal");
        StartCoroutine(SpawnSceneryObject());

        if (options.debugSkipMenu) {
            pressedStart = true;
            playerJoined[0] = true;
            StartGame();
        }

        //keyboard Inputs
        for (var i = 0; i < 4; i++) {
            switch (i) {
                case 0:
                    inputFlap[i] = KeyCode.A;
                    inputCrap[i] = KeyCode.S;
                    break;
                case 1:
                    inputFlap[i] = KeyCode.G;
                    inputCrap[i] = KeyCode.H;
                    break;
                case 2:
                    inputFlap[i] = KeyCode.K;
                    inputCrap[i] = KeyCode.L;
                    break;
                case 3:
                    inputFlap[i] = KeyCode.UpArrow;
                    inputCrap[i] = KeyCode.DownArrow;
                    break;
            }
        }

    }

    // Update is called once per frame
    void Update () {
        osc1 = Mathf.Sin(Time.time);
        osc2 = Mathf.Sin(Time.time * 1.5f);

        //Screenshot
        if (Input.GetKeyDown(KeyCode.Tab)) {
            ScreenCapture.CaptureScreenshot("ScreenShots/Screen" + Random.Range(0, 1000)+".png");
            Debug.Log("Screenshot Saved.");
        }

        if (!pressedStart) {
            if (Input.anyKeyDown) {
                pressedStart = true;
                playerSelectRoot.SetActive(true); 
            }
            return;
        }

        if (!gameStarted) {

            pressStartToBegin = false;
            for (var i = 0; i < 4; i++) {
                if (analogCooldown[i] > 0f) analogCooldown[i] -= Time.deltaTime;
                var gp = GamePad.GetState((PlayerIndex)i);
                if (!playerJoined[i]) {
                    if (gp.Buttons.A == ButtonState.Pressed || Input.GetKeyDown(inputFlap[i]) || Input.GetKeyDown(inputCrap[i])) {
                        playerJoined[i] = true;
                        var pressAPanel = GameObject.Find("PressToJoin").transform.GetChild(i);
                        var col = pressAPanel.GetComponent<Image>().color;
                        col.a = 0; pressAPanel.GetComponent<Image>().color = col;
                        pressAPanel.GetChild(0).gameObject.SetActive(false);
                    }
                } else {
                    pressStartToBegin = true;
                    if (gp.Buttons.Start == ButtonState.Pressed || Input.GetKeyDown(KeyCode.Return)) {
                        StartGame();
                    }
                    if (analogCooldown[i] <= 0) {
                        if (gp.ThumbSticks.Left.Y > .5f) {
                            hatsPicked[i]++; if (hatsPicked[i] == c.hatSprites.Length) hatsPicked[i] = -1;
                            analogCooldown[i] = .25f;
                        }
                        if (gp.ThumbSticks.Left.Y < -.5f) {
                            hatsPicked[i]--; if (hatsPicked[i] == -2) hatsPicked[i] = c.hatSprites.Length - 1;
                            analogCooldown[i] = .25f;
                        } 
                        if (gp.ThumbSticks.Left.X > .75f) {
                            skinsPicked[i]++; if (skinsPicked[i] == 4) skinsPicked[i] = 0;
                            analogCooldown[i] = .25f;
                        }
                        if (gp.ThumbSticks.Left.X < -.75f) {
                            skinsPicked[i]--; if (skinsPicked[i] == -1) skinsPicked[i] = 3;
                            analogCooldown[i] = .25f;
                        }
                    }
                }
            }
            return;
        }

        for (var i = 0; i < 4; i++) {
            if (comboTimer[i] > 0) {
                comboTimer[i] -= Time.deltaTime;
                if (comboTimer[i] <= 0) {
                    combo[i] = 0;
                    comboText[i].text = "";
                }
            } 
            if (goldShowTimer[i] > 0) {
                goldShowTimer[i] -= Time.deltaTime;
                if (goldShowTimer[i] <= 0) {
                    goldPanel[i].SetActive(false);
                }
            }
        }

        if (!gameOver) {
            distanceThisRun += Time.deltaTime * scrollSpeed;
            distanceToSpeedUp += Time.deltaTime * scrollSpeed;
            distanceText.text = Mathf.Floor(distanceThisRun).ToString() + " FT.";

            //next 
            if (currentStage < 3) {
                if (distanceThisRun > 1000 * currentStage) {
                    StartCoroutine(GoToNextStage());
                }
            }
        }

    }

    void StartGame() {
        if (!gameStarted) {
            gameStarted = true;
            StartCoroutine(PlayMusic());
            GameObject.Find("MainMenu").SetActive(false);
            DeleteObstacles();
            foreach (GameObject go in playerGo) {
                if (playerJoined[go.GetComponent<Birb>().player]) {
                    go.SetActive(true);
                    go.GetComponent<Birb>().hat = hatsPicked[go.GetComponent<Birb>().player];
                    go.GetComponent<Birb>().SetHat();
                }
            }
        }
    }

    IEnumerator PlayMusic() {
        while (true) {
            if (!options.musicOn) {
                yield return new WaitForSeconds(1f);
                continue;
            }
            var newSong = currentSong;
            while (newSong == currentSong) { newSong = songs[Random.Range(0, songs.Length)]; }
            currentSong = newSong;
            music.clip = currentSong;
            music.Play();
            yield return new WaitForSeconds(music.clip.length);
        }
    }

    IEnumerator GoToNextStage() {
        currentStage++;
        var col = FadePanel.GetComponent<Image>().color;
        while (col.a < 1) {
            col.a += Time.deltaTime * .5f;
            FadePanel.GetComponent<Image>().color = col;
            yield return null;
        }
        GameObject.Find("SceneryRoot").transform.GetChild(currentStage-1).gameObject.SetActive(true);
        GameObject.Find("SceneryRoot").transform.GetChild(currentStage-2).gameObject.SetActive(false);
        DuplicateBackgroundLayers();
        scrollSpeed = 2;
        distanceToSpeedUp = 0;
        distanceTrigger = 100;
        UpdateScrollSpeed();
        DeleteObstacles();
        while (col.a > 0) {
            col.a -= Time.deltaTime * .5f;
            FadePanel.GetComponent<Image>().color = col;
            yield return null;
        }
    }

    void DuplicateBackgroundLayers() {
        var scenery = GameObject.Find("SceneryRoot").transform.GetChild(currentStage - 1);
        Transform[] newScenery = new Transform[10];
        for (var i = 0; i < scenery.childCount; i++) {
            var width = scenery.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
            newScenery[i] = Instantiate(scenery.GetChild(i), scenery.GetChild(i).position + new Vector3(width * .95f, 0, 0), Quaternion.identity);
        }
        foreach(Transform scen in newScenery) {
            if (scen != null) {
                scen.parent = GameObject.Find("SceneryRoot").transform.GetChild(currentStage - 1);
            }
        }
    }

    public void UpdateScore(int p) {
        scoreText[p].text = string.Format("{0:#,###0}", score[p]);
        combo[p]++;
        comboTimer[p] = 4f;
        if (combo[p] > 1) comboText[p].text = "X" + combo[p].ToString();
    }

    public void UpdateGold(int p) {
        goldPanel[p].SetActive(true);
        goldText[p].text = "$"+gold[p].ToString();
        goldShowTimer[p] = 1f;
    }

    void DeleteObstacles() {
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Obstacle")) {
            Destroy(go);
        }
    }

    void UpdateScrollSpeed() {
        if (gameOver) return;
        if (distanceToSpeedUp > distanceTrigger) {
            distanceToSpeedUp = 0;
            distanceTrigger += ((scrollSpeed - 1) * 50);
            scrollSpeed++;
            if (UpdateScrollSpeedsTrigger != null) UpdateScrollSpeedsTrigger();
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("SceneryObject")) {
                go.GetComponent<Rigidbody2D>().velocity = Vector2.left * scrollSpeed;
            }
        }
    }

    public void SpawnCoins(Vector3 pos, int amount, Transform target) {
        for (int i = 0; i < amount; i++) {
            var inst = Instantiate(coinPrefab, pos + new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), 0),Quaternion.identity);
            inst.GetComponent<Coin>().target = target;
        }
    }

    IEnumerator SpawnCar() {
        while (true) {
            Instantiate(carPrefab);
            yield return new WaitForSeconds(Mathf.Max(.5f, 4f - scrollSpeed * .5f));
        }
    }

    IEnumerator SpawnBoat() {
        yield return new WaitForSeconds(15f);
        while (true) {
            if (currentStage != 1) break;
            if (Random.value < .5f) {
                var inst = Instantiate(boatPrefab);
                inst.GetComponent<Rigidbody2D>().velocity = new Vector2(-scrollSpeed * .2f, 0);
                Destroy(inst, 60f);
            }
            yield return new WaitForSeconds(70f);
        }
    }

    IEnumerator SpawnHelicopter() {
        while (true) {
            if (Random.value < .3f) Instantiate(heliPrefab, new Vector3(-12, Random.Range(-2, 5), 0), Quaternion.identity);
            yield return new WaitForSeconds(Mathf.Max(.5f, 10f - scrollSpeed * .25f));
        }
    }

    IEnumerator SpawnHouse() {
        while (true) {
            if (Random.value < .5f) {
                Instantiate(housePrefab, new Vector3(12, -1.65f), Quaternion.identity);
            }
            yield return new WaitForSeconds(Mathf.Max(.5f, 5f - scrollSpeed * .5f));
        }
    }

    IEnumerator SpawnDeathBall() {
        while (true) {
            if (Random.value < .8f) {
                Instantiate(deathBallPrefab, new Vector3(12, Random.Range(0f, 4f)), Quaternion.Euler(0, 0, Random.Range(0, 359)));
                if (currentStage == 3) {
                    if (Random.value < .5f) {
                        Instantiate(deathBallPrefab, new Vector3(12, Random.Range(0f, 4f)), Quaternion.Euler(0, 0, Random.Range(0, 359)));
                    }
                }
            }
            yield return new WaitForSeconds(Mathf.Max(3f, 8f - scrollSpeed * .5f));
        }
    }

    IEnumerator SpawnBlockEnemy() {
        yield return new WaitForSeconds(4f); //offset
        while (true) {
            if (Random.value < .5f) {
                Instantiate(blockEnemyPrefab);
                if (currentStage == 3) {
                    if (Random.value < .5f) {
                        Instantiate(blockEnemyPrefab);
                    }
                }
            }
            yield return new WaitForSeconds(Mathf.Max(3f, 7f - scrollSpeed * .5f));
        }
    }

    IEnumerator SpawnHawkEnemy() {
        while (true) {
            Instantiate(hawkPrefab);
            yield return new WaitForSeconds(Mathf.Max(3f, 7f - scrollSpeed * .5f));
        }
    }

    IEnumerator SpawnCrystal() {
        while (true) {
            Instantiate(crystalPrefab);
            yield return new WaitForSeconds(Mathf.Max(1f, 5f - scrollSpeed * .5f));
        }
    }

    IEnumerator SpawnSceneryObject() {
        for (;;) {
            if (currentStage == 3) break;
            if (Random.value < .5f) {
                var sel = Random.Range(0, 4);
                var inst = Instantiate(sceneryObjectPrefab, new Vector3(12, -3), Quaternion.identity);
                inst.transform.GetChild(sel).gameObject.SetActive(true);
                inst.GetComponent<Rigidbody2D>().velocity = Vector2.left * scrollSpeed;
                Destroy(inst, 25f / scrollSpeed);
                if (Random.value < .5f) inst.transform.GetChild(sel).GetComponent<SpriteRenderer>().flipX = true;
            }
            yield return new WaitForSeconds(Mathf.Max(.2f, 2f - scrollSpeed * .5f));
        }
    }

    public bool InBounds(Vector3 Pos) {
        if (Pos.x < -13 || Pos.x > 13) {
            return false;
        } else return true;
    }

    public void PlaySound(int index, float vol = 1) {
        if (!options.soundEffectsOn) return;
        if (snds[index].pitchChange) {
            AS.pitch = Random.Range(.8f, 1.2f);
        }
        else AS.pitch = 1;
        AS.PlayOneShot(snds[index].ac,vol);
    }

    public void GameOver() {
        gameOver = true;
        for (var i = 0; i < 4; i++) {
            if (playerJoined[i]) {
                if (playerGo[i].GetComponent<Birb>().lives > 0) {
                    gameOver = false;
                }
            }
        }
        if (gameOver) gameOverImage.SetActive(true);
    }

    

}

