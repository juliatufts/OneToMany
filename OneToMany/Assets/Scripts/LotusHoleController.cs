using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusHoleController : MonoBehaviour {

    public LotusController lotus;
    public GameObject holeEffectPrefab;
    public AnimationCurve onTouchCurve;

    [Tooltip("Time since last touch must be above this in order for next touch to have OnTouch event fired.")]
    public float onTouchCoolDownThreshold = 1f;

    [HideInInspector]
    public float timeThisTouch;
    [HideInInspector]
    public float timeSinceLastTouch;
    [HideInInspector]
    public float totalTimeTouched;
    [SerializeField, HideInInspector]
    private int submeshIndex;

    private bool touching;
    private float maxHoleAlpha = 1f;
    private MeshRenderer meshRenderer;
    private Material rimMaterial;
    private Material holeMaterial;
    private Color originalRimColor;
    
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        holeMaterial = meshRenderer.material;
        maxHoleAlpha = holeMaterial.color.a;
        holeMaterial.color = AdjustAlpha(holeMaterial.color, 0f);

        var lotusRenderer = lotus.gameObject.GetComponent<MeshRenderer>();
        if (lotusRenderer.materials.Length > submeshIndex)
        {
            rimMaterial = lotusRenderer.materials[submeshIndex];
            originalRimColor = rimMaterial.color;
        }
    }

    public void SetSubmeshIndex(int value)
    {
        submeshIndex = value;
    }

    void Update()
    {
        var t = Mathf.Max(0f, timeThisTouch - timeSinceLastTouch);
        var curveScale = 0.5f;

        holeMaterial.color = AdjustAlpha(holeMaterial.color, maxHoleAlpha * onTouchCurve.Evaluate(Mathf.Clamp01(t * curveScale)));
        Debug.Log("Alpha: " + maxHoleAlpha * Mathf.Clamp01(t * curveScale));

        if (!touching)
        {
            timeSinceLastTouch += Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.transform.CompareTag("GameController"))
		{
            touching = true;
            timeSinceLastTouch = 0;

            if (timeSinceLastTouch > onTouchCoolDownThreshold)
            {
                OnStartTouch();
            }
            
            //rimMaterial.color = Color.cyan;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (touching)
        {
            timeThisTouch += Time.deltaTime;
            totalTimeTouched += Time.deltaTime;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("GameController"))
        {
            touching = false;
            rimMaterial.color = originalRimColor;
        }
    }

    void OnStartTouch()
    {
        Instantiate(holeEffectPrefab, transform.position, transform.rotation);
    }

    Color AdjustAlpha(Color c, float a)
    {
        return new Color(c.r, c.g, c.b, a);
    }
}
