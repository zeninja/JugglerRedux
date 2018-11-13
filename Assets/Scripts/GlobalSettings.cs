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

        public bool adsDisabled;

        public bool muteMusic;
        public bool muteSfx;
        public bool invertThrows;
        public bool useRails;
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
        Settings.invertThrows    = NewHandManager.invertThrows;
        Settings.useRails        = NewBallManager.useRails;        

        Settings.timeMin         = TimeManager.GetInstance().timeRange.start;
        Settings.timeMax         = TimeManager.GetInstance().timeRange.end;

        Settings.adsDisabled     = NewAdManager.adsDisabled;

        Settings.muteMusic       = AudioManager.m_mute;
        Settings.muteSfx         = AudioManager.sfx_mute; 


        Debug.Log("INIT VALUES. musicOn: " + Settings.muteMusic);
		
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
        NewHandManager.invertThrows                      = Settings.invertThrows;
        NewBallManager.useRails                          = Settings.useRails;

        AudioManager.m_mute                              = Settings.muteMusic;
        AudioManager.sfx_mute                            = Settings.muteSfx;

        NewAdManager.adsDisabled                         = Settings.adsDisabled;

        TimeManager.GetInstance().timeRange.start        = Settings.timeMin;
        TimeManager.GetInstance().timeRange.end          = Settings.timeMax;

        UpdateSavedValues();

        Debug.Log("UPDATING IN GAME VALUE: audio manager mute: " + AudioManager.m_mute + "; settings: " + Settings.muteMusic);
    }

	public static void UpdateSavedValues() {
        // Debug.Log("SAVED INFO MANAGER: Updating PLAYERPREFS saved settings");
		jsonString = JsonUtility.ToJson(Settings);
        PlayerPrefs.SetString("JSON", jsonString);
        PlayerPrefs.SetInt("SAVED", 1);
		PlayerPrefs.Save();

        Debug.Log("UPDATING SAVED VALUE: audio manager mute: " + AudioManager.m_mute + "; settings: " + Settings.muteMusic);
	}
}
