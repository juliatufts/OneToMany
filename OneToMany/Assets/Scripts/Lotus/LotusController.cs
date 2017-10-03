using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusController : MonoBehaviour {

    public Transform LotusCollidersRoot;
    public Gradient colorGradient;
    public float timeScale = 0.1f;

	[HideInInspector]
	public float timeTouched;

	[HideInInspector]
	public float timeGazed;

    int numHoles;
    List<Color> fullPalette;

    void Awake()
    {
        // If palette isn't big enough, fill it out
        numHoles = LotusCollidersRoot.childCount;
        var newPalette = new List<Color>();
        for (int i = 0; i < numHoles; i++)
        {
            var randomKey = Random.Range(0f, 1f);
            newPalette.Add(colorGradient.Evaluate(randomKey));
        }
        fullPalette = newPalette;

        // Assign to Lotus holes
        var lotusChildren = LotusCollidersRoot.GetComponentsInChildren<LotusHoleView>();
        for (int i = 0; i < lotusChildren.Length; i++)
        {
            lotusChildren[i].holeColor = fullPalette[i];
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
