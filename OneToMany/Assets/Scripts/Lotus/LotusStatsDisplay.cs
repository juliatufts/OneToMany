using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LotusStatsDisplay : MonoBehaviour {

    public LotusController lotus;
    Text text;

	void Start ()
    {
        text = GetComponent<Text>();
	}
	
	void Update ()
    {
        var str = "Lotus Touch: " + lotus.timeTouched;
        str += "\nLotus Gaze: " + lotus.timeGazed;
        text.text = str;
	}
}
