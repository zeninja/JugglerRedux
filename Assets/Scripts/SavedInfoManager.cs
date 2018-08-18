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
        public bool adsOff;
    }

    public static Settings mySettings;
    static string jsonString;

    void Awake()
    {
        if (PlayerPrefs.HasKey("SAVED"))
        {
            jsonString = PlayerPrefs.GetString("JSON");
            UpdateInGameValues();
        }
        else
        {
            InitValues();
        }
    }

    void InitValues()
    {
        mySettings = new Settings();
        mySettings.slapForce = NewHandManager.GetInstance().touchSlapThrowForce;
        mySettings.grabForce = NewHandManager.GetInstance().touchGrabThrowForce;
        mySettings.juggleThreshold = NewBallManager.GetInstance().juggleThreshold;
        mySettings.ballScale = NewBallManager.GetInstance().ballScale;
        mySettings.timeScale = TimeManager.GetInstance().m_SlowTimeScale;
        mySettings.ballSpeedIndex = NewBallManager.GetInstance().ballSpeedIndex;
        // mySettings.allowSlaps = false;
        mySettings.adsOff = NewAdManager.forceAdsOff;
		
        UpdateSavedValues();
    }

    void UpdateInGameValues()
    {
        Debug.Log("SAVED INFO MANAGER: Updating DEVICE's saved settings.");
        mySettings = JsonUtility.FromJson<Settings>(jsonString);

		NewHandManager.GetInstance().touchSlapThrowForce = mySettings.slapForce;
        NewHandManager.GetInstance().touchGrabThrowForce = mySettings.grabForce;
        NewBallManager.GetInstance().juggleThreshold 	 = mySettings.juggleThreshold;
        NewBallManager.GetInstance().ballScale 			 = mySettings.ballScale;
        TimeManager.GetInstance().m_SlowTimeScale 		 = mySettings.timeScale;
        NewBallManager.GetInstance().ballSpeedIndex      = mySettings.ballSpeedIndex;
        // NewBallManager.allowSlaps 						 = mySettings.allowSlaps;
        NewAdManager.forceAdsOff                         = mySettings.adsOff;
        

        UpdateSavedValues();
    }

	public static void UpdateSavedValues() {
        Debug.Log("SAVED INFO MANAGER: Updating PLAYERPREFS saved settings");
		jsonString = JsonUtility.ToJson(mySettings);
        PlayerPrefs.SetString("JSON", jsonString);
        PlayerPrefs.SetInt("SAVED", 1);
		PlayerPrefs.Save();
	}
}
