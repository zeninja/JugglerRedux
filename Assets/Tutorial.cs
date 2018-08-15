using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    TutorialState state;
    enum TutorialState
    {
        intro, firstBall, ballCaught, nowDrag, ballThrown, sloMo, juggled, done
    };

    int textIndex = 0;

	public bool useTutorial = true;

    // Use this for initialization
    void Start()
    {
        state = TutorialState.intro;

        EventManager.StartListening("BallCaught", OnBallCaught);
        EventManager.StartListening("BallThrown", OnBallThrown);

		if(useTutorial) {
			StartTutorial();
		} else {
			StartCoroutine(Autospawner());
		}
    }

    public void StartTutorial()
    {
		StopAllCoroutines();
	    StartCoroutine(TutorialProcess());
    }

    IEnumerator TutorialProcess()
    {
        while (state != TutorialState.done)
        {
            switch (state)
            {
                case TutorialState.intro:
                    yield return StartCoroutine(Intro());
                    SetState(TutorialState.firstBall);
                    break;

                case TutorialState.firstBall:
                    ActivateText();
                    yield return StartCoroutine(FirstBall());
			        SetState(TutorialState.ballCaught);
                    break;

                case TutorialState.ballCaught:
                    ActivateText();
                    yield return StartCoroutine(Nice());
					SetState(TutorialState.nowDrag);
                    break;
				
				case TutorialState.nowDrag:
					ActivateText();
					yield return StartCoroutine(WaitForDrag());
					break;

                case TutorialState.ballThrown:

                    break;

                case TutorialState.sloMo:

                    break;

                case TutorialState.juggled:

                    break;


            }

            yield return new WaitForEndOfFrame();
        }
    }

	void Update() {

	}

    IEnumerator Intro()
    {
        // Hey!
        ActivateText();
        yield return new WaitForSeconds(1f);
        //Welcome to
        ActivateText();
        yield return new WaitForSeconds(1f);
        // Easy Juggling!
        ActivateText();
        yield return new WaitForSeconds(1.5f);
		DeactivateText();
		yield return new WaitForSeconds(.66f);

        // I'm gonna throw a ball in a second, okay?
        ActivateText();
        yield return new WaitForSeconds(1.5f);
		DeactivateText();
		yield return new WaitForSeconds(.25f);
    }

    public GameObject[] text;
    bool ballCaught = false;
    bool juggled = false;

    IEnumerator FirstBall()
    {
		yield return new WaitForSeconds(1.5f);

        while (!ballCaught)
        {
            if (NewBallManager._ballCount < 1 && !NewGameManager.GameOver())
            {
                yield return new WaitForSeconds(Random.Range(.2f, .5f));
                EventManager.TriggerEvent("SpawnBall");
            }

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

	IEnumerator Autospawner() {
		while (true)
        {
            if (NewBallManager._ballCount != 1 && !NewGameManager.GameOver())
            {
                yield return new WaitForSeconds(Random.Range(.2f, .5f));
                EventManager.TriggerEvent("SpawnBall");
            }

            yield return new WaitForEndOfFrame();
        }
	}

    IEnumerator Nice()
    {
        yield return new WaitForSeconds(1f);
		DeactivateText();

    }

    IEnumerator TimeSlow()
    {
        yield return null;
    }

	bool handDragged = false;

	IEnumerator WaitForDrag() {
		while (!handDragged) {
            if (NewBallManager._ballCount != 1 && !NewGameManager.GameOver())
            {
                yield return new WaitForSeconds(Random.Range(.2f, .5f));
                EventManager.TriggerEvent("SpawnBall");
            }

			yield return new WaitForEndOfFrame();
		}
	}

    void ActivateText()
    {
        // Activates a game object, deactivates the previous one, and increments the index

        float duration = .25f;

        int index = textIndex;

        if (index > 0)
        {
            // text[index - 1].AnimateOut();
            text[index - 1].SetActive(false);
        }

		if(index + 1 < text.Length) {
			text[index].SetActive(true);
			textIndex++;
		}
    }

	void DeactivateText() {
		text[textIndex-1].SetActive(false);
	}

    void ActivateOptionalText()
    {

    }

    void SetState(TutorialState s)
    {
        state = s;
    }

    void OnBallCaught()
    {
        if (!ballCaught)
        {
            ballCaught = true;
        }
        else
        {
            // juggled = true  ;
        }
    }

    void OnBallThrown()
    {
		SetState(TutorialState.ballThrown);
    }
}
