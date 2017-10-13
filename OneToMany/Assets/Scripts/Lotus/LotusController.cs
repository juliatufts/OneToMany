using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusController : MonoBehaviour {

    public Transform LotusCollidersRoot;
    public Gradient colorGradient;
    public float timeScale = 0.1f;

    int numHoles;
    List<Color> fullPalette;

    void Awake()
    {
        // If palette isn't big enough, fill it out
        var lotusChildren = LotusCollidersRoot.GetComponentsInChildren<LotusHoleView>();
        var newPalette = new List<Color>();
        for (int i = 0; i < lotusChildren.Length; i++)
        {
            var randomKey = Random.Range(0f, 0.5f);
            lotusChildren[i].value = randomKey;
            newPalette.Add(colorGradient.Evaluate(randomKey));
        }
        fullPalette = newPalette;

        // Assign to Lotus holes
        for (int i = 0; i < lotusChildren.Length; i++)
        {
            lotusChildren[i].holeColor = fullPalette[i];
            lotusChildren[i].controller = this;
        }
    }
}
