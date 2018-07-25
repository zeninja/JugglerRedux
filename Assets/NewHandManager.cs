using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HandType { holdAndThrow, throwImmediately };

public class NewHandManager : MonoBehaviour
{
    public static HandType _globalHandType = HandType.throwImmediately;
    public UnityEngine.UI.Toggle handTypeToggle;

    #region instance
    private static NewHandManager instance;

    public static NewHandManager GetInstance()
    {
        return instance;
    }
    #endregion

    public GameObject handPrefab;

    public float heldThrowForce = 4;
    public float immediateThrowForce = 10;

    List<int> fingerIds = new List<int>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start() {
        _globalHandType = handTypeToggle.isOn ? HandType.throwImmediately : HandType.holdAndThrow;
    }

    void Update()
    {
        HandleTouchInput();
        HandleMouseInput();
    }

    void HandleTouchInput()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                int fingerId = Input.GetTouch(i).fingerId;
                if (!fingerIds.Contains(fingerId))
                {
                    fingerIds.Add(fingerId);
                    SpawnHand(fingerId);
                }
            }
        }
    }

    public void RemoveID(int fingerId) {
        fingerIds.Remove(fingerId);
    }

    void HandleMouseInput() 
    {
        if(Input.GetMouseButtonDown(0)) {
            SpawnHand();
        }
    }

    void SpawnHand(int fingerId)
    {
        GameObject hand = Instantiate(handPrefab) as GameObject;
        hand.GetComponent<NewHand>().fingerId = fingerId;

        // if (NewGameManager.GetInstance().spawnBallsByTouchCount)
        // {
        //     if (NewBallManager._ballCount < Input.touchCount)
        //     {
        //         EventManager.TriggerEvent("SpawnBall");
        //     }
        // }
    }

    void SpawnHand() {
        GameObject hand = Instantiate(handPrefab) as GameObject;

        #if UNITY_EDITOR
        hand.GetComponent<NewHand>().useMouse = true;
        #endif

        if (NewGameManager.GetInstance().spawnBallsByTouchCount)
        {
            if (NewBallManager._ballCount < Input.touchCount)
            {
                EventManager.TriggerEvent("SpawnBall");
            }
        }
    }

    public void AdjustThrowForce(float amt) {
        immediateThrowForce += amt;
    }

    // public void RemoveId(int fingerId)
    // {
    //     fingerIds.Remove(fingerId);
    // }
}
