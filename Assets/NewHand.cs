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

    // public HandType handType;

    Transform myTransform;

    [System.NonSerialized]
    public float heldThrowForce = 4;
    [System.NonSerialized]
    public float immediateThrowForce = 10;

    Touch myTouch;

    // Movement
    Vector2 moveDir;
    Vector2 catchPosition;
    Vector2 lastFramePos;
    Vector2 throwVector;

    public bool useMouse = false;

    void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            if (this != instance) {
                Destroy(gameObject);
            }
        }

        myTransform = transform;

        // handType = NewHandManager._globalHandType;
        // heldThrowForce = NewHandManager.GetInstance().heldThrowForce;
        immediateThrowForce = NewHandManager.GetInstance().immediateThrowForce;

        positions = new List<Vector2>();
        moveDirections = new List<Vector2>();
    }

    void Update()
    {
        immediateThrowForce = NewHandManager.GetInstance().immediateThrowForce;
        MoveHand();
    }

    void MoveHand()
    {
        if (useMouse) {
            HandleMouseInput();
        } else {
            HandleTouchInput();
        }
    }

    List<Vector2> positions;
    List<Vector2> moveDirections;

    void HandleMouseInput()
    {
        // Step 1: Set the position to be equal to the current mouse position
        transform.position = Extensions.MouseScreenToWorld();
        
        // Step 2: Find the move direction since last frame
        if(positions.Count > 1) {
            moveDir = (Vector2)transform.position - lastFramePos;
        }
        Debug.DrawLine(transform.position, (Vector2)transform.position + moveDir.normalized * 2, Color.red);
        lastFramePos = transform.position;

        // Step 3: Find the throw vector
        throwVector = moveDir * immediateThrowForce;

        positions.Add(transform.position);
        moveDirections.Add(moveDir);
    }

    void HandleTouchInput() 
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            if(!other.GetComponent<NewBall>().launching) {
                // Debug.Log("hit a thing");
                other.GetComponent<NewBall>().GetCaughtAndThrown(throwVector);
                
                // Debug.Log("Move dir: " + moveDir + "; lastframePos" +  lastFramePos);
                // Debug.Log("Throw vector: " + throwVector);
                HandleDeath();
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
}
