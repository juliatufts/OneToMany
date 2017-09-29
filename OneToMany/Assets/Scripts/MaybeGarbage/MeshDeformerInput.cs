using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {

    public float force = 10f;
    public float forceOffset = 0.1f;
    public float raycastDist = 0.05f;
    public float raycastExtension = 1f;

    void Update()
    {
        Ray ray = new Ray(transform.position + -1f * raycastExtension * transform.forward, transform.forward);
        Debug.DrawLine(ray.origin, ray.origin + (raycastDist + raycastExtension) * ray.direction, Color.magenta);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDist + raycastExtension))
        {
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if (deformer)
            {
                Vector3 point = hit.point;
                point += hit.normal * forceOffset;
                deformer.AddDeformingForce(point, force);
            }
        }
    }
}
