using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquishyTimer : MonoBehaviour {

	public GameObject m_BackgroundCircle;
	public GameObject m_InnerCircle;
	public GameObject[] m_OuterCircles;

	public float m_OuterCircleRadius = 1.0f;
	public float m_CenterCircleRadius = .5f;

	private float m_RingCircleRadius = 0;

	public Image meter;

	// Use this for initialization
	void Start () {

	}

	float outerRingDistanceFromCenter;
	
	// Update is called once per frame
	void Update () {
		

		m_RingCircleRadius = (m_OuterCircleRadius - m_CenterCircleRadius)/2;

		m_BackgroundCircle.transform.localScale = Vector2.one * m_OuterCircleRadius;
		m_InnerCircle.transform.localScale = Vector2.one * m_CenterCircleRadius;
		m_OuterCircles[0].transform.localScale = Vector2.one * m_RingCircleRadius;
		m_OuterCircles[1].transform.localScale = Vector2.one * m_RingCircleRadius;

		outerRingDistanceFromCenter = (m_CenterCircleRadius + m_RingCircleRadius)/2;
		float angle = 360 * meter.fillAmount;

		m_OuterCircles[0].transform.position = (Vector2)transform.position + new Vector2(0, outerRingDistanceFromCenter);
		// m_OuterCircles[1].transform.position = (Vector2)transform.position + 

		// m_OuterCircles[0].transform.position = new Vector2(transform.position.x, transform.position.y + m_CenterCircleRadius + m_RingCircleRadius/2);

	}


}
