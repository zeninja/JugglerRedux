using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour
{
	GameObject start;
	GameObject end;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var line = gameObject.GetComponent<LineRenderer>();
        var distance = Vector3.Distance(start.transform.position, end.transform.position);
        line.materials[0].mainTextureScale = new Vector3(distance, 1, 1);
    }
}
