using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquishyTimer2 : MonoBehaviour {

	public GameObject[] m_OuterCircles;
	public Image m_RadialSlider;

	float outerRingDistanceFromCenter;
	Color m_MeterColor;

	// Use this for initialization
	void Start () {
		m_MeterColor = m_RadialSlider.GetComponent<Image>().color;
		outerRingDistanceFromCenter = (m_OuterCircles[0].transform.position - transform.position).magnitude;

		m_OuterCircles[0].GetComponent<Image>().color = m_MeterColor;
		// m_OuterCircles[1].GetComponentInChildren<Image>().color = m_MeterColor;
	}
	
	// Update is called once per frame
	void Update () {
		// m_OuterCircles[0].transform.position = (Vector2)transform.position + new Vector2(0, outerRingDistanceFromCenter);
		
		float angle = 180 * m_RadialSlider.fillAmount;
		float dist = outerRingDistanceFromCenter;

		m_OuterCircles[1].transform.position = new Vector2(Mathf.Sin(angle) * dist, Mathf.Cos(angle) * dist);
	}
}
