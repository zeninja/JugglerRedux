using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleEffectWithBaseRadius : MonoBehaviour
{
    const int NUM_SEGMENTS = 100;

    public float radius;
    public float lineWidth = .1f;
    LineRenderer line;

    public float spreadSpeed;
    float startTime;

    void Awake()
    {
        startTime = Time.time;
        radius = 0;
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.positionCount = (NUM_SEGMENTS + 1);
        line.useWorldSpace = false;
        line.enabled = false;
    }

    void Start()
    {
        CreatePoints();
    }

    void Update()
    {
        CreatePoints();
    }

    public float animationDuration = 1f;
    public float lineWidthPercent  = .01f;

    public void TriggerCircle(float newSpeed, float newWidth, Color newColor, float duration = 1, bool a_useAlpha = false) {
        spreadSpeed = newSpeed;
        lineWidthPercent = newWidth;
        animationDuration = duration;
        m_useAlpha = a_useAlpha;

        StartCoroutine(AnimateCircle());
    }

    IEnumerator AnimateCircle() {

		float t = 0;

		while (t < animationDuration) {
			t += Time.deltaTime;			
            float percent = t / animationDuration;

            AdjustRadiusAndWidth(percent);

            if(m_useAlpha) {
                AdjustAlpha(percent);
            }

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }

    float baseRadius;

    void AdjustRadiusAndWidth(float t) {
        radius = baseRadius + spreadSpeed * EZEasings.SmoothStart2(t);
        lineWidth = radius * lineWidthPercent;
    }

    // void SetColor() {

    // }

    public AnimationCurve alphaOverTime;
    public bool m_useAlpha = true;

    void AdjustAlpha(float t) {
        Color lineColor = GetComponent<LineRenderer>().material.color;
        lineColor.a = alphaOverTime.Evaluate(t);
        GetComponent<LineRenderer>().material.color = lineColor;
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z = -1f;

        float angle = 20f;

        for (int i = 0; i < (NUM_SEGMENTS + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.positionCount = (NUM_SEGMENTS + 1);
            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / NUM_SEGMENTS);
        }

        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.enabled = true;
    }
}