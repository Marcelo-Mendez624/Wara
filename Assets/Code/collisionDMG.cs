using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class collisionDMG : MonoBehaviour
{

    public float rayDistance = 1f; 
    public LayerMask collisionLayer;
    public float pushForce;

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, rayDistance, collisionLayer);

        Debug.DrawRay(transform.position, Vector2.right * rayDistance, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Wara"))
            {
                WaraPC waraPC = hit.collider.GetComponent<WaraPC>();

                if (waraPC != null)
                {
                    Rigidbody2D enemyRb = hit.collider.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        Vector2 pushDirection = (hit.collider.transform.position - transform.position).normalized;

                        enemyRb.AddForce(pushDirection * pushForce);
                    }

                    waraPC.TakeDamage();
                }
            }
        }
    }

}
