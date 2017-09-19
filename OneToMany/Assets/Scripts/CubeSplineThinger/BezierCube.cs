using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class BezierCube : MonoBehaviour {

	public Mesh startingMesh;
	public int numSubDivisions = 4;

	public Vector3[] verts = new Vector3[8];
	public Vector3[] controlPts = new Vector3[24];

	int[][] edges = new []{ 
		new []{0,1},
		new []{1,2},
		new []{2,3},
		new []{3,0},
		
		new []{4,5},
		new []{5,6},
		new []{6,7},
		new []{7,4},
		
		new []{0,4},
		new []{1,5},
		new []{2,6},
		new []{3,7},
	};

	int[][] sides = new []{
		new []{0,2,1,3},
		new []{4,6,5,7},
		new []{8,9,0,4},
		//new []{9,10,3,5},
		//new []{0,2,1,3},
		//new []{0,2,1,3}
	};

	// edges ordered clockwise from top left face of cube, right face of cube, then middle section starting from top right vert of left face
	// verts ordered similarly from top right of left face

	void Reset(){
		// left face verts
		verts[0] = new Vector3(-0.5f,+0.5f,-0.5f);
		verts[1] = new Vector3(-0.5f,-0.5f,-0.5f);
		verts[2] = new Vector3(-0.5f,-0.5f,+0.5f);
		verts[3] = new Vector3(-0.5f,+0.5f,+0.5f);
		// right face verts
		verts[4] = new Vector3(+0.5f,+0.5f,-0.5f);
		verts[5] = new Vector3(+0.5f,-0.5f,-0.5f);
		verts[6] = new Vector3(+0.5f,-0.5f,+0.5f);
		verts[7] = new Vector3(+0.5f,+0.5f,+0.5f);



		int index = 0;
		foreach(var edge in edges){
			controlPts[index++] = (2*verts[edge[0]] + 1*verts[edge[1]]) / 3;
			controlPts[index++] = (1*verts[edge[0]] + 2*verts[edge[1]]) / 3;
		}
	}

	Vector3 EvalCurve(Vector3 p0,
	                  Vector3 p1,
	                  Vector3 p2,
	                  Vector3 p3,
	                  float u
	                  ){

		Vector3 s0 = Vector3.Lerp(p0,p1,u);
		Vector3 s1 = Vector3.Lerp(p1,p2,u);
		Vector3 s2 = Vector3.Lerp(p2,p3,u);
		Vector3 t0 = Vector3.Lerp(s0,s1,u);
		Vector3 t1 = Vector3.Lerp(s1,s2,u);
		return Vector3.Lerp(t0,t1,u);
	}

	void ReCalcMesh(){

	}

	void OnDrawGizmos(){
		int cpIndex = 0;
		foreach(var edge in edges){
			Vector3 cp1 = controlPts[cpIndex++];
			Vector3 cp2 = controlPts[cpIndex++];
			Vector3 last = EvalCurve(verts[edge[0]],cp1,cp2,verts[edge[1]], 0);
			for(int i = 1; i <= numSubDivisions; i++){
				Vector3 next = EvalCurve(verts[edge[0]],cp1,cp2,verts[edge[1]], i/(float)numSubDivisions);
				Gizmos.DrawLine(transform.TransformPoint(last),transform.TransformPoint(next));
				last = next;
			}
		}
		foreach(var side in sides){
			var topEdge = side[0];
			var bottomEdge = side[1];
			var leftEdge = side[2];
			var rightEdge = side[3];
			for(int i = 0; i <= numSubDivisions; i++){
				for(int j = 0; j <= numSubDivisions; j++){
					float u = (float)i/(numSubDivisions);
					float v = (float)j/(numSubDivisions);
//					Vector3 t = EvalCurve(verts[edges[topEdge][0]],cp1,cp2,verts[topEdge[1]], i/(float)numSubDivisions);
				}
			}
		}
	}
}
