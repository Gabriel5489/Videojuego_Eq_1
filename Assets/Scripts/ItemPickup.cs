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
        }
        Destroy(gameObject);
    }

    private void OnItemPickup(GameObject player)
    {
        switch (type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;
            case ItemType.Flame:
                player.GetComponent<BombController>().explosionRadio++;
                break;
            case ItemType.Speed:
                player.GetComponent<PlayerController>().velocidad++;
                break;
        }
    }
}
