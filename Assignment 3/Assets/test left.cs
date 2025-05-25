using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System.Linq;
using System;
using Newtonsoft.Json.Linq;
using Unity.XR.CoreUtils;
using UnityEditor.XR.LegacyInputHelpers;

public class script : MonoBehaviour
{
    public XRRayInteractor leftHand_rayInteractor;
    public XRRayInteractor rightHand_rayInteractor;
    public InputActionReference leftGrip_triggerAction;
    public InputActionReference rightTrigger_triggerAction;
    public GameObject xrOrigin;

    public int[] selectedList = new int[] { };
    private string[] spawnedList = new string[] { "Table_3(Clone)", "Surgical_chair_1(Clone)" };


// Start is called before the first frame update
void Start()
    {
        Debug.Log("Started");
    }

    // Update is called once per frame
    void Update()
    {
        var leftGrip_triggerActionRef = leftGrip_triggerAction.action;
        leftGrip_triggerActionRef.started += TryRaycastLeft;

        var rightGrip_triggerActionRef = rightTrigger_triggerAction.action;
        rightGrip_triggerActionRef.started += TryRaycastRight;



    }

    void TryRaycastLeft(InputAction.CallbackContext context)
    {
        

        Debug.Log("test subscribe left");
        if (leftHand_rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (spawnedList.Contains(hit.collider.gameObject.name))
            {   // If hit spawned object, select and manipulate
                Debug.Log("Hit Object: " + hit.collider.gameObject.name);
                Debug.Log("Hit Position: " + hit.point);
                if (selectedList.Contains(hit.collider.gameObject.GetInstanceID()) == false) 
                {
                    selectedList = selectedList.Concat(new int[] { hit.collider.gameObject.GetInstanceID() }).ToArray();
                }
                
                Debug.Log("Selected List length: " + selectedList.Length);

            } else
            {   // If nothing hit: clear selected list
                selectedList = new int[] { };
                Debug.Log("Selected List length: " + selectedList.Length);
            }
        }
        else
        {
            Debug.Log("Ray did not hit any object");
        }
    }

    void TryRaycastRight(InputAction.CallbackContext context)
    {


        Debug.Log("test subscribe right");
        if (rightHand_rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hitRight))
        {
            //if (selectedList.Length == 0)
            //{   // If nothing selected, teleport
            Debug.Log("Hit Position: " + hitRight.point);

            Transform cameraTransform = Camera.main.transform;
            Vector3 cameraOffset = cameraTransform.position - xrOrigin.transform.position;
            cameraOffset.y = 0f;
            xrOrigin.transform.position = hitRight.point - cameraOffset;

            Debug.Log("Teleporting");
            //}
            //else
            //{   // If something selected, scaling
            //    Debug.Log("scaling");
            //}
        }
        else
        {
            Debug.Log("Ray did not hit any object");
        }
    }
}
