using UnityEngine;

public class ThrottleConstraint : MonoBehaviour
{
    public Transform anchor;
    public float minZ = 0f, maxZ = 1f;

    void LateUpdate()
    {
        Vector3 offset = transform.position - anchor.position;
        float z = Vector3.Dot(offset, anchor.forward);
        z = Mathf.Clamp(z, minZ, maxZ);
        transform.position = anchor.position + anchor.forward * z;
    }
}
