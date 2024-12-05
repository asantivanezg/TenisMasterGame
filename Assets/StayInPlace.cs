using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInPlace : MonoBehaviour
{
    private Vector3 initialPosition;

    void Start()
    {
        // Guarda la posición inicial del personaje.
        initialPosition = transform.position;
    }

    void Update()
    {
        // Mantén al personaje en su posición inicial.
        transform.position = initialPosition;
    }
}