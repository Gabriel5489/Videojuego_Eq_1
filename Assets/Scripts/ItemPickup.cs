using System.Collections;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        ExtraBomb,
        Flame,
        Speed
    }

    public ItemType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnItemPickup(collision.gameObject);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Explosion"))
        {
            Destroy(gameObject);
        }
    }

    private void OnItemPickup(GameObject player)
    {
        switch (type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<PlayerController>().AddBomb();
                break;
            case ItemType.Flame:
                player.GetComponent<PlayerController>().AddFlame();
                break;
            case ItemType.Speed:
                player.GetComponent<PlayerController>().AddSpeed();
                break;
        }
    }
}
