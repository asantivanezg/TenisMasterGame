using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    float speed = 30; // Velocidad de movimiento del bot.
    Animator animator; // Referencia al componente Animator para controlar las animaciones.
    public Transform ball; // Referencia a la pelota en el juego.
    public Transform aimTarget; // Objetivo al que apunta el bot (por ejemplo, una posici�n en la cancha).

    public Transform[] targets; // Array de posiciones objetivo que el bot puede elegir para enviar la pelota.

    float force = 13; // Fuerza base aplicada a los disparos.
    Vector3 targetPosition; // Posici�n objetivo actual del bot.

    ShotManager shotManager; // Referencia al componente que gestiona los tipos de disparos disponibles.

    void Start()
    {
        targetPosition = transform.position; // Inicializa la posici�n objetivo como la posici�n actual del bot.
        animator = GetComponent<Animator>(); // Obtiene el componente Animator adjunto al bot.
        shotManager = GetComponent<ShotManager>(); // Obtiene el componente ShotManager adjunto al bot.
    }

    // Update es llamado una vez por frame.
    void Update()
    {
        Move(); // Llama al m�todo para mover al bot.
    }

    // Mueve el bot hacia la posici�n de la pelota en el eje X.
    void Move()
    {
        targetPosition.x = ball.position.x; // Actualiza la posici�n objetivo del bot para que coincida con la posici�n X de la pelota.
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        // Mueve al bot gradualmente hacia la posici�n objetivo usando una interpolaci�n lineal.
    }

    // Elige una posici�n aleatoria entre los objetivos disponibles.
    Vector3 PickTarget()
    {
        int randomValue = Random.Range(0, targets.Length); // Selecciona un �ndice aleatorio del array de objetivos.
        return targets[randomValue].position; // Devuelve la posici�n del objetivo seleccionado.
    }

    // Elige un tipo de disparo aleatoriamente entre top spin y flat.
    Shot PickShot()
    {
        int randomValue = Random.Range(0, 2); // Genera un n�mero aleatorio entre 0 y 1.
        if (randomValue == 0)
            return shotManager.topSpin; // Devuelve el disparo tipo top spin.
        else
            return shotManager.flat; // Devuelve el disparo tipo flat.
    }

    // Detecta colisiones con otros objetos.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) // Verifica si el objeto con el que colisiona tiene la etiqueta "Ball".
        {
            // Elige un tipo de disparo al azar.
            Shot currentShot = PickShot();

            // Calcula la direcci�n hacia el objetivo elegido y aplica fuerza a la pelota.
            Vector3 dir = PickTarget() - transform.position; // Direcci�n desde el bot hacia el objetivo seleccionado.
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce
                + new Vector3(0, currentShot.upForce, 0);
            // Aplica fuerza a la pelota en la direcci�n calculada, agregando un impulso hacia arriba.

            // Determina de qu� lado est� la pelota para elegir la animaci�n adecuada.
            Vector3 ballDir = ball.position - transform.position; // Direcci�n desde el bot hacia la pelota.
            if (ballDir.x >= 0) // Si la pelota est� a la derecha del bot:
            {
                animator.Play("forehand"); // Reproduce la animaci�n de golpe de derecha.
            }
            else
            {
                animator.Play("backhand"); // Reproduce la animaci�n de golpe de rev�s.
            }

            // Indica que el �ltimo en golpear la pelota fue el bot.
            ball.GetComponent<Ball>().hitter = "bot";
        }
    }
}