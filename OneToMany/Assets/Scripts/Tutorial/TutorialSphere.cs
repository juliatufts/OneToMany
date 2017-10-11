using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSphere : MonoBehaviour {

    public float rotateSpeed = 360f;
    public float wobbleSpeed = 1f;
    public float wobbleDist = 0.1f;
    Vector3 originalPos;
    MeshRenderer meshRenderer;

    void Start()
    {
        originalPos = transform.position;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void OnDestroy()
    {
        meshRenderer.material.SetFloat("_Progress", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, rotateSpeed * Time.deltaTime, 0f));
        transform.position = new Vector3(originalPos.x, originalPos.y + wobbleDist * Klak.Math.Perlin.Noise(Time.time), originalPos.z);
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
