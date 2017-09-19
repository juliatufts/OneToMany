using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Tetra : MonoBehaviour {

	public Vector3[] points;

	void Reset () {
		points = new Vector3[]{
			new Vector3( 1, 1, 1),
			new Vector3( 1,-1,-1),
			new Vector3(-1, 1,-1),
			new Vector3(-1,-1, 1)
		};
		RebuildMesh();
	}

	void Start(){
		RebuildMesh();
	}

	public void RebuildMesh(){
		int[] triIndices = new int[]{
			0,1,2,
			0,2,3,
			0,3,1,
			1,3,2
		};
		var verts = new Vector3[12];
		for(int i = 0; i < 12;i++){
			verts[i] = points[triIndices[i]];
		}
		var uvs = new Vector2[]{
			new Vector2(0,0),
			new Vector2(0,0),
			new Vector2(0,0),
			new Vector2(0,0),
			new Vector2(0,0),
			new Vector2(0,0),
			new Vector2(0,0),
			new Vector2(0,0),
			new Vector2(0,0),
			new Vector2(0,0),
			new Vector2(0,0),
			new Vector2(0,0),
		};
		int[] tris = new int[]{
			0,1,2,
			3,4,5,
			6,7,8,
			9,10,11
		};
		var mesh = GetComponent<MeshFilter>().mesh;
		if(mesh == null){
			mesh = new Mesh();
		}
		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.triangles = tris;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		GetComponent<MeshFilter>().mesh = mesh;
	}
}
