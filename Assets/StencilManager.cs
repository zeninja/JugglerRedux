using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class StencilManager : MonoBehaviour {

	Image img;

	// public Material stencilMat;

	Material mat;

	public float stencil;
	public UnityEngine.Rendering.CompareFunction comp;
	public UnityEngine.Rendering.StencilOp pass;
	public int renderQueue;

	// public Shader stencilShader;


	// Use this for initialization
	void Start () {
		img = GetComponent<Image>();
		img.material.SetFloat("StencilRef", stencil);
		// Material m = ObjectFactory.CreateInstance<Material>();
		// m.shader = stencilShader;
		// img.material = m;
		// mat = GetComponent<Mask>().
		// Material m = ObjectFactory.CreateInstance<Material>();
		// m.shader = stencilMat.shader;
		// mat = img.material;
	}
	
	// Update is called once per frame
	void Update () {
		img.material.SetFloat("StencilRef", stencil);

		// mat.SetFloat("_StencilRef",   stencil);
		// mat.SetInt("_StencilComp",  (int)comp);
		// mat.SetInt("_StencilOp",    (int)pass);
		// mat.renderQueue = renderQueue;
	}
}
