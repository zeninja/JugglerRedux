using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InterstitialButton : MonoBehaviour
{
    public SpriteRenderer depthPrefab;
    public Transform innerAnchor;
    public Color from;
    public Color to;

    public int steps = 7;

    public Vector2 offset;

    void Start() {
        GenerateBackground();
    }

    void Update() {
        UpdateBackground();

        if(Input.GetKeyDown(KeyCode.P)) {
            MakePurchase();
        }
    }
    
    List<SpriteRenderer> depthPlates = new List<SpriteRenderer>();

    void GenerateBackground() {
        for(int i = 0; i < steps; i++) {
            SpriteRenderer s = Instantiate(depthPrefab);
            s.transform.parent = innerAnchor;
            s.transform.localScale = Vector3.one;
            s.transform.localPosition = offset * (float)i / (float)steps;
            s.transform.localPosition += new Vector3(0, 0, i);
            s.color = Color.Lerp(from, to, (float)i / (float) steps);
            s.sortingOrder = 90 - i;
            depthPlates.Add(s);
        }
    }

    void UpdateBackground() {
        for(int i = 0; i < steps; i++) {
            depthPlates[i].transform.localPosition = offset * (float)i / (float)steps;
            depthPlates[i].transform.localPosition += new Vector3(0, 0, i * .001f);
            // depthPlates[i].color = Color.Lerp(from, to, (float)i / (float) steps);
        }
    }

    public AnimationClip buttonBounce;

    public void MakePurchase() {
        Debug.Log("Button pressed");
        GetComponent<Animation>().Play(buttonBounce.name);
        
    }
}