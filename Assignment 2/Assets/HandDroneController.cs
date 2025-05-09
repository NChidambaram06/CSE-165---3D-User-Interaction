using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;
public class HandDroneController : MonoBehaviour
{

    public XRHandSubsystem handSubsystem;
    public Transform droneBody;           // The object that moves (has camera inside it)
    public float throttleSpeed = 5f;
    public float yawSensitivity = 60f;
    public float pitchSensitivity = 60f;

    // Define a comfortable range for hand movement
    public float minThrottleHeight = 0.2f;
    public float maxThrottleHeight = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (handSubsystem == null) return;

        XRHand leftHand = handSubsystem.leftHand;
        XRHand rightHand = handSubsystem.rightHand;

        if (rightHand.isTracked && rightHand.GetJoint(XRHandJointID.Palm).TryGetPose(out Pose rightPalmPose))
        {
            // Map Y height of right hand to throttle
            float normalizedThrottle = Mathf.Clamp01((rightPalmPose.position.y - minThrottleHeight) / (maxThrottleHeight - minThrottleHeight));
            Vector3 forward = droneBody.forward;
            droneBody.position += forward * normalizedThrottle * throttleSpeed * Time.deltaTime;
        }

        if (leftHand.isTracked && leftHand.GetJoint(XRHandJointID.Palm).TryGetPose(out Pose leftPalmPose))
        {
            // Use left hand local offset to determine pitch and yaw
            Vector3 localOffset = droneBody.InverseTransformPoint(leftPalmPose.position);
            float yaw = localOffset.x * yawSensitivity * Time.deltaTime;
            float pitch = -localOffset.y * pitchSensitivity * Time.deltaTime;

            droneBody.Rotate(pitch, yaw, 0, Space.Self);
        }
    }
}
