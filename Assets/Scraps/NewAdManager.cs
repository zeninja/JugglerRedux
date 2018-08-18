using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class NewAdManager : MonoBehaviour {

	static bool useAds = true;
	static int adThreshold = 3;
	static int currentPlays = 0;

	bool isShowingAd = false;

	public bool debug_TurnAdsOff = false;


	#region instance
	static NewAdManager instance;
	public static NewAdManager GetInstance() {
		return instance;
	}

	// Use this for initialization
	void Awake () {
		if(instance == null) {
			instance = this;
		} else {
			if (instance != this) {
				Destroy(gameObject);
			}
		}

		InitializeAds();
	}
	#endregion

	void InitializeAds() {
		string appleGameId = "1652958";

		Advertisement.Initialize(appleGameId);

		// string NUM_PLAYS = "numPlays";
		// string HAS_MADE_PURCHASE = "hasMadePurchase";

		isShowingAd = false;
	}

	public void ShowVideoAd() {
		#if UNITY_ADS
        if (!Advertisement.IsReady() || debug_TurnAdsOff)
        {
            Debug.Log("Ads not ready for default zone");
            return;
        } else {
			isShowingAd = true;

			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("video", options);
		}

        #endif
	}

	#if UNITY_ADS
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
				isShowingAd = false;
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
				isShowingAd = false;
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
				isShowingAd = false;
                break;
        }
    }
    #endif
	
	public bool ShowingAd() {
		return isShowingAd;
	}
}
