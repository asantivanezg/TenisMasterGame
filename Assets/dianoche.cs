using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dianoche : MonoBehaviour
{
    public int velocidadtrancicion = 10;
    void Update()
    {
        transform.Rotate(velocidadtrancicion * Time.deltaTime,0,0);
    }
}
