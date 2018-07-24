using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHand : MonoBehaviour
{
    #region Instance 
    private static NewHand instance;

    public static NewHand GetInstance()
    {
        return instance;
    }
    #endregion

    Transform myTransform;

    Vector2 lastFramePos;
    Vector2 instantMoveDir;
    Vector2 slapThrowVector;
    Vector2 grabMoveDir;
    Vector2 grabThrowVector;
    Vector2 catchPosition;

    NewBall ball;

    public bool useMouse = false; 
    public int fingerId;

    List<Vector2> positions;
    List<Vector2> moveDirections;

    [System.NonSerialized] public float grabThrowForce = 4;
    [System.NonSerialized] public float slapThrowForce = 10;

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

        grabThrowForce = NewHandManager.GetInstance().heldThrowForce;
        slapThrowForce = NewHandManager.GetInstance().immediateThrowForce;

        positions = new List<Vector2>();
        moveDirections = new List<Vector2>();
    }

    void Update()
    {
        // dEBUG
        slapThrowForce = NewHandManager.GetInstance().immediateThrowForce;
        MoveHand();

        if (ball != null) {
            ball.currentThrowVector = grabThrowVector;
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
        slapThrowVector = instantMoveDir * slapThrowForce;

        positions.Add(transform.position);
        moveDirections.Add(instantMoveDir);

        if(Input.GetMouseButton(0)) {
            if(ball != null) {
                grabMoveDir = (Vector2)transform.position - catchPosition;
                grabThrowVector = grabMoveDir * grabThrowForce;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (ball != null) {
                // Debug.Log("tryna throw");
                // heldThrowVector = heldMoveDir * heldThrowForce;
                ball.GetThrown(grabThrowVector);
                ball = null;
                HandleDeath();
            }
        }
    }

    void HandleTouchInput() 
    {
        foreach(Touch t in Input.touches) {
            if (t.fingerId == fingerId) {

                switch(t.phase) {
                    case TouchPhase.Began:
                        CheckForGrab();
                        break;
                    case TouchPhase.Moved:
                        instantMoveDir = t.deltaPosition;
                        CheckForGrab();
                        break;
                    case TouchPhase.Stationary:
                        CheckForGrab();
                        break;
                    case TouchPhase.Ended:
                        TryThrowBall();
                        break;
                    case TouchPhase.Canceled:
                        break;
                }
            }
        }
    }

    void CheckForGrab() {
        float radius = 0.5f;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, Vector2.zero);

        // Only catch one ball at a time
        if (hit && ball == null) {
            if(hit.collider.gameObject.CompareTag("Ball")) {
                NewBall ballToJuggle = hit.collider.gameObject.GetComponent<NewBall>();

                // only catch balls that are not launching
                if(!ballToJuggle.launching) {
                    
                    // Handle different input methods
                    if(positions.Count > 1) {
                        
                        // The finger has been held down, the ball can be thrown immediately
                        ballToJuggle.GetComponent<NewBall>().GetCaughtAndThrown(slapThrowVector);
                        HandleDeath();
                    } else {
                        // The finger tapped directly on the ball, catch it and drag to throw it
                        ballToJuggle.GetComponent<NewBall>().GetCaught();
                        catchPosition = transform.position;
                        ball = ballToJuggle;
                    }
                }
            }
        }

        if(ball != null) {
            grabThrowVector = grabMoveDir * grabThrowForce;
        }
    }

    void TryThrowBall() {
        if (ball != null) {
            ball.GetThrown(grabThrowVector);
            ball = null;
            HandleDeath();
        }
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     // Only allow one ball to be affected at a time
    //     if(ball == null) {

    //         if (other.CompareTag("Ball")) {

    //             // Only catch balls that are not launching
    //             if(!other.GetComponent<NewBall>().launching) {

    //                 // Handle different input methods
    //                 if(positions.Count > 1) {

    //                     // The finger has been held down, the ball can be thrown immediately
    //                     other.GetComponent<NewBall>().GetCaughtAndThrown(slapThrowVector);
    //                     HandleDeath();
    //                 } else {

    //                     // The finger tapped directly on the ball, catch it and drag to throw it
    //                     other.GetComponent<NewBall>().GetCaught();
    //                     catchPosition = transform.position;
    //                     ball = other.GetComponent<NewBall>();
    //                 }
    //             }
    //         }
    //     }
    // }

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
