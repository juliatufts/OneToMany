using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusHoleController : MonoBehaviour {

    public GameObject lotus;
    public GameObject holeEffectPrefab;
    GameObject currentEffect;
    Material lotusMaterial;

    void Start()
    {
        lotusMaterial = lotus.GetComponent<Renderer>().material;
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.transform.CompareTag("GameController"))
		{
			currentEffect = Instantiate(holeEffectPrefab, transform.position, transform.rotation);
		}
    }
}
