using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class DroneSteeringController : MonoBehaviour
{
    [Header("Input References")]
    public Transform throttleHandle;
    public Transform throttleAnchor;

    [Header("Motion Parameters")]
    public float throttleSpeed = 500f;
    public float pitchSensitivity = 50f;
    public float yawSensitivity = 50f;
    public float maxPitch = 30f; // degrees/sec
    public float maxYaw = 60f;

    [Header("Smoothing")]
    public float smoothing = 5f;

    private XRHandSubsystem handSubsystem;
    private XRHand leftHand;
    private XRHand rightHand;

    private float currentPitch = 0f;
    private float currentYaw = 0f;

    private bool isSteering = false;
    private Vector3 leftNeutralPosition;

    void Start()
    {
        handSubsystem = XRGeneralSettings.Instance.Manager.activeLoader.GetLoadedSubsystem<XRHandSubsystem>();
    }

    void Update()
    {
        if (handSubsystem == null) return;

        leftHand = handSubsystem.leftHand;
        rightHand = handSubsystem.rightHand;

        // Throttle (right hand position relative to anchor)
        if (rightHand.isTracked)
        {
            float throttle = GetThrottleAmount();
            //transform.position += transform.forward * throttle * throttleSpeed * Time.deltaTime;
            transform.position += -transform.up * throttle * throttleSpeed * Time.deltaTime;
        }

        // Steering
        if (leftHand.isTracked)
        {
            bool pinching = IsLeftPinching(leftHand);

            if (pinching && !isSteering)
            {
                // Begin steering: save neutral position
                if (leftHand.GetJoint(XRHandJointID.Palm).TryGetPose(out Pose palmPose))
                {
                    leftNeutralPosition = palmPose.position;
                    isSteering = true;
                }
            }
            else if (pinching && isSteering)
            {
                if (leftHand.GetJoint(XRHandJointID.Palm).TryGetPose(out Pose palmPose))
                {
                    Vector3 offset = palmPose.position - leftNeutralPosition;

                    float targetPitch = Mathf.Clamp(-offset.y * pitchSensitivity * 100f, -maxPitch, maxPitch);
                    float targetYaw = Mathf.Clamp(offset.x * yawSensitivity * 100f, -maxYaw, maxYaw);

                    currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * smoothing);
                    currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * smoothing);

                    //transform.Rotate(currentPitch * Time.deltaTime, currentYaw * Time.deltaTime, 0f, Space.Self);
                    transform.Rotate(-currentPitch * Time.deltaTime, 0f, 0f, Space.Self); // Pitch around local X
                    transform.Rotate(0f, 0f, -currentYaw * Time.deltaTime, Space.Self);    // Yaw around local Z
                }
            }
            else
            {
                // Stop steering when pinch released
                isSteering = false;
                currentPitch = Mathf.Lerp(currentPitch, 0f, Time.deltaTime * smoothing);
                currentYaw = Mathf.Lerp(currentYaw, 0f, Time.deltaTime * smoothing);
            }
        }
    }

    float GetThrottleAmount()
    {
        Vector3 offset = throttleHandle.position - throttleAnchor.position;
        float z = Vector3.Dot(offset, throttleAnchor.forward);
        return Mathf.Clamp01(z);

    }

    bool IsLeftPinching(XRHand hand)
    {
        if (
            hand.GetJoint(XRHandJointID.IndexTip).TryGetPose(out Pose indexPose) &&
            hand.GetJoint(XRHandJointID.ThumbTip).TryGetPose(out Pose thumbPose)
        )
        {
            float pinchDistance = Vector3.Distance(indexPose.position, thumbPose.position);
            return pinchDistance < 0.03f;
        }
        return false;
    }
}
