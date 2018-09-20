using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpriteMask : MonoBehaviour
{

    NewBall m_Ball;
	NewBallArtManager art;

    public float defaultScale = 1.45f;

	void Start() {
		m_Ball = GetComponentInParent<NewBall>();
		art    = GetComponentInParent<NewBallArtManager>();
	}

    // Update is called once per frame
    void Update()
    {
		if (NewGameManager.GameOver())
        {
            transform.localScale = Vector3.zero;
			return;
        }

        switch (m_Ball.m_State)
        {
            case NewBall.BallState.rising:
				// UpdateWidth(art.currentWidth);
			    break;
            case NewBall.BallState.falling:
                UpdateWidth(defaultScale);
			    break;
            case NewBall.BallState.caught:

                break;
        }


    }

    public void UpdateWidth(float maskScale)
    {
        transform.localScale = Vector2.one * maskScale;
    }
}
