using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class TutorialCube : MonoBehaviour {

    public float rotateSpeed = 90f;
    public float wobbleSpeed = 1f;
    public float wobbleDist = 0.1f;

    bool isGrabbed;
    Vector3 originalPos;
    MeshRenderer meshRenderer;

    void Start()
    {
        originalPos = transform.position;
        meshRenderer = GetComponent<MeshRenderer>();

        VRTK_InteractableObject vrtk = GetComponent<VRTK_InteractableObject>();
        vrtk.InteractableObjectGrabbed += Grabbed;
        vrtk.InteractableObjectUngrabbed += Ungrabbed;
    }

    void OnDestroy()
    {
        meshRenderer.material.SetFloat("_Progress", 1f);

        VRTK_InteractableObject vrtk = GetComponent<VRTK_InteractableObject>();
        vrtk.InteractableObjectGrabbed -= Grabbed;
        vrtk.InteractableObjectUngrabbed -= Ungrabbed;
    }

    void Grabbed(object sender, InteractableObjectEventArgs e)
    {
        isGrabbed = true;
    }

    void Ungrabbed(object sender, InteractableObjectEventArgs e)
    {
        isGrabbed = false;
    }

    // Update is called once per frame
    void Update()
    {
        //var rand = Klak.Math.Perlin.Noise(rotateSpeed * Time.time);
        //var posRand = (rand + 1f) / 2f;
        //transform.Rotate(new Vector3(rotateSpeed * posRand * Time.deltaTime, rotateSpeed * Time.deltaTime, rotateSpeed * posRand * Time.deltaTime));
        //transform.position = new Vector3(originalPos.x, originalPos.y + wobbleDist * Klak.Math.Perlin.Noise(wobbleSpeed * Time.time), originalPos.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("GameController"))
        {
            StartCoroutine(FadeOut());
            //SessionManager.Instance.tutorialComplete = true;
        }
    }

    IEnumerator FadeOut()
    {
        var delta = 0.05f;
        var progress = 1f;
        while (progress > 0f)
        {
            progress -= delta;
            meshRenderer.material.SetFloat("_Progress", progress);
            yield return new WaitForEndOfFrame();
        }
    }
}
