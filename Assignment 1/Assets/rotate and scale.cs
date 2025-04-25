using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class rotateandscale : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float scaleSpeed = 0.5f;

    private XRGrabInteractable grabInteractable;
    private ActionBasedController leftController;
    private ActionBasedController rightController;

    private bool isGrabbed = false;
    private bool isSelected = false;
    private bool leftGripHeld = false;
    private bool rightGripHeld = false;

    public Material originalMat;
    public Material hightlightMat;

    private script test_left;

    void Start()
    {
        //Debug.Log("Access Script: " + test_left.selectedList.Length);
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        leftController = GameObject.FindWithTag("LeftController").GetComponent<ActionBasedController>();
        rightController = GameObject.FindWithTag("RightController").GetComponent<ActionBasedController>();
        test_left = GameObject.FindWithTag("LeftController").GetComponent<script>();

        leftController.activateAction.action.performed += OnLeftGripDown;
        leftController.activateAction.action.canceled += OnLeftGripUp;

        rightController.activateAction.action.performed += OnRightGripDown;
        rightController.activateAction.action.canceled += OnRightGripUp;
    }

    void Update()
    {

        if (test_left.selectedList.Contains(gameObject.GetInstanceID()))
        { 
            isSelected = true;
        } else isSelected = false;

        var goRenderer = gameObject.GetComponentInChildren<MeshRenderer>();

        if (!isGrabbed & !isSelected)
        {
            goRenderer.material = originalMat;
            return;
        }

        goRenderer.material = hightlightMat;

        Rigidbody rb = GetComponent<Rigidbody>();

        if (leftGripHeld)
        {
            Debug.Log("Left Grip is pressed");

            if (rb != null)
            {
                rb.isKinematic = true;
            }

            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        else if (rightGripHeld)
        {
            Debug.Log("Right Grip is pressed");

            if (rb != null)
            {
                rb.isKinematic = true;
            }

            float scaleAmount = 1 + scaleSpeed * Time.deltaTime;
            transform.localScale *= scaleAmount;
        }
        else
        {
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }

        //if (rightGripHeld)
        //{
        //    Debug.Log("Right Grip is pressed");

        //    if (rb != null)
        //    {
        //        rb.isKinematic = true;
        //    }

        //    float scaleAmount = 1 + scaleSpeed * Time.deltaTime;
        //    transform.localScale *= scaleAmount;
        //}
        //else
        //{
        //    if (rb != null)
        //    {
        //        rb.isKinematic = false;
        //    }
        //}
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;

        //  Debug info on grab
        int id = gameObject.GetInstanceID();
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        Vector3 scale = transform.localScale;

        Debug.Log($"[Grabbed] Object ID: {id}, Position: {pos}, Rotation: {rot.eulerAngles}, Scale: {scale}");
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    void OnLeftGripDown(InputAction.CallbackContext ctx) => leftGripHeld = true;
    void OnLeftGripUp(InputAction.CallbackContext ctx) => leftGripHeld = false;
    void OnRightGripDown(InputAction.CallbackContext ctx) => rightGripHeld = true;
    void OnRightGripUp(InputAction.CallbackContext ctx) => rightGripHeld = false;

    void OnDestroy()
    {
        if (leftController != null)
        {
            leftController.activateAction.action.performed -= OnLeftGripDown;
            leftController.activateAction.action.canceled -= OnLeftGripUp;
        }

        if (rightController != null)
        {
            rightController.activateAction.action.performed -= OnRightGripDown;
            rightController.activateAction.action.canceled -= OnRightGripUp;
        }
    }
}