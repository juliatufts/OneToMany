using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class LotusController : MonoBehaviour {

    public Mesh originalLotusMesh;
    public GameObject lotusHolesRoot;
    public Material lotusRimMaterial;

	Renderer lotusRenderer;
	Mesh lotusMesh;
    MeshCollider lotusCollider;
    Vector3[] vertices;
    List<int> lotusTriangles;
	List<Ray> rays;
	float maxRaycastDist = 1f;
    HashSet<int> newTriangleIndices;

    public void CollidersToSubmeshes()
    {
        Reset();

        // Extract rims of lotus collider children
        foreach (Transform lotusHolesChild in lotusHolesRoot.transform)
        {
            SeparateRimMesh(lotusHolesChild);
        }

        // Remove rim triangles from lotus mesh
        var tempTriangles = new List<int>();
        for (int i = 0; i < lotusTriangles.Count; i += 3)
        {
            if (!newTriangleIndices.Contains(i))
            {
                tempTriangles.Add(lotusTriangles[i + 0]);
                tempTriangles.Add(lotusTriangles[i + 1]);
                tempTriangles.Add(lotusTriangles[i + 2]);
            }
        }
        lotusMesh.SetTriangles(tempTriangles.ToArray(), 0);

        UpdateMaterials();
    }

    public void UpdateMaterials()
    {
        lotusMesh = GetComponent<MeshFilter>().sharedMesh;
        Material[] newLotusMaterials = new Material[lotusMesh.subMeshCount];
        newLotusMaterials[0] = lotusRenderer.sharedMaterial;
        for (int i = 1; i < lotusMesh.subMeshCount; i++)
        {
            newLotusMaterials[i] = lotusRimMaterial;
        }
        lotusRenderer.sharedMaterials = newLotusMaterials;
    }

    public void Reset()
    {
        // Create fresh copy of original mesh
        lotusMesh = (Mesh)Instantiate(originalLotusMesh);
        var path = "Assets/Models/Generated/LotusCopy.asset";
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.CreateAsset(lotusMesh, path);
        AssetDatabase.SaveAssets();

        // Assign fresh copy
        GetComponent<MeshFilter>().sharedMesh = lotusMesh;
        lotusRenderer = GetComponent<Renderer>();
        lotusRenderer.sharedMaterials = new Material[] { lotusRenderer.sharedMaterials[0] };
        lotusCollider = GetComponent<MeshCollider>(); // note: this actually doesn't change, though maybe it should
        vertices = lotusMesh.vertices;
        newTriangleIndices = new HashSet<int>();
    }

    void SeparateRimMesh(Transform rimChild)
    {
        lotusTriangles = new List<int>();
        lotusMesh.GetTriangles(lotusTriangles, 0);
        var newTriangles = new List<int>();

        var extents = rimChild.GetComponent<MeshCollider>().bounds.extents;
        //DebugBoundingBox(rimChild.GetComponent<MeshCollider>(), Color.cyan, 60f);

        var epsilon = 0.01f;
        maxRaycastDist = extents.magnitude;
        maxRaycastDist += epsilon;

        // Try to use normals instead of raycast hits
        var angleThreshold = rimChild.GetComponent<LotusHoleController>().rimAngleThreshold;
        for (var i = 0; i < lotusTriangles.Count; i += 3)
        {
            var p0 = transform.TransformPoint(vertices[lotusTriangles[i + 0]]);
            var p1 = transform.TransformPoint(vertices[lotusTriangles[i + 1]]);
            var p2 = transform.TransformPoint(vertices[lotusTriangles[i + 2]]);
            var center = ((p0 + p1 + p2) / 3f);

			// Filter by distance
			var dist = Vector3.Distance(center, rimChild.position);
            if (dist <= maxRaycastDist)
            {
				var faceNormal = Vector3.Cross(p1 - p0, p2 - p0);
				faceNormal.Normalize();

                //var towardsRimChild = (rimChild.position - center).normalized;
                //Debug.DrawLine(center, center + maxRaycastDist * towardsRimChild, Color.magenta, 20f);
				//Debug.DrawLine(center, rimChild.position, Color.magenta, 20f);
				//Debug.DrawLine(center, center + dist * faceNormal, Color.cyan, 20f);

                // Filter by normal alignment
				if (Vector3.Angle(rimChild.position - center, faceNormal) <= angleThreshold &&
                    newTriangleIndices.Add(i))
				{
					newTriangles.Add(lotusTriangles[i + 0]);
					newTriangles.Add(lotusTriangles[i + 1]);
					newTriangles.Add(lotusTriangles[i + 2]);
				}
            }
        }

        // Assign rim as new submesh
        var submeshIndex = ++lotusMesh.subMeshCount - 1;
        lotusMesh.SetTriangles(newTriangles.ToArray(), submeshIndex);
        rimChild.GetComponent<LotusHoleController>().SetSubmeshIndex(submeshIndex);

        //Debug.Log("lotusMesh.subMeshCount: " + lotusMesh.subMeshCount + " tris: " + newTriangles.Count);
    }

    void DebugBoundingBox(MeshCollider collider, Color color, float duration)
    {
        var center = collider.bounds.center;
        var extents = collider.bounds.extents;
        var p0 = center + new Vector3(extents.x, extents.y, extents.z);
        var p1 = center + new Vector3(extents.x, extents.y, -extents.z);
        var p2 = center + new Vector3(extents.x, -extents.y, extents.z);
        var p3 = center + new Vector3(extents.x, -extents.y, -extents.z);
        var p4 = center + new Vector3(-extents.x, extents.y, extents.z);
        var p5 = center + new Vector3(-extents.x, extents.y, -extents.z);
        var p6 = center + new Vector3(-extents.x, -extents.y, extents.z);
        var p7 = center + new Vector3(-extents.x, -extents.y, -extents.z);

        Debug.DrawLine(p0, p1, color, duration);
        Debug.DrawLine(p0, p2, color, duration);
        Debug.DrawLine(p1, p3, color, duration);
        Debug.DrawLine(p2, p3, color, duration);

        Debug.DrawLine(p4, p5, color, duration);
        Debug.DrawLine(p4, p6, color, duration);
        Debug.DrawLine(p5, p7, color, duration);
        Debug.DrawLine(p6, p7, color, duration);

        Debug.DrawLine(p0, p4, color, duration);
        Debug.DrawLine(p1, p5, color, duration);
        Debug.DrawLine(p2, p6, color, duration);
        Debug.DrawLine(p3, p7, color, duration);
    }

    void DebugFaceNormals(float length, Color startColor, Color endColor, float duration)
    {
        for (int i = 0; i < 3000; i+=3)
        {
            var t0 = lotusMesh.triangles[i + 0];
            var t1 = lotusMesh.triangles[i + 1];
            var t2 = lotusMesh.triangles[i + 2];

            var p0 = transform.TransformPoint(lotusMesh.vertices[t0]);
            var p1 = transform.TransformPoint(lotusMesh.vertices[t1]);
            var p2 = transform.TransformPoint(lotusMesh.vertices[t2]);

            var center = ((p0 + p1 + p2) / 3f);

            var faceNormal = Vector3.Cross(p1 - p0, p2 - p0);
            faceNormal.Normalize();

            Debug.DrawLine(center, center + (length / 2f) * faceNormal, startColor, duration);
            Debug.DrawLine(center + (length / 2f) * faceNormal, center + length * faceNormal, endColor, duration);
        }
    }
}
