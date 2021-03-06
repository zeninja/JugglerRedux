﻿using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameCenter : MonoBehaviour {

	string highscoreLeaderboard = "highscore";

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
		// ReportScore((long)newScore, highscoreLeaderboard);
	}

	void ReportScore (long score, string leaderboardID) {
		Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardID);
		Social.ReportScore (score, leaderboardID, success => {
			Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
	}

	public void ShowLeaderboard() {
		Social.ShowLeaderboardUI();
	}

}


