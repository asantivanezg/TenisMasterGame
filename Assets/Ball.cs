using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Esta clase controla el comportamiento de la pelota en el juego.
/// Maneja la colisi�n con muros y zonas de fuera, y lleva un registro de la puntuaci�n.
/// </summary>
public class Ball : MonoBehaviour
{
    Vector3 initialPos; // Almacena la posici�n inicial de la pelota.
    public string hitter; // Indica qui�n golpe� la pelota (jugador o bot).

    int playerScore; // Puntuaci�n del jugador.
    int botScore; // Puntuaci�n del bot.

    [SerializeField] TextMeshProUGUI playerScoreText; // Referencia al texto de puntuaci�n del jugador.
    [SerializeField] TextMeshProUGUI botScoreText; // Referencia al texto de puntuaci�n del bot.

    public bool playing = true; // Indica si el juego est� en curso.

    /// <summary>
    /// Inicializa la posici�n de la pelota y las puntuaciones al inicio del juego.
    /// </summary>
    /// 

    [Header("Sonidos")]
    public AudioClip commonSound; // Sonido para muro, Out y Out2.
    public AudioClip playerSound; // Sonido al chocar con el jugador.

    private AudioSource audioSource; // Referencia al componente AudioSource.

    private void Start()
    {
        initialPos = transform.position; // Guarda la posici�n inicial de la pelota.
        playerScore = 0; // Inicializa la puntuaci�n del jugador.
        botScore = 0; // Inicializa la puntuaci�n del bot.
        audioSource = GetComponent<AudioSource>(); //
    }

    /// <summary>
    /// Maneja las colisiones de la pelota con otros objetos.
    /// </summary>
    /// <param name="collision">Informaci�n sobre la colisi�n.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Out"))
        {
            // L�gica si la pelota toca una zona 'Out'
            Debug.Log("La pelota toc� una zona OUT");
            // Si el comportamiento en "Out" no debe afectar el rebote, puedes devolver el flujo aqu�.
            return;
        }
        else
        {
            // L�gica normal para el rebote
            Debug.Log("La pelota rebota");
        }

        // Reproducir sonidos seg�n el objeto con el que colisiona.
        if (collision.transform.CompareTag("muro") || collision.transform.CompareTag("Out") || collision.transform.CompareTag("Out2") || collision.transform.CompareTag("neutro"))
        {
            PlaySound(commonSound); // Reproduce el sonido com�n.
        }
        else if (collision.transform.CompareTag("playerson"))
        {
            PlaySound(playerSound); // Reproduce el sonido del jugador.
        }

        // Verifica si la colisi�n es con un muro.
        if (collision.transform.CompareTag("muro"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; // Detiene la velocidad de la pelota.
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // Detiene la rotaci�n de la pelota.

            GameObject.Find("player").GetComponent<Player>().Reset(); // Resetea la posici�n del jugador.

            // Si el juego est� en curso, actualiza la puntuaci�n.
            if (playing)
            {
                if (hitter == "player") // Si el jugador fue el que golpe�.
                {
                    playerScore++; // Incrementa la puntuaci�n del jugador.
                }
                else if (hitter == "bot") // Si el bot fue el que golpe�.
                {
                    botScore++; // Incrementa la puntuaci�n del bot.
                }
                playing = false; // Cambia el estado a no jugando.
                updateScore(); // Actualiza el texto de la puntuaci�n.
            }
        }
        // Verifica si la colisi�n es con una zona fuera.
        else if (collision.transform.CompareTag("Out"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; // Detiene la velocidad de la pelota.
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // Detiene la rotaci�n de la pelota.

            GameObject.Find("player").GetComponent<Player>().Reset(); // Resetea la posici�n del jugador.

            // Si el juego est� en curso, actualiza la puntuaci�n.
            if (playing)
            {
                if (hitter == "player") // Si el jugador fue el que golpe�.
                {
                    playerScore++; // Incrementa la puntuaci�n del jugador.
                }
                else if (hitter == "bot") // Si el bot fue el que golpe�.
                {
                    botScore++; // Incrementa la puntuaci�n del bot.
                }
                playing = false; // Cambia el estado a no jugando.
                updateScore(); // Actualiza el texto de la puntuaci�n.
            }
        }
        // Verifica si la colisi�n es con otra zona fuera (Out2).
        else if (collision.transform.CompareTag("Out2"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; // Detiene la velocidad de la pelota.
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // Detiene la rotaci�n de la pelota.

            GameObject.Find("player").GetComponent<Player>().Reset(); // Resetea la posici�n del jugador.

            // Si el juego est� en curso, actualiza la puntuaci�n.
            if (playing)
            {
                if (hitter == "player") // Si el jugador fue el que golpe�.
                {
                    botScore++; // Incrementa la puntuaci�n del bot.
                }
                else if (hitter == "bot") // Si el bot fue el que golpe�.
                {
                    playerScore++; // Incrementa la puntuaci�n del jugador.
                }
                playing = false; // Cambia el estado a no jugando.
                updateScore(); // Actualiza el texto de la puntuaci�n.
            }
        }
    }
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // Reproduce el sonido una vez.
        }
    }
    /// <summary>
    /// Maneja las colisiones de la pelota con los triggers.
    /// </summary>
    /// <param name="other">Collider con el que colisiona la pelota.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si colisiona con una zona fuera.
        if (other.CompareTag("Out") && playing)
        {
            if (hitter == "player") // Si el jugador golpe� la pelota.
            {
                playerScore++;
                 // Incrementa la puntuaci�n del bot.
            }
            else if (hitter == "bot") // Si el bot golpe� la pelota.
            {
                botScore++; // Incrementa la puntuaci�n del jugador.
            }
            playing = false; // Cambia el estado a no jugando.
            updateScore(); // Actualiza el texto de la puntuaci�n.
        }
        if (other.CompareTag("playerson"))
        {
            Debug.Log("�Colisi�n con el jugador detectada en Trigger!");
            PlaySound(playerSound); // Reproduce el sonido del jugador.
        }
    }

    /// <summary>
    /// Actualiza el texto de puntuaci�n en la interfaz de usuario.
    /// </summary>
    void updateScore()
    {
        playerScoreText.text = "Player:  " + playerScore; // Actualiza la puntuaci�n del jugador en la UI.
        botScoreText.text = "Bot:  " + botScore; // Actualiza la puntuaci�n del bot en la UI.
    }
}

