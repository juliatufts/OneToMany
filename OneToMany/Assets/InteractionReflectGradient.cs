using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionReflectGradient : MonoBehaviour {

    public TouchGazeManager.InteractType interact;
    public CampReflectColor output;
    public Gradient gradient;

    void Update()
    {
        output.SetValue(gradient.Evaluate(TouchGazeManager.Instance.GetTime(interact)));
    }
}
