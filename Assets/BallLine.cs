using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLine : MonoBehaviour
{
    NewBall ball;
    NewBallArtManager ballArtManager;
    LineBackgroundMask mask;

    LineRenderer line;

    List<Vector3> trailPositions;

    float targetScale;

    // Use this for initialization
    void Awake()
    {
        trailPositions = new List<Vector3>();
        line = GetComponent<LineRenderer>();
        mask = GetComponentInChildren<LineBackgroundMask>();
    }

    void Start()
    {
		targetScale = NewBallManager.GetInstance().ballScale;
        line.startWidth = targetScale;
        line.endWidth = targetScale;

        ballArtManager = GetComponentInParent<NewBallArtManager>();
        ball = GetComponentInParent<NewBall>();
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

    void LateUpdate() {
        // mask.SetMaskPositions(trail);
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
        // targetScale = NewBallManager.GetInstance().ballScale;

        // Debug.Log("Popping In");

        float t = 0;
        float d = popInDuration;

        while (t < d)
        {
            t += Time.fixedDeltaTime;
            float lineWidth = targetScale * popInAnimation.Evaluate(t / d);

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
            float lineWidth = targetScale - targetScale * EZEasings.SmoothStop3(t / d);

            UpdateScale(lineWidth);
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Hid ball");
        GetComponentInParent<NewBall>().DestroyMe();
    }

    void UpdateScale(float val)
    {
        line.startWidth = val;
        line.endWidth = val;
    }

    public Vector3[] GetTrailPositions() {
        return trail;
    }
}
