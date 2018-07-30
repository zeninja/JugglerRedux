using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBall : MonoBehaviour
{
    Rigidbody2D rb;

    public float scale = .25f;
    public float defaultGravity = 20;
    public float drag = -0.1f;

    [HideInInspector]
    public bool canBeCaught = false;
    [HideInInspector]
    public bool launching;
    // [HideInInspector]
    public Vector2 currentThrowVector;

    public bool isHeld;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = defaultGravity;
        transform.localScale = Vector2.one * scale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)) {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }

        if(rb.velocity.y < 0) {
            launching = false;
        }
        
        if (!launching) {
            CheckBounds();
        }
    }

    void FixedUpdate() {
        // QUADRATIC DRAG
        Vector2 force = drag * rb.velocity.normalized * rb.velocity.sqrMagnitude;
        rb.AddForce(force);
    }

    public void GetCaughtAndThrown(Vector2 throwVector) {
        if(launching) { return; }

        Debug.Log("Getting caught and thrown");
        rb.velocity = Vector2.zero;
        rb.AddForce(throwVector * rb.mass, ForceMode2D.Impulse);
        rb.gravityScale = defaultGravity;
        GetComponent<LineDrawer>().HandleThrow();


        // rb.gravityScale = 0;
        // rb.velocity = Vector2.zero;

        // Method 1: Add force
        // rb.AddForce(throwVector, ForceMode2D.Impulse);
        
        // Method 2: Set velocity directly
        // rb.velocity = throwVector;
        // rb.gravityScale = defaultGravity;

        // EventManager.TriggerEvent("BallCaught");
    }

    public void GetCaught() {
        if(launching) { return; }
        Debug.Log("Got caught");

        isHeld = true;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        
        EventManager.TriggerEvent("BallCaught");
    }

    public void GetThrown(Vector2 throwVector) {
        if(launching) { return; }

        isHeld = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(throwVector * rb.mass, ForceMode2D.Impulse);
        rb.gravityScale = defaultGravity;
        GetComponent<LineDrawer>().HandleThrow();
    }

    void CheckBounds() {
        Vector3 converted = Camera.main.WorldToScreenPoint(transform.position);

        if(converted.y < 0 || converted.y > Screen.height || converted.x < 0 || converted.x > Screen.width) {
            if(!NewGameManager.GameOver()) {
                HandleDeath();
            }
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