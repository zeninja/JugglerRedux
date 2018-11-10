using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBGMask : MonoBehaviour
{

    public float maskScalar = 1.25f;

    BallArt ballLine;
    LineRenderer maskLine;

    NewBallArtManager bam;

    // Use this for initialization
    void Awake()
    {
        bam      = GetComponentInParent<NewBallArtManager>();
        ballLine = GetComponentInParent<BallArt>();
        maskLine = GetComponent<LineRenderer>();
    }

    void Start()
    {
        maskLine.startWidth = NewBallManager.GetInstance().ballScale * maskScalar;
        maskLine.endWidth   = NewBallManager.GetInstance().ballScale * maskScalar;
        maskLine.enabled    = true;
    }

    void Update()
    {
        SetMaskPositions(ballLine.GetTrailPositions());

        // if(bam.VelocityPositive()) {
        // } else {
        //     // SetMaskPositions()
        // }
        // // maskLine.enabled = bam.VelocityPositive();
    }


    public void SetMaskPositions(Vector3[] positions)
    {
        if (positions != null)
        {
            // Debug.Log("Setting positions");
            maskLine.positionCount = positions.Length;
            maskLine.SetPositions(positions);
        }
    }
}
