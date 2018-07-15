using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBall : MonoBehaviour
{
    public float scale = .25f;
    Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.localScale = Vector2.one * scale;
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.C)) {
			// HandleCatch(Vector2.up, forceModifier, Vector2.one);
		}

        CheckBounds();
    }

    private void FixedUpdate() {

    }

    public void HandleCatch(Vector3 normalizedDirection, float forceModifier, Vector2 rawMoveVector)
    {
        // rb.AddForce(normalizedDirection * forceModifier, ForceMode2D.Impulse);
		rb.velocity = Vector2.zero;
        rb.velocity = normalizedDirection * forceModifier * rawMoveVector.magnitude;
        EventManager.TriggerEvent("BallCaught");
    }

    public void GrabBall() {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        Debug.Log("BALL GRABBED!!!!");
    }

    public void GetThrown(Vector2 throwForce) {
        rb.velocity = throwForce;
        rb.gravityScale = 1;

        Debug.Log("BALL THROOOOWWWWNNNN");

        // rb.velocity = normalizedDirection * forceModifier * rawMoveVector.magnitude;
    }

    public void GetCaughtAndThrown(Vector2 throwVector) {
        Debug.Log("Getting caught and thrown");
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        rb.velocity = throwVector;
        rb.gravityScale = 1;
        Debug.Log(throwVector);
    }

    void CheckBounds() {
        Vector3 converted = Camera.main.WorldToScreenPoint(transform.position);

        if(converted.y < 0 || converted.y > Screen.height || converted.x < 0 || converted.x > Screen.width) {
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        EventManager.TriggerEvent("BallDied");
        // EventManager.TriggerEvent("GameOver");
    }

    public void Die() {
        StartCoroutine(Explode());
    }


    public AnimationCurve explosionCurve;
    public AnimationCurve implosionCurve;
    public float maxExplosionScale = 10;

    IEnumerator Explode() {
        float duration = .125f;
        float elapsedTime = 0;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            Vector2 explosionScale = Vector2.one * explosionCurve.Evaluate(elapsedTime/duration);
            explosionScale *= maxExplosionScale;
            transform.localScale = explosionScale;
            yield return new WaitForEndOfFrame();
        }
        
        elapsedTime = 0;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            Vector2 implosionScale = Vector2.one * implosionCurve.Evaluate(elapsedTime/duration);
            implosionScale *= maxExplosionScale;
            transform.localScale = implosionScale;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
        
        Destroy(gameObject);
    }
}
