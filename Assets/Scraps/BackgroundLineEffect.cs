using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLineEffect : MonoBehaviour
{
	#region
    public static BackgroundLineEffect instance;
    public static BackgroundLineEffect GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (!instance)
            {
                Destroy(gameObject);
            }
        }
    }
	#endregion

    public BackgroundLine line;
    public int numLines = 30;
    public float scrollSpeed;
    public float a, b;
    public float lineWidth = .1f;
    public int lineResolution = 100;

    List<BackgroundLine> lines;

    public Color color;


    // Use this for initialization
    void Start()
    {
        lines = new List<BackgroundLine>();
        SpawnLines();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLines();

        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     SetBGLineAlpha(.5f);
        // }

		// SetBGLineAlpha(TimeManager.timeScalePercent);
    }



    void SpawnLines()
    {
        for (int i = 0; i <= numLines; i++)
        {
            BackgroundLine newLine = Instantiate(line);
            newLine.SetValues(scrollSpeed, a, b, lineWidth);

            newLine.transform.position = new Vector3(0, i * ResolutionCompensation.WorldUnitsInCamera.y / numLines,  -1);
            lines.Add(newLine);
        }
    }

    void UpdateLines()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].transform.position = new Vector3(0, i * ResolutionCompensation.WorldUnitsInCamera.y / numLines, 0);
            lines[i].SetValues(scrollSpeed, a, b, lineWidth, lineResolution);
            lines[i].SetColor(color);
        }
    }

	public float maxAlpha = 124;
	public float minAlpha = 0;

    public void SetBGLineAlpha(float percent)
    {
        float alphaRange = maxAlpha - minAlpha;
        float targetAlpha = percent * alphaRange;

        color.a = targetAlpha;
    }
}
