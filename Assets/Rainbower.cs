using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainbower : MonoBehaviour
{
    #region
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    static Rainbower instance;
    public static Rainbower GetInstance()
    {
        return instance;
    }
    #endregion

    void Start()
    {
        SetColors();
    }

    List<StackerDot> dots;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(LotsOfSwooshes(manualSwooshCount));
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
        }
    }


    public Color[] colors;

    public static bool rainbowing;

    public int manualSwooshCount = 3;
    public int ringsPerSwoosh = 5;

    public IEnumerator LotsOfSwooshes(int numSwooshes)
    {
        if (NewScoreManager.newHighscore) {
            rainbowing = true;
            for (int i = 0; i < numSwooshes; i++)
            {
                StartCoroutine(MoveRings(colors[i]));
                yield return StartCoroutine(Extensions.Wait(swooshInterval));
            }
            yield return new WaitForSeconds(.3f);
            rainbowing = false;
        } else {
            yield return null;
        }
    }

    public float swooshDuration = .5f;
    public float swooshInterval = .5f;

    IEnumerator MoveRings(Color targetColor)
    {
        float t = 0;
        float d = swooshDuration;

        while (t < d)
        {
            t += Time.fixedDeltaTime;

            float p = t / d;
            totalLength = dots.Count + ringsPerSwoosh;
            int index = Mathf.FloorToInt(EZEasings.SmoothStart3(p) * (float)totalLength);

            // UpdateRings(index, targetColor);
            // if(index < 5) {
                UpdateParticles(index, targetColor);
            // }
            yield return new WaitForFixedUpdate();
        }
    }

    int totalLength;
    int lastIndex;

    void UpdateParticles(int index, Color targetColor) {
        if(index < dots.Count && index != lastIndex && GameOverStacker.GetInstance().spawnParticleRings) {
            ParticleRingSpawner.GetInstance().TriggerRing(index, targetColor);
            lastIndex = index;
        }
    }

    void SetColors()
    {
        colors = BallColorManager.GetInstance().ballColors;
    }

    public void SetDots(List<StackerDot> a_dots)
    {
        dots = a_dots;
    }
}
