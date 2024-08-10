using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // Velocidad del enemigo
    public int damage = 1; // Daño que hace el enemigo al jugador
    public int life = 3;
    public float KnockoutTime = .3f;
    private Transform target; // Objetivo del enemigo (el jugador)
    private Rigidbody2D rb;
    private bool dead;
    private bool golpeado;
    

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Wara").transform; // Encuentra al jugador por la etiqueta "Player"
    }

    void Update()
    {
        if (target != null && !dead && !golpeado)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wara"))
        {
            //TODO Player damage
        }
    }

    public void TakeDamage()
    {
        life = life - 1;

        StartCoroutine(GolpeadoCoroutine());

        if(life <= 0)
        {
            dead = true;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            StartCoroutine(DeadCoroutine());
        }
    }

    private IEnumerator GolpeadoCoroutine()
    {
        golpeado = true;

        // Espera por el tiempo definido
        yield return new WaitForSeconds(KnockoutTime);

        // Desactivar el estado golpeado
        golpeado = false;
    }

     private IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

}

