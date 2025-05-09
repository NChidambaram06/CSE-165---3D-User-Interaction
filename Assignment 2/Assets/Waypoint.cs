using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public Material foundMaterial;
    public Material findingMaterial;
    public GameObject arrow; // The arrow object on the drone
    public int currentIndex = 0;
    private Transform dronePos;
    public GameObject drone;
    public gameTimer gameTimer;


    void Start()
    {
        dronePos = drone.transform;

        if (waypoints.Count > 0)
        {
            SetMaterial(waypoints[0], findingMaterial);
        }
    }

    void Update()
    {
        if (currentIndex >= waypoints.Count) return;

        Transform target = waypoints[currentIndex];

        // Rotate arrow to face current target
        Vector3 dir = target.position - arrow.transform.position;
        //dir.y = 0; // Keep arrow horizontal
        if (dir != Vector3.zero)
        {
            arrow.transform.rotation = Quaternion.LookRotation(dir);
            arrow.transform.Rotate(90, 0, 0);
        }

        if (!target.GetComponent<AudioSource>().isPlaying) target.GetComponent<AudioSource>().Play();

        float dist = Vector3.Distance(dronePos.position, target.position);
        Debug.Log("Waypoint: " + dist);
        if (dist <= 20.0f)
        {
            // Mark current as found
            SetMaterial(target, foundMaterial);
            target.GetComponent<AudioSource>().Stop();

            currentIndex++;
            if (currentIndex < waypoints.Count)
            {
                SetMaterial(waypoints[currentIndex], findingMaterial);
            }
            else
            {
                Debug.Log("All waypoints reached!");
                gameTimer.StopGame();

            }
        }
    }

    void SetMaterial(Transform obj, Material mat)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = mat;
        }
    }
}
