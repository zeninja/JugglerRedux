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
    }

    public AnimationParameter ringDuration;
    public AnimationParameter maskDuration;
    public AnimationParameter baseRadius;
    public AnimationParameter maxRadius;
	public AnimationParameter delay;

    public SpriteCircleEffect effect;

    public Color effectColor;

    public bool useManualPercent = false;
    [Range(0f, 1f)]
    public float manualPercent;
    float percent;
    float percentBasedOnNumCatches;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnRing(Vector2.zero);
        }
    }

    public void SpawnRing(Vector2 position)
    {
        GetPercentage();

        SpriteCircleEffect s = Instantiate(effect, position, Quaternion.identity);
        s.ringDuration = GetValueAtPercentage(ringDuration);
        s.maskDuration = GetValueAtPercentage(maskDuration);
        s.baseRadius   = GetValueAtPercentage(baseRadius);
        s.targetMax    = GetValueAtPercentage(maxRadius);
		s.delay        = GetValueAtPercentage(delay);
        s.ringColor    = effectColor;
        s.TriggerAnimation();
    }

    float GetValueAtPercentage(AnimationParameter p)
    {
        return p.start + (p.end - p.start) * EZEasings.SmoothStart3(percent);
    }

    void GetPercentage() {
        #if UNITY_EDITOR
        if(useManualPercent) {
            percent = manualPercent;
        } else {
            percent = NewScoreManager.GetProgressPercent();
        }
        #else 
        percent = NewScoreManager.GetProgressPercent();
        #endif


    }
}
