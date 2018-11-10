using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallArt : MonoBehaviour
{
    NewBall ball;
    NewBallArtManager ballArtManager;

    LineRenderer line;
    BallBGMask mask;

    List<Vector3> trailPositions;

    float ballScale;

    // Use this for initialization
    void Awake()
    {
        trailPositions = new List<Vector3>();
        line = GetComponent<LineRenderer>();

        ballArtManager = GetComponentInParent<NewBallArtManager>();
        ball           = GetComponentInParent<NewBall>();
        mask           = GetComponentInChildren<BallBGMask>();
    }

    void Start()
    {
		ballScale = NewBallManager.GetInstance().ballScale;

        line.startWidth = ballScale;
        line.endWidth   = ballScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (ballArtManager.VelocityPositive())
        {

            trailPositions.Add(transform.position);
            DrawTrail();

            line.material.color = ballArtManager.myColor;
            line.material.color = ballArtManager.myColor;
            line.enabled = true;
        }
        else
        {
            if (ball.m_State == NewBall.BallState.caught)
            {
                line.enabled = false;
            }
            else
            {
                trailPositions.Clear();
                line.positionCount = 2;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, transform.position);

                line.enabled = true;
            }
        }
    }

    public int trailLength;
    Vector3[] trail;

    void DrawTrail()
    {
        trail = new Vector3[trailLength];
        for (int i = 0; i < trailLength; i++)
        {
            int index = trailPositions.Count - i - 1;
            index = Mathf.Max(index, 0);
            trail[i] = trailPositions[index];
        }

        line.positionCount = trail.Length;
        line.SetPositions(trail);
    }

    public AnimationCurve popInAnimation;
    public float popInDuration;

    public IEnumerator PopIn()
    {
        float t = 0;
        float d = popInDuration;

        while (t < d)
        {
            t += Time.fixedDeltaTime;
            float lineWidth = ballScale * popInAnimation.Evaluate(t / d);

            UpdateScale(lineWidth);

            // Debug.Log(lineWidth);

            yield return new WaitForFixedUpdate();
        }
    }

    public float hideDuration;

    public IEnumerator HideBall() {
        float t = 0;
        float d = hideDuration;

        while (t < d)
        {
            t += Time.fixedDeltaTime;
            float lineWidth = ballScale - ballScale * EZEasings.SmoothStop3(t / d);

            UpdateScale(lineWidth);
            mask.UpdateScale(lineWidth);
            yield return new WaitForFixedUpdate();
        }
        // Debug.Log("Hid ball");
        GetComponentInParent<NewBall>().DestroyMe();
    }

    void UpdateScale(float val)
    {
        line.startWidth = val;
        line.endWidth = val;
    }

    void AdjustDepth(int index) {
        line.sortingOrder = index;
    }

    public Vector3[] GetTrailPositions() {
        Vector3[] currentLine = new Vector3[line.positionCount];
        line.GetPositions(currentLine);
        return currentLine;
    }
}
