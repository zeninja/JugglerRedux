using UnityEngine;
using System.Collections;
using admob;

public class AdManager : MonoBehaviour {

	#region Util
	private static AdManager instance;

	public static AdManager GetInstance ()
	{
		if (!instance) {
			instance = FindObjectOfType(typeof(AdManager)) as AdManager;
			if (!instance)
				Debug.Log("No AdManager!!");
		}
		return instance;
	}

	public static bool showAds = true;
	#endregion

	public static int adThreshold = 3;
	public static int currentPlays = 0;

	// PlayerPrefs keys
	const string NUM_PLAYS = "numPlays";
	const string HAS_MADE_PURCHASE = "hasMadePurchase";

	public bool forceAdsOff;

	void Awake() {
		Admob.Instance().interstitialEventHandler += OnInterstitialEvent;
	}

	// Use this for initialization
	void Start () {
//		#if !UNITY_EDITOR
		if (PlayerPrefs.HasKey(HAS_MADE_PURCHASE)) {
			showAds = PlayerPrefs.GetInt(HAS_MADE_PURCHASE) == 0;
		} else {
			PlayerPrefs.SetInt(HAS_MADE_PURCHASE, 0);
			showAds = true;
		}

		if (forceAdsOff) {
			showAds = false;
		}

		if (PlayerPrefs.HasKey(NUM_PLAYS)) {
			currentPlays = PlayerPrefs.GetInt(NUM_PLAYS);
		} else {
			PlayerPrefs.SetInt(NUM_PLAYS, 0);
		}

		if (showAds) {
			Admob.Instance().setTesting(true); 
			InitAds();
		}

//		Debug.Log("\nSHOW ADS VALUE: " + showAds + "\n");
//		Debug.Log("\nHAS MADE PURCHASE: " + PlayerPrefs.GetInt(hasMadePurchase));
//		#endif

	}

	void InitAds() {
		// Not using the banner ads, so the ID isn't included. Second ID is interstitial ID.
		Admob.Instance().initAdmob("admob banner id", "ca-app-pub-2916476108966190/9838939664"); //admob id with format ca-app-pub-279xxxxxxxx/xxxxxxxx
		Admob.Instance().removeBanner();
		Admob.Instance().loadInterstitial();
//		Admob.Instance().loadRewardedVideo("ca-app-pub-2916476108966190/1617668865");
	}


	void TryToShowAd()
	{
		if(forceAdsOff) 
		{ 
			ReturnToMainMenu();
			return;
		}

//		#if !UNITY_EDITOR
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			ReturnToMainMenu();
		} else {
			if (Admob.Instance().isInterstitialReady()) {
				Admob.Instance().showInterstitial();
		    } else {
				Admob.Instance().loadInterstitial();
				ReturnToMainMenu();
		    }
			currentPlays = 0;
		}

//		#endif
	}

	// not being used right now
//	void TryToShowVideoAd() {
//		if(debug) { return; }
//
//		#if !UNITY_EDITOR
//		if (showAds) {
//			if (Admob.Instance().isRewardedVideoReady()) {
//				Admob.Instance().showRewardedVideo();
//		    } else {
//				Admob.Instance().loadRewardedVideo("ca-app-pub-2916476108966190/1617668865");
//		    }
//	    }
//		#endif
//	}

	public void CheckAd() {
		if(showAds) {
			currentPlays++;
			if(Application.internetReachability != NetworkReachability.NotReachable) {
				if (currentPlays >= adThreshold) {
					TryToShowAd();
				} else {
					ReturnToMainMenu();
					PlayerPrefs.SetInt(NUM_PLAYS, currentPlays);
				}
			} else {
				ReturnToMainMenu();
				PlayerPrefs.SetInt(NUM_PLAYS, currentPlays);
			}

		} else {
			ReturnToMainMenu();
		}
	}

	void OnInterstitialEvent(string eventName, string msg) {
		switch(eventName) {
			case "onAdOpened":
				currentPlays = 0;
				PlayerPrefs.SetInt(NUM_PLAYS, currentPlays);
				break;
				
			case "onAdClosed":
				ReturnToMainMenu();
				Admob.Instance().loadInterstitial();
				break;

			case "onAdFailedToLoad":
				Admob.Instance().loadInterstitial();
				ReturnToMainMenu();
				break;
		}
	}

	void ReturnToMainMenu() {
		GameManager.GetInstance().ReturnToMainMenu();
	}

	public static void HandlePurchaseMade() {
		showAds = false;
		PlayerPrefs.SetInt(AdManager.HAS_MADE_PURCHASE, 1);
	}
}