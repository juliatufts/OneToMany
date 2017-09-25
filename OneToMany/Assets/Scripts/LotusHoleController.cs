using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusHoleController : MonoBehaviour {

    public GameObject holeEffectPrefab;

    void OnTriggerEnter(Collider other)
    {
		if (other.transform.CompareTag("GameController"))
		{
            // TODO: material VFX
            // TODO: don't trigger too often

            //currentEffect = Instantiate(holeEffectPrefab, transform.position, transform.rotation);
        }
    }

}
