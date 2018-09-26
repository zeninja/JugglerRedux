using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverStacker : MonoBehaviour
{
    #region
    static GameOverStacker instance;
    public static GameOverStacker GetInstance()
    {
        return instance;
    }
    #endregion

    public int numCircles = 5;
    public StackerDot dot;
    public Extensions.Property tunnelRadius;
    Extensions.ColorProperty automatedStackColor;
    public float totalDuration = .35f;

    Extensions.Property tint;
    public Extensions.Property normalTint;
    public Extensions.Property highScoreTint;

    public Color startColor;

    public List<Extensions.Property> scaleRanges;
    List<StackerDot> dots;

    void Awake()
    {
        instance = this;
        tint = normalTint;
        SetStackColors(startColor);
    }

    public bool manualTrigger = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && manualTrigger)
        {
            SpawnCircles(transform.position);
            StartCoroutine(RevealCircles());
        }
    }

    public void SetGameOverDotCount()
    {
        if (NewScoreManager.newHighscore)
        {
            numCircles = 20;
        }
        else
        {
            numCircles = 5;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(ScreenInfo.world_TL, transform.position);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(ScreenInfo.world_TR, transform.position);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(ScreenInfo.world_BL, transform.position);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(ScreenInfo.world_BR, transform.position);
    }

    public void SpawnCircles(Vector2 startPos)
    {
        dots = new List<StackerDot>();

        // Find outer radius
        // tunnelRadius.start = 0;
        tunnelRadius.end = FindOuterRadius(startPos);

        float d = totalDuration / numCircles;
        float totalScaleDiff = tunnelRadius.start + tunnelRadius.end - tunnelRadius.start;

        for (int i = 0; i < numCircles; i++)
        {
            Extensions.Property scaleRange = new Extensions.Property();
            if(NewScoreManager.newHighscore) {
                scaleRange.start = totalScaleDiff * EZEasings.SmoothStart3((float)i / (float)numCircles);
                scaleRange.end   = totalScaleDiff * EZEasings.SmoothStart3((float)(i + 1) / (float)numCircles);
            } else {
                scaleRange.start = totalScaleDiff * EZEasings.Linear((float)i / (float)numCircles);
                scaleRange.end   = totalScaleDiff * EZEasings.Linear((float)(i + 1) / (float)numCircles);
            }


            scaleRanges.Add(scaleRange);

            SpawnProceduralCircle(startPos, d, scaleRange, i);
        }

        Rainbower.GetInstance().SetDots(dots);
        // ParticleRingSpawner.GetInstance().SetInfo(scaleRanges);
    }

    public float revealDuration;


    void SpawnProceduralCircle(Vector2 startPos, float d, Extensions.Property scaleRange, int i)
    {
        float linearPortion = (float)i / (float)numCircles;

        // Spawn the dot
        StackerDot s = Instantiate(dot, Vector2.zero, Quaternion.identity);
        Color dotColor = Color.Lerp(automatedStackColor.start, automatedStackColor.end, linearPortion);

        s.SetInfo(startPos, dotColor, i);
        s.gameObject.name = i.ToString();
        s.gameObject.SetActive(false);
        dots.Add(s);

        s.SetTargetRadius(scaleRange.end);
    }

    IEnumerator RevealCircles()
    {
        float t = 0;
        float d = revealDuration;

        while (t < d)
        {
            float p = t / d;
            int index = Mathf.CeilToInt((float)numCircles * EZEasings.SmoothStart2(p));
            index = Mathf.Min(index, numCircles - 1);

            for(int i = 0; i < dots.Count; i++) {
                if(i <= index) {
                    dots[i].gameObject.SetActive(true);
                }
            }

            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    public IEnumerator HideCircles()
    {
        float t = revealDuration;
        float d = revealDuration;

        while (t >= 0)
        {
            float p = t / d;
            int index = Mathf.CeilToInt((float)numCircles * EZEasings.SmoothStart2(p));
            index = Mathf.Clamp(index, 0, numCircles - 1);

            for(int i = 0; i < dots.Count; i++) {
                if(i >= index) {
                    dots[i].gameObject.SetActive(false);
                }
            }

            t -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        dots[0].gameObject.SetActive(false);
        

        for(int i = 0; i < dots.Count; i++) {
            Destroy(dots[i].gameObject);
        }
    }

    public void SetTint() {
        if(!NewScoreManager.newHighscore) {
            tint = normalTint;
        } else {
            tint = highScoreTint;
        }
    }

    public void SetStackColors(Color startColor)
    {
        automatedStackColor = new Extensions.ColorProperty();
        automatedStackColor.start = startColor * tint.start;
        automatedStackColor.end = startColor * tint.end;
        automatedStackColor.start.a = 1;
        automatedStackColor.end.a = 1;
    }

    float FindOuterRadius(Vector2 anchorPos)
    {
        float[] distanceToCorner = new float[4];
        distanceToCorner[0] = Vector3.Distance(ScreenInfo.world_TL, anchorPos);
        distanceToCorner[1] = Vector3.Distance(ScreenInfo.world_TR, anchorPos);
        distanceToCorner[2] = Vector3.Distance(ScreenInfo.world_BL, anchorPos);
        distanceToCorner[3] = Vector3.Distance(ScreenInfo.world_BR, anchorPos);

        float max = Mathf.Max(distanceToCorner) * 2;
        return max;
    }

    public IEnumerator HandleGameOver(Vector2 pos, Color ballColor)
    {
        SetTint();
        SetStackColors(ballColor);
        SetGameOverDotCount();
        SpawnCircles(pos);
        yield return StartCoroutine(RevealCircles());
        yield return null;
    }

    // IEnumerator ScaleCircleIn(StackerDot s, float d, Extensions.Property scaleRange)
    // {
    //     float startScale = scaleRange.start;
    //     float scaleDiff = (scaleRange.end - scaleRange.start);
    //     float targetScale = startScale;

    //     float t = 0;
    //     while (t < d)
    //     {
    //         t += Time.fixedDeltaTime;
    //         float percent = t / d;
    //         percent = Mathf.Clamp01(percent);

    //         targetScale = startScale + scaleDiff * EZEasings.SmoothStart3(percent);
    //         s.SetTargetRadius(targetScale);

    //         yield return new WaitForFixedUpdate();
    //     }

    //     yield return new WaitForEndOfFrame();
    // }
}
