using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSide : MonoBehaviour {
    public Vector3 currentPos;

    public MapChunk tileChunk;
    public Tile tile;

    public Vector3[] meshVertices;
    public int[] meshTriangles;
    public Vector2[] meshUVs;
    public Mesh mesh;
    public MeshFilter meshFilter;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentPos = transform.position;
    }

    public float getElevation()
    {
        return transform.position.y;
    }

    public void SetMaterial(Material m)
    {
        GetComponent<Renderer>().material = m;
    }

    public void DefineMeshParameters(Vector3 topA, Vector3 topB, Vector3 bottomA, Vector3 bottomB)
    {
        defineMeshVertices(topA, topB, bottomA, bottomB);
        defineMeshTriangles();
        defineMeshUVs();
    }

    public void DrawMesh()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter.mesh)
        {
            mesh = meshFilter.mesh;
        }
        else
        {
            mesh = new Mesh();
            meshFilter.mesh = mesh;
        }

        mesh.Clear();
        mesh.vertices = meshVertices;
        mesh.triangles = meshTriangles;
        mesh.uv = meshUVs;
        //mesh.Optimize();
        //UnityEditor.MeshUtility.Optimize(mesh);
        mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    private void defineMeshVertices(Vector3 topA, Vector3 topB, Vector3 bottomA, Vector3 bottomB)
    {
        meshVertices = new Vector3[5];
        meshVertices[0] = (topA + topB + bottomA + bottomB) / 4F;
        meshVertices[1] = bottomA;
        meshVertices[2] = topA;
        meshVertices[3] = topB;
        meshVertices[4] = bottomB;
    }

    private void defineMeshTriangles()
    {
        //RMEMBER: Clockwise determines which side is visible!
        meshTriangles = new int[]
        {
            0,1,2,  //Left
            0,2,3,  //Up
            0,3,4,  //Right
            0,4,1   //Down
        };
    }

    private void defineMeshUVs()
    {
        //REMEMBER: 0,0 is bottom left and 1,1 is top right!
        meshUVs = new Vector2[]
        {
            new Vector2(0.5F, 0.5F),
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,1)
        };
    }
}
