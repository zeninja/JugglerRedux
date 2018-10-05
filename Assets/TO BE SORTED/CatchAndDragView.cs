using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchAndDragView : MonoBehaviour {

	#region instance
	private static CatchAndDragView instance;
	public  static CatchAndDragView GetInstance() {
		return instance;
	}
	#endregion

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			if(this != instance) {
				Destroy(gameObject);
			}
		}
	}

	List<Vector2> catchPositions = new List<Vector2>();
	List<Vector2> throwPositions = new List<Vector2>();

	public CatchRing catchRing;
	public ThrowLine throwLine;

	bool showData;

	public void SetCatchPosition(Vector2 pos) {
		catchPositions.Add(pos);
		SpawnRing(pos);
	}

	public void SetThrowPosition(Vector2 pos) {
		throwPositions.Add(pos);
		// ThrowLine t = Instantiate(throwLine, pos, Quaternion.identity);
		SpawnRing(pos);
	}

	void SpawnRing(Vector2 pos) {
		CatchRing c = Instantiate(catchRing, pos, Quaternion.identity);
		c.instant = true;
	}
}
