using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{


    public float m_TargetTimeScale;
    public float m_CurrentTimeScale;
    public float m_SlowTimeScale = .5f;
    float m_NormalTimeScale = 1.0f;

    public float m_TimeSmoothing = 10f;

    public float m_StartingTimeJuice = 1.0f;
    float m_CurrentTimeJuice;
    public float m_TimeJuiceBuildRate = .2f;
    public float m_MaxTimeJuice = 1.0f;
    public float m_TimeJuiceDrainRate = .05f;

    public Image meter;
    public bool m_UseTimeMeter;

    // Use this for initialization
    void Start()
    {
        EventManager.StartListening("BallDied", OnBallDied);
        EventManager.StartListening("BallSlapped", OnBallSlapped);
    }

    // Update is called once per frame
    void Update()
    {
        SlowTimeBasedOnThrows();

        meter.fillAmount = m_CurrentTimeJuice / m_MaxTimeJuice;
    }

    void SlowTimeBasedOnThrows()
    {
        if (NewBallManager.GetInstance().AnyBallBeingThrown())
        {
            if (m_UseTimeMeter) {
                if( m_CurrentTimeJuice > 0) {
                    m_TargetTimeScale = m_SlowTimeScale;
                    m_CurrentTimeJuice -= m_TimeJuiceDrainRate * Time.deltaTime;
                }
            } else {
                m_TargetTimeScale = m_SlowTimeScale;
                m_CurrentTimeJuice -= m_TimeJuiceDrainRate * Time.deltaTime;
            }

        }
        else
        {
            m_TargetTimeScale = m_NormalTimeScale;
        }

        Time.timeScale = Mathf.Lerp(Time.timeScale, m_TargetTimeScale, Time.deltaTime * m_TimeSmoothing);
        m_CurrentTimeScale = Time.timeScale;
    }

    void OnBallSlapped()
    {
        m_CurrentTimeJuice = Mathf.Min(m_CurrentTimeJuice + m_TimeJuiceBuildRate, m_MaxTimeJuice);
    }

    void OnBallDied() {
        m_CurrentTimeJuice = 0;
    }
}
