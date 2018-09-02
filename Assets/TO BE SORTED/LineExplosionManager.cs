using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineExplosionManager : MonoBehaviour {

    static LineExplosionManager instance;
    public static LineExplosionManager GetInstance()
    {
        return instance;
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

	public LineExplosion lineExplosion;

	public static bool explosionHappening;

    public bool spawnLines = false;

	public IEnumerator SpawnExplosion(Vector2 spawnPosition) {
		// Debug.Log("Spawning exploision");
		LineExplosion l = Instantiate(lineExplosion);
		
        if(spawnLines) {
            l.SpawnLines(spawnPosition);
        }

		float t = 0;
		float explosionDuration = l.duration + l.hangDuration;
		
		while (t < explosionDuration) {
			t += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}
		Destroy(l.gameObject);
	}
}
