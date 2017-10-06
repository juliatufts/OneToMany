using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WriteIntoTexture : MonoBehaviour {

    public int texWidth = 1024;
    public int texHeight = 1024;
    public Color color = Color.cyan;

    float maxRaycastDist = 100f;
    Renderer mainRenderer;
    Texture2D tex;

	// Use this for initialization
	void Start ()
    {
        mainRenderer = GetComponent<Renderer>();
        tex = new Texture2D(texWidth, texHeight);
        mainRenderer.material.mainTexture = tex;
	}
	
	// Update is called once per frame
	void Update ()
    {
        var cam = Camera.main;
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        ConeCastSplat(ray.origin, ray.direction, 0.01f, 8);

        DebugHighlight();
	}

    void ConeCastSplat(Vector3 origin, Vector3 direction, float angle, int numRays)
    {
        // Construct rays
        Ray[] rays = new Ray[numRays];
        for (var i = 0; i < numRays; i++)
        {
            var p0 = origin + direction.normalized;
            var len = Mathf.Tan(angle);
            var theta = i * 2f * Mathf.PI / numRays;
            var p1 = new Vector3(p0.x + len * Mathf.Cos(theta), p0.y + len * Mathf.Sin(theta), p0.z);
            rays[i] = new Ray(origin, p1 - origin);
        }

        // Cast those rays and write to the texture
        Vector2 pixelUV;
		RaycastHit hit;
        foreach (var ray in rays)
        {
            Debug.DrawRay(ray.origin, maxRaycastDist * ray.direction, Color.magenta);
            if (Physics.Raycast(ray, out hit, maxRaycastDist) && (hit.collider.gameObject == gameObject))
			{
				pixelUV = hit.textureCoord;
				tex.SetPixel((int)(pixelUV.x * texWidth), (int)(pixelUV.y * texHeight), color);
				tex.Apply();
			}
        }
    }

	void CircleAdd(int cx, int cy, int r, Color col)
	{
		int x, y, px, nx, py, ny, d;
		Color32[] tempArray = tex.GetPixels32();
        Color32 prevCol;
        Color32 c = col;
		int len = tempArray.Length;

		for (x = 0; x <= r; x++)
		{
			d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
			for (y = 0; y <= d; y++)
			{
				px = cx + x;
				nx = cx - x;
				py = cy + y;
				ny = cy - y;

				if (0 <= py * tex.width + px && py * tex.width + px < len)
                {
                    prevCol = tempArray[py * tex.width + px];
					tempArray[py * tex.width + px] = new Color32 (
                        (byte)(prevCol.r + c.r),
                        (byte)(prevCol.g + c.g),
                        (byte)(prevCol.b + c.b),
                        (byte)(prevCol.a + c.a)
                    );
                }
					
				if (0 <= py * tex.width + nx && py * tex.width + nx < len)
                {
					prevCol = tempArray[py * tex.width + nx];
					tempArray[py * tex.width + px] = new Color32(
						(byte)(prevCol.r + c.r),
						(byte)(prevCol.g + c.g),
						(byte)(prevCol.b + c.b),
						(byte)(prevCol.a + c.a)
					);
                }
					
				if (0 <= ny * tex.width + px && ny * tex.width + px < len)
                {
                    prevCol = tempArray[ny * tex.width + px];
					tempArray[py * tex.width + px] = new Color32(
						(byte)(prevCol.r + c.r),
						(byte)(prevCol.g + c.g),
						(byte)(prevCol.b + c.b),
						(byte)(prevCol.a + c.a)
					);
                }
					
				if (0 <= ny * tex.width + nx && ny * tex.width + nx < len)
                {
                    prevCol = tempArray[ny * tex.width + nx];
					tempArray[py * tex.width + px] = new Color32(
						(byte)(prevCol.r + c.r),
						(byte)(prevCol.g + c.g),
						(byte)(prevCol.b + c.b),
						(byte)(prevCol.a + c.a)
					);
                }
			}
		}
		tex.SetPixels32(tempArray);
		tex.Apply();
	}

    void CircleFill(int cx, int cy, int r, Color col)
    {
		int x, y, px, nx, py, ny, d;
        Color32[] tempArray = tex.GetPixels32();
        int len = tempArray.Length;

		for (x = 0; x <= r; x++)
		{
			d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
			for (y = 0; y <= d; y++)
			{
				px = cx + x;
				nx = cx - x;
				py = cy + y;
				ny = cy - y;

                if (0 <= py * tex.width + px && py * tex.width + px < len)
                    tempArray[py * tex.width + px] = col;
                if (0 <= py * tex.width + nx && py * tex.width + nx < len) 
                    tempArray[py * tex.width + nx] = col;
                if (0 <= ny * tex.width + px && ny * tex.width + px < len) 
                    tempArray[ny * tex.width + px] = col;
                if (0 <= ny * tex.width + nx && ny * tex.width + nx < len) 
                    tempArray[ny * tex.width + nx] = col;
			}
		}
		tex.SetPixels32(tempArray);
		tex.Apply();
    }

    void DebugHighlight()
    {
		var cam = Camera.main;
		var ray = cam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit) && (hit.collider.gameObject == gameObject))
		{
			var mesh = hit.collider.GetComponent<MeshFilter>().sharedMesh;
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;

			Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
			Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
			Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
			Transform hitTransform = hit.collider.transform;
			p0 = hitTransform.TransformPoint(p0);
			p1 = hitTransform.TransformPoint(p1);
			p2 = hitTransform.TransformPoint(p2);
			Debug.DrawLine(p0, p1);
			Debug.DrawLine(p1, p2);
			Debug.DrawLine(p2, p0);
		}
    }
}
