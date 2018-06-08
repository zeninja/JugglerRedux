using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBall : MonoBehaviour
{
    public float scale = .25f;

	public float forceModifier = 10;

    // Use this for initialization
    void Start()
    {
        transform.localScale = Vector2.one * scale;
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.C)) {
			HandleCatch(Vector2.up, forceModifier, Vector2.one);
		}
    }

    public void HandleCatch(Vector3 normalizedDirection, float forceModifier, Vector2 rawMoveVector)
    {
        Debug.Log("Got thrown");
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce(normalizedDirection * forceModifier * rawMoveVector.magnitude, ForceMode2D.Impulse);
        EventManager.TriggerEvent("BallCaught");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillTrigger"))
        {
            EventManager.TriggerEvent("BallDeath");
        }
    }

    public void HandleDeath()
    {
        Destroy(gameObject);
    }
}
