using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregameTrailSpawner : MonoBehaviour
{

    public PregameTrail trailer;
    PregameTrail trailObj;

    public float duration;
    public float height;

    [System.NonSerialized]
    public Vector3 startPos;
    public float dotScale = .08f;

    bool drawTrail = false;

    // Use this for initialization
    void Start()
    {
        startPos = NewBallManager.GetInstance().ballSpawnPos;
        trailObj = Instantiate(trailer);
        trailObj.defaultScale = dotScale;
    }

    float t = 0;

    void FixedUpdate()
    {
        trailObj.GetComponent<PregameTrail>().drawTrail = drawTrail;
        if(!drawTrail) { return; }

        if (t < duration)
        {
            t += Time.fixedDeltaTime;
            float percent = t / duration;

            trailObj.transform.position = startPos + Vector3.up * height * percent;

        }
        else
        {
            if (!trailStarted)
            {
                trailStarted = true;
                StartTrail(delay);
            }
        }
    }

    bool trailStarted = false;
	public float delay;

    void StartTrail(float d)
    {
		trailObj.transform.position = startPos;
        Invoke("Reset", d);
    }

    public void EnableTrail(bool val) {
        drawTrail = val;
    }

    public Vector3 offset;

    public void SetPosition() {
        startPos = (Vector3)NewBallManager.GetInstance().ballSpawnPos + offset;
    }

    void Reset()
    {
        t = 0;
		trailStarted = false;
    }
}
