using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target; // El personaje que la cámara debe seguir
    public Vector2 deadZone = new Vector2(1f, 1f); // Tamaño de la dead zone en unidades de juego
    public float smoothTime = 0.3f; // Tiempo de suavizado para el movimiento de la cámara

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // Calcula la posición deseada de la cámara basada en la posición del objetivo
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Calcula la distancia entre la cámara y el objetivo
        Vector2 delta = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);

        // Solo mueve la cámara si el objetivo está fuera de la dead zone
        if (Mathf.Abs(delta.x) > deadZone.x || Mathf.Abs(delta.y) > deadZone.y)
        {
            // Ajusta la posición objetivo si está fuera de la zona muerta
            if (Mathf.Abs(delta.x) > deadZone.x)
                targetPosition.x = target.position.x - Mathf.Sign(delta.x) * deadZone.x;

            if (Mathf.Abs(delta.y) > deadZone.y)
                targetPosition.y = target.position.y - Mathf.Sign(delta.y) * deadZone.y;

            // Mueve la cámara suavemente hacia la nueva posición
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

}
