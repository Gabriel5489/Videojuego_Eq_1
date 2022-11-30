using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private int escena;

    // Start is called before the first frame update
    void Start()
    {
        escena = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && SceneManager.GetActiveScene().name != "Stage 3")
        {
            SceneManager.LoadScene(escena + 1);
        }
        else { 
            SceneManager.LoadScene("Menu");
        }
    }
}
