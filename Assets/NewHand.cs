using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHand : MonoBehaviour
{
    Transform myTransform;

    public float throwForce = 4;

    // [System.NonSerialized]
    public int fingerId;

    Vector2 moveDir;
    Vector2 throwDir;
    Vector2 currentPos, lastPos;

    Touch myTouch;

    void Awake()
    {
        myTransform = transform;
    }

    void Update()
    {
        MoveHand();
    }

    void MoveHand() {
        if (Input.touchCount > 0)
        {
            for(int i = 0; i < Input.touchCount; i++) {
                if (Input.touches[i].fingerId == fingerId) {
                    myTouch = Input.touches[i];
                    break;
                }
            }

            myTransform.position = Extensions.ScreenToWorld(myTouch.position);
            moveDir = (Vector2)myTransform.position - lastPos;
            lastPos = myTransform.position;
            
            if (moveDir != Vector2.zero)
            {
                throwDir = moveDir.normalized;
            }

            if(myTouch.phase == TouchPhase.Ended) {
                HandleDeath();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            other.GetComponent<NewBall>().HandleCatch(throwDir, throwForce, moveDir);
            HandleDeath();
        }
    }

    void HandleDeath() {
        NewHandManager.GetInstance().RemoveId(fingerId);
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(myTransform.position, (Vector2)myTransform.position + throwDir);
    }
}
