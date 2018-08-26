using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonolithManager : MonoBehaviour
{
    public static MonolithManager instance;
    public static MonolithManager GetInstance() {
        return instance;
    }

    void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public Monolith monolith;

    int numRects = 2;
    Monolith[] monoliths = new Monolith[2];

    public Color monolithColor;

    bool monoballActive = false;

    public void Initialize()
    {
        SpawnRects();
        SpawnMonoBall();

        EventManager.StartListening("BallDied", Reset);
    }

    // Update is called once per frame
    void Update()
    {
        if(monoballActive) {
            for(int i = 0; i < 2; i++) {
                monoliths[i].gameObject.SetActive(false);
            }
        } else {
            for(int i = 0; i < 2; i++) {
                monoliths[i].gameObject.SetActive(true);
            }

            if(AllMonolithsReady()) {
                ActivateMonoball();
            }
        }
    }

    void SpawnRects()
    {
        monoliths[0] = Instantiate(monolith);
        monoliths[1] = Instantiate(monolith);

		monoliths[0].useOffset = false;
		monoliths[1].useOffset = true;

        for(int i = 0; i < 2; i++) {
            monoliths[i].startColor = monolithColor;
        }
    }

    GameObject monoball;
    public GameObject ballPrefab;
    public Vector2 monoballPos = new Vector2(0, 4);

    void SpawnMonoBall() {
        if(!monoball) {
            GameObject b = Instantiate(ballPrefab);
            b.transform.localScale = Vector2.one * .65f; //NewBallManager.GetInstance().ballScale;
            b.transform.position   = monoballPos;
            b.GetComponentInChildren<NewBallArtManager>().SetColor(monolithColor);
            monoball = b;
        }
    }

    void ActivateMonoball() {
        monoball.GetComponent<Rigidbody2D>().simulated = true;
        monoballActive = true;
    }

    void Reset() {
        monoballActive = false;
        SpawnMonoBall();
    }

    bool AllMonolithsReady() {
        return monoliths[0].TransitionComplete() && monoliths[1].TransitionComplete();
    }
}
