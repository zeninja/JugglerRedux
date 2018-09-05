using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverStacker : MonoBehaviour
{
    static GameOverStacker instance;
    public static GameOverStacker GetInstance()
    {
        return instance;
    }

	public int numCircles = 5;
    // public SpriteRenderer dot;
	public StackerDot dot;

    public Extensions.Property circleRadius;

	Extensions.ColorProperty automatedStackColor;

	public float totalDuration = .35f;

	public float startTint = .55f;
	public float endTint   = .55f;


	public Color startColor;

	void Awake() {
		instance = this;
		SetStackColors(startColor);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Space)) {
			// StartCoroutine(SpawnCircles(transform.position));
		}

		SetStackColors(startColor);
	}

	public IEnumerator SpawnCircles(Vector2 startPos, int circleCount = 5) {
		dots = new List<StackerDot>();
		// scaleRanges = new Extensions.Property[circleCount];

		numCircles = circleCount;
		// numCircles = 1;

		float d = totalDuration / numCircles;

		for(int i = 0; i < numCircles; i++) {
			StartCoroutine(SpawnProceduralCircle(startPos, d, i));

			float t = 0;
			while(t < d) {
				t += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
		}
		// Debug.Log(scaleRanges.Length);
	}

    IEnumerator SpawnProceduralCircle(Vector2 startPos, float d, int i)
    {
        float t = 0;

		float evenScalePortion = (float)i     / (float)numCircles;

		float scalePortion  = EZEasings.Linear((float)i     / (float)numCircles);
		float scalePortion2 = EZEasings.Linear((float)(i+1) / (float)numCircles);

	    float scaleDifference = circleRadius.end - circleRadius.start;

		Color dotColor = Color.Lerp(automatedStackColor.start, automatedStackColor.end, evenScalePortion);

		StackerDot s = Instantiate(dot, Vector2.zero, Quaternion.identity);
		s.SetInfo(startPos, dotColor, i);
		dots.Add(s);

		float startRange = .65f;

		Extensions.Property scaleRange = new Extensions.Property();
		scaleRange.start = startRange + scaleDifference * scalePortion;
		scaleRange.end   = startRange + scaleDifference * scalePortion2;

		scaleRanges.Add(scaleRange);

		float startScale = scaleRange.start;

		float stackedCircleDifference = (scaleRange.end - scaleRange.start);
        float targetScale = startScale;

		yield return null;

        while (t < d)
        {
            t += Time.fixedDeltaTime;
            float percent = t / d;
            percent = Mathf.Clamp01(percent);

            targetScale = startScale + stackedCircleDifference * EZEasings.SmoothStart3(percent);
			// Debug.Log(targetScale);

            // s.transform.localScale = targetScale;
			s.SetTargetRadius(targetScale);

            yield return new WaitForFixedUpdate();
        }
    }

	public List<Extensions.Property> scaleRanges;
	// Extensions.Property[] scaleRanges;

	List<StackerDot> dots;

	public float shrinkDuration;

	public IEnumerator ShrinkCircles() {
		float t = 0;
		float d = shrinkDuration / numCircles;

		for(int i = numCircles - 1; i >= 0; i--) {
			// Debug.Log(i);
			t = 0;
			while(t < d) {
				float p = t / d;
				
				float start = scaleRanges[i].end;
				float end   = scaleRanges[i].start;
				float range = start - end;

				float target = end + range * (1 - EZEasings.SmoothStart3(p));

				dots[i].SetTargetRadius(target);

				t += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
			dots[i].transform.localScale = Vector2.zero;
			Destroy(dots[i].gameObject);
		}
	}

	public void SetStackColors(Color startColor) {
		automatedStackColor = new Extensions.ColorProperty();
		automatedStackColor.start = startColor * startTint;
		automatedStackColor.end   = startColor * endTint;
		automatedStackColor.end.a = 1;
	}
}
