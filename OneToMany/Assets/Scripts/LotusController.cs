using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LotusController : MonoBehaviour {

    public GameObject lotusHolesRoot;

	Renderer lotusRenderer;
	Mesh lotusMesh;
    List<int> lotusTriangles;
	Ray[] rays;
	float maxRaycastDist = 1f;

	void Start()
	{
		lotusMesh = GetComponent<MeshFilter>().mesh;
		lotusRenderer = GetComponent<Renderer>();

		var vertices = lotusMesh.vertices;
        lotusTriangles = new List<int>(lotusMesh.triangles);
        Debug.Log("tris: " + lotusMesh.triangles.Length);
		Color[] colors = new Color[vertices.Length];
		List<int> newTriangles = new List<int>();
        List<int> newTriangleIndices = new List<int>();

		// Set vertex colors to white by default
		for (int i = 0; i < vertices.Length; i++)
		{
		    colors[i] = Color.white;
		}

        // Get Lotus child and extents of its collider
		var lotusHolesChild = lotusHolesRoot.transform.GetChild(32);
        var extents = lotusHolesChild.GetComponent<MeshCollider>().bounds.extents;
        var epsilon = 0.001f;
        maxRaycastDist = Mathf.Sqrt(extents.x * extents.x +
                                    extents.y * extents.y +
                                    extents.z * extents.z);
        maxRaycastDist += epsilon;

		// Construct a shit ton of rays
        rays = new Ray[lotusTriangles.Count / 3];
		var rayIndex = 0;
		for (var i = 0; i < lotusTriangles.Count; i += 3)
		{
			var p0 = transform.TransformPoint(vertices[lotusTriangles[i + 0]]);
			var p1 = transform.TransformPoint(vertices[lotusTriangles[i + 1]]);
			var p2 = transform.TransformPoint(vertices[lotusTriangles[i + 2]]);
			var mid = (p0 + p1 + p2) / 3f;
			rays[rayIndex++] = new Ray(lotusHolesChild.position, mid - lotusHolesChild.position);
		}
		Debug.Log("Rays: " + rays.Length);

		// Cast those rays and color the hit vertices
		RaycastHit hit;
		foreach (var ray in rays)
		{
			if (Physics.Raycast(ray, out hit, maxRaycastDist) && (hit.collider.gameObject == gameObject))
			{
				//Debug.DrawLine(ray.origin, ray.origin + maxRaycastDist * ray.direction, Color.magenta, 60f);

				var t0 = lotusTriangles[hit.triangleIndex * 3 + 0];
				var t1 = lotusTriangles[hit.triangleIndex * 3 + 1];
				var t2 = lotusTriangles[hit.triangleIndex * 3 + 2];

		        colors[t0] = Color.red;
		        colors[t1] = Color.red;
		        colors[t2] = Color.red;

				newTriangles.Add(t0);
				newTriangles.Add(t1);
				newTriangles.Add(t2);
                newTriangleIndices.Add(hit.triangleIndex * 3);
			}
		}
        lotusMesh.colors = colors;

        // Remove rim triangles from lotus mesh
        var tempTriangles = new List<int>();
        for (int i = 0; i < lotusTriangles.Count; i+=3)
        {
            if (!newTriangleIndices.Contains(i))
            {
                tempTriangles.Add(lotusTriangles[i + 0]);
                tempTriangles.Add(lotusTriangles[i + 1]);
                tempTriangles.Add(lotusTriangles[i + 2]);
            }
        }
        lotusMesh.triangles = tempTriangles.ToArray();
        Debug.Log("tris: " + lotusMesh.triangles.Length);

		// Make a submesh
		lotusMesh.subMeshCount++;
		lotusMesh.SetTriangles(newTriangles.ToArray(), 1);

		Material[] newLotusMaterials = new Material[lotusMesh.subMeshCount];
		newLotusMaterials[0] = lotusRenderer.material;
		newLotusMaterials[1] = lotusHolesChild.GetComponent<Renderer>().material;
		lotusRenderer.materials = newLotusMaterials;
	}
}
