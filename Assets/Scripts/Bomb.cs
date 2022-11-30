using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    BombController bombController;
    public float tiempoRestante;
    bool pjInactivo = false;

    // Start is called before the first frame update
    void Start()
    {
        bombController = FindObjectOfType<BombController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pjInactivo)
        {
            StartCoroutine(ExplotaBomba());
        }
        else
        {
            tiempoRestante -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            bombController.explode(transform.position, Vector2.up, bombController.explosionRadio);
            bombController.explode(transform.position, Vector2.down, bombController.explosionRadio);
            bombController.explode(transform.position, Vector2.left, bombController.explosionRadio);
            bombController.explode(transform.position, Vector2.right, bombController.explosionRadio);
            DestruirBomba();
        }
    }

    public void DestruirBomba()
    {
        Destroy(gameObject);
    }

    private IEnumerator ExplotaBomba()
    {
        yield return new WaitForSeconds(tiempoRestante);
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(bombController.explosionPrefab, position, Quaternion.identity);
        //audio.Play();
        explosion.SetActiveRenderer("Start");
        explosion.DestroyAfter(bombController.explosionDuration);

        bombController.explode(position, Vector2.up, bombController.explosionRadio);
        bombController.explode(position, Vector2.down, bombController.explosionRadio);
        bombController.explode(position, Vector2.right, bombController.explosionRadio);
        bombController.explode(position, Vector2.left, bombController.explosionRadio);
        DestruirBomba();
    }
}
