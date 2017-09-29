using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusController : MonoBehaviour {

    public Transform LotusCollidersRoot;
    public Color[] palette;
    public float timeScale = 0.1f;

	[HideInInspector]
	public float timeTouched;

	[HideInInspector]
	public float timeGazed;

    int numHoles;

    void Start()
    {
        // If palette isn't big enough, fill it out
        numHoles = LotusCollidersRoot.childCount;
        var newPalette = new List<Color>();
        for (int i = 0; i < numHoles; i++)
        {
            var randomIndex = Random.Range(0, palette.Length - 1);
            newPalette.Add(palette[randomIndex]);
        }
    }

    public void BankTouchTime(float time)
    {
        timeTouched += time;
    }

	public void BankGazeTime(float time)
	{
		timeGazed += time;
	}
}
