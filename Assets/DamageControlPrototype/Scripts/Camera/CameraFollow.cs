using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothingFactor;

    Vector3 camOffset;

    void Start()
    {
        camOffset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + camOffset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothingFactor * Time.deltaTime);
    }
}
