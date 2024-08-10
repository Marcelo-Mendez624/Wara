using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boladefuego : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destruir el proyectil después de un tiempo
    }

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // Mover el proyectil
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Aquí puedes añadir lógica para detectar colisiones
        Destroy(gameObject); // Destruir el proyectil al chocar con algo
    }
}
