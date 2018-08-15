using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCircleEffectSpawner : MonoBehaviour
{

	[System.Serializable]
    public class AnimationParameter
    {
        public float start;
        public float end;
		// public AnimationCurve curve;
    }

    public AnimationParameter ringDuration;
    public AnimationParameter maskDuration;
    public AnimationParameter baseRadius;
    public AnimationParameter maxRadius;
	public AnimationParameter delay;

    public SpriteCircleEffect effect;
    [Range(0f, 1f)]
    public float percent;

    // Use this for initialization
    void Start()
    {
        // EventManager.StartListening("BallCaught", SpawnRing);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnRing(Vector2.zero);
        }
    }

    void SpawnRing(Vector2 position)
    {
        SpriteCircleEffect s = Instantiate(effect, position, Quaternion.identity);
        s.ringDuration = GetValueAtPercentage(ringDuration);
        s.maskDuration = GetValueAtPercentage(maskDuration);
        s.baseRadius   = GetValueAtPercentage(baseRadius);
        s.targetMax    = GetValueAtPercentage(maxRadius);
		s.delay        = GetValueAtPercentage(delay);
        s.TriggerAnimation();

		Debug.Log(s.baseRadius);
    }

    float GetValueAtPercentage(AnimationParameter p)
    {
		if(p == baseRadius) {
			// Debug.Log(EZEasings.SmoothStart3(percent));
			return p.start + (p.end - p.start) * EZEasings.SmoothStop3(percent);
		}
        return p.start + (p.end - p.start) * EZEasings.SmoothStart3(percent);
    }
}
