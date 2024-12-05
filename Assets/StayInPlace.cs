using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInPlace : MonoBehaviour
{
    private Vector3 initialPosition;

    void Start()
    {
        // Guarda la posici�n inicial del personaje.
        initialPosition = transform.position;
    }

    void Update()
    {
        // Mant�n al personaje en su posici�n inicial.
        transform.position = initialPosition;
    }
}