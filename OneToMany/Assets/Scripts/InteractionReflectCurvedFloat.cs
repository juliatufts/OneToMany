using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionReflectCurvedFloat : MonoBehaviour {

    public TouchGazeManager.InteractType interact;
    public CampReflectFloat output;
    public AnimationCurve curve;

    void Update()
    {
        output.SetValue(curve.Evaluate(TouchGazeManager.Instance.GetTime(interact)));
    }
}
