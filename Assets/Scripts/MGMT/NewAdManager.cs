using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class NewAdManager : MonoBehaviour
{

    public static int adThreshold = 3;
    public static int playcount = 0;

    bool isShowingAd = false;

    public static bool forceAdsOff = false;
    public static bool adsDisabled = false;

    #region instance
    static NewAdManager instance;
    public static NewAdManager GetInstance()
    {
        return instance;
    }

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        InitializeAds();
    }
    #endregion

    void InitializeAds()
    {
        string appleGameId = "1652958";

        Advertisement.Initialize(appleGameId);

        isShowingAd = false;
    }

    public void ShowVideoAd()
    {
		#if UNITY_ADS
        if (forceAdsOff || adsDisabled) { return; }

		if(NewScoreManager._lastPeakCount >= 5) {
			playcount++;
		}

		// If the ad isn't ready, there isn't anything we can do.. I think?
		if (!Advertisement.IsReady())
		{
			Debug.Log("Ads not ready for default zone");
			return;
		}
		else
		{
			if (playcount >= adThreshold)
			{
				isShowingAd = true;
				var options = new ShowOptions { resultCallback = HandleShowResult };
				Advertisement.Show("video", options);
			}
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
                playcount = 0;
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                isShowingAd = false;
                playcount = 0;
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                isShowingAd = false;
                break;
        }
    }
#endif

    public bool ShowingAd()
    {
        return isShowingAd;
    }

    public void HandlePurchaseMade() {
        // Purchase was successful
        adsDisabled = true;
        GlobalSettings.Settings.adsDisabled = true;
        GlobalSettings.UpdateSavedValues();
    }
}
