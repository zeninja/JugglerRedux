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

    public HandType handType;

    Transform myTransform;

    [System.NonSerialized]
    public float heldThrowForce = 4;
    [System.NonSerialized]
    public float immediateThrowForce = 10;

    Touch myTouch;

    // Movement
    Vector2 moveDir;
    // Vector2[] collectionOfPrevPositions = new Vector2[3];
    Vector2 catchPosition;
    Vector2 lastFramePos;
    Vector2 throwVector;

    NewBall caughtBall;    

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
        line = GetComponent<LineRenderer>();

        handType = NewHandManager._globalHandType;
        heldThrowForce = NewHandManager.GetInstance().heldThrowForce;
        immediateThrowForce = NewHandManager.GetInstance().immediateThrowForce;
    }

    void Update()
    {
        MoveHand();
    }

    void MoveHand()
    {
        if (useMouse)
        {
            HandleMouseInput();
        }
        else
        {
            HandleTouchInput();
        }

        DrawLine();
    }

    void HandleTouchInput()
    {
        if(Input.touchCount != 0) {
            moveDir = Input.GetTouch(0).deltaPosition;
            transform.position = Extensions.ScreenToWorld(Input.GetTouch(0).position);
            throwVector = moveDir * immediateThrowForce;
        }

        if(Input.GetTouch(0).phase == TouchPhase.Ended) {
            HandleDeath();
        }

        #region old
    //     if (Input.touchCount > 0)
    //     {
    //         for (int i = 0; i < Input.touchCount; i++)
    //         {
    //             if (Input.touches[i].fingerId == fingerId)
    //             {
    //                 myTouch = Input.touches[i];
    //                 break;
    //             }
    //         }

    //         myTransform.position = Extensions.ScreenToWorld(myTouch.position);
    //         moveDir = (Vector2)myTransform.position - lastPos;
    //         lastPos = myTransform.position;

    //         if (moveDir != Vector2.zero)
    //         {
    //            // normalizedMoveDirection = moveDir.normalized;
    //         }

    //         if (myTouch.phase == TouchPhase.Ended)
    //         {
    //             HandleDeath();
    //         }
    //     }
        #endregion
    }

    public int pastMouseIndex = 10;

    void HandleMouseInput()
    {
        #region Calculate movement
        transform.position = Extensions.MouseScreenToWorld();


        Vector2 screenMousePos = (Vector2)Input.mousePosition;
        Vector2 pastMousePos   = (Vector2) mousePositions[pastMouseIndex];

        moveDir = pastMousePos - screenMousePos;
        lastFramePos = Input.mousePosition;

        line.SetPosition(0, pastMousePos);
        line.SetPosition(1, screenMousePos);


        // Debug.Log(moveDir);

        CalculateAveragedMoveDir();
    

        #endregion

        #region Find throw direction
        // if(caughtBall) {
            switch(handType) {
                case HandType.holdAndThrow:
                    throwVector = ((Vector2)transform.position - catchPosition) * heldThrowForce;
                    break;
                case HandType.throwImmediately:
                    throwVector = moveDir * immediateThrowForce;
                    Debug.Log(throwVector);

                    break;
            }
        // }
        #endregion

        #region Handle throws
        if (Input.GetMouseButtonUp(0))
        {
            if(caughtBall) {
                caughtBall.GetThrown(throwVector);
                caughtBall = null;
            }
            HandleDeath();
        }
        #endregion
    }

    Vector3[] mousePositions = new Vector3[10];
    Vector3[] linePositions = new Vector3[10];

    int numPositionsToAverage = 10;

    void CalculateAveragedMoveDir() {
        for(int i = numPositionsToAverage - 1; i > 0; i--) {
            mousePositions[i] = mousePositions[i - 1];
            linePositions[i]  = linePositions [i - 1];
        }
        mousePositions[0] = moveDir;
        linePositions[0]  = transform.position;



    }

    LineRenderer line;
    void DrawLine() {
        // line.SetPosition(0, transform.position);
        // line.SetPosition(1, (Vector2)transform.position + throwVector);

        // Vector3[] linePositions = new Vector3[10];
        // for(int i = 0; i < mousePositions.Length; i++) {
        //     linePositions[i] = transform.position + mousePositions[i];
        // }

        // line.SetPositions(linePositions);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("hit a thing");

            caughtBall = other.GetComponent<NewBall>();
            // HandleBallCaught();

            if(handType == HandType.throwImmediately) {
                Debug.Log("immed");
                Debug.Log(caughtBall);
                // caughtBall.GrabBall();
                // caughtBall.GetThrown(throwVector);
                caughtBall.GetCaughtAndThrown(throwVector);
                caughtBall = null;
            }
        }
    }

    // void HandleBallCaught() {
    //     Debug.Log("Handling ball cauht");
    //     caughtBall.GrabBall();
    //     catchPosition = transform.position;
    //     // Invoke("HandleBallThrown", Time.deltaTime);
    //     HandleBallThrown();
    // }

    // void HandleBallThrown() {
    //     if(handType == HandType.throwImmediately) {
    //         caughtBall.GetThrown(throwVector);
    //         Debug.Log(throwVector);
    //         caughtBall = null;
    //     }
    // }

    void HandleDeath()
    {
        // NewHandManager.GetInstance().RemoveId(fingerId);
        Destroy(gameObject);
    }

    public float GetThrowForce() {
        return immediateThrowForce;
    }

    void OnDrawGizmos()
    {
        // Gizmos.color = Color.black;
        // Gizmos.DrawLine(myTransform.position, (Vector2)myTransform.position + moveDir.normalized * heldThrowForce * moveDir.magnitude);
    }
}
