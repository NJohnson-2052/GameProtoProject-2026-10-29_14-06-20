using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // Drag the Player here
    public float smoothSpeed = 8f;
    public Vector3 offset = new Vector3(0, 1, -10);

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
    }
}