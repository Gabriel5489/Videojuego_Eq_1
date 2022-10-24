using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrullar : MonoBehaviour
{
    public float velocidaMov;
    public Transform[] puntos;
    private int aleatorio;
    private Animator anim;
    

    // Start is called before the first frame update
    void Start()
    {
        aleatorio = Random.Range(0, puntos.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == puntos[aleatorio].position) CambiarRuta();
        transform.position = Vector2.MoveTowards(transform.position, puntos[aleatorio].position, velocidaMov * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player") {
            CambiarRuta();
        }
    }

    void CambiarRuta()
    {
        aleatorio = aleatorio > 0 ? 0 : 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            Destroy(gameObject);
        }
    }
}
