using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLines : MonoBehaviour {

	private static WarningLines instance;
	public  static WarningLines GetInstance() {
		return instance;
	}

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			if(this != instance) {
				Destroy(gameObject);
			}
		}
	}

	[System.NonSerialized] public SpriteMask  warningMask;

	public WarningLine warningLine;
	public Color lineColor;
	public int numLines = 9;
	public float lineWidth;
	public float rotation = 200;
	public float scrollSpeed = 50;
	public float width = 15;
	public int   scrollDirection = -1;
	public float maxMaskScale = 25f;

	List<WarningLine> lines;

	// Use this for initialization
	void Start () {
		lines = new List<WarningLine>();
		SpawnLines();
	}
	
	// Update is called once per frame
	void Update () {
		AdjustLines();

		CheckMaskPosition();
	}

	void SpawnLines() {
		for(int i = 0; i < numLines; i++) {
			WarningLine w = Instantiate(warningLine);
			w.offset = (float)i / (float)numLines;
			w.transform.parent = transform;
			lines.Add(w);
		}
		transform.rotation = Quaternion.Euler(0, 0, rotation);
	}

	void AdjustLines() {
		foreach(WarningLine w in lines) {
			w.scrollSpeed = scrollSpeed;
			w.width = width;
			w.scrollDirection = scrollDirection;
			w.lineWidth = lineWidth;
			w.GetComponent<LineRenderer>().material.color = lineColor;
		}
	}

	public void SetWarningMask(SpriteMask mask) {
		warningMask = mask;
	}

	float t = 0;

	public float maskGrowRate = 1;
	public float maskShrinkRate = .75f;

	public void CheckMaskPosition() {
		if(warningMask == null) { return; }

		Vector2 edge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

		float x = edge.x;
		float y = edge.y;

		if (warningMask.transform.position.x > x || warningMask.transform.position.x < -x ||
		    warningMask.transform.position.y > y || warningMask.transform.position.y < -y) {
			t += maskGrowRate;
		} else {
			t -= maskShrinkRate;
		}

		Vector3 warningPos = warningMask.transform.position;
		float wx = warningPos.x;
		float wy = warningPos.y;
		warningMask.transform.position = new Vector3(Mathf.Clamp(wx, -x, x), Mathf.Clamp(wy, -y, y), 0);

		t = Mathf.Clamp01(t);

		UpMask(t);
	}

	public void UpMask(float t) {
		warningMask.transform.localScale = Vector2.one * EZEasings.SmoothStop3(t) * maxMaskScale;
	}

	public void DownMask(float t) {
		warningMask.transform.localScale = Vector2.one * EZEasings.SmoothStart3(t);
	}
}
