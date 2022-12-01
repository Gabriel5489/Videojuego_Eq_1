using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private int escena;
    private Menu menu;
    public GameObject transicion;
    private Animator animator;
    [SerializeField] private float transitionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        escena = SceneManager.GetActiveScene().buildIndex;
        menu = FindObjectOfType<Menu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextScene());
        if (collision.CompareTag("Player") && SceneManager.GetActiveScene().name != "Stage 3")
        {
            SceneManager.LoadSceneAsync(escena + 1);
        }
        else {
            menu.ResetearYGuardar();
            SceneManager.LoadScene("Menu");
        }
    }

    IEnumerator LoadNextScene()
    {
        animator = transicion.GetComponent<Animator>();
        animator.SetTrigger("planetin");
        yield return new WaitForSeconds(transitionTime + 0.3f);
    }
}
