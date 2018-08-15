using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEffect : MonoBehaviour
{
    Color myColor;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            HandleDeath();
        }
    }


    void SetColor(Color newColor)
    {
        myColor = newColor;
    }

    // GAME OVER!!
    public void HandleDeath()
    {
        Debug.Log("Handlin death");

        exploding = true;
        transform.localScale = Vector3.one;

        // myColor = NewBallManager.GetInstance().deadBallColor;
        myColor = Color.black;

        SetColor(myColor);

        StartCoroutine(GameOver());
    }

    bool exploding, waiting, imploding;
    public float targetScale = 40;
    // public float spreadRate;
    // public float recedeRate;

    public float explodeDuration;
    public float waitDuration;
    public float implodeDuration;

    IEnumerator GameOver()
    {

        float t = 0;

        while (exploding)
        {
            if (t < explodeDuration)
            {
                // Debug.Log("growing");
                t += Time.deltaTime;
                transform.localScale = Vector3.one + Vector3.one * targetScale * EZEasings.SmoothStart5(t / explodeDuration);
                yield return new WaitForEndOfFrame();
            } else {
                exploding = false;
                waiting = true;
            }
        }
        
        EventManager.TriggerEvent("Reset");

        // wait at max scale
        t = 0;
        while (waiting)
        {
            if (t < waitDuration)
            {
                // Debug.Log("waiting");
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            } else {
                waiting = false;
                imploding = true;
            }
        }

        Vector3 largeScale = transform.localScale;

        t = 0;
        while (imploding)
        {
            if (t < implodeDuration)
            {
                // Debug.Log("imploding");
                t += Time.deltaTime;
                transform.localScale = largeScale - largeScale * EZEasings.SmoothStop5(t / implodeDuration);

                yield return new WaitForEndOfFrame();
            } else {
                transform.localScale = Vector3.zero;
                imploding = false;
            }
        }

        NewGameManager.GetInstance().ResetGame();

        Destroy(gameObject);
    }
}
