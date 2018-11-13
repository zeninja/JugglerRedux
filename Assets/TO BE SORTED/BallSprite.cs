using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSprite : MonoBehaviour
{

    NewBall m_Ball;
    public SpriteRenderer m_Sprite;

    public SpriteMask peakMask;
    public SpriteRenderer peakSprite;

    public Sprite hardSprite;

    public float peakSpriteTint = .5f;

    // Use this for initialization
    void Start()
    {
        m_Ball = GetComponentInParent<NewBall>();
        // m_Sprite = GetComponent<SpriteRenderer>();

        // peakSprite.color = GetComponentInParent<NewBallArtManager>().myColor * modifier;
        peakSprite.color = GetComponent<SpriteRenderer>().color * .5f;

        EventManager.StartListening("CleanUp", DisableSprite);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_Sprite.enabled = m_Ball.m_State == NewBall.BallState.rising ||
                           m_Ball.m_State == NewBall.BallState.falling ||
                           m_Ball.m_State == NewBall.BallState.firstBall ||
                           m_Ball.m_State == NewBall.BallState.launching;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(Peak());
        }
        peakSprite.color = GetComponent<SpriteRenderer>().color * peakSpriteTint;

    }

    // Called via Broadcast message
    public void HandlePeak()
    {
        // peakSprite.color = m_Sprite.color;
        StartCoroutine(Peak());
    }

    void DisableSprite()
    {
        gameObject.SetActive(false);
    }

    public void UpdateToHard()
    {
        m_Sprite.sprite = hardSprite;
        peakSprite.sprite = hardSprite;
    }

    public float peakDuration;

    public float maskDuration = .15f;
    public float spriteDuration = .15f;

    public float maxMaskScale;

    IEnumerator MaskIn()
    {
        peakMask.transform.localScale = Vector2.zero;
        peakMask.enabled = true;

        float t = 0;
        float d = maskDuration;

        while (t < d)
        {
            t += Time.fixedDeltaTime;
            float p = t / d;
            peakMask.transform.localScale = Vector2.one * EZEasings.SmoothStop3(p) * maxMaskScale;
            yield return new WaitForFixedUpdate();
        }

		t = 0;
        d = maskDuration;

        while (t < d)
        {
            t += Time.fixedDeltaTime;
            float p = t / d;
            peakMask.transform.localScale = Vector2.one - Vector2.one * EZEasings.SmoothStop3(p);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator SpriteIn()
    {
        peakSprite.transform.localScale = Vector2.zero;
        peakSprite.enabled = true;

        float t = 0;
        float d = spriteDuration;

        while (t < d)
        {
            t += Time.fixedDeltaTime;
            float p = t / d;
            peakSprite.transform.localScale = Vector2.one * EZEasings.SmoothStart2(p) * maxMaskScale;
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Peak()
    {
		yield return StartCoroutine(MaskIn());
		yield return StartCoroutine(SpriteIn());

		peakMask.enabled = false;
        peakSprite.enabled = false;
    }

    public void AdjustDepth(int index)
    {
        m_Sprite.sortingOrder   = index;
        peakSprite.sortingOrder = index + 1;
    }
}
