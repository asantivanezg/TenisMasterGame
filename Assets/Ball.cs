using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Esta clase controla el comportamiento de la pelota en el juego.
/// Maneja la colisión con muros y zonas de fuera, y lleva un registro de la puntuación.
/// </summary>
public class Ball : MonoBehaviour
{
    Vector3 initialPos; // Almacena la posición inicial de la pelota.
    public string hitter; // Indica quién golpeó la pelota (jugador o bot).

    int playerScore; // Puntuación del jugador.
    int botScore; // Puntuación del bot.

    [SerializeField] TextMeshProUGUI playerScoreText; // Referencia al texto de puntuación del jugador.
    [SerializeField] TextMeshProUGUI botScoreText; // Referencia al texto de puntuación del bot.

    public bool playing = true; // Indica si el juego está en curso.

    /// <summary>
    /// Inicializa la posición de la pelota y las puntuaciones al inicio del juego.
    /// </summary>
    /// 

    [Header("Sonidos")]
    public AudioClip commonSound; // Sonido para muro, Out y Out2.
    public AudioClip playerSound; // Sonido al chocar con el jugador.

    private AudioSource audioSource; // Referencia al componente AudioSource.

    private void Start()
    {
        initialPos = transform.position; // Guarda la posición inicial de la pelota.
        playerScore = 0; // Inicializa la puntuación del jugador.
        botScore = 0; // Inicializa la puntuación del bot.
        audioSource = GetComponent<AudioSource>(); //
    }

    /// <summary>
    /// Maneja las colisiones de la pelota con otros objetos.
    /// </summary>
    /// <param name="collision">Información sobre la colisión.</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Out"))
        {
            // Lógica si la pelota toca una zona 'Out'
            Debug.Log("La pelota tocó una zona OUT");
            // Si el comportamiento en "Out" no debe afectar el rebote, puedes devolver el flujo aquí.
            return;
        }
        else
        {
            // Lógica normal para el rebote
            Debug.Log("La pelota rebota");
        }

        // Reproducir sonidos según el objeto con el que colisiona.
        if (collision.transform.CompareTag("muro") || collision.transform.CompareTag("Out") || collision.transform.CompareTag("Out2") || collision.transform.CompareTag("neutro"))
        {
            PlaySound(commonSound); // Reproduce el sonido común.
        }
        else if (collision.transform.CompareTag("playerson"))
        {
            PlaySound(playerSound); // Reproduce el sonido del jugador.
        }

        // Verifica si la colisión es con un muro.
        if (collision.transform.CompareTag("muro"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; // Detiene la velocidad de la pelota.
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // Detiene la rotación de la pelota.

            GameObject.Find("player").GetComponent<Player>().Reset(); // Resetea la posición del jugador.

            // Si el juego está en curso, actualiza la puntuación.
            if (playing)
            {
                if (hitter == "player") // Si el jugador fue el que golpeó.
                {
                    playerScore++; // Incrementa la puntuación del jugador.
                }
                else if (hitter == "bot") // Si el bot fue el que golpeó.
                {
                    botScore++; // Incrementa la puntuación del bot.
                }
                playing = false; // Cambia el estado a no jugando.
                updateScore(); // Actualiza el texto de la puntuación.
            }
        }
        // Verifica si la colisión es con una zona fuera.
        else if (collision.transform.CompareTag("Out"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; // Detiene la velocidad de la pelota.
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // Detiene la rotación de la pelota.

            GameObject.Find("player").GetComponent<Player>().Reset(); // Resetea la posición del jugador.

            // Si el juego está en curso, actualiza la puntuación.
            if (playing)
            {
                if (hitter == "player") // Si el jugador fue el que golpeó.
                {
                    playerScore++; // Incrementa la puntuación del jugador.
                }
                else if (hitter == "bot") // Si el bot fue el que golpeó.
                {
                    botScore++; // Incrementa la puntuación del bot.
                }
                playing = false; // Cambia el estado a no jugando.
                updateScore(); // Actualiza el texto de la puntuación.
            }
        }
        // Verifica si la colisión es con otra zona fuera (Out2).
        else if (collision.transform.CompareTag("Out2"))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero; // Detiene la velocidad de la pelota.
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero; // Detiene la rotación de la pelota.

            GameObject.Find("player").GetComponent<Player>().Reset(); // Resetea la posición del jugador.

            // Si el juego está en curso, actualiza la puntuación.
            if (playing)
            {
                if (hitter == "player") // Si el jugador fue el que golpeó.
                {
                    botScore++; // Incrementa la puntuación del bot.
                }
                else if (hitter == "bot") // Si el bot fue el que golpeó.
                {
                    playerScore++; // Incrementa la puntuación del jugador.
                }
                playing = false; // Cambia el estado a no jugando.
                updateScore(); // Actualiza el texto de la puntuación.
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
            if (hitter == "player") // Si el jugador golpeó la pelota.
            {
                playerScore++;
                 // Incrementa la puntuación del bot.
            }
            else if (hitter == "bot") // Si el bot golpeó la pelota.
            {
                botScore++; // Incrementa la puntuación del jugador.
            }
            playing = false; // Cambia el estado a no jugando.
            updateScore(); // Actualiza el texto de la puntuación.
        }
        if (other.CompareTag("playerson"))
        {
            Debug.Log("¡Colisión con el jugador detectada en Trigger!");
            PlaySound(playerSound); // Reproduce el sonido del jugador.
        }
    }

    /// <summary>
    /// Actualiza el texto de puntuación en la interfaz de usuario.
    /// </summary>
    void updateScore()
    {
        playerScoreText.text = "Player:  " + playerScore; // Actualiza la puntuación del jugador en la UI.
        botScoreText.text = "Bot:  " + botScore; // Actualiza la puntuación del bot en la UI.
    }
}

