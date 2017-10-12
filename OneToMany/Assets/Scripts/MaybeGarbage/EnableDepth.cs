using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDepth : MonoBehaviour {

    // note: harrison made me
	void Update ()
    {
        foreach (var cam in Camera.allCameras)
        {
            cam.depthTextureMode = DepthTextureMode.Depth;
        }
	}

}
