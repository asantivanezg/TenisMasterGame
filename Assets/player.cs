using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Esta clase controla el comportamiento del jugador en el juego.
/// Permite el movimiento del jugador y la ejecución de diferentes tipos de golpes.
/// </summary>
public class Player : MonoBehaviour
{
    public Transform aimTarget; // Un objetivo al que el jugador puede apuntar.
    float speed = 3.5f; // Velocidad del jugador.
    float force = 13; // La fuerza que se aplicará a la pelota al golpearla.
    bool hitting; // Indica si el jugador está en el estado de "golpear" o no.

    public Transform ball; // Objeto pelota.
    Animator animator; // Componente Animator para gestionar animaciones.

    Vector3 aimTargetInitialPosition; // Posición inicial del objetivo de apuntar.
    ShotManager shotManager; // Componente que gestiona los tipos de golpes.
    Shot currentShot; // Almacena el tipo de golpe actual.

    [SerializeField] Transform serveRight; // Punto de servicio a la derecha.
    [SerializeField] Transform serveLeft; // Punto de servicio a la izquierda.

    bool servedRight = true; // Indica si el último servicio fue a la derecha.

    /// <summary>
    /// Inicializa los componentes y valores al inicio del juego.
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>(); // Obtiene el componente Animator.
        aimTargetInitialPosition = aimTarget.position; // Guarda la posición inicial del objetivo.
        shotManager = GetComponent<ShotManager>(); // Obtiene el ShotManager.
        currentShot = shotManager.topSpin; // Establece el golpe inicial.
    }

    /// <summary>
    /// Actualiza el estado del jugador cada frame.
    /// </summary>
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); // Obtiene la entrada horizontal del usuario.
        float v = Input.GetAxisRaw("Vertical"); // Obtiene la entrada vertical del usuario.

        // Manejo del golpe de top spin.
        if (Input.GetKeyDown(KeyCode.F))
        {
            hitting = true; // Indica que se está golpeando.
            currentShot = shotManager.topSpin; // Establece el golpe a top spin.
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            hitting = false; // Termina el golpe.
        }

        // Movimiento del objetivo de apuntar si el jugador está golpeando.
        if (hitting)
        {
            aimTarget.Translate(new Vector3(h, 0, 0) * speed * 2 * Time.deltaTime);
        }

        // Manejo del golpe plano.
        if (Input.GetKeyDown(KeyCode.E))
        {
            hitting = true; // Indica que se está golpeando.
            currentShot = shotManager.flat; // Establece el golpe a plano.
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            hitting = false; // Termina el golpe.
        }

        // Manejo del servicio a la derecha.
        if (Input.GetKeyDown(KeyCode.U))
        {
            hitting = true; // Indica que se está golpeando.
            currentShot = shotManager.flatServe; // Establece el golpe a flat serve.
            GetComponent<BoxCollider>().enabled = false; // Desactiva el collider durante el servicio.
            animator.Play("serve-prepare"); // Reproduce la animación de preparación para el servicio.
        }

        // Manejo del servicio a la izquierda (kick serve).
        if (Input.GetKeyDown(KeyCode.I))
        {
            hitting = true; // Indica que se está golpeando.
            currentShot = shotManager.kickServe; // Establece el golpe a kick serve.
            GetComponent<BoxCollider>().enabled = false; // Desactiva el collider durante el servicio.
            animator.Play("serve-prepare"); // Reproduce la animación de preparación para el servicio.
        }

        // Finaliza el servicio y aplica la fuerza a la pelota.
        if (Input.GetKeyUp(KeyCode.U) || Input.GetKeyUp(KeyCode.I))
        {
            hitting = false; // Termina el golpe.
            GetComponent<BoxCollider>().enabled = true; // Reactiva el collider.
            ball.transform.position = transform.position + new Vector3(0.2f, 1, 0); // Coloca la pelota en la posición correcta para servir.
            Vector3 dir = aimTarget.position - transform.position; // Calcula la dirección hacia el objetivo.
            ball.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0); // Aplica fuerza a la pelota.
            animator.Play("serve"); // Reproduce la animación de servicio.
            ball.GetComponent<Ball>().hitter = "player"; // Establece al jugador como el que golpea la pelota.
            ball.GetComponent<Ball>().playing = true; // Marca la pelota como en juego.
        }

        // Movimiento del jugador si no está golpeando.
        if ((h != 0 || v != 0) && !hitting)
        {
            Vector3 movement = new Vector3(h, 0, v) * speed * Time.deltaTime; // Calcula el movimiento.
            transform.Translate(movement); // Aplica el movimiento al jugador.
        }
    }

    /// <summary>
    /// Maneja las colisiones del jugador con la pelota.
    /// </summary>
    /// <param name="other">Collider con el que colisiona el jugador.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si colisiona con la pelota.
        if (other.CompareTag("Ball"))
        {
            Vector3 dir = aimTarget.position - transform.position; // Calcula la dirección hacia el objetivo de apuntar.
            other.GetComponent<Rigidbody>().velocity = dir.normalized * currentShot.hitForce + new Vector3(0, currentShot.upForce, 0); // Aplica fuerza a la pelota.

            Vector3 ballDir = ball.position - transform.position; // Calcula la dirección hacia la pelota.
            if (ballDir.x >= 0) // Si la pelota está a la derecha del jugador.
            {
                animator.Play("forehand"); // Reproduce la animación de "forehand".
            }
            else // Si la pelota está a la izquierda.
            {
                animator.Play("backhand"); // Reproduce la animación de "backhand".
            }

            ball.GetComponent<Ball>().hitter = "player"; // Establece al jugador como el que golpea.
        }
    }

    /// <summary>
    /// Restablece la posición del jugador al inicio del servicio.
    /// </summary>
    public void Reset()
    {
        if (servedRight)
            transform.position = serveLeft.position; // Mueve al jugador a la posición de servicio a la izquierda.
        else
            transform.position = serveRight.position; // Mueve al jugador a la posición de servicio a la derecha.

        servedRight = !servedRight; // Cambia el lado de servicio para el siguiente.
    }
}
