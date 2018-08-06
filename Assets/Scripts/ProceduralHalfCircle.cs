using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralHalfCircle : MonoBehaviour
{

    Mesh mesh;
    Vector3[] verts;
    int[] tris;

	public float circleRadius = .5f;
	public int halfCircleResolution = 4;

	public float forwardDirectionAngle = 0;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Use this for initialization
    void Start()
    {
        MakeMesh();
    }

    void Update()
    {
        MakeMesh();
        UpdateMesh();
    }

    void MakeMesh()
    {
        // The more verts, the more 'round' the circle appears  
        // It's hard coded here but it would better if you could pass it in as an argument to this function  
        
		int numVerts = halfCircleResolution;
        
        verts = new Vector3[numVerts];
        // Vector2[] uvs = new Vector2[numVerts];
        tris = new int[(numVerts * 3)];
        
        // The first vert is in the center of the circle  
        verts[0] = Vector3.zero;
        // uvs[0] = new Vector2(0.5f, 0.5f);
        float angle = 180.0f / (float)(numVerts - 2);


        for (int i = 1; i < numVerts; ++i)
        {
            verts[i] = Quaternion.AngleAxis(forwardDirectionAngle + angle * (float)(i - 1), Vector3.back) * Vector3.up * circleRadius;
            // float normedHorizontal = (verts[i].x + 1.0f) * 0.5f;
            // float normedVertical = (verts[i].x + 1.0f) * 0.5f;
            // uvs[i] = new Vector2(normedHorizontal, normedVertical);
        }

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
    }

    void UpdateMesh()
    {
		mesh.Clear();
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.RecalculateNormals();
    }

	public GUIStyle style;

	void OnGUI() {
		GUI.color = Color.black;
		int i = 0;
		foreach (Vector3 vec in verts) {
			Vector2 offset = Camera.main.WorldToScreenPoint(vec);
			offset.y = Screen.height - offset.y;
			GUI.Label(new Rect(offset.x, offset.y, 0, 0), i.ToString(), style);
			i++;
		}
	}
}
