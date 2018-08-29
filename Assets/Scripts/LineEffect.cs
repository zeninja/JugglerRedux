using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineEffect : MonoBehaviour
{
    LineRenderer line;
    public float animationDuration = .4f;

    public Vector2 startPos, endPos;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        // line.useWorldSpace = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayLine(startPos, endPos, animationDuration);
        }
    }


    public void PlayLine(Vector2 startPos, Vector2 endPos, float duration)
    {
        animationDuration = duration;
        StartCoroutine(AnimateLine());
    }
    Vector2 lineStart;
    Vector2 lineEnd;

    public float linePercentage = 0.95f;

    IEnumerator AnimateLine()
    {
        lineStart = startPos;
        lineEnd = startPos;

        StartCoroutine(AnimatePoint(lineStart, 0));
        while(animationCompletion < linePercentage) {
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(AnimatePoint(lineEnd, 1));


    }

    bool animatingStart = false;

    IEnumerator AnimatePoint(Vector2 targetPos, int positionIndex)
    {

        float t = 0;
        float percent = t;

        animatingStart = true;

        while (t < animationDuration)
        {
            t += Time.fixedDeltaTime;
            percent = t / animationDuration;
            percent = Mathf.Clamp01(percent);

            targetPos = startPos + (endPos - startPos) * EZEasings.SmoothStart3(percent);
            line.SetPosition(positionIndex, targetPos);

            animationCompletion = percent;
            yield return new WaitForFixedUpdate();
        }
    }

    float animationCompletion;
}