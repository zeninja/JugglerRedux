using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleEffect : MonoBehaviour
{
    float startTime;
    int segments = 100;

    float xradius, yradius;
	public float radius;
    public float maxRadius = 2f;
	public float lineWidth = .1f;
    LineRenderer line;

    public float spreadSpeed;
    






    void Awake() {
        startTime = Time.time;
        radius = 0;
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.positionCount = (segments + 1);
        line.useWorldSpace = false;
        line.enabled = false;
        
    }

    void Start ()
    {
        CreatePoints ();
    }

	void Update() {
        AnimateCircle();
		CreatePoints();
	}


    void AnimateCircle() {
        float t = Time.time - startTime;
        if(t <= 1) {
            radius = spreadSpeed * EZEasings.SmoothStop2(t);
            lineWidth = radius; 
        }
    }
   
    void CreatePoints ()
    {
        float x;
        float y;
        float z = 0f;
        xradius = radius;
        yradius = radius;
       
        float angle = 20f;
       
        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;
            
	        line.positionCount = (segments + 1);
            line.SetPosition (i,new Vector3(x,y,z) );
            
            angle += (360f / segments);
        }

		line.startWidth = lineWidth;
		line.endWidth   = lineWidth;
        line.enabled = true;
    }
}