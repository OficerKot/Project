using System.Collections;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;
    private Vector3 targetPosition;

    public Transform target;
    public Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        targetPosition = target.position;
        targetPosition.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping);
    }
}
