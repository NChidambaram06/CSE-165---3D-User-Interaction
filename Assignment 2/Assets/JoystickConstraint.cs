using UnityEngine;

public class JoystickConstraint : MonoBehaviour
{
    public Transform anchor;
    public float maxX = 0.2f, maxY = 0.2f;

    void LateUpdate()
    {
        Vector3 localOffset = anchor.InverseTransformPoint(transform.position);
        localOffset.z = 0;
        localOffset.x = Mathf.Clamp(localOffset.x, -maxX, maxX);
        localOffset.y = Mathf.Clamp(localOffset.y, -maxY, maxY);
        transform.position = anchor.TransformPoint(localOffset);
    }
}
