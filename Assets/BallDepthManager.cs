using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDepthManager : MonoBehaviour {
    
	#region instance
	private static BallDepthManager instance;
	public  static BallDepthManager GetInstance() {
		return instance;
	}
	#endregion

	void Awake() {
		if(instance == null) {
			instance = this;
		} else {
			if(this != instance) {
				Destroy(gameObject);
			}
		}
	}

	void Start() {
		EventManager.StartListening("CleanUp", HandleGameOver);
	}
	List<NewBallArtManager> artManagers = new List<NewBallArtManager>();

    public void UpdateBallDepth(NewBallArtManager caughtBall) {
		artManagers.Remove(caughtBall);
		artManagers.Insert(0, caughtBall);

		for(int i = 0; i < artManagers.Count; i++) {
			artManagers[i].SetDepth(i);
		}
    }

	void HandleGameOver() {
		artManagers.Clear();
	}
	
}
