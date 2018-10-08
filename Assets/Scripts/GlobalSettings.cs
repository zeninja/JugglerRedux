using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
	[System.Serializable]
    public struct GameSettings
    {
        public float slapForce;
        public float grabForce;
        public int   juggleThreshold;
        public float ballScale;
        public int   ballSpeedIndex;
        public bool adsOff;

        public bool offsetXSpawnPosition;

        public float timeMin;
        public float timeMax;

        public bool disableAds;

        public bool musicOn;
        public bool sfxOn;
        public bool invertThrows;
    }

    [SerializeField]
    public static GameSettings Settings;
    static string jsonString;

    void Awake()
    {
        // #if !UNITY_EDITOR
        if (PlayerPrefs.HasKey("SAVED"))
        {
            jsonString = PlayerPrefs.GetString("JSON");
            UpdateInGameValues();
        }
        else
        {
            InitValues();
        }
        // #else 
        // InitValues();
        // #endif
        // InitValues();
    }

    void InitValues()
    {
        Settings = new GameSettings();
        Settings.slapForce       = NewHandManager.GetInstance().touchSlapThrowForce;
        Settings.grabForce       = NewHandManager.GetInstance().touchGrabThrowForce;
        Settings.juggleThreshold = NewBallManager.GetInstance().juggleThreshold;
        Settings.ballScale       = NewBallManager.GetInstance().ballScale;
        // mySettings.timeScale       = TimeManager   .GetInstance().m_SlowTimeScale;
        Settings.ballSpeedIndex  = NewBallManager.GetInstance().ballSpeedIndex;
        Settings.adsOff          = NewAdManager.forceAdsOff;
        Settings.invertThrows    =!NewHandManager.dragUpToThrow;

        Settings.timeMin         = TimeManager.GetInstance().timeRange.start;
        Settings.timeMax         = TimeManager.GetInstance().timeRange.end;

        Settings.disableAds      = false;

        Settings.musicOn         = !AudioManager.m_mute;
        Settings.sfxOn           = !AudioManager.sfx_mute; 
        

        // Debug.Log("INIT VALUES. BALL SCALE: " + Settings.ballScale);
		
        UpdateSavedValues();
    }

    void UpdateInGameValues()
    {
        // Debug.Log("SAVED INFO MANAGER: Updating DEVICE's saved settings.");
        Settings = JsonUtility.FromJson<GameSettings>(jsonString);

		NewHandManager.GetInstance().touchSlapThrowForce = Settings.slapForce;
        NewHandManager.GetInstance().touchGrabThrowForce = Settings.grabForce;
        NewBallManager.GetInstance().juggleThreshold 	 = Settings.juggleThreshold;
        NewBallManager.GetInstance().ballScale 			 = Settings.ballScale;
        // TimeManager.GetInstance().m_SlowTimeScale 		 = mySettings.timeScale;
        NewBallManager.GetInstance().ballSpeedIndex      = Settings.ballSpeedIndex;
        NewAdManager.forceAdsOff                         = Settings.adsOff;
        NewHandManager.dragUpToThrow                     = !Settings.invertThrows;

        AudioManager.m_mute                              = !Settings.musicOn;
        AudioManager.sfx_mute                            = !Settings.sfxOn;



        TimeManager.GetInstance().timeRange.start        = Settings.timeMin;
        TimeManager.GetInstance().timeRange.end          = Settings.timeMax;

        UpdateSavedValues();

        // Debug.Log(NewBallManager.GetInstance().ballScale + "; " + Settings.ballScale);
    }

	public static void UpdateSavedValues() {
        // Debug.Log("SAVED INFO MANAGER: Updating PLAYERPREFS saved settings");
		jsonString = JsonUtility.ToJson(Settings);
        PlayerPrefs.SetString("JSON", jsonString);
        PlayerPrefs.SetInt("SAVED", 1);
		PlayerPrefs.Save();
	}
}
