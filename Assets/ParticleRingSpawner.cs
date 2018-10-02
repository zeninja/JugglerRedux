using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRingSpawner : MonoBehaviour {

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
    
    static ParticleRingSpawner instance;
    public static ParticleRingSpawner GetInstance()
    {
        return instance;
    }

	List<ParticleRing> rings;

	public ParticleRing prefab;



	public void SetInfo(Vector2 startPos, List<Extensions.Property> scaleRanges) {
		rings = new List<ParticleRing>();

		for(int i = 0; i < scaleRanges.Count; i++) {
			ParticleRing p = Instantiate(prefab);
            p.transform.position = startPos;
			p.SetInfo(scaleRanges[i].end, scaleRanges[i].start);
            float t = (float) i / scaleRanges.Count;
            p.SetParticleCount(t);
            p.SetParticleSize(t);
            p.SetParticleLifetime(t);
			rings.Add(p);
		}
	}

	public void TriggerRing(int index, Color color) {
        rings[index].useRingColor = useRingColor;
		rings[index].TriggerParticles(color);
	}

    // public void KillRings() {
    //     foreach(ParticleRing p in rings) {
    //         Destroy(p.gameObject);
    //     }
    // }

    public bool useRingColor;
    public Extensions.Property maxParticleCount;
    public Extensions.Property particleSize;
    public Extensions.Property lifetime;
}
