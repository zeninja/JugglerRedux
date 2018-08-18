using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleStacker : MonoBehaviour
{

    public SpriteRenderer circle;

    public int numStacks = 3;
    public float delayBetweenStacks = .15f;

    public float startScale = 1;
    public float scalar = .35f;
	public Color myColor;

	public float alphaReductionRate = .4f;

    // Use this for initialization
    void Start()
    {
		myColor = GetComponent<NewBallArtManager>().myColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(StackCircles());
        }
    }

    IEnumerator StackCircles()
    {
        for (int i = 0; i < numStacks; i++)
        {
            SpriteRenderer c = Instantiate(circle);
            c.transform.localScale = Vector3.one * (startScale + scalar * (i+1));
            c.transform.parent = transform;
			
			Color spriteColor = myColor;
			spriteColor.a = 1 - alphaReductionRate * (i + 1) ;
			c.color = spriteColor;
			c.sortingLayerName = "Effects";
			c.sortingOrder = -i;

            yield return new WaitForSeconds(delayBetweenStacks);
        }
    }
}
