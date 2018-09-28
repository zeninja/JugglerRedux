 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    float m_TargetTimeScale;
    public float m_CurrentTimeScale;
    // public float m_SlowTimeScale = .5f;
    float m_NormalTimeScale = 1.0f;

    public Extensions.Property timeRange;

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

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D)) {
            PauseTime();
        }
    }

    void PauseTime(){
        Time.timeScale = 0;
    }

    public Extensions.Property slowTimeScaleRange;

    void SlowTimeBasedOnThrows()
    {
        float juggleTime = m_NormalTimeScale;

        if (NewBallManager.GetInstance().JuggleThresholdReached() || Input.GetKey(KeyCode.LeftShift))
        {
            timeSlowing = true;
            juggleTime = Extensions.GetSmoothStepRange(slowTimeScaleRange, (float)NewBallManager._ballCount / (float)9);
            m_TargetTimeScale = juggleTime;
        }
        else
        {
            timeSlowing = false;
            m_TargetTimeScale = m_NormalTimeScale;
        }

        

        Time.timeScale = Mathf.Lerp(Time.timeScale, m_TargetTimeScale, Time.deltaTime * m_TimeSmoothing);
        m_CurrentTimeScale = Time.timeScale;

        timeScalePercent = 1 - (m_CurrentTimeScale - juggleTime) / (m_NormalTimeScale - juggleTime);
        // Debug.Log(timeScalePercent);
    }

    public static bool TimeSlowing() {
        return timeSlowing;
    }

    void OnGUI() {
        // GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Timescale: " + Time.timeScale + "...\n Timescale percent: " + timeScalePercent.ToString());
    }
}
