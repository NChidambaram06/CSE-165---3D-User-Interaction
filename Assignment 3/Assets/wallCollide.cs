using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class wallCollide : MonoBehaviour
{
    public bool hasCollidedWithWall = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("test collision - not wall");
        if (other.CompareTag("Wall"))
        {
            OnHitWall();
        }
    }

    void OnHitWall()
    {
        Debug.Log("test collision - Character hit a wall!");
        // Add your behavior here
        hasCollidedWithWall = true;
    }
}
