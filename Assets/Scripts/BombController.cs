using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class BombController : MonoBehaviour
{
    [Header("Bombas")]
    public Bomb bombaPrefab;
    public KeyCode bombaKey = KeyCode.Space;
    public float timeExplosion = 3f;
    public int cantidadBombas;
    public int bombasRes;
    private Vector2 position;
    public TextMeshProUGUI txtBombas, txtFlama;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadio;
    private AudioSource audio;

    [Header("Bloques")]
    public Tilemap destructibleTiles;
    public Bloques bloquePrefab;

    // Start is called before the first frame update
    void Start()
    {
        AsignarItems();
        audio = GetComponent<AudioSource>();
        bombasRes = cantidadBombas;
    }

    // Update is called once per frame
    void Update()
    {
        if(bombasRes > 0 && Input.GetKeyDown(bombaKey))
        {
            StartCoroutine(ponerBomba());
        }
    }

    private IEnumerator ponerBomba()
    {
        float x, y;
        position = transform.position;
        x = position.x;
        y = position.y;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);

        Bomb bomba = Instantiate(bombaPrefab, position, Quaternion.identity);
        bomba.tiempoRestante = timeExplosion;
        bombasRes--;

        yield return new WaitForSeconds(timeExplosion);
        if (bomba)
        {
            position = bomba.transform.position;
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);

            Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            audio.Play();
            explosion.SetActiveRenderer("Start");
            explosion.DestroyAfter(explosionDuration);

            explode(position, Vector2.up, explosionRadio);
            explode(position, Vector2.down, explosionRadio);
            explode(position, Vector2.right, explosionRadio);
            explode(position, Vector2.left, explosionRadio);
            bomba.DestruirBomba();
        }
        bombasRes++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
        }
    }

    public void explode(Vector2 position, Vector2 direction, int length)
    {
        if(length <= 0)
        {
            return;
        }

        position += direction;

        if(Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask)){
            ClearDestructible(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? "Middle" : "End");
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        explode(position, direction, length - 1);
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if(tile != null)
        {
            Instantiate(bloquePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    private void AsignarItems()
    {
        cantidadBombas = PlayerPrefs.GetInt("Bomba");
        explosionRadio = PlayerPrefs.GetInt("Flama");
        txtBombas.SetText(cantidadBombas.ToString());
        txtFlama.SetText(explosionRadio.ToString());
    }

    public void AddBomb()
    {
        if(cantidadBombas < 6)
        {
            cantidadBombas++;
            PlayerPrefs.SetInt("Bomba", cantidadBombas);
            txtBombas.SetText(cantidadBombas.ToString());
            bombasRes++;
        }
    }

    public void AddFlame()
    {
        if (explosionRadio < 6) explosionRadio++;
        PlayerPrefs.SetInt("Flama", explosionRadio);
        txtFlama.SetText(explosionRadio.ToString());
    }
}
