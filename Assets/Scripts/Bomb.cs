using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    BombController bombController;

    // Start is called before the first frame update
    void Start()
    {
        bombController = FindObjectOfType<BombController>();
    }

    // Update is called once per frame
    void Update()
    {
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


}
