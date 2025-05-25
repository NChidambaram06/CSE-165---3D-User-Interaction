using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class GamePlay : MonoBehaviour
{
    public gameTimer gameTimer;
    public TMP_Text counter;
    public GameObject drone;
    public WaypointManager waypointManager;
    public GameObject throtle;
    public GameObject throtleOffset;

    // Start is called before the first frame update
    void Start()
    {
        drone.transform.position = waypointManager.waypoints[waypointManager.currentIndex].position;
        drone.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y + 10f, drone.transform.position.z);
        drone.transform.rotation = GetRot(waypointManager.currentIndex);
        StartCoroutine(Started());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator countdown()
    {
        throtle.GetComponent<XRGrabInteractable>().enabled = false;
        counter.text = "3";
        yield return new WaitForSeconds(1f);
        counter.text = "2";
        yield return new WaitForSeconds(1f);
        counter.text = "1";
        drone.transform.rotation = GetRot(waypointManager.currentIndex - 1);
        yield return new WaitForSeconds(1f);
        counter.color = Color.green;
        counter.text = "GO";
        throtle.GetComponent<XRGrabInteractable>().enabled = true;
        yield return new WaitForSeconds(1f);
        counter.color = Color.red;
        counter.text = "";
    }

    public void Crashed()
    {
        throtle.transform.position = throtleOffset.transform.position;
        drone.transform.position = waypointManager.waypoints[waypointManager.currentIndex - 1].position;
        drone.transform.position = new Vector3(drone.transform.position.x, drone.transform.position.y + 10f, drone.transform.position.z);
        drone.transform.rotation = GetRot(waypointManager.currentIndex - 1);
        StartCoroutine(countdown());

        Debug.Log("Crashed");
    }
    IEnumerator Started()
    {
        throtle.GetComponent<XRGrabInteractable>().enabled = false;
        counter.text = "5";
        yield return new WaitForSeconds(1f);
        counter.text = "4";
        yield return new WaitForSeconds(1f); 
        counter.text = "3";
        yield return new WaitForSeconds(1f);
        counter.text = "2";
        yield return new WaitForSeconds(1f);
        counter.text = "1";
        yield return new WaitForSeconds(1f);
        counter.color = Color.green;
        counter.text = "GO";
        throtle.GetComponent<XRGrabInteractable>().enabled = true;
        gameTimer.StartGame();
        yield return new WaitForSeconds(1f);
        counter.color = Color.red;
        counter.text = "";
    }

    public void Finish()
    {
        throtle.transform.position = throtleOffset.transform.position;
    }

    Quaternion GetRot(int index)
    {
        if (index + 1 >= waypointManager.waypoints.Count)
        {
            // Prevent out-of-bounds
            return drone.transform.rotation;
        }

        Vector3 currentPosition = waypointManager.waypoints[index].position;
        currentPosition = new Vector3(currentPosition.x, currentPosition.y + 10f, currentPosition.z);
        Vector3 nextPosition = waypointManager.waypoints[index + 1].position;

        Vector3 direction = (nextPosition - currentPosition).normalized;

        if (direction == Vector3.zero)
            return drone.transform.rotation;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Quaternion modelRotationOffset = Quaternion.Euler(90f, 0f, -180f);
        return lookRotation * modelRotationOffset;
    }
}
