using UnityEngine;

public class ControlRigFollower : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 localOffset = new Vector3(0, -0.3f, 0.5f);

    void LateUpdate()
    {
        transform.position = cameraTransform.position + cameraTransform.TransformDirection(localOffset);
        transform.rotation = Quaternion.LookRotation(cameraTransform.forward, Vector3.up);
    }
}
