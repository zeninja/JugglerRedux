using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineEffect : MonoBehaviour
{
    LineRenderer line;
    public float animationDuration = .4f;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
    }


    public void PlayLine(float duration)
    {
        animationDuration = duration;
    }

    IEnumerator AnimateLine()
    {
        float t = 0;

        while (t < animationDuration)
        {
            t += Time.fixedDeltaTime;
            // line.stuff
            yield return new WaitForFixedUpdate();
        }
    }
}