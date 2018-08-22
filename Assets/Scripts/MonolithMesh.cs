using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonolithMesh : MonoBehaviour
{

    Mesh mesh;
    Vector3[] verts;
    int[] tris;
    Vector3[] normals;
    int numVerts;

    public Vector2 anchorPos;


    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Use this for initialization
    void Start()
    {

    }

    public void UpdateValues(Vector2 center, List<Vector3> vertList) {
        // Debug.Log("vert list changing");

        anchorPos = center;

        vertList.Insert(0, anchorPos);
        verts = vertList.ToArray();
   		numVerts = verts.Length;
        tris = new int[(numVerts * 3)];

        MakeMesh();
        UpdateMesh();
    }

    void MakeMesh()
    {
        float angle = 180.0f / (float)(numVerts - 2);

        for (int i = 0; i + 2 < numVerts; ++i)
        {
            int index = i * 3;
            tris[index + 0] = 0;
            tris[index + 1] = i + 1;
            tris[index + 2] = i + 2;
        }

        // The last triangle has to wrap around to the first vert so we do this last and outside the lop  
        var lastTriangleIndex = tris.Length - 3;
        tris[lastTriangleIndex + 0] = 0;
        tris[lastTriangleIndex + 1] = numVerts - 1;
        tris[lastTriangleIndex + 2] = 1;

        // Vector3[] normals = new Vector3[tris.Length];
        // for(int i = 0; i < normals.Length; i++) {
        //     normals[i] = normal;
        // } 
    }

    public Vector3 normal;

    void UpdateMesh()
    {
		mesh.Clear();
		mesh.vertices = verts;
		mesh.triangles = tris;
        // mesh.normals = normals;
		mesh.RecalculateNormals();
    }

	public GUIStyle style;
}
