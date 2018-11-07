using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIScaler : MonoBehaviour {
		
	void Update () {
		GetComponent<RectTransform>().sizeDelta = transform.root.GetComponent<RectTransform>().sizeDelta;
	}
}
