using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGazeManager : MonoBehaviour {

    public static TouchGazeManager Instance;

    public Camera ViveCamera;
    const string lotusLayer = "Sculpt0";
    const string cubesLayer = "Sculpt1";

    private float lotusTouchInSeconds;
    public float LotusTouch
    {
        get
        {
            return lotusTouchInSeconds / 60f;
        }
    }

    private float lotusGazeInSeconds;
    public float LotusGaze
    {
        get
        {
            return lotusGazeInSeconds / 60f;
        }
    }

    private float cubesTouchInSeconds;
    public float CubesTouch
    {
        get
        {
            return cubesTouchInSeconds / 60f;
        }
    }

    private float cubesGazeInSeconds;
    public float CubesGaze
    {
        get
        {
            return cubesGazeInSeconds / 60f;
        }
    }

    void Start ()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one TouchGazeManager in scene");
        }
        Instance = this;

        lotusTouchInSeconds = 0f;
        lotusGazeInSeconds = 0f;
        cubesTouchInSeconds = 0f;
        cubesGazeInSeconds = 0f;
    }
	
	void Update ()
    {
        // Update Gaze counts
        var ray = new Ray(ViveCamera.transform.position, ViveCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (LayerMask.NameToLayer(lotusLayer) == hit.collider.gameObject.layer)
            {
                lotusGazeInSeconds += Time.deltaTime;
            }
            else if (LayerMask.NameToLayer(cubesLayer) == hit.collider.gameObject.layer)
            {
                cubesGazeInSeconds += Time.deltaTime;
            }
        }

        // Update Shader values

	}

    public void BankLotusTouchTime(float time)
    {
        lotusTouchInSeconds += time;
    }

    public void BankCubesTouchTime(float time)
    {
        cubesTouchInSeconds += time;
    }
}
