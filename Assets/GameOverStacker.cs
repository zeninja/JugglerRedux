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
	public StackerDot dot;
    public Extensions.Property circleRadius;
	Extensions.ColorProperty automatedStackColor;
	public float totalDuration = .35f;
	public float startTint = .55f;
	public float endTint   = .55f;

	public Color startColor;

	public List<Extensions.Property> scaleRanges;
	List<StackerDot> dots;
	public float shrinkDuration;

	void Awake() {
		instance = this;
		SetStackColors(startColor);
	}

	void Update() {
		SetCircleRadius();

		if(Input.GetKeyDown(KeyCode.Space)) {
			StartCoroutine(SpawnCircles(transform.position, numCircles));
		}
	}

	void SetCircleRadius() {
		circleRadius.start = 1; //NewBallManager.GetInstance().ballScale;
		circleRadius.end   = FindOuterRadius();
	}

	public void SetGameOverDotCount() {
		if(NewScoreManager.newHighscore) {
			numCircles = 20;
			// numCircles = NewScoreManager._numBalls * NewScoreManager._numBalls;
			// Debug.Log("squared circle: " + numCircles);
		} else {
			numCircles = 5;
			// numCircles = NewScoreManager._numBalls;
		}
	}

	public float[] floats;

	float FindOuterRadius() {
		floats = new float[4];
		floats[0] = Vector3.Distance(ScreenInfo.world_TL, (Vector2)transform.position);
		floats[1] = Vector3.Distance(ScreenInfo.world_TR, (Vector2)transform.position);
		floats[2] = Vector3.Distance(ScreenInfo.world_BL, (Vector2)transform.position);
		floats[3] = Vector3.Distance(ScreenInfo.world_BR, (Vector2)transform.position);
		
		float max = Mathf.Max(floats) * 2;

		Debug.Log((ScreenInfo.world_TL - ScreenInfo.world_BR).magnitude);

		// float test = ScreenInfo.pixel_TL - Camera.main.WorldToScreenPoint(transform.position);

		Debug.Log(max);
		return max;
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawLine(ScreenInfo.world_TL, transform.position);
		
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(ScreenInfo.world_TR, transform.position);

		Gizmos.color = Color.green;
		Gizmos.DrawLine(ScreenInfo.world_BL, transform.position);

		Gizmos.color = Color.white;
		Gizmos.DrawLine(ScreenInfo.world_BR, transform.position);
	}

	public IEnumerator SpawnCircles(Vector2 startPos, int circleCount) {
		dots = new List<StackerDot>();
		numCircles = circleCount;

		float d = totalDuration / numCircles;

		for(int i = 0; i < numCircles; i++) {
			StartCoroutine(SpawnProceduralCircle(startPos, d, i));

			float t = 0;
			while(t < d) {
				t += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
		}
		
		if (GetComponent<Rainbower>() != null) {
			GetComponent<Rainbower>().SetDots(dots);
		}
	}

	public IEnumerator SpawnCircles(Vector2 startPos) {
		dots = new List<StackerDot>();

		float d = totalDuration / numCircles;

		for(int i = 0; i < numCircles; i++) {
			StartCoroutine(SpawnProceduralCircle(startPos, d, i));

			float t = 0;
			while(t < d) {
				t += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
		}
		
		if (GetComponent<Rainbower>() != null) {
			GetComponent<Rainbower>().SetDots(dots);
		}
	}

    IEnumerator SpawnProceduralCircle(Vector2 startPos, float d, int i)
    {
        float t = 0;

		float evenScalePortion = (float)i     / (float)numCircles;

		float scalePortion  = EZEasings.Linear((float)i     / (float)numCircles);
		float scalePortion2 = EZEasings.Linear((float)(i+1) / (float)numCircles);

		Debug.Log(scalePortion2);

		// float adjustedEnd   = Extensions.ScreenToWorld()

	    float scaleDifference = circleRadius.end - circleRadius.start;

		Color dotColor = Color.Lerp(automatedStackColor.start, automatedStackColor.end, evenScalePortion);

		StackerDot s = Instantiate(dot, Vector2.zero, Quaternion.identity);
		s.SetInfo(startPos, dotColor, i);
		dots.Add(s);

		float startRange = 1; //NewBallManager.GetInstance().ballScale;

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
		automatedStackColor.start.a = 1;
		automatedStackColor.end.a   = 1;
	}
}
