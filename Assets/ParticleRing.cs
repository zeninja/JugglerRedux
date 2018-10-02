using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleRing : MonoBehaviour {

	ParticleSystem p;

	// Use this for initialization
	void Awake () {
		p = GetComponent<ParticleSystem>();
	}

	void Start() {
		EventManager.StartListening("KillParticles", HandleDeath);
	}

	public float m_innerRadius;
	public float m_outerRadius;

	public float particleMultiplier;

	public int numMaxParticles = 12000;

	public void SetInfo(float outerRadius, float innerRadius) {
		ParticleSystem.ShapeModule shape = GetComponent<ParticleSystem>().shape;
		m_outerRadius = outerRadius;
		m_innerRadius = innerRadius;

		shape.radius = m_outerRadius / 2;

		float diff = m_outerRadius - m_innerRadius;
		float perc = diff / m_outerRadius;
		shape.radiusThickness = perc;
	}

	public void SetParticleCount(float p) {
		ParticleSystem.Burst b = GetComponent<ParticleSystem>().emission.GetBurst(0);
		b.count =  Extensions.GetSmoothStart3Range(ParticleRingSpawner.GetInstance().maxParticleCount, p);
	}

	public void SetParticleSize(float p) {
		ParticleSystem.MainModule m = GetComponent<ParticleSystem>().main;
		m.startSizeMultiplier = Extensions.GetSmoothStart3Range(ParticleRingSpawner.GetInstance().particleSize, p);
	}

	public void SetParticleLifetime(float p) {
		ParticleSystem.MainModule m = GetComponent<ParticleSystem>().main;
		m.startLifetime = Extensions.GetSmoothStop3Range(ParticleRingSpawner.GetInstance().lifetime, p);
	}

	[System.NonSerialized] public bool useRingColor;

	public void TriggerParticles(Color newColor) {
		if(useRingColor) {
			ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
			main.startColor = newColor;
		}
		p.Play();
	}

	void HandleDeath() {
		Invoke("Die", 3);
	}

	void Die() {
		Destroy(gameObject);
	}
}
