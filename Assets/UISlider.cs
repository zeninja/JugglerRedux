using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlider : MonoBehaviour
{
    public UnityEngine.UI.Slider slider;
    public Extensions.Property range;

	public Transform offset;
    public Transform back;
    public Transform knob;
    public Transform mask;
    public Transform fore;

    List<LineRenderer> lines = new List<LineRenderer>();

    public float sliderWidth;

    public Vector3 leftPt, rightPt;
    public Vector2 inset;

    // Use this for initialization
    void Start()
    {
        lines.Add(back.GetComponent<LineRenderer>());
        // lines.Add(knob.GetComponent<LineRenderer>());
        lines.Add(mask.GetComponent<LineRenderer>());
        lines.Add(fore.GetComponent<LineRenderer>());

        slider.onValueChanged.AddListener(delegate {UpdateValues(); });
    }

    [Range(0, 1)]
    public float percent;
    public float value;

    void Update() {
        UpdateValues();
        UpdateGraphics();
        SetLines();
    }

    float spread;
    
    void UpdateValues() {
        percent = slider.value;
        spread  = range.end - range.start;
        value   = range.start + spread * percent;
    }

    void UpdateGraphics() {
        Vector2 knobPos = leftPt + (rightPt - leftPt) * percent;
        // knobPos = (Vector3)knobPos + new Vector3(0, 0, knobDepth);

        float scale = range.start + spread * percent;
        Vector2 knobScale = Vector2.one * scale;

        knob.transform.position   = knobPos;
        knob.transform.localScale = knobScale;
    }

    public void SetLines() {
        foreach(LineRenderer l in lines) {
            
            List<Vector3> linePositions = new List<Vector3>();

            linePositions.Add(leftPt);
            linePositions.Add(rightPt);

            if (l == fore.GetComponent<LineRenderer>()) {
                linePositions[0] = leftPt  + (Vector3)inset;
                linePositions[1] = rightPt + (Vector3)inset;
            }

            l.SetPositions(linePositions.ToArray());
        }

        // back.GetComponent<LineRenderer>().SetWidth(range.start * 1.15f, range.end * 1.15f);
        // mask.GetComponent<LineRenderer>().SetWidth(range.start, range.end);
        // fore.GetComponent<LineRenderer>().SetWidth(range.start, range.end);

    }
}
