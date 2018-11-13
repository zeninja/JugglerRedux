using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallArtManager : MonoBehaviour
{

    List<Vector3> m_LinePointList;
    List<Vector3> predictedPointList;
    List<Vector3> launchPointList = new List<Vector3>();

    NewBall m_Ball;
    Rigidbody2D m_Rigidbody;

    GrabSquishLine grabSquishLine;

    public SpriteRenderer m_BallSprite;
    public SpriteRenderer m_BallBackground;
    
    [System.NonSerialized] public Color myColor;
    [System.NonSerialized] public int ballIndex;
    public BallPredictor m_BallPredictor;

    bool initialized;

    // Use this for initialization
    void Start()
    {
        grabSquishLine  = GetComponent<GrabSquishLine>();
        m_Ball          = GetComponentInParent<NewBall>();
        m_Rigidbody     = GetComponentInParent<Rigidbody2D>();
        m_BallSprite    = GetComponentInChildren<SpriteRenderer>();

        m_LinePointList    = new List<Vector3>();
        predictedPointList = new List<Vector3>();

        CheckLaunch();

        initialized = true;
    }

    void CheckLaunch() {
        if(m_Ball.IsLaunching()) {
            GetLaunchLine();
        }
    }

    public void SetInfo(int newIndex)
    {
        ballIndex = newIndex;

        SetColor();
    }

    public int currentDepth;

    public void SetDepth(int sortIndex) {
        currentDepth = sortIndex;
        BroadcastMessage("AdjustDepth", sortIndex, SendMessageOptions.DontRequireReceiver);
    }

    public void SetColor()
    {
        ballIndex = Mathf.Clamp(ballIndex, 0, 8);
        myColor = NewBallManager.GetInstance().m_BallColors[ballIndex];
        // m_BallSprite.color = myColor;
        m_BallBackground.color = myColor;

        GetComponent<SpriteCircleEffectSpawner>().effectColor = myColor;
    }

    public void SetColor(Color newColor)
    {
        // overload method for setting the color directly
        myColor = newColor;
        // m_BallSprite.color = myColor;
        m_BallBackground.color = myColor;
    }

    void HandleCatch() {
        BallDepthManager.GetInstance().UpdateBallDepth(this);
    }

    // void GetPredictedLine(Vector2 throwVector)
    // {
    //     Vector2 anchorPos = transform.position;
    //     predictedPointList = m_BallPredictor.GetPositionList(anchorPos, throwVector);
    // }

    void GetLaunchLine()
    {
        Vector2 launchPos    = transform.position;
        Vector2 launchVector = Vector2.up * NewBallManager.GetInstance().ballLaunchForce;

        // Debug.Log(m_BallPredictor);

        launchPointList = m_BallPredictor.GetPositionList(launchPos, launchVector);
    }

    Color normalStageColor;

    public void UpdateToNormal() {
        int lastIndex = NewBallManager.GetInstance().m_BallColors.Length - 1;
        normalStageColor = NewBallManager.GetInstance().m_BallColors[lastIndex];
        GetComponent<DotTrail>().TriggerEndgame();
        SetColor(normalStageColor);
    }

    public void UpdateToHard() {
        m_BallSprite.GetComponent<BallSprite>().UpdateToHard();
    }

    public bool VelocityPositive()
    {
        if (m_Rigidbody == null) { return false; }
        return m_Rigidbody.velocity.y > 0;
    }
}
