using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusHoleView : MonoBehaviour {

    public float minDensity = 0f;
    public float maxDensity = 4f;
    public float value;
    public Color holeColor;
    LotusHoleController lotus;

    [HideInInspector]
    public LotusController controller;

    private float originalValue;
    private float maxHoleAlpha = 1f;
	private MeshRenderer meshRenderer;
	private Material rimMaterial;
	private Material holeMaterial;
	private Color originalRimColor;
    private Color originalHoleColor;
    private Gradient gradient;

    void Awake()
    {
        lotus = GetComponent<LotusHoleController>();
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
        originalValue = value;

		// Check if rim materials have been generated
		var lotusRenderer = lotus.gameObject.GetComponent<MeshRenderer>();
		if (lotusRenderer.materials.Length > lotus.SubmeshIndex)
		{
			rimMaterial = lotusRenderer.materials[lotus.SubmeshIndex];
			originalRimColor = rimMaterial.color;
		}

        if (controller != null)
        {
            gradient = controller.colorGradient;
        }
        else
        {
            Debug.LogError("Color gradient not found");
        }
    }
	
	void Update ()
    {
        var density = lotus.TouchIntensity * (maxDensity - minDensity) + minDensity;
        holeMaterial.SetFloat("_Density", density);

        holeMaterial.color = gradient.Evaluate(originalValue + Mathf.Lerp(0f, 1f - originalValue, lotus.TouchIntensity));
	}

	Color AdjustAlpha(Color c, float a)
	{
		return new Color(c.r, c.g, c.b, a);
	}
}
