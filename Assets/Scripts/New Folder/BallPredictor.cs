﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPredictor : MonoBehaviour
{
    public List<Vector3> GetPositionList(Vector2 anchorPos, Vector3 currentVelocity) {
        List<Vector3> linePositionList = new List<Vector3>();
        Vector3       currentLinePoint         = Vector2.zero;

        const float dragPerFrame = -0.1f;
        Vector3 gravity = (Physics2D.gravity * Time.fixedDeltaTime);

        int numIterations = 0;

        for(int i = 0; i < 500; i++) {
            linePositionList.Add((Vector3)anchorPos + currentLinePoint);

            //Add Drag
            Vector3 dragForce = dragPerFrame * currentVelocity.normalized * currentVelocity.sqrMagnitude;
            currentVelocity += dragForce * Time.fixedDeltaTime;

            //Add Gravity
            currentVelocity += gravity;
            numIterations++;

            currentLinePoint += currentVelocity * Time.fixedDeltaTime;

            if(currentVelocity.y < 0) {
                break;
            }
        }

        return linePositionList;
    }
}
