using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ProceduralMesh : MonoBehaviour {

	Mesh mesh;
	Vector3[] vertices;
	int[] triangles;

	void Awake() {
		mesh = GetComponent<MeshFilter>().mesh;
	}

	// Use this for initialization
	void Start () {
		MakeMeshData();
	}
	
	// Update is called once per frame
	void Update () {
		MakeMeshData();
		UpdateMesh();
	}

	void MakeMeshData() {
		// create an array of vertices
		vertices = new Vector3[] { 
			new Vector3(0, 0, 0),
			new Vector3(0, 0, 1),
			new Vector3(1, 0, 0),
			new Vector3(1, 0, 1)
		};

		// create an array of integers
		triangles = new int[] { 0, 1, 2, 2, 1, 3 };
	}

	void UpdateMesh() {
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}
}
