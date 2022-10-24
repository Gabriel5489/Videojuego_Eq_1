using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombController : MonoBehaviour
{
    [Header("Bombas")]
    public GameObject bombaPrefab;
    public KeyCode bomba = KeyCode.Space;
    public float timeExplosion = 3f;
    public int cantidadBombas = 1;
    public int bombasRes;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadio = 1;
    private AudioSource audio;

    [Header("Bloques")]
    public Tilemap destructibleTiles;
    public Bloques bloquePrefab;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        bombasRes = cantidadBombas;
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


        GameObject bomba = Instantiate(bombaPrefab, position, Quaternion.identity);
        bombasRes--;

        yield return new WaitForSeconds(timeExplosion);

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


        Destroy(bomba);
        bombasRes++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
        }
    }

    private void explode(Vector2 position, Vector2 direction, int length)
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

    public void AddBomb()
    {
        if(cantidadBombas < 6)
        {
            cantidadBombas++;
            bombasRes++;
        }
    }
}
