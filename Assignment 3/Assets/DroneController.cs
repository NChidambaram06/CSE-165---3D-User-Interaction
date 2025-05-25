using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DroneController : MonoBehaviour
{
    public Transform throttleHandle;
    public Transform throttleAnchor;
    public Transform joystickHandle;
    public Transform joystickAnchor;

    public float throttleSpeed = 5f;
    public float yawSensitivity = 60f;
    public float pitchSensitivity = 60f;

    private Vector3 joystickNeutralOffset;
    private bool isJoyStickGrabbed = false;

    private void Start()
    {
        var grab = joystickHandle.GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnJoystickGrabbed);
        grab.selectExited.AddListener(OnJoystickReleased);
    }

    void OnJoystickGrabbed(SelectEnterEventArgs args)
    {
        joystickNeutralOffset = joystickAnchor.InverseTransformPoint(joystickHandle.position);
        isJoyStickGrabbed = true;
    }

    void  OnJoystickReleased(SelectExitEventArgs args)
    {
        isJoyStickGrabbed = false;
    }

    void Update()
    {
        Vector3 currentLocalOffset = joystickAnchor.InverseTransformPoint(joystickAnchor.position);

        // --- THROTTLE ---
        float zOffset = Vector3.Dot(throttleHandle.position - throttleAnchor.position, throttleAnchor.forward);
        float throttle = Mathf.Clamp01(zOffset);
        transform.position += transform.forward * throttle * throttleSpeed * Time.deltaTime;

        // --- JOYSTICK ---
        Vector3 localOffset = joystickAnchor.InverseTransformPoint(joystickHandle.position);
        float yaw = localOffset.x * yawSensitivity * Time.deltaTime;
        float pitch = -localOffset.y * pitchSensitivity * Time.deltaTime;

        transform.Rotate(pitch, yaw, 0f, Space.Self);

        //if (isJoyStickGrabbed)
        // {
        //Vector3 delta = currentLocalOffset - joystickNeutralOffset;

        //float yaw = delta.x * yawSensitivity * Time.deltaTime;
        //float pitch = -delta.y * pitchSensitivity * Time.deltaTime;

        //transform.Rotate(pitch, yaw, 0f, Space.Self);
        // }
    }
}
