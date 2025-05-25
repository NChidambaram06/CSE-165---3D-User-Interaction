using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPlacer : MonoBehaviour
{
    public GameObject FloorObject;
    public GameObject WallObject;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(FloorObject, new Vector3(0,0,0), Quaternion.identity);
        Instantiate(WallObject, new Vector3(0, 1, 2), Quaternion.identity);
    }
}
