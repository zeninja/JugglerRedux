using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineEffect : MonoBehaviour
{
    [System.NonSerialized]
    public LineExplosion lineExplosion;

    LineRenderer line;
    public float duration = .4f;
    public float hangDuration;

    public float outerWidth;
    public float innerWidth;

    public Vector2 startPos, endPos;

    float animationCompletion;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayLine(startPos, endPos, duration, innerWidth, outerWidth);
        }
    }


    public void PlayLine(Vector2 a_startPos, Vector2 a_endPos, float a_duration, float a_innerWidth, float a_outerWidth, float a_hangDuration = 0)
    {
        startPos = a_startPos;
        endPos   = a_endPos;
        duration = a_duration;
        innerWidth = a_innerWidth;
        outerWidth = a_outerWidth;
        hangDuration = a_hangDuration;
        StartCoroutine(AnimateLine());
    }
    Vector2 lineStart;
    Vector2 lineEnd;

    public float linePercentage = 0.95f;

    IEnumerator AnimateLine()
    {
        lineStart = startPos;
        lineEnd = startPos;

        line.SetPosition(0, startPos);
        line.SetPosition(1, startPos);

        StartCoroutine(AnimatePoint(lineStart, 0));
        while(animationCompletion < linePercentage) {
            line.startWidth = innerWidth + (outerWidth - innerWidth) * EZEasings.SmoothStart3(animationCompletion);
            line.endWidth   = innerWidth;
            yield return new WaitForFixedUpdate();
        }

        float t = 0;
        while (t < hangDuration) {
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(AnimatePoint(lineEnd, 1));
        while(animationCompletion < linePercentage) {
            yield return new WaitForFixedUpdate();
        }

        LineExplosionManager.explosionHappening = false;
        Destroy(gameObject);
    }

    bool animatingStart = false;

    IEnumerator AnimatePoint(Vector2 targetPos, int positionIndex)
    {

        float t = 0;
        float percent = t;

        animatingStart = true;

        while (t < duration)
        {
            t += Time.fixedDeltaTime;
            percent = t / duration;
            percent = Mathf.Clamp01(percent);

            targetPos = startPos + (endPos - startPos) * EZEasings.SmoothStart3(percent);
            line.SetPosition(positionIndex, targetPos);

            animationCompletion = percent;
            yield return new WaitForFixedUpdate();
        }
    }

    public void SetColor(Color newColor) {
        line.material.color = newColor;
    }
}