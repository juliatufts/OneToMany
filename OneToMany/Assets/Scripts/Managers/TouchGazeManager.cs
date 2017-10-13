using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGazeManager : MonoBehaviour {

    public enum InteractType
    {
        LotusTouch,
        LotusGaze,
        CubesTouch,
        CubesGaze
    }

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
        

        lotusTouchInSeconds = PlayerPrefs.GetFloat("touchLotus");
        lotusGazeInSeconds = PlayerPrefs.GetFloat("gazeLotus"); ;
        cubesTouchInSeconds = PlayerPrefs.GetFloat("touchCubes"); ;
        cubesGazeInSeconds = PlayerPrefs.GetFloat("gazeCubes"); ;
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            lotusTouchInSeconds = lotusGazeInSeconds = cubesGazeInSeconds = cubesTouchInSeconds = 0;
        }
        // Update Gaze counts
        var ray = new Ray(ViveCamera.transform.position, ViveCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (LayerMask.NameToLayer(lotusLayer) == hit.collider.gameObject.layer)
            {
                lotusGazeInSeconds += Time.deltaTime;
                PlayerPrefs.SetFloat("gazeLotus", lotusGazeInSeconds);
            }
            else if (LayerMask.NameToLayer(cubesLayer) == hit.collider.gameObject.layer)
            {
                cubesGazeInSeconds += Time.deltaTime;
                PlayerPrefs.SetFloat("gazeCubes", cubesGazeInSeconds);
            }
        }
        // Update Shader values

    }

    void OnSerializeNetworkView(BitStream stream)
    {
        stream.Serialize(ref lotusTouchInSeconds);
        stream.Serialize(ref lotusGazeInSeconds);
        stream.Serialize(ref cubesTouchInSeconds);
        stream.Serialize(ref cubesGazeInSeconds);
    }

    public void BankLotusTouchTime(float time)
    {
        lotusTouchInSeconds += time;
        PlayerPrefs.SetFloat("touchLotus", lotusTouchInSeconds);
    }

    public void BankCubesTouchTime(float time)
    {
        cubesTouchInSeconds += time;
        PlayerPrefs.SetFloat("touchCubes", cubesTouchInSeconds);
    }

    public float GetTime(InteractType interact)
    {
        if (interact == InteractType.CubesGaze)
            return cubesGazeInSeconds;
        if (interact == InteractType.CubesTouch)
            return cubesTouchInSeconds;
        if (interact == InteractType.LotusGaze)
            return lotusGazeInSeconds;
        if (interact == InteractType.LotusTouch)
            return lotusTouchInSeconds;
        return 0;
    }

    public void SetTime(InteractType interact, float time)
    {
        if (interact == InteractType.CubesGaze)
        {
            cubesGazeInSeconds += time;
            PlayerPrefs.SetFloat("gazeCubes", cubesGazeInSeconds);
        }
        else if (interact == InteractType.CubesTouch)
        {
            BankCubesTouchTime(time);
        }
        else if (interact == InteractType.LotusGaze)
        {
            lotusGazeInSeconds += time;
            PlayerPrefs.SetFloat("gazeLotus", lotusGazeInSeconds);
        }
        else if (interact == InteractType.LotusTouch)
        {
            BankLotusTouchTime(time);
        }
    }

}
