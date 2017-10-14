using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionReflectFloat : MonoBehaviour {

    public TouchGazeManager.InteractType interact;
    public CampReflectFloat output;
    public float timeMultiplier = 1;
    
	void Update () {
        output.SetValue(TouchGazeManager.Instance.GetTime(interact) * timeMultiplier);	
	}
}