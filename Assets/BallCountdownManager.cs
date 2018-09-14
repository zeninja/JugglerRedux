using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCountdownManager : MonoBehaviour
{

	#region instance
    private static BallCountdownManager instance;
    public static BallCountdownManager GetInstance()
    {
        return instance;
    }
    #endregion

    public CountdownTally tally;

    public int countdownToNextball = 5;
    public int framesBetweenBalls = 3;

    public Vector2 anchor = Vector2.zero;

    List<CountdownTally> tallys;

    public CountdownTally nextTally;

    void Awake()
    {
		if(instance == null) {
			instance = this;
		} else {
			if(instance != this) {
				Destroy(gameObject);
			}
		}

        tallys = new List<CountdownTally>();
    }

	void Start() {
		EventManager.StartListening("BallPeaked", PopOutTally);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetCountdownNumber(countdownToNextball);
            // StartCoroutine(SpawnCountdownBalls());
        }

        if (ballsInstantiated)
        {
            UpdateBallPositions();
        }

		if(Input.GetKeyDown(KeyCode.Return)) {
			PopOutTally();
		}

		if(Input.GetKeyDown(KeyCode.P)) {

		}
    }


    public void SetCountdownNumber(int countdownNumber)
    {
		// Debug.Log("Set countdown");
		// Debug.Log(countdownNumber);
        countdownToNextball = countdownNumber;
        StartCoroutine(SpawnCountdownBalls());
    }

    public float xSpacing;
    public float ySpacing;
    int ballsPerLine = 5;


    bool ballsInstantiated = false;
    IEnumerator SpawnCountdownBalls()
    {

        for (int i = 0; i < countdownToNextball; i++)
        {

            CountdownTally c = Instantiate(tally);
            c.transform.parent = transform;


            float x = xSpacing * (i % ballsPerLine);
            float y = ySpacing * Mathf.FloorToInt(i / ballsPerLine);

            Vector2 convertedAnchor = transform.InverseTransformPoint(anchor);

            float xOffset = convertedAnchor.x;
            float yOffset = convertedAnchor.y;

            float xHalf = ((ballsPerLine - 1) * xSpacing) / 2;
            // float yHalf = (((countdownToNextball - 1) / ballsPerLine ) * ySpacing)/2;

            x += xOffset - xHalf;
            // y += yOffset - yHalf;
            y += yOffset;

            c.transform.position = transform.TransformPoint(new Vector2(x, y));

            tallys.Add(c);

            int f = 0;
            while (f < framesBetweenBalls)
            {
                f++;
                yield return new WaitForEndOfFrame();
            }
        }

        nextTally = tallys[tallys.Count - 1];

        ballsInstantiated = true;

		// Debug.Break();
    }

    // util for now
    void UpdateBallPositions()
    {
        // X VALUES		
        // 0 1 2 3 4
        // 0 1 2 3 4
        // 0 1 2 3 4

        // Y VALUES
        // 0 0 0 0 0
        // 1 1 1 1 1
        // 2 2 2 2 2

        for (int i = 0; i < tallys.Count; i++)
        {
            CountdownTally c = tallys[i];
            c.transform.parent = transform;

            float x = xSpacing * (i % ballsPerLine);
            float y = ySpacing * Mathf.FloorToInt(i / ballsPerLine);

            Vector2 convertedAnchor = transform.InverseTransformPoint(anchor);

            float xOffset = convertedAnchor.x;
            float yOffset = convertedAnchor.y;

            float xHalf = ((ballsPerLine - 1) * xSpacing) / 2;
            // float yHalf = (((countdownToNextball - 1) / ballsPerLine ) * ySpacing)/2;

            x += xOffset - xHalf;
            // y += yOffset - yHalf;
            y += yOffset;

            c.transform.position = transform.TransformPoint(new Vector2(x, y));
        }
    }

	void PopOutTally() {
		nextTally.StartPopOutProcess();
		tallys.Remove(nextTally);

		if(tallys.Count > 0) {
			nextTally = tallys[tallys.Count - 1];
		}
	}

	public void SetUpCountdown() {
		Debug.Log("Setting up countdown");
		SetCountdownNumber(5);
	}
}
