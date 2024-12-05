using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class SceneLoader : MonoBehaviour
{
    // Este método se llamará cuando el botón sea presionado
    public void LoadScene(string parte2)
    {
        // Carga la escena por su nombre
        SceneManager.LoadScene(parte2);
    }
}