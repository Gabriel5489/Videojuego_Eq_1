using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour
{
    public GameObject bombaPrefab;
    public KeyCode bomba = KeyCode.Space;
    public float timeExplosion = 3f;
    public int cantidadBombas = 1;
    public int bombasRes;
    private Animator animacion;

    // Start is called before the first frame update
    void Start()
    {
        bombasRes = cantidadBombas;
        animacion = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bombasRes > 0 && Input.GetKeyDown(bomba))
        {
            StartCoroutine(ponerBomba());
        }
    }

    private IEnumerator ponerBomba()
    {
        float x, y;
        Vector2 position = transform.position;
        x = position.x;
        y = position.y;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);

        if(x <= 0 && y >= 0)
        {
            position.x -= .5f;
            position.y -= .5f;
        }else if (x >= 0 && y >= 0)
        {
            position.x += .5f;
            position.y -= .5f;
        }
        else if(x >= 0 && y <= 0)
        {
            position.x += .5f;
            position.y -= .5f;
        }
        else if (x <= 0 && y <= 0)
        {
            position.x -= .5f;
            position.y += .5f;
        }

        Debug.Log(position.y);

        GameObject bomba = Instantiate(bombaPrefab, position, Quaternion.identity);
        bombasRes--;

        yield return new WaitForSeconds(timeExplosion);

        Destroy(bomba);
        bombasRes++;
    }
}
