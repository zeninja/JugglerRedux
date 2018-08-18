using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCircleEffect : MonoBehaviour
{

    public SpriteRenderer foreground, background;
    public float ringDuration;
    public float maskDuration;

    public float baseRadius;
    public float targetMax;

    public float delay;

    // [Range(0f, 1f)]
    // public float percent = .05f;

	public Color ringColor;

    void Start()
    {
        foreground.sortingLayerName = "Effects";

        foreground.transform.localScale = Vector3.one * baseRadius;
        background.transform.localScale = Vector3.one * baseRadius;

        foreground.color = ringColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            TriggerAnimation();
        }
    }

    public void TriggerAnimation()
    {
        StartCoroutine(RingEffect());
    }

    IEnumerator RingEffect()
    {
        StartCoroutine(AnimateRing());
        yield return new WaitForSeconds(delay);
        StartCoroutine(AnimateMask());
        yield return null;
    }

    IEnumerator AnimateRing()
    {
        SpriteRenderer target = foreground;
        float time = 0;

        float targetRadius = targetMax - baseRadius;

        while (time < ringDuration)
        {
            time += Time.fixedDeltaTime;

            float t = time / ringDuration;
            float radius = 0;
            radius = baseRadius + targetRadius * EZEasings.SmoothStart3(t);
            target.transform.localScale = Vector3.one * radius;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator AnimateMask()
    {
        SpriteRenderer target = background;
        float time = 0;

        float targetRadius = targetMax - baseRadius;

        while (time < maskDuration)
        {
            time += Time.fixedDeltaTime;

            float t = time / maskDuration;
            float radius = 0;
            radius = baseRadius + targetRadius * EZEasings.SmoothStart3(t) * 1.01f;
            target.transform.localScale = Vector3.one * radius;
            yield return new WaitForFixedUpdate();
        }
		Destroy(gameObject);
    }


}
