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


    float percent;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnRing(Vector2.zero);
        }
    }

    public bool useManualPercent;
    [Range(0, 1)]
    public float manualPercent;

    public void SpawnRing(Vector2 position)
    {
        percent = GetPercentage();

        #if UNITY_EDITOR
        if(useManualPercent) {
            percent = manualPercent;
        }
        #endif

        SpriteCircleEffect s = Instantiate(effect, position, Quaternion.identity);
        s.ringDuration = GetSmoothStepRange(ringDuration);
        s.maskDuration = GetSmoothStepRange(maskDuration);
        s.baseRadius   = GetSmoothStepRange(baseRadius);
        s.targetMax    = GetSmoothStepRange(maxRadius);
		s.delay        = GetSmoothStepRange(delay);
        s.ringColor    = effectColor;
        s.TriggerAnimation();
    }

    float GetSmoothStepRange(AnimationParameter p)
    {
        return p.start + (p.end - p.start) * EZEasings.SmoothStep3(percent);
    }

    float GetPercentage() {
        return NewScoreManager.GetProgressPercent();
    }
}
