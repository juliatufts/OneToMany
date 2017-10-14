using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDepth : MonoBehaviour {

    static EnableDepth primary;
    void Update() {
        if(!primary || !primary.enabled) {
            primary = this;
        }

        if(primary == this) {
            foreach(var cam in Camera.allCameras) {
                if(!cam.GetComponent<EnableDepth>()) {
                    cam.gameObject.AddComponent<EnableDepth>();
                }
            }
        }

        var camera = GetComponent<Camera>();
        if(camera) {
            camera.depthTextureMode |= DepthTextureMode.Depth;
        }
    }

    void OnPreRender() {
        var cam = GetComponent<Camera>();
        if(!cam) return;
        var mode = cam.depthTextureMode;

        if((mode & DepthTextureMode.DepthNormals) == DepthTextureMode.DepthNormals) {
            Shader.EnableKeyword("CAMERA_DEPTHNORMALS");
        }
        else {
            Shader.EnableKeyword("CAMERA_DEPTH");
        }
    }
}
