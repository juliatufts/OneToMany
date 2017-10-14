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

    void OnDisable()
    {
        Save();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("gazeLotus", lotusGazeInSeconds);
        PlayerPrefs.SetFloat("gazeCubes", cubesGazeInSeconds);
        PlayerPrefs.SetFloat("touchLotus", lotusTouchInSeconds);
        PlayerPrefs.SetFloat("touchCubes", cubesTouchInSeconds);
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F5) && Input.GetKey(KeyCode.LeftShift))
        {
            lotusTouchInSeconds = lotusGazeInSeconds = cubesGazeInSeconds = cubesTouchInSeconds = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }
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

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), "Lotus Touch: " + LotusTouch);
        GUI.Label(new Rect(10, 30, 100, 20), "Lotus Gaze: " + LotusGaze);
        GUI.Label(new Rect(10, 50, 100, 20), "Cubes Touch: " + CubesTouch);
        GUI.Label(new Rect(10, 70, 100, 20), "Cubes Gaze: " + CubesGaze);
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
    }

    public void BankCubesTouchTime(float time)
    {
        cubesTouchInSeconds += time;
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

    public void AddTime(InteractType interact, float time)
    {
        if (interact == InteractType.CubesGaze)
        {
            cubesGazeInSeconds += time;
        }
        else if (interact == InteractType.CubesTouch)
        {
            BankCubesTouchTime(time);
        }
        else if (interact == InteractType.LotusGaze)
        {
            lotusGazeInSeconds += time;
        }
        else if (interact == InteractType.LotusTouch)
        {
            BankLotusTouchTime(time);
        }
    }

}
