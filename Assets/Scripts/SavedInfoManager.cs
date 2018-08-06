using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedInfoManager : MonoBehaviour
{
	[System.Serializable]
    public struct Settings
    {
        public float slapForce;
        public float grabForce;
        public int   juggleThreshold;
        public float ballScale;
        public float timeScale;
        public int   ballSpeedIndex;
        public bool  allowSlaps;
    }

    public static Settings mySettings;
    static string jsonString;

    void Awake()
    {
        if (PlayerPrefs.HasKey("SAVED"))
        {
            jsonString = PlayerPrefs.GetString("JSON");
            UpdateDeviceValues();
        }
        else
        {
            InitValues();
        }
    }

    void InitValues()
    {
        mySettings.slapForce = .5f;
        mySettings.grabForce = 13.0f;
        mySettings.juggleThreshold = 1;
        mySettings.ballScale = .65f;
        mySettings.timeScale = .7f;
        mySettings.ballSpeedIndex = 2;
        mySettings.allowSlaps = false;
		
		PlayerPrefs.SetInt("SAVED", 1);
		PlayerPrefs.Save();
    }

    void UpdateDeviceValues()
    {
        mySettings = JsonUtility.FromJson<Settings>(jsonString);

		NewHandManager.GetInstance().touchSlapThrowForce = mySettings.slapForce;
        NewHandManager.GetInstance().touchGrabThrowForce = mySettings.grabForce;
        NewBallManager.GetInstance().juggleThreshold 	 = mySettings.juggleThreshold;
        NewBallManager.GetInstance().ballScale 			 = mySettings.ballScale;
        TimeManager.GetInstance().m_SlowTimeScale 		 = mySettings.timeScale;
        NewBallManager.GetInstance().ballSpeedIndex      = mySettings.ballSpeedIndex;
        NewBallManager.allowSlaps 						 = mySettings.allowSlaps;
    }

	public static void UpdateSavedValues() {
		jsonString = JsonUtility.ToJson(mySettings);
	}
}
