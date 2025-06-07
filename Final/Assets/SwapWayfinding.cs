using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapWayfiding : MonoBehaviour
{
    public GameObject arrow;
    public WaypointManager waypointManager;
    private Vector3 initialLocalPosition;

    private void Awake()
    {
        initialLocalPosition = transform.localPosition;
    }

    void LateUpdate()
    {
        transform.localPosition = initialLocalPosition;
    }

    public void swapMethod()
    {
        if (arrow.GetComponent<Renderer>().enabled)
        {
            arrow.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            arrow.GetComponent<Renderer>().enabled = true;
        }

        foreach (Transform waypoint in waypointManager.waypoints)
        {
            if (waypoint.GetComponent<AudioSource>().enabled)
            {
                waypoint.GetComponent<AudioSource>().enabled = false;
            }
            else
            {
                waypoint.GetComponent<AudioSource>().enabled = true;
            }
        }
    }
}
