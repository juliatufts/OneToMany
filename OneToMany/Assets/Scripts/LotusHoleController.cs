using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusHoleController : MonoBehaviour {

    public GameObject lotus;
    public GameObject holeEffectPrefab;
    
    GameObject currentEffect;
    Renderer lotusRenderer;
    Mesh lotusMesh;
    Ray[] rays;
    float maxRaycastDist = 50f;

    void Start()
    {
        lotusMesh = lotus.GetComponent<MeshFilter>().mesh;
        lotusRenderer = lotus.GetComponent<Renderer>();

        var vertices = lotusMesh.vertices;
        var triangles = lotusMesh.triangles;
        Color[] colors = new Color[vertices.Length];
        List<int> newTriangles = new List<int>();

        // Set vertex colors to white by default
        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = Color.white;
        }

        // Construct a shit ton of rays
        rays = new Ray[triangles.Length / 3];
        var rayIndex = 0;
        for (var i = 0; i < triangles.Length; i += 3)
        {
            var p0 = lotus.transform.TransformPoint(vertices[triangles[i + 0]]);
            var p1 = lotus.transform.TransformPoint(vertices[triangles[i + 1]]);
            var p2 = lotus.transform.TransformPoint(vertices[triangles[i + 2]]);
            var mid = (p0 + p1 + p2) / 3f;
            rays[rayIndex++] = new Ray(transform.position, mid - transform.position);
        }
        Debug.Log("Rays: " + rays.Length);

        // Cast those rays and color the hit vertices
        RaycastHit hit;
        foreach (var ray in rays)
        {
            if (Physics.Raycast(ray, out hit, maxRaycastDist) && (hit.collider.gameObject == lotus))
            {
                //Debug.DrawRay(ray.origin, maxRaycastDist * ray.direction, Color.magenta, 60f);

                var t0 = triangles[hit.triangleIndex * 3 + 0];
                var t1 = triangles[hit.triangleIndex * 3 + 1];
                var t2 = triangles[hit.triangleIndex * 3 + 2];

                colors[t0] = Color.red;
                colors[t1] = Color.red;
                colors[t2] = Color.red;

                newTriangles.Add(t0);
                newTriangles.Add(t1);
                newTriangles.Add(t2);
            }
        }
        lotusMesh.colors = colors;

        // Make a submesh
        lotusMesh.subMeshCount++;
        lotusMesh.SetTriangles(newTriangles.ToArray(), 1);

        Material[] newLotusMaterials = new Material[lotusMesh.subMeshCount];
        newLotusMaterials[0] = lotusRenderer.material;
        newLotusMaterials[1] = GetComponent<Renderer>().material;
        lotusRenderer.materials = newLotusMaterials;
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.transform.CompareTag("GameController"))
		{
            // TODO: material VFX

            //currentEffect = Instantiate(holeEffectPrefab, transform.position, transform.rotation);
        }
    }

}
