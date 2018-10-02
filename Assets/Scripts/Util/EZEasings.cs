using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EZEasings : MonoBehaviour
{

    // float RangeMap(float in, float inStart, float inEnd, float outStart, float outEnd) {

    // }

    public static float Linear(float t)
    {
        return t;
    }

    // Smooth START
    public static float SmoothStart2(float t)
    {               //
        return t * t;                                           //
    }

    public static float SmoothStart3(float t)
    {
        return t * t * t;
    }

    public static float SmoothStart4(float t)
    {
        return t * t * t * t;
    }

    public static float SmoothStart5(float t)
    {
        return t * t * t * t * t;
    }

    // Smooth STOP
    // "Flip it, Square it, Flip it again"
    public static float SmoothStop2(float t)
    {
        return 1 - ((1 - t) * (1 - t));
    }

    public static float SmoothStop3(float t)
    {
        return 1 - ((1 - t) * (1 - t) * (1 - t));
    }

    public static float SmoothStop4(float t)
    {
        return 1 - ((1 - t) * (1 - t) * (1 - t) * (1 - t));
    }

    public static float SmoothStop5(float t)
    {
        return 1 - ((1 - t) * (1 - t) * (1 - t) * (1 - t) * (1 - t));
    }

    // Smooth STEP
    public static float SmoothStep2(float t)
    {
        return Mix(SmoothStart2(t), SmoothStop2(t), t, t);
    }

    public static float SmoothStep3(float t)
    {
        return Mix(SmoothStart3(t), SmoothStop3(t), t, t);
    }

    public static float SmoothStep4(float t)
    {
        return Mix(SmoothStart4(t), SmoothStop4(t), t, t);
    }

    public static float SmoothStep5(float t)
    {
        return Mix(SmoothStart5(t), SmoothStop5(t), t, t);
    }

    // Arch
    public static float Arch2(float t)
    {
        return Scale(Flip(t), t);
    }

    // public static float Arch3(float t)
    // {
    //     return Scale(Flip(t), t);
    // }

    // public static float Arch4(float t)
    // {
    //     return Scale(Flip(t), t);
    // }

    // public static float Arch5(float t)
    // {
    //     return Scale(Flip(t), t);
    // }


    public static float SmoothStartArch2(float t)
    {
        return Scale(Arch2(t), t);
    }

    public static float SmoothStopArch2(float t)
    {
        return ReverseScale(Arch2(t), t);
    }

    public static float SmoothStepArch2(float t)
    {
        return ReverseScale(Scale(Arch2(t), t), t);
    }

	// public static float SmoothStartArch3(float t)
    // {
    //     return Scale(Arch3(t), t);
    // }

    // public static float SmoothStopArch3(float t)
    // {
    //     return ReverseScale(Arch3(t), t);
    // }

    // public static float SmoothStepArch3(float t)
    // {
    //     return ReverseScale(Scale(Arch3(t), t), t);
    // }

	//     public static float SmoothStartArch4(float t)
    // {
    //     return Scale(Arch4(t), t);
    // }

    // public static float SmoothStopArch4(float t)
    // {
    //     return ReverseScale(Arch4(t), t);
    // }

    // public static float SmoothStepArch4(float t)
    // {
    //     return ReverseScale(Scale(Arch4(t), t), t);
    // }

	// public static float BellCurve4(float t)
    // {
    //     return SmoothStop2(t) * SmoothStart2(t);
    // }

    public static float BellCurve6(float t)
    {
        return SmoothStop3(t) * SmoothStart3(t);
    }

	// public static float BellCurve8(float t)
    // {
    //     return SmoothStop4(t) * SmoothStart4(t);
    // }

	// public static float BellCurve10(float t)
    // {
    //     return SmoothStop5(t) * SmoothStart5(t);
    // }

    // Techniques
    public static float Square(float t)
    {
        return t * t;
    }

    public static float Flip(float t)
    {
        return 1 - t;
    }

    public static float Mix(float a, float b, float weightB, float t)
    {
        return (1 - weightB) * a + (weightB) * b;
    }

    public static float Scale(float function, float x)
    {
        return function * x;
    }

    public static float ReverseScale(float function, float x)
    {
        return (1 - x) * function * x;
    }

    // Util
    float BounceClampBottom(float t)
    {
        return Mathf.Abs(t);
    }

    float BounceClampTop(float t)
    {
        return 1.0f - Mathf.Abs(1.0f - t);
    }

    float BounceClampBottomTop(float t)
    {
        return BounceClampTop(BounceClampBottom(t));
    }

    public float NormalizedBezierCurve3(float b, float c, float t)
    {
        float s = 1.0f - t;
        float t2 = t * t;
        float s2 = s * s;
        float t3 = t2 * t;
        return (3 * b * s2 * t) + (3 * c * s * t2) + t3;
    }
}
