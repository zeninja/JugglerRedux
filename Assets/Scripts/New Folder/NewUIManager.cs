using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewUIManager : MonoBehaviour {

	public Text ui_SlapForce;
	public Text ui_GrabForce;
	public Text ui_JuggleThreshold;
	public Text ui_TimeSlowFactor;
	public Text ui_BallScale;
	public Text ui_BallSpawnRate;
	public Text ui_playcount;
	public Toggle ui_ShowAds;

	public GameObject debugMenu;

	bool showDebugMenu = false;
	bool canShowMenu = true;

	// Update is called once per frame
	void Update () {
		ui_SlapForce.text       = NewHandManager .GetInstance(). touchSlapThrowForce.ToString("F2");
		ui_GrabForce.text       = NewHandManager .GetInstance(). touchGrabThrowForce.ToString("F2");
		ui_JuggleThreshold.text = NewBallManager .GetInstance(). juggleThreshold    .ToString("");
		ui_BallScale.text       = NewBallManager .GetInstance(). ballScale          .ToString("F2");
		// ui_TimeSlowFactor.text  = TimeManager    .GetInstance(). m_SlowTimeScale    .ToString("F2");
		ui_playcount.text       = NewAdManager.playcount.ToString();
		ui_ShowAds.isOn			= ! NewAdManager.forceAdsOff;

		bool showDebug = Input.touchCount == 3 || Input.GetKey(KeyCode.D);

		if (showDebug) {
			if(canShowMenu) {
				canShowMenu = false;
				showDebugMenu = !showDebugMenu;
			}
		} else {
			canShowMenu = true;
		}
		
		debugMenu.SetActive(showDebugMenu);
	}

	public void ToggleAds() {
		// NewAdManager.forceAdsOff = !NewAdManager.forceAdsOff;
			
		GlobalSettings.mySettings.adsOff = NewAdManager.forceAdsOff;
		GlobalSettings.UpdateSavedValues();
		Debug.Log("FORCE ADS OFF: " + NewAdManager.forceAdsOff);
		Debug.Log("SAVED INFO VALUE: " + GlobalSettings.mySettings.adsOff);
	}

	public void AdjustJuggleThreshold(int adj) {
		int juggleThreshold = NewBallManager.GetInstance().juggleThreshold;
        juggleThreshold += adj;
        juggleThreshold = Mathf.Max(1, juggleThreshold);
		NewBallManager.GetInstance().juggleThreshold = juggleThreshold;

		GlobalSettings.mySettings.juggleThreshold = juggleThreshold;
		GlobalSettings.UpdateSavedValues();
    }

    public void AdjustBallScale(float adj) {
		float ballScale = NewBallManager.GetInstance().ballScale;
        ballScale += adj;
        ballScale = Mathf.Clamp(ballScale, .1f, 3f);
        NewBallManager.GetInstance().ballScale = ballScale;

		GlobalSettings.mySettings.ballScale = ballScale;
		GlobalSettings.UpdateSavedValues();
    }

	public void AdjustSlapThrowForce(float amt)
    {
		float touchSlapThrowForce = NewHandManager.GetInstance().touchSlapThrowForce;
        touchSlapThrowForce += amt;
        PlayerPrefs.SetFloat("touchSlapforce", touchSlapThrowForce);
		NewHandManager.GetInstance().touchSlapThrowForce = touchSlapThrowForce;

		GlobalSettings.mySettings.slapForce = touchSlapThrowForce;
		GlobalSettings.UpdateSavedValues();
    }

    public void AdjustGrabThrowForce(float amt)
    {
		float touchGrabThrowForce = NewHandManager.GetInstance().touchGrabThrowForce;
        touchGrabThrowForce += amt;
        PlayerPrefs.SetFloat("touchGrabForce", touchGrabThrowForce);
		NewHandManager.GetInstance().touchGrabThrowForce = touchGrabThrowForce;

		GlobalSettings.mySettings.grabForce = touchGrabThrowForce;
		GlobalSettings.UpdateSavedValues();
    }

	public void AdjustTimeScale(float adj) {
        // float timeScale = TimeManager.GetInstance().m_SlowTimeScale;
        // timeScale += adj;
        // timeScale = Mathf.Min(1, timeScale);
        // TimeManager.GetInstance().m_SlowTimeScale = timeScale;

		// SavedInfoManager.mySettings.timeScale = timeScale;
		// SavedInfoManager.UpdateSavedValues();
   }

	// public void SwitchSlapsAllowed() {
	// 	bool slapsAllowed = ui_AllowSlaps.isOn;
	// 	// NewBallManager.allowSlaps = slapsAllowed;

	// 	int slapInt = slapsAllowed ? 1: 0; // converted to an int just in case???

	// 	SavedInfoManager.mySettings.allowSlaps = slapsAllowed;
	// 	SavedInfoManager.UpdateSavedValues();
	// }
	
	public void SwitchBallLaunchSpeed() {		
		NewBallManager.BallSpawnSpeed ballSpawnSpeed = NewBallManager.GetInstance().ballSpawnSpeed;
        int ballSpeedIndex = (int)ballSpawnSpeed;
        ballSpeedIndex = (ballSpeedIndex + 1) % 3;
        NewBallManager.GetInstance().ballSpawnSpeed = (NewBallManager.BallSpawnSpeed)ballSpeedIndex;
        NewBallManager.GetInstance().SetBallLaunchScores();

		string ballSpeedText = "UNSET";

        switch(ballSpawnSpeed) {
            case NewBallManager.BallSpawnSpeed.slow:
                ballSpeedText = "Ball Spawn Speed:\nSlow";
                break;

            case NewBallManager.BallSpawnSpeed.med:
                ballSpeedText = "Ball Spawn Speed:\nMedium";
                break;

            case NewBallManager.BallSpawnSpeed.fast:
                ballSpeedText = "Ball Spawn Speed:\nFast";
                break;
        }
        ui_BallSpawnRate.text = ballSpeedText;

		GlobalSettings.mySettings.ballSpeedIndex = ballSpeedIndex;
		GlobalSettings.UpdateSavedValues();
	}
}