using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frame : MonoBehaviour {

	List<LineRenderer> lines = new List<LineRenderer>();

	public float width;
	public float height;
	public float cornerWidth;
	public Color color;


	// Use this for initialization
	void Start () {
		MakeFrame();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateFrame();
	}

	void MakeFrame() {
		for(int i = 0; i < 5; i++) {
			lines.Add(transform.GetChild(i).GetComponent<LineRenderer>());
		}

		float w = width / 2;
		float h = height / 2;

		// lines[0].SetPositions(new Vector3[] { new Vector2( w, h), new Vector2(   w, -h)});
		// lines[1].SetPositions(new Vector3[] { new Vector2( w, -h), new Vector2( -w, -h)});
		// lines[2].SetPositions(new Vector3[] { new Vector2( -w, -h), new Vector2(-w,  h)});
		// lines[3].SetPositions(new Vector3[] { new Vector2( -w, h), new Vector2(  w,  h)});

		// for(int i = 0; i < 4; i++) {
		// 	lines[i].SetWidth(cornerWidth, cornerWidth);
		// 	lines[i].material.color = color;
		// }

		// lines[4].SetPositions(new Vector3[] { new Vector2(0, h), new Vector2(0, -h)});
		// lines[4].SetPositions(new Vector3[] { new Vector2(0, h), new Vector2(0, -h)});
		// lines[4].SetWidth(w*2,w*2);
		// lines[4].material.color = color;
	}


	public bool useMask;

	public float stencil;
	public UnityEngine.Rendering.CompareFunction comp;
	public UnityEngine.Rendering.StencilOp pass;
	public int renderQueue;

	void UpdateFrame() {
		for(int i = 0; i < 5; i++) {
			// lines[i].material.SetFloat("_StencilRef", stencil);
			// lines[i].material.SetInt("_StencilComp", (int)comp);
			// lines[i].material.SetInt("_StencilOp", (int)pass);
			// lines[i].material.renderQueue = renderQueue;
		}
	}
}
