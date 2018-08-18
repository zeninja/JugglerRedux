using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    float m_TargetTimeScale;
    float m_CurrentTimeScale;
    public float m_SlowTimeScale = .5f;
    float m_NormalTimeScale = 1.0f;

    public float m_TimeSmoothing = 10f;

    static bool timeSlowing;

    public static float timeScalePercent; 

    #region instance
    private static TimeManager instance;


    public static TimeManager GetInstance()
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
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        SlowTimeBasedOnThrows();
    }

    void SlowTimeBasedOnThrows()
    {
        if (NewBallManager.GetInstance().JuggleThresholdReached() || Input.GetKey(KeyCode.LeftShift))
        {
            timeSlowing = true;
            m_TargetTimeScale = m_SlowTimeScale;
        }
        else
        {
            timeSlowing = false;
            m_TargetTimeScale = m_NormalTimeScale;
        }

        

        Time.timeScale = Mathf.Lerp(Time.timeScale, m_TargetTimeScale, Time.deltaTime * m_TimeSmoothing);
        m_CurrentTimeScale = Time.timeScale;

        timeScalePercent = 1 - (m_CurrentTimeScale - m_SlowTimeScale) / (m_NormalTimeScale - m_SlowTimeScale);
        // Debug.Log(timeScalePercent);
    }

    public static bool TimeSlowing() {
        return timeSlowing;
    }

    void OnGUI() {
        // GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Timescale: " + Time.timeScale + "...\n Timescale percent: " + timeScalePercent.ToString());
    }
}
