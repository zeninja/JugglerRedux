using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverStacker : MonoBehaviour
{

	// public int numCircles = 5;
    public SpriteRenderer dot;
    public Extensions.Property circleRadius;
    public Extensions.ColorProperty stackColor;

	Extensions.ColorProperty automatedStackColor;

	public float totalDuration = .35f;

	public float tint = .55f;

    void Start()
    {
        // circleRadius.start = NewBallManager.GetInstance().ballScale;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // TriggerStack();
        }
	}

	// public void TriggerStack(int circleCount = 10) {
	// 	numCircles = circleCount;
	// 	StartCoroutine(SpawnCircles());
	// }

	// IEnumerator SpawnCircles() {
	// 	float d = totalDuration / numCircles;

	// 	for(int i = 0; i < numCircles; i++) {
	// 		StartCoroutine(SpawnCircle(d, i));

	// 		yield return new WaitForSeconds(d);
	// 	}
	// }

	public IEnumerator SpawnCircles(int numCircles) {
		float d = totalDuration / numCircles;

		for(int i = 0; i < numCircles; i++) {
			StartCoroutine(SpawnCircle(d, i, numCircles));

			yield return new WaitForSeconds(d);
		}

	}

    IEnumerator SpawnCircle(float d, int i, int numCircles)
    {
        float t = 0;

		float evenScalePortion = (float)i     / (float)numCircles;

		float scalePortion  = EZEasings.Linear((float)i     / (float)numCircles);
		float scalePortion2 = EZEasings.Linear((float)(i+1) / (float)numCircles);

	    float scaleDifference = circleRadius.end - circleRadius.start;

        SpriteRenderer s = Instantiate(dot, transform.position, Quaternion.identity);
        s.color = Color.Lerp(automatedStackColor.start, automatedStackColor.end, evenScalePortion);
		s.sortingOrder = -1 - i;

		// Debug.Log(scalePortion);
		// Debug.Log(scalePortion2);

		float startRange = .65f;

		Extensions.Property scaleRange = new Extensions.Property();
		scaleRange.start = startRange + scaleDifference * scalePortion;
		scaleRange.end   = startRange + scaleDifference * scalePortion2;

		// Debug.Log(scaleRange.start);
		// Debug.Log(scaleRange.end);

		Vector2 startScale = Vector2.one * scaleRange.start;

		Vector2 stackedCircleDifference = Vector2.one * (scaleRange.end - scaleRange.start);
        Vector2 targetScale = startScale;


		yield return null;

        while (t < d)
        {
            t += Time.fixedDeltaTime;
            float percent = t / d;
            percent = Mathf.Clamp01(percent);

            targetScale = startScale + stackedCircleDifference * EZEasings.SmoothStart3(percent);
			// Debug.Log(targetScale);

            s.transform.localScale = targetScale;

            yield return new WaitForFixedUpdate();
        }
    }

	public void SetStackColors(Color startColor) {
		automatedStackColor = new Extensions.ColorProperty();
		automatedStackColor.start = startColor;
		automatedStackColor.end   = startColor * tint;
		automatedStackColor.end.a = 1;
	}
}
