﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralCircle : MonoBehaviour
{
    int resolution = 1000;
    public List<Vector3> ringPositions;
    public Vector2 anchorPos;
    public float radius = .5f;

    void Awake()
    {
        if (resolution % 2 != 0)
        {
            resolution--;
        }
    }

    void Update()
    {
        FindRingPositions();
        ready = true;
    }

    void FixedUpdate()
    {
        UpdateMesh();
    }

    bool ready = false;

    void UpdateMesh() {
        if(!ready) { return; }
        GetComponent<MonolithMesh>().UpdateValues(anchorPos, ringPositions);
    }

    void FindRingPositions()
    {
        ringPositions = new List<Vector3>();

        // anchorPos = Extensions.MouseScreenToWorld();

        float x;
        float y;
        float z = 0;

        float angle = 0;

        for (int i = 0; i < resolution; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            Vector3 ringPoint = (Vector3)anchorPos + new Vector3(x, y, z);
            ringPositions.Add(ringPoint);

            angle += (360f / resolution);
        }

        angle = 0;

        x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
        y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

        Vector3 lastPoint = (Vector3)anchorPos + new Vector3(x, y, z);
        ringPositions.Add(lastPoint);
    }

    public void UpdateValues(float newRadius) {
        radius = newRadius;
    }
}