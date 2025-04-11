using System;
using System.Collections;
using System.Collections.Generic;

// BrickWallGenerator.cs
using UnityEngine;

public class BrickWallGenerator : MonoBehaviour
{
    public GameObject brickPrefab;  // The brick prefab to be used
    public int layers = 20;         // Number of layers in the wall
    public float radius = 5f;       // Radius of the cylinder
    public int bricksPerLayer = 10; // Number of bricks in each layer
    public float heightPerLayer = 0.1f;  // Height of each layer

    void Start()
    {
        BuildWall();
    }

    void BuildWall()
    {
        for (int layer = 0; layer < layers; layer++)
        {
            // Calculate the vertical position of the current layer
            float layerHeight = ((float)layer) * brickPrefab.transform.localScale.y;

            // Calculate the angle between each brick on the current layer
            float angleStep = 360f / bricksPerLayer;

            Debug.Log(((float)layer) + " * " + brickPrefab.transform.localScale.y + " = " + layerHeight);

            for (int i = 0; i < bricksPerLayer; i++)
            {
                // Calculate the position of each brick in this layer
                float angle = i * angleStep;
                float x = radius * Mathf.Cos(Mathf.Deg2Rad * angle);
                float z = radius * Mathf.Sin(Mathf.Deg2Rad * angle);
                Vector3 position = new Vector3(x, layerHeight, z);

                // Instantiate the brick at the calculated position
                GameObject brick = Instantiate(brickPrefab, position, Quaternion.identity);
                brick.transform.SetParent(transform); // Parent to the wall object to keep hierarchy clean

            }
        }
    }
}