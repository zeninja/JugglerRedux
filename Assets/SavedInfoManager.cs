using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedInfoManager : MonoBehaviour {

	public static string key_slapForce = "touchSlapForce";
	public static string key_grabForce = "touchGrabForce";
	public static string key_juggleThreshold = "juggleThreshold";
	public static string key_BallScale = "ballScale";
	public static string key_TimeScale = "timeScale";
	public static string key_BallSpawnSpeed = "ballSpawnSpeed";
	public static string key_allowSlaps = "allowSlaps";


	void Awake() {
		InitValues();
	}

	void InitValues() {
		NewHandManager.GetInstance().touchSlapThrowForce = PlayerPrefs.GetFloat(key_slapForce);
		NewHandManager.GetInstance().touchGrabThrowForce = PlayerPrefs.GetFloat(key_grabForce);
	
		NewBallManager.GetInstance().juggleThreshold     = PlayerPrefs.GetInt(key_juggleThreshold);
		NewBallManager.GetInstance().ballScale           = PlayerPrefs.GetFloat(key_BallScale);
		NewBallManager.GetInstance().ballSpawnSpeed      = (NewBallManager.BallSpawnSpeed) PlayerPrefs.GetInt(key_BallSpawnSpeed);
		NewBallManager.allowSlaps 					     = PlayerPrefs.GetInt(key_allowSlaps) == 1;

		TimeManager.GetInstance().m_SlowTimeScale        = PlayerPrefs.GetFloat(key_TimeScale);
	}

	// void SetValue(string key) {
	// 	// switch(key) {
	// 	// 	case key_slapForce:
	// 	// 		break;
	// 	// 	case key_grabForce:
	// 	// 		break;
	// 	// 	case key_juggleThreshold:
	// 	// 		break;
	// 	// 	case key_BallScale:
	// 	// 		break;
	// 	// 	case key_TimeScale:
	// 	// 		break;
	// 	// 	case key_BallSpawnSpeed:
	// 	// 		break;
	// 	// 	case key_allowSlaps:
	// 	// 		break;
	// 	// }
	// }
}
