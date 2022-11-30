using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menupausa;
    private KeyCode pause = KeyCode.Escape;
    private bool pauseActivo = false;
    PlayerController player;
    ControladorJuego controladorJuego;
    [SerializeField] Conexion conexion;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        controladorJuego = FindObjectOfType<ControladorJuego>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(pause) && !pauseActivo) MenuPausaActivar();
    }

    public void Botones(int opcion)
    {
        switch (opcion){
            case 1:
                SceneManager.LoadScene("Stage 1");
                break;
            case 2:
                SceneManager.LoadScene("Niveles");
                break;
            case 3:
                SceneManager.LoadScene("Acceso");
                break;
            case 4:
                SceneManager.LoadScene("Menu");
                break;

        }
    }

    public void Niveles(int nivel)
    {
        SceneManager.LoadSceneAsync("Stage " + nivel);
    }

    public void MenuPausaActivar()
    {
        pauseActivo = true;
        menupausa.SetActive(true);
        player.setMovimiento(!pauseActivo);
        controladorJuego.CambiarTemporizador(false);
    }

    public void MenuPausaDesactivar()
    {
        pauseActivo = false;
        menupausa.SetActive(false);
        player.setMovimiento(!pauseActivo);
        controladorJuego.CambiarTemporizador(true);
    }

    public void RegresarMenuPrincipal()
    {
        ResetearYGuardar();
        SceneManager.LoadScene("Menu");
    }

    private void ResetearYGuardar()
    {
        PlayerPrefs.SetInt("Vidas", 3);
        PlayerPrefs.SetInt("Flama", 1);
        PlayerPrefs.SetInt("Bomba", 1);
        PlayerPrefs.SetInt("Velocidad", 1);
        if(PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("HighScorePJ")) conexion.Guardar();
        PlayerPrefs.SetInt("Score", 0);
    }
}
