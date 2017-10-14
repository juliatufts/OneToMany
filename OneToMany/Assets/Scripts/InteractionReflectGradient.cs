using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionReflectGradient : MonoBehaviour {

    public TouchGazeManager.InteractType interact;
    public CampReflectColor output;
    public Gradient gradient;
    public float timeMultiplier = 1;

    void Update()
    {
        output.SetValue(gradient.Evaluate(TouchGazeManager.Instance.GetTime(interact)*timeMultiplier % 1));
    }
}
