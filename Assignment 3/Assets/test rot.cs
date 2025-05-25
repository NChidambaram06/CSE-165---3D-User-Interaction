using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class testrot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (leftHand.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
        {
            Debug.Log("left controller rotation: " + rotation.eulerAngles);
        }
    }
}
