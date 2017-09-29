using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public float maxTouchRadius = 0.5f;
    public float touchRadiusSpeed = 1f;

    const float maxDist = 100f;
    const string controllerTag = "GameController";

    Material mainMaterial;
    float touchRadius;
    float touchTimeInSeconds;
    public bool touching;

	void Start ()
    {
        mainMaterial = GetComponent<Renderer>().material;
        mainMaterial.SetFloat("_Strength", 1f);
        mainMaterial.SetFloat("_TouchRadius", 0f);
        touchTimeInSeconds = 0f;
        touchRadius = 0f;
        touching = false;
	}

    void Update()
    {
        if (touching)
        {
            touchRadius += (Time.deltaTime * touchRadiusSpeed);
            touchRadius = Mathf.Clamp(touchRadius, 0f, maxTouchRadius);
        }
        else
        {
            touchRadius -= (Time.deltaTime * touchRadiusSpeed);
			touchRadius = Mathf.Clamp(touchRadius, 0f, maxTouchRadius);
        }

        Debug.Log("touching: " + touching);
        mainMaterial.SetFloat("_TouchRadius", touchRadius);
    }

    void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag(controllerTag))
		{
			mainMaterial.SetVector("_ContactPoint", other.ClosestPoint(transform.position));
            touching = true;
		}
	}

    void OnTriggerStay(Collider other)
    {
		if (other.gameObject.CompareTag(controllerTag))
		{
			touchTimeInSeconds += Time.deltaTime;
            mainMaterial.SetVector("_ContactPoint", other.ClosestPoint(transform.position));
		}
    }

    void OnTriggerExit(Collider other)
    {
		if (other.gameObject.CompareTag(controllerTag))
		{
            touchTimeInSeconds = 0f;
            touching = false;
		}
    }
}
