using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandControllerSwitcher : MonoBehaviour
{
//    [Header("Left")]
//    public GameObject leftHandModel;
//    public GameObject leftControllerModel;

//    [Header("Right")]
//    public GameObject rightHandModel;
//    public GameObject rightControllerModel;

//    void Update()
//    {
//        bool isLeftHandActive = OVRInput.IsControllerConnected(OVRInput.Controller.LHand);
//        bool isRightHandActive = OVRInput.IsControllerConnected(OVRInput.Controller.RHand);

//        if (leftHandModel != null) leftHandModel.SetActive(isLeftHandActive);
//        if (leftControllerModel != null) leftControllerModel.SetActive(!isLeftHandActive);

//        if (rightHandModel != null) rightHandModel.SetActive(isRightHandActive);
//        if (rightControllerModel != null) rightControllerModel.SetActive(!isRightHandActive);
//    }
//}

//using UnityEngine;

//public class InputModeController : MonoBehaviour
//{
    public OVRHand rightHand;
    public OVRSkeleton rightSkeleton;
    public GameObject agent;

    public float moveSpeed = 1.0f;

    void Update()
    {
        var activeController = OVRInput.GetActiveController();
        Debug.Log("active controller: " + activeController);

        if (activeController == OVRInput.Controller.Hands)
        {
            HandleHandTrackingInput();
        }
    }

    void HandleHandTrackingInput()
    {
        if (rightHand.IsTracked && rightSkeleton.IsDataValid && rightSkeleton.IsDataHighConfidence)
        {
            if (IsPointingGesture(rightSkeleton))
            {
                Vector3 direction = rightSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.forward;
                Vector3 moveDirection = new Vector3(direction.x, 0, direction.z).normalized;
                Debug.Log($"MoveDirection: {moveDirection}");
                agent.transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            }
        }
    }

    bool IsPointingGesture(OVRSkeleton skeleton)
    {
        Transform indexTip = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform;
        Transform middleTip = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_MiddleTip].Transform;
        Transform ringTip = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_RingTip].Transform;
        Transform pinkyTip = skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_PinkyTip].Transform;

        float spread = (Vector3.Distance(middleTip.position, ringTip.position) +
                        Vector3.Distance(ringTip.position, pinkyTip.position)) / 2f;



        bool indexUp = indexTip.position.y > middleTip.position.y + 0.01f;
        bool othersDown = spread < 0.03f;

        return indexUp && othersDown;
    }
}

