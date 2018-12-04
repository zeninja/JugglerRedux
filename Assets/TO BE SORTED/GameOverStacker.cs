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
    public float circleSpawnDuration = .35f;

    Extensions.Property tint;
    public Extensions.Property normalTint;
    public Extensions.Property highScoreTint;

    public Color startColor;

    public List<Extensions.Property> scaleRanges;
    List<StackerDot> dots;

    public bool spawnParticleRings;

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

        // UpdateColors();

        // if (Input.GetKeyDown(KeyCode.R) && manualTrigger)
        // {
        //     ResetDots();
        // }

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

    public bool testing = true;

    public void SpawnCircles(Vector2 startPos)
    {
        dots = new List<StackerDot>();

        // Find outer radius
        // tunnelRadius.start = 0;
        tunnelRadius.end = FindOuterRadius(startPos);

        float d = circleSpawnDuration / numCircles;
        float totalScaleDiff = tunnelRadius.start + tunnelRadius.end - tunnelRadius.start;

        for (int i = 0; i < numCircles; i++)
        {
            Extensions.Property scaleRange = new Extensions.Property();
            if (NewScoreManager.newHighscore || testing)
            {
                scaleRange.start = totalScaleDiff * EZEasings.SmoothStart3((float) i      / (float)numCircles);
                scaleRange.end   = totalScaleDiff * EZEasings.SmoothStart3((float)(i + 1) / (float)numCircles);
            }
            else
            {
                scaleRange.start = totalScaleDiff * EZEasings.Linear((float) i      / (float)numCircles);
                scaleRange.end   = totalScaleDiff * EZEasings.Linear((float)(i + 1) / (float)numCircles);
            }


            scaleRanges.Add(scaleRange);

            SpawnProceduralCircle(startPos, d, scaleRange, i);
        }

        Rainbower.GetInstance().SetDots(dots);
        if( spawnParticleRings ) {
            ParticleRingSpawner.GetInstance().SetInfo(startPos, scaleRanges);
        }
    }

    public float revealDuration;
    public float weight;

    void ResetDots()
    {
        // foreach (StackerDot d in dots)
        // {
        //     Destroy(d.gameObject);
        // }
        // dots.Clear();
    }

    void UpdateColors()
    {
        if (dots == null || Rainbower.rainbowing) { return; }
        if (dots.Count > 0)
        {
            for (int i = 0; i < dots.Count; i++)
            {
                dots[i].SetColor(FindDotColor(i));
            }
        }
    }

    Color FindDotColor(int i)
    {
        if (NewScoreManager.newHighscore || testing)
        {
            float linearPortion = (float)i / (float)numCircles;
            float ease1 = EZEasings.SmoothStart2(linearPortion);
            float ease2 = EZEasings.SmoothStart3(linearPortion);
            float mix = EZEasings.Mix(ease1, ease2, weight, linearPortion);
            return Color.Lerp(automatedStackColor.start, automatedStackColor.end, mix);
        }
        else
        {
            float linearPortion = (float)i / (float)numCircles;
            return Color.Lerp(automatedStackColor.start, automatedStackColor.end, linearPortion);
        }
    }

    void SpawnProceduralCircle(Vector2 startPos, float d, Extensions.Property scaleRange, int i)
    {
        // Spawn the dot
        StackerDot s = Instantiate(dot, Vector2.zero, Quaternion.identity);
        Color dotColor = FindDotColor(i);

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

            for (int i = 0; i < dots.Count; i++)
            {
                if (i <= index)
                {
                    dots[i].gameObject.SetActive(true);
                }
            }

            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    public float hideDuration = .35f;

    public IEnumerator HideCircles()
    {
        float t = revealDuration;
        float d = revealDuration;

        for(int i = dots.Count - 1; i > 0; i--) {
            yield return StartCoroutine(dots[i].HideDot(hideDuration / dots.Count, dots[i - 1].radius));
        }

        yield return StartCoroutine(dots[0].HideDot(hideDuration / dots.Count, 0));


        for (int i = 0; i < dots.Count; i++)
        {
            Destroy(dots[i].gameObject);
        }
    }

    public void SetTint()
    {
        if (!NewScoreManager.newHighscore)
        {
            tint = normalTint;
        }
        else
        {
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

}
