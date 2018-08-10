using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineEffect : MonoBehaviour
{
    public int segments;

	// public float radius;
	public float lineWidth = .1f;
    LineRenderer line;

    public float spreadSpeed;
    // public float maxRadius = 2f;

    public float length;
       
    void Start ()
    {
        line = gameObject.GetComponent<LineRenderer>();
       
        line.positionCount = (segments + 1);
        // line.useWorldSpace = false;
        // CreatePoints ();
    }

	void Update() {
        // AnimateCircle();
		// CreatePoints();
	}


    void AnimateCircle() {
        float t = Time.time;

        // if(radius < maxRadius) {
        //     radius = spreadSpeed * (t*t*t*t*t*t);
        // }
    }
   
    // void CreatePoints ()
    // {
    //     float x;
    //     float y;
    //     float z = 0f;
       
    //     float angle = 20f;
       
    //     for (int i = 0; i < (segments + 1); i++)
    //     {
    //         x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
    //         y = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;
            
	//         line.positionCount = (segments + 1);
    //         line.SetPosition (i,new Vector3(x,y,z) );
            
    //         angle += (360f / segments);
    //     }

	// 	line.startWidth = lineWidth;
	// 	line.endWidth   = lineWidth;
    // }
}