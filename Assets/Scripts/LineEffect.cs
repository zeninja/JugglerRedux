using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineEffect : MonoBehaviour
{
    LineRenderer line;
    public float animationDuration = .4f;
    public float lineLength;
    public float lineScalar;


    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.useWorldSpace = false;
    }


    public void PlayLine(float duration)
    {
        animationDuration = duration;
    }

    IEnumerator AnimateLine(Vector2 lineDirection)
    {
        float t = 0;

        while (t < animationDuration)
        {
            t += Time.fixedDeltaTime;

            Vector2 startPos = transform.position;
            Vector2 endPos = (Vector2)transform.position + lineDirection * lineLength;

            line.SetPosition(0, startPos);
            line.SetPosition(1, endPos);
            yield return new WaitForFixedUpdate();
        }
    }
}