using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusHoleView : MonoBehaviour {

    public float minDensity = 0f;
    public float maxDensity = 4f;
    public Color holeColor;
    LotusHoleController lotus;

    private float maxHoleAlpha = 1f;
	private MeshRenderer meshRenderer;
	private Material rimMaterial;
	private Material holeMaterial;
	private Color originalRimColor;
    private Color originalHoleColor;

    void Awake()
    {
        lotus = GetComponent<LotusHoleController>();
        lotus.onStartTouch += OnStartTouch;
    }

    void OnDestroy()
    {
        lotus.onStartTouch -= OnStartTouch;
    }

    void Start ()
    {
		// Initialize view
		meshRenderer = GetComponent<MeshRenderer>();
		holeMaterial = meshRenderer.material;
        originalHoleColor = holeMaterial.color;
		maxHoleAlpha = holeMaterial.color.a;

		holeMaterial.color = AdjustAlpha(holeColor, 0f);
        holeMaterial.SetFloat("_Density", minDensity);

		// Check if rim materials have been generated
		var lotusRenderer = lotus.gameObject.GetComponent<MeshRenderer>();
		if (lotusRenderer.materials.Length > lotus.SubmeshIndex)
		{
			rimMaterial = lotusRenderer.materials[lotus.SubmeshIndex];
			originalRimColor = rimMaterial.color;
		}
    }
	
	void Update ()
    {
        var density = lotus.TouchIntensity * (maxDensity - minDensity) + minDensity;
        holeMaterial.SetFloat("_Density", density);
        //holeMaterial.color = AdjustAlpha(holeMaterial.color, maxHoleAlpha * lotus.TouchIntensity);
	}

    void OnStartTouch()
    {
        //TODO: sound
    }

	Color AdjustAlpha(Color c, float a)
	{
		return new Color(c.r, c.g, c.b, a);
	}
}
