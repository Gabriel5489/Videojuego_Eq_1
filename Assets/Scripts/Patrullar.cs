using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrullar : MonoBehaviour
{
    public float velocidaMov;
    public Transform[] puntos;
    private int aleatorio;
    private Animator anim;
    public PlayerController pj;
    public GameObject crystal;
    private bool disparador = false;
    public bool movimientoEnemigo = true;
    

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        aleatorio = Random.Range(0, puntos.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (movimientoEnemigo)
        {
            if (transform.position == puntos[aleatorio].position)
            {
                CambiarRuta();
            }
            transform.position = Vector2.MoveTowards(transform.position, puntos[aleatorio].position, velocidaMov * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player") {
            CambiarRuta();
        }
        else
        {
            //anim.SetBool("hit", true);
        }
    }

    void CambiarRuta()
    {
        aleatorio = aleatorio > 0 ? 0 : 1;
        anim.SetBool("up", aleatorio == 1 ? true : false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion") && !disparador)
        {
            disparador = true;
            pj.enemigosDerrotados++;
            pj.ActualizaPuntaje();
            anim.SetTrigger("death");
            Invoke(nameof(DropCrystal), 1);
            gameObject.SetActive(false);
        }
    }

    private void DropCrystal()
    {
        if(pj.enemigosDerrotados == 3) {
            Instantiate(crystal, transform.position, Quaternion.identity);
        }
    }
}
