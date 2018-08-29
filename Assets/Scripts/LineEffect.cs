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

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            PlayLine(startPos, endPos, animationDuration);
        }
    }


    public void PlayLine(Vector2 startPos, Vector2 endPos, float duration)
    {
        animationDuration = duration;
        StartCoroutine(AnimateLine());
    }

    IEnumerator AnimateLine()
    {
        float t = 0;
        float percent = t;

        Vector2 lineStart = startPos;
        Vector2 lineEnd   = startPos;

        while (t < animationDuration)
        {
            t += Time.fixedDeltaTime;
            percent = t / animationDuration;

            lineStart = startPos + (endPos - startPos) * EZEasings.SmoothStart3(percent);
            lineEnd   = startPos + (endPos - startPos) * EZEasings.SmoothStart5(percent);

            line.SetPosition(0, lineStart);
            line.SetPosition(1, lineEnd);
            yield return new WaitForFixedUpdate();
        }
    }
}