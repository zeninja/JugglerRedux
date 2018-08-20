using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandType { holdAndThrow, throwImmediately };

public class NewHandManager : MonoBehaviour
{
    public static HandType _globalHandType = HandType.throwImmediately;

    #region instance
    private static NewHandManager instance;

    public static NewHandManager GetInstance()
    {
        return instance;
    }
    #endregion

    public NewHand m_HandPrefab;

    public float mouseGrabThrowForce = 4;
    public float mouseSlapThrowForce = 10;

    public float touchGrabThrowForce = 4;
    public float touchSlapThrowForce = 10;

    List<int> m_FingerIdList = new List<int>();

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

    void Start()
    {
        InitSavedInfo();
    }

    void InitSavedInfo()
    {
#if UNITY_EDITOR
        PlayerPrefs.SetFloat("touchSlapForce", touchSlapThrowForce);
        PlayerPrefs.SetFloat("touchGrabForce", touchGrabThrowForce);
#endif

        if (PlayerPrefs.HasKey("touchSlapForce"))
        {
            touchSlapThrowForce = PlayerPrefs.GetFloat("touchSlapForce");
        }
        else
        {
            PlayerPrefs.SetFloat("touchSlapForce", touchSlapThrowForce);
        }

        if (PlayerPrefs.HasKey("touchGrabForce"))
        {
            touchGrabThrowForce = PlayerPrefs.GetFloat("touchGrabForce");
        }
        else
        {
            PlayerPrefs.SetFloat("touchGrabForce", touchGrabThrowForce);
        }
    }

    void Update()
    {
        #if UNITY_EDITOR
        HandleMouseInput();
        #else
        HandleTouchInput();
        #endif
    }

    void HandleTouchInput()
    {
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                SpawnTouchHand(t);
            }
        }
    }

    public void RemoveID(int fingerId)
    {
        // Debug.Log("Removing finger IDs");
        m_FingerIdList.Remove(fingerId);
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log("Mouse spawning hand");
            SpawnHand();
        }
    }

    void SpawnTouchHand(Touch t)
    {
        NewHand hand = Instantiate(m_HandPrefab);
        hand.m_FingerID = t.fingerId;
        // Debug.Log("Spawning hand. FINGER ID: " + t.fingerId);
    }

    void SpawnHand()
    {
        NewHand hand = Instantiate(m_HandPrefab);

        #if UNITY_EDITOR
        hand.useMouse = true;
        #endif

        // if (NewGameManager.GetInstance().spawnBallsByTouchCount)
        // {
        //     if (NewBallManager._ballCount < Input.touchCount)
        //     {
        //         EventManager.TriggerEvent("SpawnBall");
        //     }
        // }
    }

    public static int GetCurrentFingerIDCount()
    {
        return NewHandManager.GetInstance().m_FingerIdList.Count;
    }

    private void OnDrawGizmos()
    {

        foreach (Touch t in Input.touches)
        {
            Gizmos.DrawWireSphere(t.position, .5f);
        }
    }
}