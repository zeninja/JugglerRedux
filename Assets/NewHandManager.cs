using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHandManager : MonoBehaviour
{
    #region instance
    private static NewHandManager instance;

    public static NewHandManager GetInstance()
    {
        return instance;
    }
    #endregion

    public GameObject handPrefab;

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

    private void Update()
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

    void SpawnHand(int fingerId)
    {
        GameObject hand = Instantiate(handPrefab) as GameObject;
        hand.GetComponent<NewHand>().fingerId = fingerId;

        if (NewGameManager.GetInstance().spawnBallsByTouchCount) {
            if(NewBallManager._ballCount < Input.touchCount) {
    			EventManager.TriggerEvent("SpawnBall"); 
            }
        }
    }

    public void RemoveId(int fingerId) {
        fingerIds.Remove(fingerId);
    }
}
