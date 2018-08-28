using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrator : MonoBehaviour
{

    public static bool vibeEnabled;

    void Start()
    {
        // EventManager.StartListening("BallCaught", HandleBallCaught);
        InitRumble();
    }

    public static void Vibrate()
    {
        Handheld.Vibrate();
    }

    void HandleBallCaught()
    {
        StartCoroutine(Vibrate(.05f));
    }

    public static IEnumerator Vibrate(float d)
    {
        float t = 0;
        while (t < d)
        {
            t += Time.fixedDeltaTime;
            Handheld.Vibrate();
            yield return new WaitForFixedUpdate();
        }
    }

    public void ToggleRumble()
    {
        vibeEnabled = !vibeEnabled;
    }

    public static bool rumbleOn;
    string rumbleKey = "rumble";

    void InitRumble()
    {
        if (!PlayerPrefs.HasKey(rumbleKey))
        {
            PlayerPrefs.SetInt(rumbleKey, 0);
        }
        else
        {
            rumbleOn = PlayerPrefs.GetInt(rumbleKey) == 1;
        }
        vibeEnabled = rumbleOn;
    }
}
