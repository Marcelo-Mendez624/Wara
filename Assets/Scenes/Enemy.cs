using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // Velocidad del enemigo
    public int damage = 1; // Daño que hace el enemigo al jugador
    private Transform target; // Objetivo del enemigo (el jugador)

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Wara").transform; // Encuentra al jugador por la etiqueta "Player"
    }

    void Update()
    {
        // Mover al enemigo hacia el jugador
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wara"))
        {
            //TODO Player damage

            // Destruir al enemigo después de colisionar con el jugador
            Destroy(gameObject);
        }
    }
}

