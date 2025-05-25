using UnityEngine;
using TMPro; // Or UnityEngine.UI if using the legacy Dropdown

public class VRObjectSpawner : MonoBehaviour
{
    public TMP_Dropdown objectDropdown; // Assign in Inspector
    public GameObject[] objectPrefabs;  // Assign: [0] = Cube, [1] = Sphere
    public Transform spawnLocation;     // Where to instantiate

    public void SpawnSelectedObject()
    {
        int index = objectDropdown.value;
        if (index < objectPrefabs.Length)
        {
            Instantiate(objectPrefabs[index], spawnLocation.position, spawnLocation.rotation);
        }
        else
        {
            Debug.LogWarning("Selected index out of range!");
        }
    }
}
