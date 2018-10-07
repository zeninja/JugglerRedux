﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchRingSpawner : MonoBehaviour {

	// Catch rings are spawned separately from hands because hands destroy themselves instantly on finger up
	public CatchRing prefab;

	public void SpawnRing(Color ballColor) {
		CatchRing c = Instantiate(prefab);
		c.transform.position = transform.position;

		c.TriggerRing(ballColor);
	}
}