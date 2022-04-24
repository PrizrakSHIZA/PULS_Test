using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    void Update()
    {
        if (PlayerController.Singleton.basicGravity)
            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, -50 * LevelGenerator.Singleton.Speed * Time.deltaTime));
        else
            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, 50 * LevelGenerator.Singleton.Speed * Time.deltaTime));
    }
}
