using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRing : MonoBehaviour {

	ParticleSystem p;

	// Use this for initialization
	void Awake () {
		p = GetComponent<ParticleSystem>();
	}

	public void SetInfo(float outerRadius, float innerRadius) {
		ParticleSystem.ShapeModule shape = GetComponent<ParticleSystem>().shape;
		shape.radius = outerRadius;

		float diff = outerRadius - innerRadius;
		float perc = diff / outerRadius;
		shape.radiusThickness = perc;
	}

	public bool useRingColor;

	public void TriggerParticles(Color newColor) {
		if(useRingColor) {
			ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
			main.startColor = newColor;
		}
		p.Play();
	}
}
