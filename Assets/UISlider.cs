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

    // public float sliderWidth;

    public Vector3 leftPt, rightPt;
    public Vector2 inset;

    

    // Use this for initialization
    void Start()
    {
        lines.Add(back.GetComponent<LineRenderer>());
        // lines.Add(knob.GetComponent<LineRenderer>());
        lines.Add(mask.GetComponent<LineRenderer>());
        lines.Add(fore.GetComponent<LineRenderer>());

        slider.onValueChanged.AddListener(delegate { UpdateValues(); });
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
    
    // void SetSliderValue(float p) {
    //     slider.value = p;
    // }

    void UpdateValues() {
        percent = slider.value;
        spread  = range.end - range.start;
        value   = range.start + spread * percent;
    }

    void UpdateGraphics() {
        Vector2 knobPos = leftPt + (rightPt - leftPt) * percent;
        // Debug.Log((Vector3)knobPos);
        knobPos = new Vector3(knobPos.x, knobPos.y, 0);

        float scale = range.start + spread * percent;
        Vector2 knobScale = Vector2.one * scale;

        knob.transform.position   = (Vector3)knobPos + offset.position;
        knob.transform.localScale = knobScale;
    }

    public void SetLines() {
        foreach(LineRenderer l in lines) {
            
            List<Vector3> linePositions = new List<Vector3>();

            linePositions.Add(leftPt  + offset.position);
            linePositions.Add(rightPt + offset.position);

            if (l == fore.GetComponent<LineRenderer>()) {
                linePositions[0] += (Vector3)inset;
                linePositions[1] += (Vector3)inset;
            }

            l.SetPositions(linePositions.ToArray());
        }
    }

    public void SetScale(float value) {
        slider.value = value;
    }
}
