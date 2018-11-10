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
    }


    public Color[] colors;

    public static bool rainbowing;

    public int manualSwooshCount = 3;
    public Extensions.Property ringsPerSwoosh;
    int swooshRingCount;

    public Extensions.Property waveInterval;
    public bool loopRainbow = true;

    public IEnumerator LotsOfSwooshes(int numSwooshes)
    {
        if (NewScoreManager.newHighscore) {
            rainbowing = true;
            for (int i = 0; i < numSwooshes; i++)
            {
                float t = (float)i / (float)numSwooshes;
                swooshRingCount = (int)Extensions.GetSmoothStart3Range(ringsPerSwoosh, t);

                StartCoroutine(MoveRings(colors[i]));
                float interval = Extensions.GetSmoothStepRange(swooshInterval, (float) numSwooshes / 9f);
                // Debug.Log((float) numSwooshes / 9f);
                // Debug.Log(interval);
                yield return StartCoroutine(Extensions.Wait(interval));
            }
            rainbowing = false;

            if(loopRainbow) {
                yield return new WaitForSeconds(Extensions.GetSmoothStepRange(swooshInterval, (float) numSwooshes / 9f));
                StartCoroutine(LotsOfSwooshes(numSwooshes));
            }
        } else {
            yield return null;
        }
    }

    public void ExitLoop() {
        loopRainbow = false;
        this.StopAllCoroutines();
    }

    public float swooshDuration = .5f;
    public Extensions.Property swooshInterval;

    IEnumerator MoveRings(Color targetColor)
    {
        float t = 0;
        float d = swooshDuration;

        while (t < d)
        {
            t += Time.fixedDeltaTime;

            float p = t / d;
            totalLength = dots.Count + swooshRingCount;
            int index = Mathf.FloorToInt(EZEasings.SmoothStart3(p) * (float)totalLength);

            // UpdateRings(index, targetColor);
            // if(index < 5) {
                UpdateDots(index, targetColor);
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

    void UpdateDots(int index, Color targetColor) {
        if(index != lastIndex) {
            for(int i = index; i > index - swooshRingCount && i >= 0; i--) {
                if(i < dots.Count) {
                    dots[i].SetColor(targetColor);
                }
            }

            for(int i = index - swooshRingCount; i >= 0; i--) {
                if(i < dots.Count) {
                    dots[i].ReturnToDefaultColor();
                }
            }
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
