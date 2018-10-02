using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakSpriteMask : MonoBehaviour {

	NewBallArtManager art;

	void Start() {
    	art = GetComponentInParent<NewBallArtManager>();
	}

	void AdjustDepth() {
        int maskIndex = art.currentDepth;
        GetComponent<SpriteMask>().frontSortingOrder = maskIndex;
        GetComponent<SpriteMask>().backSortingOrder  = maskIndex - 1;
    }
}
