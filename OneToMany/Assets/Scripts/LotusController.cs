using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class LotusController : MonoBehaviour {

    public Mesh originalLotusMesh;
    public GameObject lotusHolesRoot;
    public Material lotusChildMaterial;

	Renderer lotusRenderer;
	Mesh lotusMesh;
    Vector3[] vertices;
    List<int> lotusTriangles;
	Ray[] rays;
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
        Material[] newLotusMaterials = new Material[lotusMesh.subMeshCount];
        newLotusMaterials[0] = lotusRenderer.sharedMaterial;
        for (int i = 1; i < lotusMesh.subMeshCount; i++)
        {
            newLotusMaterials[i] = lotusChildMaterial;
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

        // Reassign
        GetComponent<MeshFilter>().sharedMesh = lotusMesh;
        lotusRenderer = GetComponent<Renderer>();
        lotusRenderer.sharedMaterials = new Material[] { lotusRenderer.sharedMaterials[0] };
        vertices = lotusMesh.vertices;
        newTriangleIndices = new HashSet<int>();
    }

    void SeparateRimMesh(Transform rimChild)
    {
        lotusTriangles = new List<int>();
        lotusMesh.GetTriangles(lotusTriangles, 0);
        var newTriangles = new List<int>();

        var extents = rimChild.GetComponent<MeshCollider>().bounds.extents;
        var epsilon = 0.01f;
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
            rays[rayIndex++] = new Ray(rimChild.position, mid - rimChild.position);
        }

        // Cast those rays
        RaycastHit hit;
        foreach (var ray in rays)
        {
            // Try using Collider.Raycast instead maybe ?
            if (Physics.Raycast(ray, out hit, maxRaycastDist) && (hit.collider.gameObject == gameObject) &&
                newTriangleIndices.Add(hit.triangleIndex * 3))
            {
                //Debug.DrawLine(ray.origin, ray.origin + maxRaycastDist * ray.direction, Color.magenta, 60f);

                var t0 = lotusTriangles[hit.triangleIndex * 3 + 0];
                var t1 = lotusTriangles[hit.triangleIndex * 3 + 1];
                var t2 = lotusTriangles[hit.triangleIndex * 3 + 2];

                newTriangles.Add(t0);
                newTriangles.Add(t1);
                newTriangles.Add(t2);
            }
        }

        // Assign rim as new submesh
        lotusMesh.SetTriangles(newTriangles.ToArray(), ++lotusMesh.subMeshCount - 1);
        //Debug.Log("lotusMesh.subMeshCount: " + lotusMesh.subMeshCount + " tris: " + newTriangles.Count);
    }
}
