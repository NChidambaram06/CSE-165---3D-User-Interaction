using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class xyzparser : MonoBehaviour
{
    public GameObject checkpointPrefab;
    public TextAsset file;
    public WaypointManager waypointManager;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Waypoint List: " + waypointManager.waypoints.Count);
        List<Vector3> positions = ParseFile();
        foreach (Vector3 position in positions)
        {
            Debug.Log("xyzFile: " + position);
        }
        //Debug.Log("Waypoint List: " + waypointManager.waypoints.Count);
    }

    List<Vector3> ParseFile()
    {
        float ScaleFactor = 1.0f / 39.37f;
        //float ScaleFactor = 1.0f;
        List<Vector3> positions = new List<Vector3>();
        string content = file.ToString();
        string[] lines = content.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string[] coords = lines[i].Split(' ', '\t');
            Vector3 pos = new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
            positions.Add(pos * ScaleFactor);

            GameObject checkpoint = Instantiate(checkpointPrefab, pos * ScaleFactor, Quaternion.identity, transform);

            // Add to waypoint manager
            if (waypointManager != null)
            {
                waypointManager.waypoints.Add(checkpoint.transform);
            }
        }
        //Debug.Log("xyzFile: " + positions);
        return positions;
    }
}