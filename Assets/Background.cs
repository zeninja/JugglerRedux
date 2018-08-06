using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    public SpriteRenderer m_Bar;
    public int numBars = 30;

    List<SpriteRenderer> odd;
    List<SpriteRenderer> even;
    List<SpriteRenderer> all;

    List<bool> values;

    public float offset;

    public int flashFrameCount;
    int framesElapsed;

    bool canSwitch = true;

    // Use this for initialization
    void Start()
    {
        odd = new List<SpriteRenderer>();
        even = new List<SpriteRenderer>();
        all = new List<SpriteRenderer>();
        values = new List<bool>();

        SpawnBars();
    }

    void SpawnBars()
    {
        for (int i = 0; i < numBars; i++)
        {
            SpriteRenderer bar = Instantiate(m_Bar);
            bar.transform.position = new Vector2(0, i * m_Bar.size.y * m_Bar.transform.localScale.y - offset);

            if (i % 2 == 0)
            {
                even.Add(bar);
            }
            else
            {
                odd.Add(bar);
            }
            all.Add(bar);

            values.Add(i % 2 == 0);
        }
    }

    void Update()
    {
        for (int i = 0; i < numBars; i++)
        {
            SpriteRenderer bar = all[i];

            bar.transform.position = new Vector2(0, i * m_Bar.size.y * m_Bar.transform.lossyScale.y - offset);
            // all.Add(bar);
        }

        if (Input.GetKey(KeyCode.F) || NewBallManager.GetInstance().JuggleThresholdReached())
        {
            // StartCoroutine("Flash");

            if (canSwitch)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    values[i] = !values[i];
                    SpriteRenderer s = all[i];
                    s.color = values[i] ? Color.white : Color.black;
                    canSwitch = false;

                }
            }
            else
            {
                framesElapsed++;
                if (framesElapsed >= flashFrameCount)
                {
                    canSwitch = true;
                    framesElapsed = 0;

                }

            }
        }
    }
    
    // public int flashFrameCount = 1;

    // IEnumerator Flash() {
    // 	for(int i = 0; i < all.Count; i++) {
    // 		values[i] = !values[i];
    // 		all[i].color = values[i] ? Color.white : Color.black;
    // 	}

    // 	for(int i = 0; i < flashFrameCount; i++)  {
    // 		yield return new WaitForEndOfFrame();
    // 	}
    // }


}
