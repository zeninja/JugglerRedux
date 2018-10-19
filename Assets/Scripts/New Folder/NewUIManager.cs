﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewUIManager : MonoBehaviour {

	// In-Game
	public TextMeshPro ui_juggleTime;


	// Debug
	public Text ui_SlapForce;
	public Text ui_GrabForce;
	public Text ui_JuggleThreshold;
	// public Text ui_TimeSlowFactor;
	public Text ui_BallScale;
	public Text ui_BallSpawnRate;
	public Text ui_playcount;
	public Toggle ui_ShowAds;


	// public GameObject debugMenu;

	// bool showDebugMenu = false;
	// bool canShowMenu = true;

	public GameObject menuButtons;

	// Update is called once per frame
	void Update () {
		ui_SlapForce.text       = NewHandManager .GetInstance(). touchSlapThrowForce.ToString("F2");
		ui_GrabForce.text       = NewHandManager .GetInstance(). touchGrabThrowForce.ToString("F2");
		ui_JuggleThreshold.text = NewBallManager .GetInstance(). juggleThreshold    .ToString("");
		ui_BallScale.text       = NewBallManager .GetInstance(). ballScale          .ToString("F2");
		// ui_TimeSlowFactor.text  = TimeManager    .GetInstance(). m_SlowTimeScale    .ToString("F2");
		ui_playcount.text       = NewAdManager.playcount.ToString();
		ui_ShowAds.isOn			= ! NewAdManager.forceAdsOff;

		ui_juggleTime.text		= TimeManager.GetInstance().m_CurrentTimeScale.ToString("F2");

		// bool showDebug = Input.touchCount == 3 || Input.GetKey(KeyCode.D);

		// if (showDebug) {
		// 	if(canShowMenu) {
		// 		canShowMenu = false;
		// 		showDebugMenu = !showDebugMenu;
		// 	}
		// } else {
		// 	canShowMenu = true;
		// }
		
		// debugMenu.SetActive(showDebugMenu);

		menuButtons.SetActive(NewGameManager.gameState == GameState.preGame || NewGameManager.gameState == GameState.settings);
	}

	public void ToggleAds() {
		// NewAdManager.forceAdsOff = !NewAdManager.forceAdsOff;
			
		GlobalSettings.Settings.adsOff = NewAdManager.forceAdsOff;
		GlobalSettings.UpdateSavedValues();
		Debug.Log("FORCE ADS OFF: " + NewAdManager.forceAdsOff);
		Debug.Log("SAVED INFO VALUE: " + GlobalSettings.Settings.adsOff);
	}

	public void AdjustJuggleThreshold(int adj) {
		int juggleThreshold = NewBallManager.GetInstance().juggleThreshold;
        juggleThreshold += adj;
        juggleThreshold = Mathf.Max(1, juggleThreshold);
		NewBallManager.GetInstance().juggleThreshold = juggleThreshold;

		GlobalSettings.Settings.juggleThreshold = juggleThreshold;
		GlobalSettings.UpdateSavedValues();
    }

    public void AdjustBallScale(float adj) {
		float ballScale = NewBallManager.GetInstance().ballScale;
        ballScale += adj;
        ballScale = Mathf.Clamp(ballScale, .1f, 3f);
        NewBallManager.GetInstance().ballScale = ballScale;

		GlobalSettings.Settings.ballScale = ballScale;
		GlobalSettings.UpdateSavedValues();
    }

	public void AdjustSlapThrowForce(float amt)
    {
		float touchSlapThrowForce = NewHandManager.GetInstance().touchSlapThrowForce;
        touchSlapThrowForce += amt;
        PlayerPrefs.SetFloat("touchSlapforce", touchSlapThrowForce);
		NewHandManager.GetInstance().touchSlapThrowForce = touchSlapThrowForce;

		GlobalSettings.Settings.slapForce = touchSlapThrowForce;
		GlobalSettings.UpdateSavedValues();
    }

    public void AdjustGrabThrowForce(float amt)
    {
		float touchGrabThrowForce = NewHandManager.GetInstance().touchGrabThrowForce;
        touchGrabThrowForce += amt;
        PlayerPrefs.SetFloat("touchGrabForce", touchGrabThrowForce);
		NewHandManager.GetInstance().touchGrabThrowForce = touchGrabThrowForce;

		GlobalSettings.Settings.grabForce = touchGrabThrowForce;
		GlobalSettings.UpdateSavedValues();
    }

	public void AdjustTimeMin(float adj) {
        float timeScale = TimeManager.GetInstance().timeRange.start;
        timeScale += adj;
        timeScale = Mathf.Min(1, timeScale);
        TimeManager.GetInstance().timeRange.start = timeScale;

		GlobalSettings.Settings.timeMin = timeScale;
		GlobalSettings.UpdateSavedValues();
   }

	public void AdjustTimeMax(float adj) {
        float timeScale = TimeManager.GetInstance().timeRange.end;
        timeScale += adj;
        timeScale = Mathf.Min(1, timeScale);
        TimeManager.GetInstance().timeRange.end = timeScale;

		GlobalSettings.Settings.timeMax = timeScale;
		GlobalSettings.UpdateSavedValues();
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

		GlobalSettings.Settings.ballSpeedIndex = ballSpeedIndex;
		GlobalSettings.UpdateSavedValues();
	}

	// public ThrowDirectionSprite throwDirectionSprite;

	public void InvertThrowDirection() {
		NewHandManager.invertThrows = !NewHandManager.invertThrows;
		GlobalSettings.Settings.invertThrows = NewHandManager.invertThrows;
		// Debug.Log("settings set to: " + GlobalSettings.Settings.invertThrows);
		GlobalSettings.UpdateSavedValues();
	}

	public static void UpdateBallScale() {
		GlobalSettings.Settings.ballScale = NewBallManager.GetInstance().ballScale;
		GlobalSettings.UpdateSavedValues();
		// Debug.Log(GlobalSettings.Settings.ballScale);
	}

	public void OpenContactInfo() {
		#if UNITY_EDITOR
		#else
		Application.OpenURL("https://www.twitter.com/adnanwho");
		Application.OpenURL("twitter:///user?screen_name=adnanwho");
		#endif
	}
}