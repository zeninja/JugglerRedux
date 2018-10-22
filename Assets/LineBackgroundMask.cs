using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineBackgroundMask : MonoBehaviour
{

    public float maskScale = 1.25f;

    BallLine ballLine;
    LineRenderer maskLine;

    NewBallArtManager bam;

    // Use this for initialization
    void Awake()
    {
        ballLine = GetComponentInParent<BallLine>();
        maskLine = GetComponent<LineRenderer>();
        bam = GetComponentInParent<NewBallArtManager>();
    }

    void Start()
    {
        maskLine.startWidth = NewBallManager.GetInstance().ballScale * maskScale;
        maskLine.endWidth = NewBallManager.GetInstance().ballScale * maskScale;
    }

    void LateUpdate()
    {
        if(bam.VelocityPositive()) {
        	// Debug.Log("Velocity Positive");
			SetMaskPositions(ballLine.GetTrailPositions());
			maskLine.enabled = true;
        } else {
        	// Debug.Log("Not so much");
        	maskLine.enabled = false;
        }

    }


    public void SetMaskPositions(Vector3[] positions)
    {
        if (positions != null)
        {
            // Debug.Log("Setting positions");
            maskLine.SetPositions(positions);
        }
    }
}
