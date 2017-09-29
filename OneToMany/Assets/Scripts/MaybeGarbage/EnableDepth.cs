using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDepth : MonoBehaviour {

	void Start ()
    {
        foreach (var cam in Camera.allCameras)
        {
            cam.depthTextureMode = DepthTextureMode.Depth;
        }
	}

}
