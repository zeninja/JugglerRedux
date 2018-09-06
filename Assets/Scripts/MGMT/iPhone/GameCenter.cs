using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameCenter : MonoBehaviour {

	string leaderboardID = "unset";
	string iphoneString  = "highscore_iphone";
	string ipadString    = "highscore_ipad";

//	public static string get10Pts	   = "Get10Pts";		// achievement ID in itunes Connect
//	public static string get100Pts	   = "Get100Pts";		// achievement ID in itunes Connect
//	public static string get50Pts1Hand = "LookMaOneHand";	// achievement ID in itunes Connect

	private static GameCenter instance;
	private static bool instantiated = false;

	public static GameCenter GetInstance()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(GameCenter)) as GameCenter;
			if (!instance)
				Debug.Log("No GameCenter!!");		
		}
		return instance;
	}

	void Awake() {
		if(!instantiated) {
			instance = this;
			instantiated = true;
		}
	}

	void Start () {
		// Authenticate and register a ProcessAuthentication callback
		// This call needs to be made before we can proceed to other calls in the Social API
		if(!Social.localUser.authenticated) {
			Social.localUser.Authenticate (ProcessAuthentication);
		}

		#if UNITY_IPHONE
		Debug.Log("Set string to iphone string");
		leaderboardID = iphoneString;
		#endif

		#if UNITY_IPAD
		leaderboardID = ipadString;
		#endif
	}

	// This function gets called when Authenticate completes
	// Note that if the operation is successful, Social.localUser will contain data from the server. 
	void ProcessAuthentication (bool success) {
		if (success) {
			Debug.Log ("Authenticated, checking achievements");

			// Request loaded achievements, and register a callback for processing them
			Social.LoadAchievements (ProcessLoadedAchievements);
		}
		else
			Debug.Log ("Failed to authenticate");
	}

	// This function gets called when the LoadAchievement call completes
	void ProcessLoadedAchievements (IAchievement[] achievements) {
		if (achievements.Length == 0)
			Debug.Log ("Error: no achievements found");
		else
			Debug.Log ("Got " + achievements.Length + " achievements");
	}

	public void SetHighScore(float newScore) {
		Debug.Log("Got highscore: " + newScore);
		ReportScore(newScore, leaderboardID);
	}

	void ReportScore (float score, string id) {
		// Don't send a score if we're in the editor
		#if UNITY_EDITOR
		return;
		#endif
 
		string s1 = string.Format("{0:0.0}",score); // "123.0"
		string s2 = string.Format("{0:0.00}",score); // "123.50"
		string s3 = string.Format("{0:0.000}",score); // "123.500"

		string s = "";
		string end = "";

		if(s1 == score.ToString()) {
			Debug.Log("FOUND SINGLE DECIMAL");
			s = s1;
			end = "_1x";
		} else if(s2 == score.ToString()) {
			Debug.Log("FOUND DOUBLE DECIMAL");
			s = s2;
			end = "_2x";
		} else {
			Debug.Log("FOUND TRIPLE DECIMAL");
			s = s3;
			end = "_3x";
		}

		id = id + end;

		Debug.Log ("Reporting score " + score + " on leaderboard " + id);
		
		Social.ReportScore (long.Parse(s.Replace(".", "")), id, success => {
			Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
	}

	public void ShowLeaderboard() {
		Social.ShowLeaderboardUI();
	}

}


