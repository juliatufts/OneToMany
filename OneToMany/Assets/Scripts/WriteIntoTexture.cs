using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WriteIntoTexture : MonoBehaviour {

    Renderer mainRenderer;
    Texture2D tex;

	// Use this for initialization
	void Start ()
    {
        mainRenderer = GetComponent<Renderer>();
        tex = new Texture2D(256, 256);
        mainRenderer.material.mainTexture = tex;
	}
	
	// Update is called once per frame
	void Update ()
    {
        var cam = Camera.main;
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject == gameObject))
        {
            var mesh = hit.collider.GetComponentInParent<MeshFilter>().sharedMesh;
            var tri = hit.triangleIndex;
            var weight = hit.barycentricCoordinate;
            var uvs = mesh.uv;

            // Check how to index triangles using hit
            var tris = mesh.triangles.Skip(tri*3).Take(3)
                           .Select(i => uvs[i]).ToArray();

            var uv = Vector2.zero;
            for (int i = 0; i < 3; ++i) uv += weight[i] * tris[i];

            tex.SetPixel( (int) (uv.x * 256), (int)(uv.y * 256), Color.red);
            tex.Apply();
        }
	}
}
