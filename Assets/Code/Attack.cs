using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Transform SwordPosition;
    public float Radius;
    public LayerMask CollisionLayer;
    public float pushForce = 10f; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Golpe();
    }

    
    void Golpe()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D Hit = Physics2D.CircleCast(SwordPosition.position, Radius, Vector2.zero, 0, CollisionLayer);


            if (Hit.collider != null)
            {
                // Aplica daño al enemigo
                Hit.collider.GetComponent<Enemy>().TakeDamage();

                // Empuja al enemigo hacia atrás
                Rigidbody2D enemyRb = Hit.collider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    // Calcula la dirección de empuje
                    Vector2 pushDirection = (Hit.collider.transform.position - SwordPosition.position).normalized;

                    // Aplica la fuerza de empuje
                    enemyRb.AddForce(pushDirection * pushForce);
                }
            }
        }
    }

}
