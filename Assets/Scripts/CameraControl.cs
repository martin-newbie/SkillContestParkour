using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;

    private void FixedUpdate()
    {
        Vector3 followPos = Vector3.Lerp(transform.position, target.position, Time.deltaTime * 10f);
        followPos.z = -10f;
        transform.position = followPos;
    }
}
