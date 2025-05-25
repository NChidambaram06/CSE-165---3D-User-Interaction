using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    public GamePlay gamePlay;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Machu"))
        {
            gamePlay.Crashed();
        }
    }

}
