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

	public void SetInfo(List<Extensions.Property> scaleRanges) {
		rings = new List<ParticleRing>();

		for(int i = 0; i < scaleRanges.Count; i++) {
			ParticleRing p = Instantiate(prefab);
			p.SetInfo(scaleRanges[i].end, scaleRanges[i].start);
			rings.Add(p);
		}
	}

	public void TriggerRing(int index, Color color) {
		rings[index].TriggerParticles(color);
	}
}
