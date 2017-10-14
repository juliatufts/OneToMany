using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Camp Cult/Displacement/SineShift")]
public class SineShift : ImageEffectBase
{

    public Vector2 freq;
    public Vector2 phase;
    public Vector2 phasePerSecond;
    public Vector2 amp;
    Vector2 p = Vector2.zero;
    
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        p += phasePerSecond * Time.deltaTime* Mathf.PI * 2;
        material.SetVector("_X", new Vector4(freq.x*Mathf.PI*2, p.x, amp.x, 0));
        material.SetVector("_Y", new Vector4(freq.y * Mathf.PI * 2, p.y, amp.y, 0));
        Graphics.Blit(source, destination, material);
    }
}
