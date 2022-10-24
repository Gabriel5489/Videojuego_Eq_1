using System.Collections;
using UnityEngine;

public class Bloques : MonoBehaviour
{
    public float destructionTime = 1f;

    [Range(0f, 1f)]
    public float itemChance = 0.2f;
    public GameObject[] Items;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destructionTime);
    }

    public void OnDestroy()
    {
        if(Items.Length > 0 && Random.value < itemChance)
        {
            int RandomIndex = Random.Range(0, Items.Length);
            Instantiate(Items[RandomIndex], transform.position, Quaternion.identity);
        }
    }

}
