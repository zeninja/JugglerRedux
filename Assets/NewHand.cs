using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHand : MonoBehaviour
{
    #region 
    private static NewHand instance;

    public static NewHand GetInstance()
    {
        return instance;
    }
    #endregion

    Transform myTransform;

    Touch myTouch;

    Vector2 lastFramePos;
    Vector2 instantMoveDir;
    Vector2 immediateThrowVector;
    Vector2 heldMoveDir;
    Vector2 heldThrowVector;
    Vector2 catchPosition;

    NewBall ball;

    public bool useMouse = false;

    List<Vector2> positions;
    List<Vector2> moveDirections;

    [System.NonSerialized] public float heldThrowForce = 4;
    [System.NonSerialized] public float immediateThrowForce = 10;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            if (this != instance) {
                Destroy(gameObject);
            }
        }

        InitHand();
    }

    void InitHand() {
        myTransform = transform;

        heldThrowForce = NewHandManager.GetInstance().heldThrowForce;
        immediateThrowForce = NewHandManager.GetInstance().immediateThrowForce;

        positions = new List<Vector2>();
        moveDirections = new List<Vector2>();
    }

    void Update()
    {
        // dEBUG
        immediateThrowForce = NewHandManager.GetInstance().immediateThrowForce;
        MoveHand();

        if (ball != null) {
            ball.currentThrowVector = heldThrowVector;
        }
    }

    void MoveHand()
    {
        if (useMouse) {
            HandleMouseInput();
        } else {
            HandleTouchInput();
        }
    }

    void HandleMouseInput()
    {
        // Step 1: Set the position to be equal to the current mouse position
        transform.position = Extensions.MouseScreenToWorld();
        
        // Step 2: Find the move direction since last frame
        if(positions.Count > 1) {
            // Check num positions so that first frame catches do not throw the ball immediately
            instantMoveDir = (Vector2)transform.position - lastFramePos;
        }

        // Step 3: Set the last frame position (for next frame)
        lastFramePos = transform.position;

        // Step 4: Find the throw vector
        immediateThrowVector = instantMoveDir * immediateThrowForce;

        positions.Add(transform.position);
        moveDirections.Add(instantMoveDir);

        if(Input.GetMouseButton(0)) {
            if(ball != null) {
                heldMoveDir = (Vector2)transform.position - catchPosition;
                heldThrowVector = heldMoveDir * heldThrowForce;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (ball != null) {
                Debug.Log("tryna throw");
                // heldThrowVector = heldMoveDir * heldThrowForce;
                ball.GetThrown(heldThrowVector);
                ball = null;
                HandleDeath();
            }
        }
    }

    void HandleTouchInput() 
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only allow one ball to be affected at a time
        if(ball == null) {

            if (other.CompareTag("Ball")) {

                // Only catch balls that are not launching
                if(!other.GetComponent<NewBall>().launching) {

                    // Handle different input methods
                    if(positions.Count > 1) {

                        // The finger has been held down, the ball can be thrown immediately
                        other.GetComponent<NewBall>().GetCaughtAndThrown(immediateThrowVector);
                        HandleDeath();
                    } else {

                        // The finger tapped directly on the ball, catch it and drag to throw it
                        other.GetComponent<NewBall>().GetCaught();
                        catchPosition = transform.position;
                        ball = other.GetComponent<NewBall>();
                    }
                }
            }
        }
    }

    void HandleDeath() {
        Destroy(gameObject);
    }


    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, .1f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(lastFramePos, .1f);    
    }

    void OnGUI() {
        
    }
}
