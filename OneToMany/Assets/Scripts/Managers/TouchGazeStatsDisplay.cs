using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchGazeStatsDisplay : MonoBehaviour {

    Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        var str = "Lotus Touch: " + TouchGazeManager.Instance.LotusTouch;
        str += "\nLotus Gaze: " + TouchGazeManager.Instance.LotusGaze;
        str += "\nCubes Touch: " + TouchGazeManager.Instance.CubesTouch;
        str += "\nCubes Gaze: " + TouchGazeManager.Instance.CubesGaze;
        text.text = str;
    }
}
